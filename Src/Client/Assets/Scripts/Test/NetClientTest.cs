using SkillBridge.Message;
using UnityEngine;


namespace Assets.Scripts.Test
{
    public class NetClientTest : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            // �ͻ������ӵ�������
            Network.NetClient.Instance.Init("127.0.0.1", 8000); // ��ʼ��IP�Ͷ˿�
            Network.NetClient.Instance.Connect();

            // ������Ϣ�������
            NetMessage msg = new NetMessage(); // ��������Ϣ
            msg.Request = new NetMessageRequest();  // ������������Ϣ
            msg.Request.firstTestRequest = new FirstTestRequest(); // �����Լ��������Ϣ
            msg.Request.firstTestRequest.Helloworld = "Hello World!"; // Ϊ�����Լ��������Ϣ�������
            Network.NetClient.Instance.SendMessage(msg); // ������Ϣ

            
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}