﻿using GameServer.Entities;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Managers
{
    class StatusManager
    {
        Character Owner;
        private List<NStatus> Status { get; set; }

        public bool HasStatus
        {
            get { return Status.Count > 0; }
        }

        public StatusManager(Character owner)
        {
            this.Owner = owner;
            Status = new List<NStatus>();
        }

        public void AddStatus(StatusType type, int id, int value, StatusAction action)
        {
            Status.Add(new NStatus() { Type = type, Id = id, Value = value, Action = action });
        }

        public void AddGoldChange(int goldDelta)
        {
            if(goldDelta > 0)
            {
                this.AddStatus(StatusType.Money, 0, goldDelta, StatusAction.Add);
            }
            if(goldDelta < 0)
            {
                AddStatus(StatusType.Money, 0, -goldDelta, StatusAction.Delete);
            }
        }

        public void AddItemChange(int id, int count, StatusAction action)
        {
            AddStatus(StatusType.Item, id, count, action);
        }

        public void ApplyResponse(NetMessageResponse message)
        {
            if(message.statusNotify == null)
            {
                message.statusNotify = new StatusNotify();
            }
            foreach (var status in Status)
            {
                message.statusNotify.Status.Add(status);
            }
            Status.Clear();
        }
    }
}
