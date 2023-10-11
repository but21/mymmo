using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Models;
using SkillBridge.Message;
using Services;

namespace Assets.Scripts.UI
{
    public class UIQuestDialog : UIWindow
    {
        public UIQuestInfo QuestInfo;

        public Quest GetQuest;

        public GameObject OpenButtons;
        public GameObject SubmitButtons;

        public void SetQuest(Quest quest)
        {
            GetQuest = quest;
            UpdataQuest();
            if(GetQuest.Info == null)
            {
                OpenButtons.SetActive(true);
                SubmitButtons.SetActive(false);
            }
            else
            {
                if(GetQuest.Info.Status == QuestStatus.Completed)
                {
                    OpenButtons.SetActive(false);
                    SubmitButtons.SetActive(true);
                }
                else
                {
                    OpenButtons.SetActive(false);
                    SubmitButtons.SetActive(false);
                }
            }
        }

        void UpdataQuest()
        {
            if (this.GetQuest != null)
            {
                if (QuestInfo != null)
                {
                    QuestInfo.SetQuestInfo(GetQuest);
                }
            }
        }

        
    }
}
