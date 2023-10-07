using Assets.Scripts.UI;
using Models;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managers
{
    public enum NpcQuestStatus
    {
        None = 0, // 无任务
        Complete, // 拥有已完成可提交任务
        Available, // 拥有可接受任务
        Incomplete, // 拥有未完成任务 
    }

    class QuestManager : Singleton<QuestManager>
    {
        // 所有有效任务
        public List<NQuestInfo> QuestInfos;
        public Dictionary<int, Quest> AllQuests = new Dictionary<int, Quest>();

        // int: npcId
        public Dictionary<int, Dictionary<NpcQuestStatus, List<Quest>>> NpcQuests = new Dictionary<int, Dictionary<NpcQuestStatus, List<Quest>>>();

        public void Init(List<NQuestInfo> quests)
        {
            QuestInfos = quests;
            AllQuests.Clear();
            NpcQuests.Clear();
            InitQuests();
        }

        void InitQuests()
        {
            // 已接任务
            foreach (var info in QuestInfos)
            {
                Quest quest = new Quest(info);
                AddNpcQuest(quest.Define.AcceptNpc, quest);
                AddNpcQuest(quest.Define.SubmitNpc, quest);
                AllQuests[quest.Info.QuestId] = quest;
            }

            // 初始化可接任务
            foreach (var kv in DataManager.Instance.Quests)
            {
                if (kv.Value.LimitClass != CharacterClass.None && kv.Value.LimitClass != User.Instance.CurrentCharacter.Class)
                    continue; // 不符合职业
                if (kv.Value.LimitLevel > User.Instance.CurrentCharacter.Level)
                    continue; // 不符合等级
                if (AllQuests.ContainsKey(kv.Key))
                    continue; // 任务已存在

                if (kv.Value.PreQuest > 0)
                {
                    Quest preQuest;
                    if (AllQuests.TryGetValue(kv.Value.PreQuest, out preQuest)) // 获取前置任务
                    {
                        if (preQuest.Info == null)
                        {
                            continue; // 未接取前置任务
                        }
                        if (preQuest.Info.Status != QuestStatus.Completed)
                            continue; // 前置任务未完成
                    }
                    else
                    {
                        continue; // 前置任务未接取
                    }
                }
                Quest quest = new Quest(kv.Value);
                AddNpcQuest(quest.Define.AcceptNpc, quest);
                AddNpcQuest(quest.Define.SubmitNpc, quest);
                AllQuests[quest.Define.ID] = quest;
            }
        }

        void AddNpcQuest(int npcId, Quest quest)
        {
            if (!NpcQuests.ContainsKey(npcId))
            {
                NpcQuests[npcId] = new Dictionary<NpcQuestStatus, List<Quest>>();
            }

            List<Quest> availables;
            List<Quest> completes;
            List<Quest> incompletes;

            if (!NpcQuests[npcId].TryGetValue(NpcQuestStatus.Available, out availables))
            {
                availables = new List<Quest>();
                NpcQuests[npcId][NpcQuestStatus.Available] = availables;
            }

            if (!NpcQuests[npcId].TryGetValue(NpcQuestStatus.Complete, out completes))
            {
                completes = new List<Quest>();
                NpcQuests[npcId][NpcQuestStatus.Complete] = completes;
            }

            if (!NpcQuests[npcId].TryGetValue(NpcQuestStatus.Incomplete, out incompletes))
            {
                incompletes = new List<Quest>();
                NpcQuests[npcId][NpcQuestStatus.Incomplete] = incompletes;
            }

            if (quest.Info == null)
            {
                if (npcId == quest.Define.AcceptNpc && !NpcQuests[npcId][NpcQuestStatus.Available].Contains(quest))
                {
                    NpcQuests[npcId][NpcQuestStatus.Available].Add(quest);
                }
            }
            else
            {
                if (quest.Define.SubmitNpc == npcId && quest.Info.Status == QuestStatus.Completed)
                {
                    if (!NpcQuests[npcId][NpcQuestStatus.Complete].Contains(quest))
                    {
                        NpcQuests[npcId][NpcQuestStatus.Complete].Add(quest);
                    }
                }
                if (quest.Define.SubmitNpc == npcId && quest.Info.Status == QuestStatus.InProgress)
                {
                    if (!NpcQuests[npcId][NpcQuestStatus.Incomplete].Contains(quest))
                    {
                        NpcQuests[npcId][NpcQuestStatus.Incomplete].Add(quest);
                    }
                }
            }
        }

        // 获取npc任务状态
        public NpcQuestStatus GetQuestStatusByNpc(int npcId)
        {
            Dictionary<NpcQuestStatus, List<Quest>> status = new Dictionary<NpcQuestStatus, List<Quest>>();
            if (NpcQuests.TryGetValue(npcId, out status))
            {
                if (status[NpcQuestStatus.Complete].Count > 0)
                {
                    return NpcQuestStatus.Complete;
                }
                if (status[NpcQuestStatus.Available].Count > 0)
                {
                    return NpcQuestStatus.Available;
                }
                if (status[NpcQuestStatus.Incomplete].Count > 0)
                {
                    return NpcQuestStatus.Incomplete;
                }
            }
            return NpcQuestStatus.None;
        }

        public bool OpenNpcQuest(int npcId)
        {
            Dictionary<NpcQuestStatus, List<Quest>> status = new Dictionary<NpcQuestStatus, List<Quest>>();
            if (NpcQuests.TryGetValue(npcId, out status))
            {
                if (status[NpcQuestStatus.Complete].Count > 0)
                {
                    return ShowQuestDialog(status[NpcQuestStatus.Complete].First());
                }
                if (status[NpcQuestStatus.Available].Count > 0)
                {
                    return ShowQuestDialog(status[NpcQuestStatus.Available].First());
                }
                if (status[NpcQuestStatus.Incomplete].Count > 0)
                {
                    return ShowQuestDialog(status[NpcQuestStatus.Incomplete].First());
                }
            }

            return false;
        }

        bool ShowQuestDialog(Quest quest)
        {
            if (quest.Info == null || quest.Info.Status == QuestStatus.Completed)
            {
                UIQuestDialog dlg = UIManager.Instance.Show<UIQuestDialog>();
                dlg.SetQuest(quest);
                dlg.OnClose += OnQuestDialogClose;
                return true;
            }
            if (quest.Info != null || quest.Info.Status == QuestStatus.InProgress)
            {
                if (!string.IsNullOrEmpty(quest.Define.DialogIncomplete))
                    MessageBox.Show(quest.Define.DialogIncomplete);
            }
            return true;
        }

        void OnQuestDialogClose(UIWindow sender, UIWindow.WindowResult result)
        {
            UIQuestDialog dlg = (UIQuestDialog)sender;
            if (result == UIWindow.WindowResult.Yes)
            {
                MessageBox.Show(dlg.GetQuest.Define.DialogAccept);
            }
            else if (result == UIWindow.WindowResult.No)
            {
                MessageBox.Show(dlg.GetQuest.Define.DialogDeny);
            }
        }

        public void OnQuestAccepted(Quest quest)
        {

        }
    }
}
