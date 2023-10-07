using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Tab
{

    public class TabButton : MonoBehaviour
    {
        public Sprite NormalIamge;
        public Sprite ActiveImage;

        public TabView TabView;
        public int TabIndex = 0;
        public bool Selected = false;

        private Image _tapImage;

        void Start()
        {
            _tapImage = GetComponent<Image>();
            NormalIamge = _tapImage.sprite;
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        public void Select(bool select)
        {
            _tapImage.overrideSprite = select ? ActiveImage : NormalIamge;
        }

        void OnClick()
        {
            TabView.SelectTab(TabIndex);
        }

        void Update()
        {

        }
    }
}