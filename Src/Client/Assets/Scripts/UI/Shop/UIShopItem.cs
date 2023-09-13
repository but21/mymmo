using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Common.Data;
using Managers;

namespace Assets.Scripts.UI.Shop
{

    public class UIShopItem : MonoBehaviour
    {
        public Image Icon;
        public Text Title;
        public Text Count;
        public Text Price;

        public Image Background;
        public Sprite NormalBg;
        public Sprite SelectBg;

        private bool _isSelected;
        public bool Selected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                Background.overrideSprite = _isSelected ? SelectBg : NormalBg;
            }
        }


        public int ShopItemID { get; set; }
        private UIShop _shop;
        private ItemDefine _item;
        private ShopItemDefine ShopItem { get; set; }

        public void SetShopItem(int id, ShopItemDefine shopItem, UIShop owner)
        {
            this._shop = owner;
            ShopItemID = id;
            ShopItem = shopItem;
            _item = DataManager.Instance.Items[ShopItem.ItemID];

            Title.text = _item.Name;
            Count.text = ShopItem.Count.ToString();
            Price.text = ShopItem.Price.ToString();
            Icon.overrideSprite = Resloader.Load<Sprite>(_item.Icon);
        }

        public void OnSelect()
        {
            Selected = true;
            _shop.SelectShopItem(this);
        }
    }
}
