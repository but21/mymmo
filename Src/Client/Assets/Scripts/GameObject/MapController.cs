using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

// 每次地图加载的时候执行
public class MapController : MonoBehaviour
{
    public Collider minimapBoundingBox;

    void Start()
    {
        MinimapManager.Instance.UpdateMinimap(minimapBoundingBox);
    }

}
