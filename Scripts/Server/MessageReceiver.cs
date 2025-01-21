using System.IO;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Linq;

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
        /// 传入连接对象
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
            Console.WriteLine("消息接受器已开启");
        }

        public void Close()
        {
            if (mCts != null) 
            {
                mCts.Cancel();
            }
            Console.WriteLine("消息接受器已关闭");
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
                        if (cmd != CmdConfig.HeartBeat)
                        {
                            Console.WriteLine($"Receive 0x{cmd:x4}, Length {length}, bytesRead {bytesRead}");

                            uint len = length - 4;
                            byte[] data = new byte[len];
                            for (int i = 0, j = 4; i < len; i++, j++)
                            {
                                data[i] = buffer[j];
                            }

                            mCSConnect.ProtosListener.TriggerEvent(cmd, data);
                        }

                        if (cmd == CmdConfig.HeartBeat)
                        {
                            mCSConnect.HeartBeat.SetHeartBeatFlag(true);
                            mCSConnect.Sender.SendMessage(CmdConfig.HeartBeat, mCSConnect.HeartBeat.GetHeatBeatData(), false);
                        }
                    }
                    else
                    {
                        EventManager.Instance.TriggerEvent(Server.Event_OnConnectInterrupt, new NetworkEventPackage() { ID = mCSConnect.ID });
                    }
                }
                catch (IOException ex)
                {
                    // 处理连接断开等IO异常
                    Console.WriteLine("接收数据时发生IOException：" + ex.Message);
                    break;
                }
                catch (OperationCanceledException)
                {
                    // 正常情况下，当取消令牌被请求时，会抛出此异常
                    break;
                }
            }
        }

    }
}
