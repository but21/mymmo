using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using Network;
using UnityEngine;

using SkillBridge.Message;
using Models;
using Managers;

namespace Services
{
    class UserService : Singleton<UserService>, IDisposable
    {
        public UnityEngine.Events.UnityAction<Result, string> OnRegister;
        public UnityEngine.Events.UnityAction<Result, string> OnLogin;
        public UnityEngine.Events.UnityAction<Result, string> OnCreateCharacter;
        NetMessage pendingMessage = null;
        bool connected = false;

        public UserService()
        {
            NetClient.Instance.OnConnect += OnGameServerConnect;
            NetClient.Instance.OnDisconnect += OnGameServerDisconnect;

            MessageDistributer.Instance.Subscribe<UserRegisterResponse>(this.OnUserRegister);
            MessageDistributer.Instance.Subscribe<UserLoginResponse>(OnUserLogin);
            MessageDistributer.Instance.Subscribe<UserCreateCharacterResponse>(OnUserCreateCharacter);
            MessageDistributer.Instance.Subscribe<UserGameEnterResponse>(OnGameEnter);
            MessageDistributer.Instance.Subscribe<UserGameLeaveResponse>(OnGameLeave);
            //MessageDistributer.Instance.Subscribe<MapCharacterEnterResponse>(OnCharacterEnter);
        }

        public void Dispose()
        {
            MessageDistributer.Instance.Unsubscribe<UserRegisterResponse>(this.OnUserRegister);
            MessageDistributer.Instance.Unsubscribe<UserLoginResponse>(OnUserLogin);
            MessageDistributer.Instance.Unsubscribe<UserCreateCharacterResponse>(OnUserCreateCharacter);
            MessageDistributer.Instance.Unsubscribe<UserGameEnterResponse>(OnGameEnter);
            MessageDistributer.Instance.Unsubscribe<UserGameLeaveResponse>(OnGameLeave);
            //MessageDistributer.Instance.Unsubscribe<MapCharacterEnterResponse>(OnCharacterEnter);


            NetClient.Instance.OnConnect -= OnGameServerConnect;
            NetClient.Instance.OnDisconnect -= OnGameServerDisconnect;
        }

        public void Init()
        {

        }

        public void ConnectToServer()
        {
            Debug.Log("ConnectToServer() Start ");
            //NetClient.Instance.CryptKey = this.SessionId;
            NetClient.Instance.Init("127.0.0.1", 8000);
            NetClient.Instance.Connect();
        }


        void OnGameServerConnect(int result, string reason)
        {
            Log.InfoFormat("LoadingMesager::OnGameServerConnect :{0} reason:{1}", result, reason);
            if (NetClient.Instance.Connected)
            {
                this.connected = true;
                if (this.pendingMessage != null)
                {
                    NetClient.Instance.SendMessage(this.pendingMessage);
                    this.pendingMessage = null;
                }
            }
            else
            {
                if (!this.DisconnectNotify(result, reason))
                {
                    MessageBox.Show(string.Format("网络错误，无法连接到服务器！\n RESULT:{0} ERROR:{1}", result, reason), "错误", MessageBoxType.Error);
                }
            }
        }

        public void OnGameServerDisconnect(int result, string reason)
        {
            this.DisconnectNotify(result, reason);
            return;
        }

        bool DisconnectNotify(int result, string reason)
        {
            if (this.pendingMessage != null)
            {
                if (this.pendingMessage.Request.userRegister != null)
                {
                    if (this.OnRegister != null)
                    {
                        this.OnRegister(Result.Failed, string.Format("服务器断开！\n RESULT:{0} ERROR:{1}", result, reason));
                    }
                }
                return true;
            }
            return false;
        }

        public void SendRegister(string user, string psw)
        {
            Debug.LogFormat("UserRegisterRequest::user :{0} psw:{1}", user, psw);
            NetMessage message = new NetMessage(); // 构建主消息
            message.Request = new NetMessageRequest(); // 构建请求
            message.Request.userRegister = new UserRegisterRequest(); // 构建用户注册
            message.Request.userRegister.User = user; // 用户注册的用户名
            message.Request.userRegister.Passward = psw; // 用户注册的密码

            // 判断是否连接上服务器
            if (this.connected && NetClient.Instance.Connected)
            {
                this.pendingMessage = null;
                NetClient.Instance.SendMessage(message);
            }
            else
            {
                // pendingMessage类似于一个队列, 先将消息记录下来, 连接到服务器之后, 自动将消息发送出去
                this.pendingMessage = message;
                this.ConnectToServer();
            }
        }

        public void SendLogin(string username, string password)
        {
            Debug.LogFormat($"UserLoginRequest::username :{username}, password :{password}");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.userLogin = new UserLoginRequest();
            message.Request.userLogin.User = username;
            message.Request.userLogin.Passward = password;

            if (connected && NetClient.Instance.Connected)
            {
                pendingMessage = null;
                NetClient.Instance.SendMessage(message);
            }
            else
            {
                pendingMessage = message;
                ConnectToServer();
            }
        }

        public void SendCharacterCreate(string charName, CharacterClass characterClass)
        {
            Debug.LogFormat($"CharacterCreateRequest::charName :{charName} characterClass :{characterClass}");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.createChar = new UserCreateCharacterRequest();
            message.Request.createChar.Name = charName;
            message.Request.createChar.Class = characterClass;

            if (connected && NetClient.Instance.Connected)
            {
                pendingMessage = null;
                NetClient.Instance.SendMessage(message);
            }

        }

        public void SendGameEnter(int characterIndex)
        {
            Debug.Log($"UserGameEnterRequest::characterId :{characterIndex}");

            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.gameEnter = new UserGameEnterRequest();
            message.Request.gameEnter.characterIdx = characterIndex;
            NetClient.Instance.SendMessage(message);
        }

        public void SendGameLeave()
        {
            Debug.Log("UserGameLeaveRequest");
            NetMessage message = new NetMessage();
            message.Request = new NetMessageRequest();
            message.Request.gameLeave = new UserGameLeaveRequest();
            NetClient.Instance.SendMessage(message);
        }

        void OnUserRegister(object sender, UserRegisterResponse response)
        {
            Debug.LogFormat("OnUserRegister:{0} [{1}]", response.Result, response.Errormsg);

            if (this.OnRegister != null)
            {
                this.OnRegister(response.Result, response.Errormsg);
            }
        }

        void OnUserLogin(object sender, UserLoginResponse response)
        {
            Debug.Log($"OnUserLogin:{response.Result} [{response.Errormsg}]");

            if (response.Result == Result.Success)
            {
                // 登录成功逻辑, 将服务器返回的用户信息记录到本地
                Models.User.Instance.SetupUserInfo(response.Userinfo);
            }

            if (OnLogin != null)
            {
                OnLogin(response.Result, response.Errormsg);
            }
        }

        void OnUserCreateCharacter(object sender, UserCreateCharacterResponse response)
        {
            Debug.Log($"OnCharacterCreate:{response.Result} [{response.Errormsg}]");

            if (response.Result == Result.Success)
            {
                Models.User.Instance.Info.Player.Characters.Clear();
                Models.User.Instance.Info.Player.Characters.AddRange(response.Characters);
                
            }

            if (OnCreateCharacter != null)
            {
                OnCreateCharacter(response.Result, response.Errormsg);
            }
        }

        void OnGameEnter(object sender, UserGameEnterResponse response)
        {
            Debug.LogFormat($"OnGameEnter::{response.Result}, {response.Errormsg}");

            if (response.Result == Result.Success)
            {
                if (response.Character != null)
                {
                    ItemManager.Instance.Init(response.Character.Items);
                    BagManager.Instance.Init(response.Character.Bag);
                    EquipManager.Instance.Init(response.Character.Equips);
                }
            }
        }

        void OnGameLeave(object sender, UserGameLeaveResponse response)
        {
            MapService.Instance.CurrentMapId = 0;
            User.Instance.CurrentCharacter = null;
            Debug.Log($"OnGameLeave::{response.Result}, {response.Errormsg}");
        }

        //private void OnCharacterEnter(object sender, MapCharacterEnterResponse response)
        //{
        //    Debug.Log($"MapCharacterEnterResponse::mapId{response.mapId}");
        //    NCharacterInfo info = response.Characters[0];
        //    User.Instance.CurrentCharacter = info;
        //    SceneManager.Instance.LoadScene(DataManager.Instance.Maps[response.mapId].Resource);
        //}
    }
}
