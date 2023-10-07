using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.UI.Tab
{

    public class TabView : MonoBehaviour
    {
        public TabButton[] TabButtons;
        public GameObject[] TabPages;

        public UnityAction<int> OnTabSelect;

        public int index = -1;
        IEnumerator Start()
        {
            for (int i = 0; i < TabButtons.Length; i++)
            {
                TabButtons[i].TabView = this;
                TabButtons[i].TabIndex = i;
            }

            yield return new WaitForEndOfFrame();

            SelectTab(0);
        }

        public void SelectTab(int index)
        {
            if (this.index != index)
            {
                for (int i = 0; i < TabButtons.Length; i++)
                {
                    TabButtons[i].Select(i == index);
                    if (i < TabPages.Length - 1)
                        TabPages[i].SetActive(i == index);
                }
                if (OnTabSelect != null)
                    OnTabSelect(index);
            }
        }
    }

}
