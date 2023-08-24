using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINameBar : MonoBehaviour
{
    public Text AvatarName;

    public Character character;

    // Start is called before the first frame update
    void Start()
    {
        if (character != null)
        {
            UpdateInfo();
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInfo();

        // 使角色信息条的前方等于摄像机的前方
        transform.forward = Camera.main.transform.forward;
    }

    // 更新角色头顶信息
    void UpdateInfo()
    {
        string info = this.character.Name + " Lv." + this.character.Info.Level;

        if (info != AvatarName.text)
        {
            AvatarName.text = info;
        }
    }
}
