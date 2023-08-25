using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;

namespace Managers
{
    public class MinimapManager : Singleton<MinimapManager>
    {
        public Transform PlayerTransform
        {
            get
            {
                if(User.Instance.CurrentCharacterObject == null)
                {
                    return null;
                }
                return User.Instance.CurrentCharacterObject.transform;
            }
        }

        // 加载当前地图的小地图
        public Sprite LoadCurrentMinimap()
        {
            Debug.Log($"User.Instance.CurrentMapData.Minimap:{User.Instance.CurrentMapData.Minimap}");
            return Resloader.Load<Sprite>("UI/Minimap/" + User.Instance.CurrentMapData.Minimap);
        }
    }
}

