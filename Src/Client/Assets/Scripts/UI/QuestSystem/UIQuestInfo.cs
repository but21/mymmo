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

namespace Assets.Scripts.UI
{
    public class UIQuestInfo : MonoBehaviour
    {
        public Text Title;
        public Text[] Targets;
        public Text Description;

        public UIIconItem RewardItems;

        public Text RewardMoney;
        public Text RewardExp;

        public void SetQuestInfo(Quest quest)
        {
            Title.text = string.Format($"[{quest.Define.Type}][{quest.Define.Name}]");
            if (quest.Info == null)
            {
                Description.text = quest.Define.Dialog;
            }
            else
            {
                if (quest.Info.Status == QuestStatus.Completed)
                {
                    Description.text = quest.Define.DialogFinish;
                }
            }

            RewardMoney.text = quest.Define.RewardGold.ToString();
            RewardExp.text = quest.Define.RewardExp.ToString();

            foreach (var fitter in this.GetComponentsInChildren<ContentSizeFitter>())
            {
                fitter.SetLayoutVertical();
            }
        }


    }
}
