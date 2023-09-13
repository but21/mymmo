using Common.Data;
using Models;
using Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Managers
{
    public class ItemManager : Singleton<ItemManager>
    {
        public Dictionary<int, Item> Items = new Dictionary<int, Item>();

        internal void Init(List<NItemInfo> items)
        {
            Items.Clear();
            foreach (var info in items)
            {
                Item item = new Item(info);
                Items.Add(item.Id, item);
                Debug.Log($"ItemManager: Init:{item}");
            }
            StatusService.Instance.RegisterStatusNotify(StatusType.Item, OnItemNotify);
        }

        public ItemDefine GetDefine(int itemId)
        {
            return null;
        }

        public bool UseItem(int itemId)
        {
            return false;
        }

        public bool GetItem(ItemDefine item)
        {
            return false;
        }

        private bool OnItemNotify(NStatus status)
        {
            if (status.Action == StatusAction.Add)
            {
                AddItem(status.Id, status.Value);
            }
            else if (status.Action == StatusAction.Delete)
            {
                RemoveItem(status.Id, status.Value);
            }
            return true;
        }

        private void AddItem(int itemId, int count)
        {
            Item item = null;
            if (Items.TryGetValue(itemId, out item))
            {
                Items[itemId].Count += count;
            }
            else
            {
                item = new Item(itemId, count);
                Items.Add(itemId, item);
            }
            BagManager.Instance.AddItem(itemId, count);
        }

        private void RemoveItem(int itemId, int count)
        {
            if (!Items.ContainsKey(itemId))
            {
                return;
            }
            Item item = Items[itemId];
            if(item.Count < count)
            {
                return;
            }
            item.Count -= count;

            BagManager.Instance.RemoveItem(itemId, count);
        }
    }
}
