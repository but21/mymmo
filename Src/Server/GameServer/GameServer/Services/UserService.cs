using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Network;
using SkillBridge.Message;
using GameServer.Entities;

namespace GameServer.Services
{

    class UserService : Singleton<UserService>
    {
        public UserService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserRegisterRequest>(OnRegister);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserLoginRequest>(OnLogin);
        }

        public void Init()
        {

        }

        void OnRegister(NetConnection<NetSession> sender, UserRegisterRequest request)
        {
            // 收到消息后打出日志, 用户名, 密码
            Log.InfoFormat($"UserRegisterRequest: User:{request.User}, Password:{request.Passward}");

            // 准备好发给客户端的消息
            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.userRegister = new UserRegisterResponse();


            // 从数据库中查找用户
            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();


            // 如果用户已存在
            if (user != null)
            {
                message.Response.userRegister.Result = Result.Failed;
                message.Response.userRegister.Errormsg = "用户已存在";
            }
            else
            {
                TPlayer player = DBService.Instance.Entities.Players.Add(new TPlayer());
                DBService.Instance.Entities.Users.Add(new TUser() { Username = request.User, Password = request.Passward, Player = player });
                DBService.Instance.Entities.SaveChanges();
                message.Response.userRegister.Result = Result.Success;
                message.Response.userRegister.Errormsg = "None";
            }

            byte[] data = PackageHandler.PackMessage(message);
            sender.SendData(data, 0, data.Length);
        }

        void OnLogin(NetConnection<NetSession> sender, UserLoginRequest request)
        {
            Log.InfoFormat($"UserLoginRequest: User:{request.User}, Password:{request.Passward}");

            NetMessage message = new NetMessage();
            message.Response = new NetMessageResponse();
            message.Response.userLogin = new UserLoginResponse();

            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();
            if (user == null)
            {
                message.Response.userLogin.Result = Result.Failed;
                message.Response.userLogin.Errormsg = "用户名不存在";
                return;
            }
            if (request.Passward != user.Password)
            {
                message.Response.userLogin.Result = Result.Failed;
                message.Response.userLogin.Errormsg = "用户名或密码错误";
            }
            else
            {
                message.Response.userLogin.Result = Result.Success;
                message.Response.userLogin.Errormsg = "None";
            }

        }

    }
}