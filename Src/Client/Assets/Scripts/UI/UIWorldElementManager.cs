using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Managers;
using Assets.Scripts.UI;

public class UIWorldElementManager : MonoSingleton<UIWorldElementManager>
{
    public GameObject nameBarPref;
    public GameObject npcStatusPref;

    private Dictionary<Transform, GameObject> _elementNames = new Dictionary<Transform, GameObject>();
    private Dictionary<Transform, GameObject> _elementStatus = new Dictionary<Transform, GameObject>();

    public void AddCharacterNameBar(Transform owner, Character character)
    {
        GameObject goNameBar = Instantiate(nameBarPref, transform);
        goNameBar.name = "NameBar" + character.entityId;
        goNameBar.GetComponent<UIWorldElement>().owner = owner;
        goNameBar.GetComponent<UINameBar>().character = character;
        goNameBar.SetActive(true);
        _elementNames[owner] = goNameBar;
    }

    public void RemoveCharacterNameBar(Transform owner)
    {
        if (_elementNames.ContainsKey(owner))
        {
            Destroy(_elementNames[owner]);
            _elementNames.Remove(owner);
        }
    }

    public void AddNpcQuestStatus(Transform owner, NpcQuestStatus status)
    {
        if (_elementStatus.ContainsKey(owner))
        {
            _elementStatus[owner].GetComponent<UIQuestStatus>().SetQuestStatus(status);
        }
        else
        {
            GameObject go = Instantiate(npcStatusPref, transform);
            go.name = "NpcQuestStatus" + owner.name;
            go.GetComponent<UIWorldElement>().owner = owner;
            go.GetComponent<UIQuestStatus>().SetQuestStatus(status);
            go.SetActive(true);
            _elementStatus[owner] = go;
        }
    }

    public void RemoveNpcQuestStatus(Transform owner)
    {
        if (_elementStatus.ContainsKey(owner))
        {
            Destroy(_elementStatus[owner]);
            _elementStatus.Remove(owner);
        }
    }
}
