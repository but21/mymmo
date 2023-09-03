using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class UITest : UIWindow
{
    public Text Title;

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void SetTitle(string title)
    {
        Title.text = title;
    }
}
