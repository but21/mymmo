using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;

public class UILogin : MonoBehaviour
{
    public InputField Username;
    public InputField Password;

    // Start is called before the first frame update
    void Start()
    {

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
}
