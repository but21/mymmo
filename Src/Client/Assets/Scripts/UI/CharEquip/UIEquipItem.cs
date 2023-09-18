using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Models;
using UnityEngine.EventSystems;
using Managers;

namespace Assets.Scripts.UI
{
    public class UIEquipItem : MonoBehaviour, IPointerClickHandler
    {
        public Image Icon;
        public Text Title;
        public Text Level;
        public Text LimitClass;
        public Text LimitCategory;

        public Image Background;
        public Sprite NormalBg;
        public Sprite SelectdBg;

        private bool _selected;
        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                Background.overrideSprite = _selected ? SelectdBg : NormalBg;
            }
        }

        public int Index { get; set; }
        private UICharEquip _owner;
        private Item _item;
        bool isEquiped = false;


        public void SetEquipItem(int id, Item item, UICharEquip owner, bool isEquiped)
        {
            this._owner = owner;
            this.Index = id;
            this._item = item;
            this.isEquiped = isEquiped;

            if (this.Title != null) { Title.text = item.ItemInfo.Name; }

            if (this.Level != null) { Level.text = item.ItemInfo.Level.ToString(); }

            if (LimitClass != null) { LimitClass.text = item.ItemInfo.LimitClass.ToString(); }

            if (LimitCategory != null) { LimitCategory.text = item.ItemInfo.Category; }

            if (Icon != null) { Icon.overrideSprite = Resloader.Load<Sprite>(item.ItemInfo.Icon); }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (isEquiped)
            {
                UnEquip();
            }
            else
            {
                if (_selected)
                {
                    DoEquip();
                    Selected = false;
                }
                else
                {
                    Selected = true;
                }
            }
        }

        void DoEquip()
        {
            var msg = MessageBox.Show(string.Format($"要装备{_item.ItemInfo.Name}"), "确认", MessageBoxType.Confirm);
            msg.OnYes = () =>
            {
                var oldEquip = EquipManager.Instance.GetEquip(_item.EquipInfo.Slot);
                if (oldEquip != null)
                {
                    var newMsg = MessageBox.Show(string.Format($"要替换掉{oldEquip.ItemInfo.Name}"), "确认", MessageBoxType.Confirm);
                    newMsg.OnYes = () =>
                    {
                        _owner.DoEquip(_item);
                    };
                }
                else
                {
                    _owner.DoEquip(_item);
                }
            };
        }

        void UnEquip()
        {
            var msg = MessageBox.Show(string.Format($"要取下装备{_item.ItemInfo.Name}吗?"), "确认", MessageBoxType.Confirm);
            msg.OnYes = () =>
            {
                _owner.UnEquip(_item);
            };
        }
    }
}
