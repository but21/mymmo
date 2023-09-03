using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Data;
using UnityEngine;

namespace Managers
{
    class TestManager :Singleton<TestManager>
    {
        public void Init()
        {
            NpcManager.Instance.RegisterNpcEvent(NpcDefine.NpcFunction.InvokeShop, OnNpcInvokeShop);
            NpcManager.Instance.RegisterNpcEvent(NpcDefine.NpcFunction.InvokeInsrance, OnNpcInvokeInsrance);
        }

        private bool OnNpcInvokeShop(NpcDefine npc)
        {
            Debug.Log($"TestManager.OnNpcInvokeShop:Npc:{npc.Name}, {npc.ID} Type:{npc.Type} Func:{npc.Function}");
            UITest test = UIManager.Instance.Show<UITest>();
            test.SetTitle(npc.Name);
            return true;
        }

        private bool OnNpcInvokeInsrance(NpcDefine npc)
        {
            Debug.Log($"TestManager.OnNpcInvokeInsrance:Npc:{npc.Name}, {npc.ID} Type:{npc.Type} Func:{npc.Function}");
            MessageBox.Show($"点击了Npc:{npc.Name} [{npc.Function}]");
            return true;
        }

    }
}
