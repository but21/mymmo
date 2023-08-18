using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorldElementManager : MonoSingleton<UIWorldElementManager>
{
    public GameObject nameBarPref;

    private Dictionary<Transform, GameObject> _elements = new Dictionary<Transform, GameObject>();

    void Start()
    {

    }

    void Update()
    {

    }

    public void AddCharacterNameBar(Transform owner, Character character)
    {
        GameObject goNameBar = Instantiate(nameBarPref, transform);
        goNameBar.name = "NameBar" + character.entityId;
        goNameBar.GetComponent<UIWorldElement>().owner = owner;
        goNameBar.GetComponent<UINameBar>().character = character;
        goNameBar.SetActive(true);
        _elements[owner] = goNameBar;
    }

    public void RemoveCharacterNameBar(Transform owner)
    {
        if (_elements.ContainsKey(owner))
        {
            Destroy(_elements[owner]);
            _elements.Remove(owner);
        }
    }
}
