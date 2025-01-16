using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FengShengServer
{
    public class CSConnect
    {
        public TcpClient TcpClient { get; set; }

        public int ID { get; set; }

        /// <summary>
        /// 心跳处理器
        /// </summary>
        private HeartBeat mHeartBeat;
        public HeartBeat HeartBeat => mHeartBeat;

        /// <summary>
        /// 消息接收器
        /// </summary>
        private MessageReceiver mReceiver;

        /// <summary>
        /// 消息发送器
        /// </summary>
        private MessageSender mSender;

        public CSConnect(TcpClient tcpClient, int id)
        {
            TcpClient = tcpClient;
            mHeartBeat = new HeartBeat();
            mReceiver = new MessageReceiver();
            mSender = new MessageSender();
            ID = id;
        }

        public void Start()
        {
            //消息发送器初始化
            mSender.SetTcpClient(TcpClient);

            //消息接收器初始化
            mReceiver.SetCSConnect(this);

            //心跳处理器初始化
            mHeartBeat.SetCSConnect(this);
            mHeartBeat.SetTimer(1000);

            mSender.Start();
            mReceiver.Start();
            mHeartBeat.Start();
        }

        public void Close()
        {
            mSender.Close();
            mHeartBeat.Close();
            mReceiver.Close();
        }

    }
}
