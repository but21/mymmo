using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkillBridge.Message;

using Common;
using Common.Data;

using Network;
using GameServer.Managers;
using GameServer.Entities;
using GameServer.Services;

namespace GameServer.Models
{
    class Map
    {
        internal class MapCharacter
        {
            public NetConnection<NetSession> connection;
            public Character character;

            public MapCharacter(NetConnection<NetSession> conn, Character cha)
            {
                this.connection = conn;
                this.character = cha;
            }
        }

        public int ID
        {
            get { return this.Define.ID; }
        }
        internal MapDefine Define;

        Dictionary<int, MapCharacter> MapCharacters = new Dictionary<int, MapCharacter>();


        internal Map(MapDefine define)
        {
            this.Define = define;
        }

        internal void Update()
        {
        }

        /// <summary>
        /// 角色进入地图
        /// </summary>
        /// <param name="character"></param>
        internal void CharacterEnter(NetConnection<NetSession> sender, Character character)
        {
            Log.InfoFormat("CharacterEnter: Map:{0} characterId:{1}", this.Define.ID, character.Id);

            character.Info.mapId = this.ID;



            sender.Session.Response.mapCharacterEnter = new MapCharacterEnterResponse();
            sender.Session.Response.mapCharacterEnter.mapId = this.Define.ID;
            sender.Session.Response.mapCharacterEnter.Characters.Add(character.Info);

            this.MapCharacters[character.Id] = new MapCharacter(sender, character);

            foreach (var kv in this.MapCharacters)
            {
                sender.Session.Response.mapCharacterEnter.Characters.Add(kv.Value.character.Info);
                this.SendCharacterEnterMap(kv.Value.connection, character.Info);
            }


            sender.SendResponse();
        }

        void SendCharacterEnterMap(NetConnection<NetSession> sender, NCharacterInfo character)
        {


            sender.Session.Response.mapCharacterEnter = new MapCharacterEnterResponse();
            sender.Session.Response.mapCharacterEnter.mapId = this.Define.ID;
            sender.Session.Response.mapCharacterEnter.Characters.Add(character);

            sender.SendResponse();
        }

        internal void CharacterLeave(Character character)
        {
            Log.InfoFormat($"CharacterLeave: Map:{Define.ID} characterID:{character.Id}");

            foreach (var kv in MapCharacters)
            {
                SendCharacterLeaveMap(kv.Value.connection, character);
            }

            MapCharacters.Remove(character.Id);
        }

        private void SendCharacterLeaveMap(NetConnection<NetSession> sender, Character character)
        {

            sender.Session.Response.mapCharacterLeave = new MapCharacterLeaveResponse();
            sender.Session.Response.mapCharacterLeave.characterId = character.Id;

            sender.SendResponse();
        }


        internal void UpdateEntity(NEntitySync entity)
        {
            foreach (var kv in MapCharacters)
            {
                if (kv.Value.character.entityId == entity.Id)
                {
                    kv.Value.character.Position = entity.Entity.Position;
                    kv.Value.character.Direction = entity.Entity.Direction;
                    kv.Value.character.Speed = entity.Entity.Speed;
                }
                else
                {
                    MapService.Instance.SendEntityUpdate(kv.Value.connection, entity);
                }
            }
        }
    }
}
