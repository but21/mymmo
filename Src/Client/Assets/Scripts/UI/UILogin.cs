using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;
using SkillBridge.Message;

public class UILogin : MonoBehaviour
{
    public InputField Username;
    public InputField Password;

    // Start is called before the first frame update
    void Start()
    {
        UserService.Instance.OnLogin = OnLogin;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // �����¼
    public void OnClickLogin()
    {
        if (string.IsNullOrEmpty(Username.text))
        {
            MessageBox.Show("�������û���");
            return;
        }
        if (string.IsNullOrEmpty(Password.text))
        {
            MessageBox.Show("����������");
            return;
        }

        UserService.Instance.SendLogin(Username.text, Password.text);
    }

    void OnLogin(Result result, string message)
    {
        if(result == Result.Success)
        {
            SceneManager.Instance.LoadScene("CharSelect");
        }
        else
        {
            MessageBox.Show(message, "����", MessageBoxType.Error);
        }
    }
}
