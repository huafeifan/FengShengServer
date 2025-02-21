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

        /// <summary>
        /// 游戏模块托管
        /// </summary>
        public Game Game { get; private set; }

        public CSConnect(TcpClient tcpClient, int id)
        {
            TcpClient = tcpClient;
            ID = id;
            RemoteEndPoint = tcpClient.Client.RemoteEndPoint;

            HeartBeat = new HeartBeat();
            Receiver = new MessageReceiver();

            Login = new Login();
            Room = new Room();
            Game = new Game();
        }

        public void Start()
        {
            //登陆模块托管初始化
            Login.SetCSConnect(this);
            Login.SetDebug(false);

            //房间模块托管初始化
            Room.SetCSConnect(this);
            Room.SetDebug(false);

            //游戏模块托管初始化
            Game.SetCSConnect(this);
            Game.SetDebug(true);

            //消息接收器初始化
            Receiver.SetCSConnect(this);
            Receiver.SetDebug(false, true);

            //心跳处理器初始化
            HeartBeat.SetCSConnect(this);
            HeartBeat.SetTimer(1000);
            HeartBeat.SetDebug(false);

            //注册协议监听器
            ProtosManager.Instance.RegisterProtosListener(ID);

            Login.Start();
            Room.Start();
            Game.Start();
            Receiver.Start();
            HeartBeat.Start();
        }

        public void Close()
        {
            Login.Close();
            Room.Close();
            Game.Close();

            ProtosManager.Instance.RemoveProtosListener(ID);

            HeartBeat.Close();
            Receiver.Close();
            TcpClient?.Close();
        }

    }
}
