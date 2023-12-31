﻿using Common.Data;
using GameServer.Core;
using GameServer.Managers;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Entities
{
    class Character : CharacterBase
    {

        public TCharacter Data;

        public ItemManager ItemManager;
        public StatusManager StatusManager;
        public QuestManager QuestManager;

        public Character(CharacterType type, TCharacter cha) :
            base(new Core.Vector3Int(cha.MapPosX, cha.MapPosY, cha.MapPosZ), new Core.Vector3Int(100, 0, 0))
        {
            this.Data = cha;
            this.Info = new NCharacterInfo();
            this.Info.Type = type;
            this.Info.Id = cha.ID;
            this.Info.Name = cha.Name;
            this.Info.Level = 10;//cha.Level;
            this.Info.Tid = cha.TID;
            this.Info.Class = (CharacterClass)cha.Class;
            this.Info.mapId = cha.MapID;
            this.Info.Entity = this.EntityData;
            this.Info.Gold = cha.Gold;

            QuestManager = new QuestManager(this);
            QuestManager.GetQuestInfos(this.Info.Quests);

            this.Define = DataManager.Instance.Characters[this.Info.Tid];

            ItemManager = new ItemManager(this);
            ItemManager.GetItemInfos(Info.Items);

            Info.Bag = new NBagInfo();
            Info.Bag.Items = this.Data.Bag.Items;
            Info.Bag.Unlocked = Data.Bag.Unlocked;
            Info.Equips = Data.Equips;

            StatusManager = new StatusManager(this);
        }

        public long Gold
        {
            get { return this.Data.Gold; }
            set
            {
                if (this.Data.Gold == value)
                {
                    return;
                }
                this.StatusManager.AddGoldChange((int)(value - Data.Gold));
                Data.Gold = value;
            }
        }
    }
}
