using Common;
using Common.Data;
using GameServer.Entities;
using GameServer.Managers;
using Network;
using SkillBridge.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    class MapService : Singleton<MapService>
    {
        public MapService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapEntitySyncRequest>(OnMapEntitySync);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<MapTeleportRequest>(OnMapTeleport);
        }



        public void Init()
        {
            MapManager.Instance.Init();
        }
            
        private void OnMapEntitySync(NetConnection<NetSession> sender, MapEntitySyncRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat($"OnMapEntitySync: characterID:{character.Id}:{character.Info.Name} EntityID:{request.entitySync.Id} Event:{request.entitySync.Event} Entity:{request.entitySync.Entity.String()}");
            MapManager.Instance[character.Info.mapId].UpdateEntity(request.entitySync);
        }

        internal void SendEntityUpdate(NetConnection<NetSession> connection, NEntitySync entity)
        {
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.mapEntitySync = new MapEntitySyncResponse();
            message.Response.mapEntitySync.entitySyncs.Add(entity);

            byte[] data = PackageHandler.PackMessage(message);
            connection.SendData(data, 0, data.Length);
        }

        private void OnMapTeleport(NetConnection<NetSession> sender, MapTeleportRequest request)
        {
            Character character = sender.Session.Character;
            Log.InfoFormat($"OnMapTeleport: characterID:[{character.Id}:{character.Data}] TeleporterID:{request.teleporterId}");

            if (!DataManager.Instance.Teleporters.ContainsKey(request.teleporterId))
            {
                Log.Warning($"Source TeleporterID:[{request.teleporterId} not existed]");
                return;
            }

            TeleporterDefine source = DataManager.Instance.Teleporters[request.teleporterId];
           if(source.LinkTo == 0 || !DataManager.Instance.Teleporters.ContainsKey(source.LinkTo))
            {
                Log.Warning($"Source TeleporterID : [{request.teleporterId}] LinkTo ID:[{source.LinkTo}] not existed!");
                return;
            }

            TeleporterDefine target = DataManager.Instance.Teleporters[source.LinkTo];

            MapManager.Instance[source.MapID].CharacterLeave(character);
            character.Position = target.Position;
            character.Direction = target.Direction;
            MapManager.Instance[target.MapID].CharacterEnter(sender, character);
        }
    }
}
