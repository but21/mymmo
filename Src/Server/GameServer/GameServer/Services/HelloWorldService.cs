using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Network;
using SkillBridge.Message;

namespace GameServer.Services
{
    // Singleton<> 用于单例
    class HelloWorldService : Singleton<HelloWorldService>
    {
        public void Init()
        {
            ExtremeWorldEntities entities = new ExtremeWorldEntities();
        }

        public void Start()
        {
            // 订阅需要处理的消息
            MessageDistributer<NetConnection<NetSession>>.Instance.Subscribe<FirstTestRequest>(this.OnFirstTestRequest);
        }

        public void Stop()
        {

        }

        // 消息响应函数
        void OnFirstTestRequest(NetConnection<NetSession> sender, FirstTestRequest request)
        {
            Log.InfoFormat($"FirstTestRequeset: Helloworld:{request.Helloworld}");
        }

    }
}
