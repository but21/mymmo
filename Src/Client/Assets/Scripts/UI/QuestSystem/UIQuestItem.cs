using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class UIQuestItem : ListView.ListViewItem
    {
        public Text Title;
        public Image Background;

        public Sprite NormalBg;
        public Sprite SelectedBg;

        public Quest quest;

        bool isEquiped = false;
        public override void OnSelected(bool selected)
        {
            Background.overrideSprite = selected ? SelectedBg : NormalBg;
        }

        public void SetQuestInfo(Quest quest)
        {
            this.quest = quest;
            if (this.Title != null)
            {
                this.Title.text = this.quest.Define.Name;
            }
        }
    }
}
