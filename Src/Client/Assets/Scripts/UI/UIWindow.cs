using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class UIWindow : MonoBehaviour
{
    public delegate void CloseHandler(UIWindow sender, WindowResult result);
    public event CloseHandler OnClose;

    public virtual Type Type{ get { return this.GetType(); } }

    public enum WindowResult
    {
        None = 0,
        Yes,
        No,
    }

    public void Close(WindowResult result = WindowResult.None)
    {
        UIManager.Instance.Close(Type);
        if(OnClose != null)
        {
            OnClose(this,result);
        }
        OnClose = null;
    }

    public void OnCloseClick()
    {
        Close(WindowResult.None);
    }

    public virtual void OnNoClick()
    {
        this.Close(WindowResult.No);
    }

    public virtual void OnYesClick()
    {
        this.Close(WindowResult.Yes);
    }

    void OnMouseDown()
    {
        Debug.Log($"{this.name} Clicked");
    }
}
