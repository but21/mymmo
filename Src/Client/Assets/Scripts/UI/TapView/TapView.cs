using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapView : MonoBehaviour
{
    public TapButton[] TapButtons;
    public GameObject[] TapPages;

    public int index = -1;
    IEnumerator Start()
    {
        for (int i = 0; i < TapButtons.Length; i++)
        {
            TapButtons[i].TapView = this;
            TapButtons[i].TapIndex = i;
        }

        yield return new WaitForEndOfFrame();

        SelectTap(0);
    }

    public void SelectTap(int index)
    {
        if(this.index != index)
        {
            for (int i = 0; i < TapButtons.Length; i++)
            {
                TapButtons[i].Select(i == index);
                TapPages[i].SetActive(i == index);
            }
        }
        this.index = index;
    }
    
    void Update()
    {

    }
}
