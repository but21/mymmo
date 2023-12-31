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
        MinimapManager.Instance.minimap = this;
        UpdateMap();
    }

    void Update()
    {
        if (_playerTransform == null)
        {
            _playerTransform = MinimapManager.Instance.PlayerTransform;
        }
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


    }

    public void UpdateMap()
    {
        // 设置小地图名
        MapName.text = User.Instance.CurrentMapData.Name;

        // 加载小地图图片
        Minimap.overrideSprite = MinimapManager.Instance.LoadCurrentMinimap();
        Minimap.SetNativeSize();

        // 将小地图位置设置为原点
        Minimap.transform.localPosition = Vector3.zero;

        // 更新包围盒
        MinimapBoundingBox = MinimapManager.Instance.MinimapBoundingBox;

        // 更新角色位置信息, 以更新角色光标在小地图中的位置
        _playerTransform = null;

    }
}
