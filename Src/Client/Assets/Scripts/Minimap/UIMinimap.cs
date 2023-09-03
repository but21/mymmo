using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Models;
using Managers;

public class UIMinimap : MonoBehaviour
{
    // С��ͼ
    public Image Minimap;
    // ���ڵ�����ͼ��
    public Image Arrow;
    // ��ͼ��
    public Text MapName;
    // ��ȡ��ͼ��С
    public Collider MinimapBoundingBox;
    // ��ɫTransform
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

        // ��ȡ��ʵ��ͼ�Ŀ�Ⱥ͸߶�
        float realWidth = MinimapBoundingBox.bounds.size.x;
        float realHeight = MinimapBoundingBox.bounds.size.z;

        // ��ɫ���ͼ�����λ��
        float relationX = _playerTransform.position.x - MinimapBoundingBox.bounds.min.x;
        float relationZ = _playerTransform.position.z - MinimapBoundingBox.bounds.min.z;

        // ����ɫλ������Ϊ��ͼ���ĵ�
        float pivotX = relationX / realWidth;
        float pivotZ = relationZ / realHeight;

        Minimap.rectTransform.pivot = new Vector2(pivotX, pivotZ);
        Minimap.rectTransform.localPosition = Vector2.zero;
        Arrow.transform.eulerAngles = new Vector3(0, 0, -_playerTransform.eulerAngles.y);


    }

    public void UpdateMap()
    {
        // ����С��ͼ��
        MapName.text = User.Instance.CurrentMapData.Name;

        // ����С��ͼͼƬ
        Minimap.overrideSprite = MinimapManager.Instance.LoadCurrentMinimap();
        Minimap.SetNativeSize();

        // ��С��ͼλ������Ϊԭ��
        Minimap.transform.localPosition = Vector3.zero;

        // ���°�Χ��
        MinimapBoundingBox = MinimapManager.Instance.MinimapBoundingBox;

        // ���½�ɫλ����Ϣ, �Ը��½�ɫ�����С��ͼ�е�λ��
        _playerTransform = null;

    }
}
