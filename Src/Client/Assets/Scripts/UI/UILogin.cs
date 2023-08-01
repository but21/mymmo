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

    // 点击登录
    public void OnClickLogin()
    {
        if (string.IsNullOrEmpty(Username.text))
        {
            MessageBox.Show("请输入用户名");
            return;
        }
        if (string.IsNullOrEmpty(Password.text))
        {
            MessageBox.Show("请输入密码");
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
            MessageBox.Show(message, "错误", MessageBoxType.Error);
        }
    }
}
