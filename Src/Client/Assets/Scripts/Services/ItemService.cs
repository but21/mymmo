using Network;
using System;
using SkillBridge.Message;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Models;
using Common.Data;
using Managers;

namespace Services
{
    public class ItemService : Singleton<ItemService>, IDisposable
    {
        public UnityEngine.Events.UnityAction<Result, long> OnBuy;
        public ItemService()
        {
            MessageDistributer.Instance.Subscribe<ItemBuyResponse>(OnItemBuy);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<ItemBuyResponse>(OnItemBuy);
        }

        public void SendBuyItem(int shopId, int shopItemId)
        {
            Debug.Log($"SendBugItemRequest");

            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.itemBuy = new ItemBuyResquest();
            message.Request.itemBuy.shopId = shopId;
            message.Request.itemBuy.shopItemId = shopItemId;
            NetClient.Instance.SendMessage(message);
        }

        private void OnItemBuy(object sender, ItemBuyResponse response)
        {
            if (OnBuy != null)
            {
                OnBuy(response.Result, response.Gold);
            }
            MessageBox.Show($"购买结果:{response.Result}{response.Errormsg}, \n购买完成");
        }
    }
}
