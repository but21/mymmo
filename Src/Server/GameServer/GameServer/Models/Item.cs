using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Models
{
    class Item
    {
        public TCharacterItem dbItem;

        public int ItemId;

        public int Count;


        public Item(TCharacterItem item)
        {
            dbItem = item;
            ItemId = (short)item.ItemId;
            Count = (short)item.ItemCount;
        }

        public void Add(int count)
        {
            Count += count;
            dbItem.ItemCount = Count;
        }

        public void Remove(int count)
        {
            Count -= count;
            dbItem.ItemCount = Count;
        }

        public bool Use(int count = 1)
        {
            return false;
        }

        public override string ToString()
        {
            return string.Format($"ID:{ItemId} Count:{Count}");
        }
    }
}
