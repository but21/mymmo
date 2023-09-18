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
        public ItemDefine ItemInfo;
        public EquipDefine EquipInfo;


        public Item(NItemInfo item) : this(item.Id, item.Count)
        {
        }

        public Item(int id, int count)
        {
            Id = id;
            Count = count;
            DataManager.Instance.Items.TryGetValue(id, out ItemInfo);
            DataManager.Instance.Equips.TryGetValue(id, out EquipInfo);

        }

        public override string ToString()
        {
            return string.Format($"Id:{Id}, Count:{Count}");
        }
    }
}
