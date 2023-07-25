using UnityEngine;


namespace Assets.Scripts.Test
{
    public class NetClientTest : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            // 客户端连接到服务器
            Network.NetClient.Instance.Init("127.0.0.1", 8000); // 初始化IP和端口
            Network.NetClient.Instance.Connect();

            // 发送消息到服务端
            SkillBridge.Message.NetMessage msg = new SkillBridge.Message.NetMessage(); // 构建主消息
            msg.Request = new SkillBridge.Message.NetMessageRequest();  // 创构建请求消息
            msg.Request.firstTestRequest = new SkillBridge.Message.FirstTestRequest(); // 构建自己定义的消息
            msg.Request.firstTestRequest.Helloworld = "Hello World!"; // 为我们自己定义的消息填充数据
            Network.NetClient.Instance.SendMessage(msg); // 发送消息
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}