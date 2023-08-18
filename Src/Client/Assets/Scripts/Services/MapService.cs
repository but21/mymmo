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

namespace Services
{
    class MapService : Singleton<MapService>, IDisposable
    {
        public MapService()
        {

            MessageDistributer.Instance.Subscribe<MapCharacterEnterResponse>(OnMapCharacterEnter);
            MessageDistributer.Instance.Subscribe<MapCharacterLeaveResponse>(OnMapCharacterLeave);

        }

        public int CurrentMapId { get; private set; }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<MapCharacterEnterResponse>(OnMapCharacterEnter);
            MessageDistributer.Instance.Unsubscribe<MapCharacterLeaveResponse>(OnMapCharacterLeave);


        }

        public void Init()
        {

        }

        private void OnMapCharacterEnter(object sender, MapCharacterEnterResponse response)
        {
            Debug.Log($"OnMapCharacterEnterRequest::mapId:{response.mapId}, count:{response.Characters.Count}");
            foreach (var character in response.Characters)
            {
                if (character.Id == User.Instance.CurrentCharacter.Id)
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
        }

        private void EnterMap(int mapId)
        {
            if (DataManager.Instance.Maps.ContainsKey(mapId))
            {
                MapDefine map = DataManager.Instance.Maps[mapId];
                SceneManager.Instance.LoadScene(map.Resource);
            }
            else
            {
                Debug.LogError($"EnterMap: Map {mapId} not existed");
            }
        }
    }
}
