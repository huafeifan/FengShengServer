using System;
using System.Net;
using System.Net.Sockets;

namespace FengShengServer
{
    public class CSConnect
    {
        public TcpClient TcpClient { get; private set; }

        public int ID { get; set; }
        public EndPoint RemoteEndPoint { get; private set; }

        /// <summary>
        /// 心跳处理器
        /// </summary>
        public HeartBeat HeartBeat { get; private set; }

        /// <summary>
        /// 消息接收器
        /// </summary>
        public MessageReceiver Receiver { get; private set; }

        /// <summary>
        /// 消息发送器
        /// </summary>
        public MessageSender Sender { get; private set; }

        /// <summary>
        /// 用户数据
        /// </summary>
        public UserData UserData { get; set; }

        /// <summary>
        /// 登录模块托管
        /// </summary>
        public Login Login { get; private set; }

        /// <summary>
        /// 房间模块托管
        /// </summary>
        public Room Room { get; private set; }

        public CSConnect(TcpClient tcpClient, int id)
        {
            TcpClient = tcpClient;
            ID = id;
            RemoteEndPoint = tcpClient.Client.RemoteEndPoint;

            HeartBeat = new HeartBeat();
            Receiver = new MessageReceiver();
            Sender = new MessageSender();

            Login = new Login();
            Room = new Room();
        }

        public void Start()
        {
            //登陆模块托管初始化
            Login.SetCSConnect(this);
            Login.SetDebug(false);

            //房间模块托管初始化
            Room.SetCSConnect(this);
            Room.SetDebug(false);

            //消息发送器初始化
            Sender.SetCSConnect(this);
            Sender.SetDebug(false);

            //消息接收器初始化
            Receiver.SetCSConnect(this);
            Receiver.SetDebug(false, true);

            //心跳处理器初始化
            HeartBeat.SetCSConnect(this);
            HeartBeat.SetTimer(1000);
            HeartBeat.SetDebug(false);

            //注册协议监听器
            EventManager.Instance.RegisterProtosListener(ID);

            Login.Start();
            Room.Start();
            Sender.Start();
            Receiver.Start();
            HeartBeat.Start();
        }

        public void Close()
        {
            Login.Close();
            Room.Close();

            EventManager.Instance.RemoveProtosListener(ID);

            Sender.Close();
            HeartBeat.Close();
            Receiver.Close();
            TcpClient?.Close();
        }

    }
}
