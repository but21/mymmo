using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;

// ÿ�ε�ͼ���ص�ʱ��ִ��
public class MapController : MonoBehaviour
{
    public Collider minimapBoundingBox;

    void Start()
    {
        MinimapManager.Instance.UpdateMinimap(minimapBoundingBox);
    }

}
