using Managers;
using Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    class UIBag : UIWindow
    {
        public Text Money;
        public Transform[] pages;
        public GameObject BagItem;

        List<Image> slots;

        private void Start()
        {
            if (slots == null)
            {
                slots = new List<Image>();
                for (int page = 0; page < pages.Length; page++)
                {
                    slots.AddRange(pages[page].GetComponentsInChildren<Image>(true));
                }
            }
            StartCoroutine(InitBags());
        }
        private void Update()
        {

        }

        IEnumerator InitBags()
        {
            for (int i = 0; i < BagManager.Instance.Items.Length; i++)
            {
                /* 
                 * 取出item,
                 * 将BagItem实例化, 并将父节点设置为slots
                 * 得到ui图标和item在配置表中的数据
                 * 设置ui图标
                 */
                var item = BagManager.Instance.Items[i];

                // 格子是否为空
                if (item.ItemId > 0)
                {
                    GameObject go = Instantiate(BagItem, slots[i].transform);
                    var ui = go.GetComponent<UIIconItem>();
                    var define = ItemManager.Instance.Items[item.ItemId].Define;
                    ui.SetMainIcon(define.Icon, item.Count.ToString());
                }
            }

            /* 
             * 判断背包中已经有了多少个道具
             * 将未解锁的背包格子设置成灰色
             */
            for (int i = BagManager.Instance.Items.Length; i < slots.Count; i++)
            {
                slots[i].color = Color.gray;
            }
            yield return null;
        }

        public void SetTitle(string title)
        {
            Money.text = User.Instance.CurrentCharacter.Id.ToString();
        }

        public void OnReset()
        {
            BagManager.Instance.Reset();
        }

    }
}
