using Common;
using GameServer.Entities;
using GameServer.Models;
using GameServer.Services;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class ItemManager
    {
        public Dictionary<int, Item> Items = new Dictionary<int, Item>();

        public Character Owner;

        public ItemManager(Character owner)
        {
            Owner = owner;
            foreach (var item in owner.Data.Items)
            {
                Items.Add(item.ItemId, new Item(item));
            }
        }

        public bool UseItem(int itemId, int count = 1)
        {
            Log.Info($"ItemOwner:{Owner.Data.ID}, UseItem:{itemId}, {count}");

            Item item = null;
            if (Items.TryGetValue(itemId, out item))
            {
                if (item.Count < count)
                {
                    return false;
                }
                // todo: 使用逻辑
                item.Remove(count);
                return true;
            }
            return false;
        }

        public bool HasItem(int itemId)
        {
            Item item = null;
            if (Items.TryGetValue(itemId, out item))
            {
                return item.Count > 0;
            }
            return false;
        }

        public Item GetItem(int itemId)
        {
            Item item = null;
            Items.TryGetValue(itemId, out item);

            return item;

        }

        public bool AddItem(int itemId, int count)
        {
            Item item = null;
            if(Items.TryGetValue(itemId, out item))
            {
                item.Add(count);
            }
            else
            {
                TCharacterItem dbItem = new TCharacterItem();
                dbItem.TCharacterID = Owner.Data.ID;
                dbItem.Owner = Owner.Data;
                dbItem.ItemId = itemId;
                dbItem.ItemCount = count;
                Owner.Data.Items.Add(dbItem);
                item = new Item(dbItem);
                Items.Add(itemId, item);
            }
            Owner.StatusManager.AddItemChange(itemId, count, StatusAction.Add);
            Log.Info($"{Owner.Data.ID}AddItem:{item} AddCount:{count}");
            //DBService.Instance.Save();
            return true;
        }

        public bool RemoveItem(int itemId, int count)
        {
            if (!Items.ContainsKey(itemId))
            {
                return false;
            }

            Item item = Items[itemId];
            if(item.Count < count)
            {
                return false;
            }
            item.Remove(count);
            Owner.StatusManager.AddItemChange(itemId, count, StatusAction.Delete);
            Log.Info($"{Owner.Data.ID}RemoveItem:{item} RemoveCount:{count}");
            //   DBService.Instance.Save();
            return true;
        }

        public void GetItemInfos(List<NItemInfo> list)
        {
            foreach (var item in Items)
            {
                list.Add(new NItemInfo() { Id = item.Value.ItemId, Count = item.Value.Count});
            }
        }
    }
}
