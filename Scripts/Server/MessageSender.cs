using System;
using System.IO;
using System.Net.Sockets;

namespace FengShengServer
{
    public class MessageSender
    {
        private TcpClient mTcpClient;
        private Stream mStream;

        public MessageSender()
        {
            
        }

        /// <summary>
        /// 传入连接对象
        /// </summary>
        /// <param name="tcpClient"></param>
        public void SetTcpClient(TcpClient tcpClient)
        {
            mTcpClient = tcpClient;
            mStream = tcpClient.GetStream();
        }

        public void Start()
        {
            Console.WriteLine("消息发送器已开启");
        }

        public void Close()
        {
            Console.WriteLine("消息发送器已关闭");
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="cmd">协议号</param>
        /// <param name="data">数据</param>
        /// <param name="isLog">是否显示日志</param>
        public void SendMessage(uint cmd, byte[] data, bool isLog = true)
        {
            if (mTcpClient != null && mTcpClient.Connected)
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
                    Console.WriteLine($"Send 0x{cmd:x4}, Length {len}");
            }
            else
            {
                Console.WriteLine($"连接已断开,消息{cmd:x4}发送失败");
            }

        }

    }
}
