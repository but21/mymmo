using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using SkillBridge.Message;
using Common.Data;
using Managers;

namespace Assets.Scripts.UI
{
    public class UIQuestInfo : MonoBehaviour
    {
        public Text Title;
        public Text[] Targets;
        public Text Description;

        public UIIconItem[] RewardItems;

        public Text RewardMoney;
        public Text RewardExp;

        public void SetQuestInfo(Quest quest)
        {
            Title.text = string.Format($"[{quest.Define.Type}][{quest.Define.Name}]");
            /*if (quest.Info == null)
            {
                Description.text = quest.Define.Dialog;
            }
            else
            {
                if (quest.Info.Status == QuestStatus.Finished)
                {
                    Description.text = quest.Define.DialogFinish;
                }else if(quest.Info.Status == QuestStatus.InProgress)
                {
                    Description.text = quest.Define.Dialog;                    
                }
            }*/
            if (quest.Define.RewardItem1Count > 0)
            {
                RewardItems[0].MainIcon.overrideSprite = Resloader.Load<Sprite>(DataManager.Instance.Items[quest.Define.RewardItem1].Icon);
                if (quest.Define.RewardItem2Count > 0)
                {
                    RewardItems[1].MainIcon.overrideSprite = Resloader.Load<Sprite>(DataManager.Instance.Items[quest.Define.RewardItem2].Icon);
                    if (quest.Define.RewardItem3Count > 0)
                    {
                        RewardItems[2].MainIcon.overrideSprite = Resloader.Load<Sprite>(DataManager.Instance.Items[quest.Define.RewardItem3].Icon);
                    }
                    else
                    {
                        RewardItems[2].gameObject.SetActive(false);
                    }
                }
                else
                {
                    RewardItems[1].gameObject.SetActive(false);
                    RewardItems[2].gameObject.SetActive(false);
                }
            }
            else
            {
                RewardItems[0].gameObject.SetActive(false);
                RewardItems[1].gameObject.SetActive(false);
                RewardItems[2].gameObject.SetActive(false);
            }

            Description.text = quest.Define.Dialog;
            RewardMoney.text = quest.Define.RewardGold.ToString();
            RewardExp.text = quest.Define.RewardExp.ToString();

            foreach (var fitter in this.GetComponentsInChildren<ContentSizeFitter>())
            {
                fitter.SetLayoutVertical();
            }
        }


    }
}
