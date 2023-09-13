using Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Models
{
    class User : Singleton<User>
    {
        SkillBridge.Message.NUserInfo userInfo;


        public SkillBridge.Message.NUserInfo Info
        {
            get { return userInfo; }
        }


        public void SetupUserInfo(SkillBridge.Message.NUserInfo info)
        {
            this.userInfo = info;
        }

        // 当前地图
        public MapDefine CurrentMapData { get; set; }
        // 当前角色
        public SkillBridge.Message.NCharacterInfo CurrentCharacter { get; set; }
        // 当前角色游戏对象
        public GameObject CurrentCharacterObject { get; set; }

        public void AddGold(int gold)
        {
            CurrentCharacter.Gold += gold;
        }
    }
}
