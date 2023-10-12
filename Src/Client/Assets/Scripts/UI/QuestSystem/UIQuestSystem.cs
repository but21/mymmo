using Assets.Scripts.UI.Tab;
using Common.Data;
using Managers;
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
    public class UIQuestSystem : UIWindow
    {
        public Text Title;

        public GameObject ItemPrefab;

        public TabView Tabs;
        public ListView ListMain;
        public ListView ListBranch;

        public UIQuestInfo QuestInfo;

        // 是否显示可接任务
        private bool _showAvailableList = false;

        private void Start()
        {
            ListMain.onItemSelected += OnQuestSelected;
            ListBranch.onItemSelected += OnQuestSelected;
            Tabs.OnTabSelect += OnSelectTab;
            RefreshUI();
        }

        void OnSelectTab(int idx)
        {
            _showAvailableList = idx == 1;
            RefreshUI();
        }

        void RefreshUI()
        {
            ClearAllQuestList();
            InitAllQuestItems();
        }

        void ClearAllQuestList()
        {
            ListMain.RemoveAll();
            ListBranch.RemoveAll();
        }

        void InitAllQuestItems()
        {
            foreach (var kv in QuestManager.Instance.AllQuests)
            {
                if (_showAvailableList)
                {
                    if (kv.Value.Info != null)
                    {
                        continue;
                    }
                }
                else
                {
                    // todo: kv.Value.Info.Status == SkillBridge.Message.QuestStatus.Finished
                    if (kv.Value.Info == null || kv.Value.Info.Status == SkillBridge.Message.QuestStatus.Finished)
                    {
                        continue;
                    }
                }

                GameObject go = Instantiate(ItemPrefab, kv.Value.Define.Type == QuestType.Main ? ListMain.transform : ListBranch.transform);
                UIQuestItem ui = go.GetComponent<UIQuestItem>();
                ui.SetQuestInfo(kv.Value);
                /*if(kv.Value.Define.Type == QuestType.Main)
                {
                    ListMain.AddItem(ui);
                }
                else
                {
                    ListBranch.AddItem(ui);
                }*/
                ListMain.AddItem(ui);
                ListBranch.AddItem(ui);
            }
        }

        public void OnQuestSelected(ListView.ListViewItem item)
        {
            UIQuestItem questItem = item as UIQuestItem;
            QuestInfo.SetQuestInfo(questItem.quest);
        }
    }
}
