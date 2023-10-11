using Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UIQuestStatus : MonoBehaviour
    {
        public Image[] StatusImages;

        private NpcQuestStatus _questStatus;

        public void SetQuestStatus(NpcQuestStatus status)
        {
            _questStatus = status;
            for (int i = 0; i < 4; i++)
            {
                if (StatusImages[i] != null)
                {
                    StatusImages[i].gameObject.SetActive(i == (int)status);
                }   
            }
        }
    }
}
