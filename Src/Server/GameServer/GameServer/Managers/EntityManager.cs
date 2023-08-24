﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Entities;

namespace GameServer.Managers
{

    class EntityManager : Singleton<EntityManager>
    {
        private int index;
        public List<Entity> AllEntities = new List<Entity>();
        public Dictionary<int, List<Entity>> MapEntities = new Dictionary<int, List<Entity>>();


        public void AddEntity(int mapId, Entity entity)
        {
            AllEntities.Add(entity);
            // 加入管理器生成唯一ID
            entity.EntityData.Id = ++index;
            List<Entity> entities = null;
            if(!MapEntities.TryGetValue(mapId, out entities))
            {
                entities = new List<Entity>();
                MapEntities[mapId] = entities;
            }
            entities.Add(entity);
        }

        public void RemoveEntity(int mapId, Entity entity)
        {
            AllEntities.Remove(entity);
            MapEntities[mapId].Remove(entity);
        }
    }
}
