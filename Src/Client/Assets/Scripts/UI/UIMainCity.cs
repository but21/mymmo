using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Models;

public class UIMainCity : MonoBehaviour
{
    public Text Name;
    public Text Level;

    void Start()
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
}
