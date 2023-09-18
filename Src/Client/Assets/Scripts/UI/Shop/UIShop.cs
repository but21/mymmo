using Common.Data;
using Managers;
using Models;
using Services;
using SkillBridge.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Shop
{
    public class UIShop : UIWindow
    {
        public Text Title;
        public Text Money;

        public GameObject ShopItem;
        ShopDefine shop;
        public Transform[] ItemRoot;



        private void Start()
        {
            ItemService.Instance.OnBuy = OnBuy;
            StartCoroutine(InitItems());
        }

        IEnumerator InitItems()
        {
            int count = 0;
            int page = 0;
            foreach (var kv in DataManager.Instance.ShopItems[shop.ID])
            {
                if (kv.Value.Status > 0)
                {
                    GameObject go = Instantiate(ShopItem, ItemRoot[page]);
                    UIShopItem ui = go.GetComponent<UIShopItem>();
                    ui.SetShopItem(kv.Key, kv.Value, this);
                    count++;
                    if (count >= 20)
                    {
                        count = 0;
                        page++;
                        ItemRoot[page].gameObject.SetActive(true);
                    }
                }
            }
            yield return null;
        }

        public void SetShop(ShopDefine shop)
        {
            this.shop = shop;
            this.Title.text = shop.Description;
            Money.text = User.Instance.CurrentCharacter.Gold.ToString();
        }

        void OnBuy(Result result, long gold)
        {
            if (result == Result.Success)
            {
                Money.text = gold.ToString();
            }
        }

        private UIShopItem _selectedItem;
        public void SelectShopItem(UIShopItem item)
        {
            if (_selectedItem != null)
            {
                _selectedItem.Selected = false;
            }
            _selectedItem = item;
        }

        public void OnClickBuy()
        {
            if (this._selectedItem == null)
            {
                MessageBox.Show($"请选择要购买的道具", "购买提示");
                return;
            }
            ItemService.Instance.SendBuyItem(shop.ID, _selectedItem.ShopItemID);
        }
    }
}
