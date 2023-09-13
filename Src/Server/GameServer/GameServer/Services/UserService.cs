using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Network;
using SkillBridge.Message;
using GameServer.Entities;
using GameServer.Managers;
using GameServer.Models;

namespace GameServer.Services
{

    class UserService : Singleton<UserService>
    {
        public UserService()
        {
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserRegisterRequest>(OnRegister);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserLoginRequest>(OnLogin);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserCreateCharacterRequest>(OnCreateCharacter);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserGameEnterRequest>(OnGameEnter);
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<UserGameLeaveRequest>(OnGameLeave);
        }


        public void Init()
        {

        }

        void OnRegister(NetConnection<NetSession> sender, UserRegisterRequest request)
        {
            // 收到消息后打出日志, 用户名, 密码
            Log.InfoFormat($"UserRegisterRequest: User:{request.User}, Password:{request.Passward}");

            sender.Session.Response.userRegister = new UserRegisterResponse();


            // 从数据库中查找用户
            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();


            // 如果用户已存在
            if (user != null)
            {
                sender.Session.Response.userRegister.Result = Result.Failed;
                sender.Session.Response.userRegister.Errormsg = "用户已存在";
            }
            else
            {
                TPlayer player = DBService.Instance.Entities.Players.Add(new TPlayer());
                DBService.Instance.Entities.Users.Add(new TUser() { Username = request.User, Password = request.Passward, Player = player });
                DBService.Instance.Entities.SaveChanges();
                sender.Session.Response.userRegister.Result = Result.Success;
                sender.Session.Response.userRegister.Errormsg = "None";
            }

            sender.SendResponse();
        }

        void OnLogin(NetConnection<NetSession> sender, UserLoginRequest request)
        {
            Log.InfoFormat($"UserLoginRequest: User:{request.User}, Password:{request.Passward}");

            sender.Session.Response.userLogin = new UserLoginResponse();

            TUser user = DBService.Instance.Entities.Users.Where(u => u.Username == request.User).FirstOrDefault();
            if (user == null)
            {
                sender.Session.Response.userLogin.Result = Result.Failed;
                sender.Session.Response.userLogin.Errormsg = "用户名不存在";
            }
            else if (request.Passward != user.Password)
            {
                sender.Session.Response.userLogin.Result = Result.Failed;
                sender.Session.Response.userLogin.Errormsg = "密码错误";
            }
            else
            {
                // Session的作用是当不同消息从客户端发送到服务端时, 通过Session知道当前接收的是谁的消息, 用户是谁...
                sender.Session.User = user;

                sender.Session.Response.userLogin.Result = Result.Success;
                sender.Session.Response.userLogin.Errormsg = "None";

                sender.Session.Response.userLogin.Userinfo = new NUserInfo();
                sender.Session.Response.userLogin.Userinfo.Id = (int)user.ID;
                sender.Session.Response.userLogin.Userinfo.Player = new NPlayerInfo();
                sender.Session.Response.userLogin.Userinfo.Player.Id = user.Player.ID;

                foreach (var c in user.Player.Characters)
                {
                    NCharacterInfo info = new NCharacterInfo();
                    info.Id = c.ID;
                    info.Name = c.Name;
                    info.Type = CharacterType.Player;
                    info.Class = (CharacterClass)c.Class;
                    info.Tid = c.ID;
                    sender.Session.Response.userLogin.Userinfo.Player.Characters.Add(info);

                }
            }
            sender.SendResponse();
        }

        void OnCreateCharacter(NetConnection<NetSession> sender, UserCreateCharacterRequest request)
        {
            Log.InfoFormat($"UserCreateCharacterRequest: name:{request.Name}, class:{request.Class}");

            TCharacter character = new TCharacter()
            {
                Name = request.Name,
                Class = (int)request.Class,
                TID = (int)request.Class,
                MapID = 1,
                MapPosX = 5000,
                MapPosY = 4000,
                MapPosZ = 820,
                Gold = 100000,
            };

            var bag = new TCharacterBag();
            bag.Owner = character;
            bag.Items = new byte[0];
            bag.Unlocked = 20;
            character.Bag = DBService.Instance.Entities.CharacterBags.Add(bag);

            character = DBService.Instance.Entities.Characters.Add(character);
            sender.Session.User.Player.Characters.Add(character);
            DBService.Instance.Entities.SaveChanges();


            sender.Session.Response.createChar = new UserCreateCharacterResponse();
            sender.Session.Response.createChar.Result = Result.Success;
            sender.Session.Response.createChar.Errormsg = "None";

            foreach (var c in sender.Session.User.Player.Characters)
            {
                NCharacterInfo info = new NCharacterInfo();
                info.Id = 0;
                info.Type = CharacterType.Player;
                info.Name = c.Name;
                info.Class = (CharacterClass)c.Class;
                info.Tid = c.TID;
                sender.Session.Response.createChar.Characters.Add(info);
            }

            sender.SendResponse();
        }

        private void OnGameEnter(NetConnection<NetSession> sender, UserGameEnterRequest request)
        {
            TCharacter dbCharacter = sender.Session.User.Player.Characters.ElementAt(request.characterIdx);
            Log.InfoFormat($"UserGameEnterRequest:: characterId:{dbCharacter.ID} name:{dbCharacter.Name} mapId:{dbCharacter.MapID}");
            Character character = CharacterManager.Instance.AddCharacter(dbCharacter);


            sender.Session.Response.gameEnter = new UserGameEnterResponse();
            sender.Session.Response.gameEnter.Result = Result.Success;
            sender.Session.Response.gameEnter.Errormsg = "None";

            sender.Session.Response.gameEnter.Character = character.Info;

            // 道具系统测试
            int ItemId = 2;
            bool hasItem = character.ItemManager.HasItem(ItemId);
            //Log.Info($"hasItem:{hasItem}");
            if (!hasItem)
            {
                //character.ItemManager.RemoveItem(ItemId, 1);
                character.ItemManager.AddItem(1, 200);
                character.ItemManager.AddItem(2, 100);
                character.ItemManager.AddItem(3, 30);
                character.ItemManager.AddItem(4, 120);
            }
            //else
            //{

            //}
            //Item item = character.ItemManager.GetItem(ItemId);
            //Log.Info($"{ItemId} {item}");
            Log.Info($"Item1:{character.ItemManager.GetItem(1)}");
            Log.Info($"Item2:{character.ItemManager.GetItem(2)}");
            Log.Info($"Item3:{character.ItemManager.GetItem(3)}");
            Log.Info($"Item4:{character.ItemManager.GetItem(4)}");
            DBService.Instance.Save();

            sender.SendResponse();
            sender.Session.Character = character;

            MapManager.Instance[dbCharacter.MapID].CharacterEnter(sender, character);
        }

        private void OnGameLeave(NetConnection<NetSession> sender, UserGameLeaveRequest request)
        {
            // 从本地存储获取角色
            Character character = sender.Session.Character;
            Log.Info($"UserGameLeaveRequest:characterID: {character.Id}:{character.Info.Name} Map:{character.Info.mapId}");
            CharacterLeave(character);


            sender.Session.Response.gameLeave = new UserGameLeaveResponse();
            sender.Session.Response.gameLeave.Result = Result.Success;
            sender.Session.Response.gameLeave.Errormsg = "None";

            sender.SendResponse();
        }

        internal void CharacterLeave(Character character)
        {
            // 从角色管理器中把角色移除掉
            CharacterManager.Instance.RemoveCharacter(character.Id);
            // 角色离开地图
            MapManager.Instance[character.Info.mapId].CharacterLeave(character);
        }
    }
}
