using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;

namespace Managers
{
    public class MinimapManager : Singleton<MinimapManager>
    {
        void Start()
        {

        }

        void Update()
        {

        }

        // ���ص�ǰ��ͼ��С��ͼ
        public Sprite LoadMinimap()
        {
            Debug.Log($"User.Instance.CurrentMapData.Minimap:{User.Instance.CurrentMapData.Minimap}");
            return Resloader.Load<Sprite>("UI/Minimap/" + User.Instance.CurrentMapData.Minimap);
        }
    }
}

