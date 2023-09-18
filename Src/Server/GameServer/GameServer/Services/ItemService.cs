using Common;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    class ItemService : Singleton<ItemService>
    {
        public ItemService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<ItemBuyResquest>(OnItemBuy);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<ItemEquipRequest>(OnItemEquip);
        }
        public void Init()
        {
        }

        void OnItemBuy(NetConnection<NetSession> sender, ItemBuyResquest request)
        {
            Character character = sender.Session.Character;
            Log.Info($"OnItemBuy:character:{character.Id} shop:{request.shopId} item:{request.shopItemId}");
            var result = ShopManager.Instance.BuyItem(sender, request.shopId, request.shopItemId);
            sender.Session.Response.itemBuy = new ItemBuyResponse();
            sender.Session.Response.itemBuy.Result = result;
            sender.Session.Response.itemBuy.Gold = character.Gold;
            sender.SendResponse();
        }

        void OnItemEquip(NetConnection<NetSession> sender, ItemEquipRequest request)
        {
            Character character = sender.Session.Character;
            Log.Info($"OnItemEquip:charcter:{character.Id} slot:{request.Slot} item:{request.itemId} equip:{request.isEquip}");

            var result = EquipManager.Instance.EquipItem(sender, request.Slot, request.itemId, request.isEquip);
            sender.Session.Response.itemEquip = new ItemEquipResponse();
            sender.Session.Response.itemEquip.Result = result;
            sender.SendResponse();
        }

    }
}
