using System.IO;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;

namespace FengShengServer
{
    public class MessageReceiver
    {
        private CSConnect mCSConnect;
        private NetworkStream mStream;
        private CancellationTokenSource mCts;

        public MessageReceiver()
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

        public void Start()
        {
            mCts = new CancellationTokenSource();
            _ = Task.Run(() => ReceiveDataAsync());
            Console.WriteLine("��Ϣ�������ѿ���");
        }

        public void Close()
        {
            if (mCts != null) 
            {
                mCts.Cancel();
            }
            Console.WriteLine("��Ϣ�������ѹر�");
        }

        private async Task ReceiveDataAsync()
        {
            byte[] buffer = new byte[1024];
            int bytesRead;

            while (!mCts.IsCancellationRequested)
            {
                try
                {
                    bytesRead = await mStream.ReadAsync(buffer, 0, 1024);
                    if (bytesRead > 0)
                    {
                        uint length = (uint)(buffer[0] << 8) + (uint)buffer[1];
                        uint cmd = (uint)(buffer[2] << 8) + (uint)buffer[3];
                        Console.WriteLine($"Receive 0x{cmd:x4}, Length {length}");

                        if (cmd == HeartBeat.Cmd)
                        {
                            mCSConnect.HeartBeat.SetHeartBeatFlag(true);
                        }
                    }
                }
                catch (IOException ex)
                {
                    // �������ӶϿ���IO�쳣
                    Console.WriteLine("��������ʱ����IOException��" + ex.Message);
                    break;
                }
                catch (OperationCanceledException)
                {
                    // ��������£���ȡ�����Ʊ�����ʱ�����׳����쳣
                    break;
                }
            }
        }

    }
}
