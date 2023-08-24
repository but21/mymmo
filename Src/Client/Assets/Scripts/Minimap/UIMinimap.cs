using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Models;
using Managers;

public class UIMinimap : MonoBehaviour
{
    // 小地图
    public Image Minimap;
    // 用于导航的图标
    public Image Arrow;
    // 地图名
    public Text MapName;
    // 获取地图大小
    public Collider MinimapBoundingBox;
    // 角色Transform
    private Transform _playerTransform;
    void Start()
    {
        InitMap();
    }

    void Update()
    {
        if (MinimapBoundingBox == null || _playerTransform == null) return;

        // 获取真实地图的宽度和高度
        float realWidth = MinimapBoundingBox.bounds.size.x;
        float realHeight = MinimapBoundingBox.bounds.size.z;

        // 角色与地图的相对位置
        float relationX = _playerTransform.position.x - MinimapBoundingBox.bounds.min.x;
        float relationZ = _playerTransform.position.z - MinimapBoundingBox.bounds.min.z;

        // 将角色位置设置为地图中心点
        float pivotX = relationX / realWidth;
        float pivotZ = relationZ / realHeight;

        Minimap.rectTransform.pivot = new Vector2(pivotX, pivotZ);
        Minimap.rectTransform.localPosition = Vector2.zero;
        Arrow.transform.eulerAngles = new Vector3(0, 0, -_playerTransform.eulerAngles.y);

        _playerTransform = User.Instance.CurrentCharacterObject.transform;

    }

    void InitMap()
    {
        // 设置小地图名
        MapName.text = User.Instance.CurrentMapData.Name;
        if (Minimap.overrideSprite == null)
        {
            // 加载小地图图片
            Minimap.overrideSprite = MinimapManager.Instance.LoadMinimap();
        }
        Minimap.SetNativeSize();
        // 将小地图位置设置为原点
        Minimap.transform.localPosition = Vector3.zero;
        // 角色Transform赋值
        //_playerTransform = User.Instance.CurrentCharacterObject.transform;

    }
}
