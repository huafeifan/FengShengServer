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
        /// 协议监听器
        /// </summary>
        public ProtosListener ProtosListener { get; private set; }

        /// <summary>
        /// 登录模块托管
        /// </summary>
        public Login Login { get; private set; }

        public CSConnect(TcpClient tcpClient, int id)
        {
            TcpClient = tcpClient;
            HeartBeat = new HeartBeat();
            Receiver = new MessageReceiver();
            Sender = new MessageSender();
            ProtosListener = new ProtosListener();
            Login = new Login();
            ID = id;
            RemoteEndPoint = tcpClient.Client.RemoteEndPoint;
        }

        public void Start()
        {
            //登陆托管初始化
            Login.SetCSConnect(this);

            //消息发送器初始化
            Sender.SetTcpClient(TcpClient);

            //消息接收器初始化
            Receiver.SetCSConnect(this);

            //心跳处理器初始化
            HeartBeat.SetCSConnect(this);
            HeartBeat.SetTimer(1000);

            Login.Start();
            Sender.Start();
            Receiver.Start();
            HeartBeat.Start();
        }

        public void Close()
        {
            TcpClient?.Close();
            Sender.Close();
            HeartBeat.Close();
            Receiver.Close();
            Login.Close();
            ProtosListener.Close();
        }

    }
}
