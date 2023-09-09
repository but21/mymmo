using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Models;
using UI;

// 主UI, 进入地图后一直存在的UI, 如角色血条, 地图等
public class UIMain : MonoSingleton<UIMain>
{
    public Text Name;
    public Text Level;

    protected override void OnStart()
    {
        UpdateAvatar();
    }

    void Update()
    {

    }

    void UpdateAvatar()
    {
        Name.text = string.Format($"{User.Instance.CurrentCharacter.Name}[{User.Instance.CurrentCharacter.Id}]");
        Level.text = User.Instance.CurrentCharacter.Level.ToString();
    }

    public void BackToCharSelect()
    {
        SceneManager.Instance.LoadScene("CharSelect");
        Services.UserService.Instance.SendGameLeave();
    }

    public void OnClickTest()
    {
        UITest test = UIManager.Instance.Show<UITest>();
        test.SetTitle("UITest");
        test.OnClose += Test_OnClose;
    }

    private void Test_OnClose(UIWindow sender, UIWindow.WindowResult result)
    {
        MessageBox.Show($"result:{result}");
    }

    public void OnClickBag()
    {
        UIManager.Instance.Show<UIBag>();
    }
}
