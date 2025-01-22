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
        /// �������Ӷ���
        /// </summary>
        /// <param name="tcpClient"></param>
        public void SetCSConnect(CSConnect cSConnect)
        {
            mCSConnect = cSConnect;
            mStream = cSConnect.TcpClient.GetStream();
        }

        /// <summary>
        /// �Ƿ��ӡ���ʹ�������־
        /// </summary>
        /// <param name="flag"></param>
        public void SetDebug(bool flag)
        {
            mIsDebug = flag;
        }

        public void Start()
        {
            if (mIsDebug)
                Console.WriteLine($"�ͻ���ID:{mCSConnect.ID} RemoteEndPoint:{mCSConnect.RemoteEndPoint} ��Ϣ�������ѿ���");
        }

        public void Close()
        {
            if (mIsDebug)
                Console.WriteLine($"�ͻ���ID:{mCSConnect.ID} RemoteEndPoint:{mCSConnect.RemoteEndPoint} ��Ϣ�������ѹر�");
        }

        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="cmd">Э���</param>
        /// <param name="data">����</param>
        /// <param name="isLog">�Ƿ���ʾ��־</param>
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
                    Console.WriteLine($"�ͻ���ID:{mCSConnect.ID} RemoteEndPoint:{mCSConnect.RemoteEndPoint} Send 0x{cmd:x4}, Length {len}");
            }
            else
            {
                Console.WriteLine($"�ͻ���ID:{mCSConnect.ID} RemoteEndPoint:{mCSConnect.RemoteEndPoint} �����ѶϿ�,��Ϣ{cmd:x4}����ʧ��");
            }

        }

    }
}
