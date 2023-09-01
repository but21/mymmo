using Common.Data;
using Managers;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterObject : MonoBehaviour
{
    // ���͵��ʶ
    public int ID;

    Mesh mesh = null;
    void Start()
    {
        mesh = GetComponent<MeshFilter>().sharedMesh;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerInputController playerInputController = other.GetComponent<PlayerInputController>();
        if (playerInputController != null)
        {
            TeleporterDefine td = DataManager.Instance.Teleporters[this.ID];
            if (td == null)
            {
                Debug.Log($"TeleporterObject: Character:[{playerInputController.character.Info.Name}] Enter Teleporter:[{ID}], But TeleporterDefine not existed");
                return;
            }
            Debug.Log($"TeleporterObject: Character:[{playerInputController.character.Info.Name}] Enter Teleporter: [{td.ID}:{td.Name}]");
            if (td.LinkTo > 0)
            {
                if (DataManager.Instance.Teleporters.ContainsKey(td.LinkTo))
                {
                    MapService.Instance.SendMapTeleporter(ID);
                }
                else
                {
                    Debug.Log($"Teleporter ID:{td.ID} LinkID:{td.LinkTo} error!");
                }
            }
        }
    }
    // ���ڱ༭��ģʽ����Ч
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        if (mesh != null)
        {
            Gizmos.DrawWireMesh(mesh, this.transform.position + Vector3.up * transform.localScale.y * .5f, transform.rotation, transform.localScale);
        }
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.ArrowHandleCap(0, transform.position, transform.rotation, 1f, EventType.Repaint);
    }
#endif
}
