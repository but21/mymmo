using Common.Data;
using Models;
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
    }
}
