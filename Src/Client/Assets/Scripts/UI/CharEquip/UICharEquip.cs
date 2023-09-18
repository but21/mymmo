using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using Managers;
using Models;
using SkillBridge.Message;

namespace Assets.Scripts.UI
{
    public class UICharEquip : UIWindow
    {
        public Text Title;
        public Text Money;

        public GameObject ItemPrefab;
        public GameObject ItemEquipedPrefab;

        public Transform ItemListRoot;
        public List<Transform> Slots;

        private void Start()
        {
            RefreshUI();
            EquipManager.Instance.OnEquipChanged += RefreshUI;
        }

        private void OnDestroy()
        {
            EquipManager.Instance.OnEquipChanged -= RefreshUI;
        }

        void RefreshUI()
        {
            ClearAllEquipList();
            InitAllEquipItems();
            ClearEquipedList();
            InitEquipedItem();
            Money.text = User.Instance.CurrentCharacter.Gold.ToString();
        }

        // 清空左侧拥有装备列表
        private void ClearAllEquipList()
        {
            foreach (var item in ItemListRoot.GetComponentsInChildren<UIEquipItem>())
            {
                Destroy(item.gameObject);
            }
        }

        // 初始化左侧拥有装备列表
        private void InitAllEquipItems()
        {
            foreach (var kv in ItemManager.Instance.Items)
            {
                if (kv.Value.ItemInfo.Type == ItemType.Equip)
                {
                    if (EquipManager.Instance.Contains(kv.Key) || kv.Value.ItemInfo.LimitClass != User.Instance.CurrentCharacter.Class)
                    {
                        continue;
                    }
                    GameObject go = Instantiate(ItemPrefab, ItemListRoot);
                    UIEquipItem ui = go.GetComponent<UIEquipItem>();
                    ui.SetEquipItem(kv.Key, kv.Value, this, false);
                }
            }
        }

        // 清空已装备列表
        private void ClearEquipedList()
        {

            foreach (var item in Slots)
            {
                if (item.childCount > 0)
                {
                    Destroy(item.GetChild(0).gameObject);
                }
            }

        }

        // 初始化已装备列表
        private void InitEquipedItem()
        {
            for (int i = 0; i < (int)EquipSlot.SlotMax; i++)
            {
                var item = EquipManager.Instance.Equips[i];

                if (item != null)
                {
                    GameObject go = Instantiate(ItemEquipedPrefab, Slots[i]);
                    UIEquipItem ui = go.GetComponent<UIEquipItem>();
                    ui.SetEquipItem(i, item, this, true);
                }


            }
        }

        public void DoEquip(Item item)
        {
            EquipManager.Instance.EquipItem(item);
        }

        public void UnEquip(Item item)
        {
            EquipManager.Instance.UnEquipItem(item);
        }
    }
}
