using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Models;

namespace GameServer.Managers
{
    class MapManager : Singleton<MapManager>
    {
        Dictionary<int, Map> Maps = new Dictionary<int, Map>();


        public void Init()
        {
            foreach (var mapDefine in DataManager.Instance.Maps.Values)
            {
                Map map = new Map(mapDefine);
                Log.InfoFormat($"MapManager.Init > Map:{map.Define.ID}:{map.Define.Name}");
                Maps[mapDefine.ID] = map;
            }
        }

        public Map this[int key]
        {
            get
            {
                return this.Maps[key];
            }
        }

        public void Update()
        {
            foreach (var map in this.Maps.Values)
            {
                map.Update();
            }
        }
    }
}
