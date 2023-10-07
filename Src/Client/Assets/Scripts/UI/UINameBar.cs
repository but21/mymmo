using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UINameBar : MonoBehaviour
{

    //public Image Avatar;
    public Text AvatarName;

    public Character character;

    // Start is called before the first frame update
    void Start()
    {
        if (character != null)
        {
/*            if(character.Info.Type == SkillBridge.Message.CharacterType.Monster)
            {
                Avatar.gameObject.SetActive(false);
            }
            else{
                Avatar.gameObject.SetActive(true);
            }*/
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInfo();

        // ʹ��ɫ��Ϣ����ǰ�������������ǰ��
        //transform.forward = Camera.main.transform.forward;
    }

    // ���½�ɫͷ����Ϣ
    void UpdateInfo()
    {
        string info = this.character.Name + " Lv." + this.character.Info.Level;

        if (info != AvatarName.text)
        {
            AvatarName.text = info;
        }
    }
}
