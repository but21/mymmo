using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Data;
using GameServer.Entities;
using GameServer.Services;
using Network;
using SkillBridge.Message;

namespace GameServer.Managers
{
    class QuestManager
    {
        Character owner;

        public QuestManager(Character owner)
        {
            this.owner = owner;
        }

        public void GetQuestInfos(List<NQuestInfo> list)
        {

            foreach (var quest in owner.Data.Quests)
            {
                list.Add(GetQuestInfo(quest));
            }
        }

        public NQuestInfo GetQuestInfo(TCharacterQuest quest)
        {
            return new NQuestInfo()
            {
                QuestId = quest.QuestID,
                QuestGuid = quest.Id,
                Status = (QuestStatus)quest.Status,
                Targets = new int[3]
                {
                    quest.Target1,
                    quest.Target2,
                    quest.Target3,
                }
            };
        }

        public Result AcceptQuest(NetConnection<NetSession> sender, int questId)
        {
            Character character = sender.Session.Character;

            QuestDefine quest;
            if (DataManager.Instance.Quests.TryGetValue(questId, out quest))
            {
                var dbQuest = DBService.Instance.Entities.CharacterQuests.Create();

                dbQuest.QuestID = quest.ID;
                if (quest.Target1 == QuestTarget.None)
                {
                    dbQuest.Status = (int)QuestStatus.Completed;
                }
                else
                {
                    dbQuest.Status = (int)QuestStatus.InProgress;
                }

                sender.Session.Response.questAccept.Quest = GetQuestInfo(dbQuest);
                character.Data.Quests.Add(dbQuest);
                DBService.Instance.Save();
                return Result.Success;
            }
            else
            {
                sender.Session.Response.questAccept.Errormsg = "任务不存在";
                return Result.Failed;
            }
        }

        public Result SubmitQuest(NetConnection<NetSession> sender, int questId)
        {
            Character character = sender.Session.Character;
            QuestDefine quest;
            if (DataManager.Instance.Quests.TryGetValue(questId, out quest))
            {
                var dbQuest = character.Data.Quests.Where(q => q.QuestID == quest.ID).FirstOrDefault();
                if (dbQuest != null)
                {
                    // Completed:完成未提交
                    if (dbQuest.Status != (int)QuestStatus.Completed)
                    {
                        sender.Session.Response.questSubmit.Errormsg = "任务未完成";
                        return Result.Failed;
                    }

                    // Finished代表已提交
                    dbQuest.Status = (int)QuestStatus.Finished;
                    sender.Session.Response.questSubmit.Quest = GetQuestInfo(dbQuest);
                    DBService.Instance.Save();

                    if (quest.RewardGold > 0)
                    {
                        character.Gold += quest.RewardGold;
                    }

                    if (quest.RewardExp > 0)
                    {
                        // todo: 角色经验增加
                    }

                    if (quest.RewardItem1Count > 0)
                    {
                        character.ItemManager.AddItem(quest.RewardItem1, quest.RewardItem1Count);
                    }

                    if (quest.RewardItem2Count > 0)
                    {
                        character.ItemManager.AddItem(quest.RewardItem2, quest.RewardItem2Count);
                    }

                    if (quest.RewardItem3Count > 0)
                    {
                        character.ItemManager.AddItem(quest.RewardItem3, quest.RewardItem3Count);
                    }

                    DBService.Instance.Save();
                    return Result.Success;
                }

                // 数据库中不存在
                sender.Session.Response.questSubmit.Errormsg = "任务不存在[2]";
                return Result.Failed;
            }
            else
            {
                // 配置表中不存在
                sender.Session.Response.questSubmit.Errormsg = "任务不存在[1]";
                return Result.Failed;
            }
        }
    }
}
