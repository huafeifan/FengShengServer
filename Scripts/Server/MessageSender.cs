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
        /// �������Ӷ���
        /// </summary>
        /// <param name="tcpClient"></param>
        public void SetTcpClient(TcpClient tcpClient)
        {
            mTcpClient = tcpClient;
            mStream = tcpClient.GetStream();
        }

        public void Start()
        {
            Console.WriteLine("��Ϣ�������ѿ���");
        }

        public void Close()
        {
            Console.WriteLine("��Ϣ�������ѹر�");
        }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="cmd">Э���</param>
        /// <param name="data">����</param>
        /// <param name="isLog">�Ƿ���ʾ��־</param>
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
                Console.WriteLine($"�����ѶϿ�,��Ϣ{cmd:x4}����ʧ��");
            }

        }

    }
}
