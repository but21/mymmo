using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIIconItem : MonoBehaviour
{
    public Image MainIcon;
    public Image SecondIcon;

    public Text MainText;

    void Start()
    {

    }

    void Update()
    {

    }

    public void SetMainIcon(string iconName, string text)
    {
        MainIcon.overrideSprite = Resloader.Load<Sprite>(iconName);
        MainText.text = text;
    }
}
