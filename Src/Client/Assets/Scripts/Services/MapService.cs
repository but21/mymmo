using Network;
using SkillBridge.Message;
using System;
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
    class MapService : Singleton<MapService>, IDisposable
    {
        public MapService()
        {

            MessageDistributer.Instance.Subscribe<MapCharacterEnterResponse>(OnMapCharacterEnter);
            MessageDistributer.Instance.Subscribe<MapCharacterLeaveResponse>(OnMapCharacterLeave);
            MessageDistributer.Instance.Subscribe<MapEntitySyncResponse>(OnMapEntitySync);
        }

        public int CurrentMapId = 0;

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<MapCharacterEnterResponse>(OnMapCharacterEnter);
            MessageDistributer.Instance.Unsubscribe<MapCharacterLeaveResponse>(OnMapCharacterLeave);
            MessageDistributer.Instance.Unsubscribe<MapEntitySyncResponse>(OnMapEntitySync);
        }

        public void Init()
        {

        }

        private void OnMapCharacterEnter(object sender, MapCharacterEnterResponse response)
        {
            Debug.Log($"OnMapCharacterEnter::mapId:{response.mapId}, count:{response.Characters.Count}");
            foreach (var character in response.Characters)
            {
                if (User.Instance.CurrentCharacter == null || character.Id == User.Instance.CurrentCharacter.Id)
                {
                    // 刷新本地数据
                    User.Instance.CurrentCharacter = character;
                }
                CharacterManager.Instance.AddCharacter(character);

            }
            if (CurrentMapId != response.mapId)
            {
                EnterMap(response.mapId);
                CurrentMapId = response.mapId;
            }
        }

        private void OnMapCharacterLeave(object sender, MapCharacterLeaveResponse response)
        {
            Debug.Log($"OnMapCharacterLeave: CharacterID:{response.characterId}");
            if (response.characterId != User.Instance.CurrentCharacter.Id)
            {
                CharacterManager.Instance.RemoveCharacter(response.characterId);
            }
            else
            {
                CharacterManager.Instance.Clear();
            }
        }

        private void EnterMap(int mapId)
        {
            if (DataManager.Instance.Maps.ContainsKey(mapId))
            {
                MapDefine map = DataManager.Instance.Maps[mapId];
                User.Instance.CurrentMapData = map;
                SceneManager.Instance.LoadScene(map.Resource);
            }
            else
            {
                Debug.LogError($"EnterMap: Map {mapId} not existed");
            }
        }

        public void SendMapEntitySync(EntityEvent entityEvent, NEntity entity)
        {
            Debug.Log($"MapEntitySyncRequest :ID:{entity.Id} POS:{entity.Position.String()} DIR:{entity.Direction.String()} SPEED:{entity.Speed}");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.mapEntitySync = new MapEntitySyncRequest();
            message.Request.mapEntitySync.entitySync = new NEntitySync()
            {
                Id = entity.Id,
                Event = entityEvent,
                Entity = entity,
            };
            NetClient.Instance.SendMessage(message);
        }

        private void OnMapEntitySync(object sender, MapEntitySyncResponse response)
        {
            foreach (var entity in response.entitySyncs)
            {
                EntityManager.Instance.OnEntitySync(entity);
            }
        }

        internal void SendMapTeleporter(int teleporterID)
        {
            Debug.LogFormat($"MapTeleporterRequest :teleporterID:{teleporterID}");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.mapTeleport = new MapTeleportRequest();
            message.Request.mapTeleport.teleporterId = teleporterID;
            NetClient.Instance.SendMessage(message);
        }

    }
}
