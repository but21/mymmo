using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharacterView : MonoBehaviour
{
    public GameObject[] Characters;

    private int _currentCharacter = 0;
    public int CurrentCharacter
    {
        get
        {
            return _currentCharacter;
        }
        set
        {
            _currentCharacter = value;
            UpdateCharacter();
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateCharacter()
    {
        for (int i = 0; i < 3; i++)
        {
            Characters[i].SetActive(i == _currentCharacter);
        }
    }
}
