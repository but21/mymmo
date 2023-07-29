using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Services;

namespace Assets.Scripts.UI
{
    public class UIRegister : MonoBehaviour
    {
        public InputField Username;
        public InputField Password;
        public InputField PasswordConfirm;

        private void Start()
        {

        }

        private void Update()
        {

        }

        // 点击注册
        public void OnClickRegister()
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
            if (string.IsNullOrEmpty(PasswordConfirm.text))
            {
                MessageBox.Show("请输入确认密码");
                return;
            }
            if (Password.text != PasswordConfirm.text)
            {
                MessageBox.Show("两次密码不一致");
                return;
            }

            // 注册验证
            UserService.Instance.SendRegister(Username.text, Password.text);
        }
    }
}