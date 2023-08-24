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
        InitMap();
    }

    void Update()
    {
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

        _playerTransform = User.Instance.CurrentCharacterObject.transform;

    }

    void InitMap()
    {
        // ����С��ͼ��
        MapName.text = User.Instance.CurrentMapData.Name;
        if (Minimap.overrideSprite == null)
        {
            // ����С��ͼͼƬ
            Minimap.overrideSprite = MinimapManager.Instance.LoadMinimap();
        }
        Minimap.SetNativeSize();
        // ��С��ͼλ������Ϊԭ��
        Minimap.transform.localPosition = Vector3.zero;
        // ��ɫTransform��ֵ
        //_playerTransform = User.Instance.CurrentCharacterObject.transform;

    }
}
