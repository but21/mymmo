using Common.Data;
using Managers;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Item
    {
        public int Id;
        public int Count;
        public ItemDefine Define;


        public Item(NItemInfo item) : this(item.Id, item.Count)
        {
        }

        public Item(int id, int count)
        {
            Id = id;
            Count = count;
            Define = DataManager.Instance.Items[Id];
        }

        public override string ToString()
        {
            return string.Format($"Id:{Id}, Count:{Count}");
        }
    }
}
