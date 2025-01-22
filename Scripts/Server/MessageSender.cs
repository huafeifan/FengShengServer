using System;
using System.IO;
using System.Net.Sockets;

namespace FengShengServer
{
    public class MessageSender
    {
        private CSConnect mCSConnect;
        private Stream mStream;

        private bool mIsDebug;

        public MessageSender()
        {
            
        }

        /// <summary>
        /// 传入连接对象
        /// </summary>
        /// <param name="tcpClient"></param>
        public void SetCSConnect(CSConnect cSConnect)
        {
            mCSConnect = cSConnect;
            mStream = cSConnect.TcpClient.GetStream();
        }

        /// <summary>
        /// 是否打印发送处理器日志
        /// </summary>
        /// <param name="flag"></param>
        public void SetDebug(bool flag)
        {
            mIsDebug = flag;
        }

        public void Start()
        {
            if (mIsDebug)
                Console.WriteLine($"客户端ID:{mCSConnect.ID} RemoteEndPoint:{mCSConnect.RemoteEndPoint} 消息发送器已开启");
        }

        public void Close()
        {
            if (mIsDebug)
                Console.WriteLine($"客户端ID:{mCSConnect.ID} RemoteEndPoint:{mCSConnect.RemoteEndPoint} 消息发送器已关闭");
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="cmd">协议号</param>
        /// <param name="data">数据</param>
        /// <param name="isLog">是否显示日志</param>
        public void SendMessage(uint cmd, byte[] data, bool isLog = true)
        {
            if (mCSConnect.TcpClient != null && mCSConnect.TcpClient.Connected)
            {
                int len = data.Length + 4;
                byte b1 = (byte)((uint)len >> 8 & 0xFFu);
                byte b2 = (byte)((uint)len & 0xFFu);
                byte b3 = (byte)(cmd >> 8 & 0xFFu);
                byte b4 = (byte)(cmd & 0xFFu);

                byte[] bytes = new byte[len];
                bytes[0] = b1;
                bytes[1] = b2;
                bytes[2] = b3;
                bytes[3] = b4;
                data.CopyTo(bytes, 4);

                mStream.Write(bytes, 0, bytes.Length);

                if (isLog)
                    Console.WriteLine($"客户端ID:{mCSConnect.ID} RemoteEndPoint:{mCSConnect.RemoteEndPoint} Send 0x{cmd:x4}, Length {len}");
            }
            else
            {
                Console.WriteLine($"客户端ID:{mCSConnect.ID} RemoteEndPoint:{mCSConnect.RemoteEndPoint} 连接已断开,消息{cmd:x4}发送失败");
            }

        }

    }
}
