using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FengShengServer
{
    public class Server
    {
        public const string Event_OnConnectInterrupt = "OnConnectInterrupt";

        /// <summary>
        /// 端口号
        /// </summary>
        private const int Port = 8000;

        /// <summary>
        /// 最大连接数
        /// </summary>
        private const int MaxClients = 10;

        /// <summary>
        /// 线程安全的客户端连接列表
        /// </summary>
        private static ConcurrentBag<CSConnect> mClients = new ConcurrentBag<CSConnect>();

        public static async Task Main(string[] args)
        {
            EventManager.Instance.AddListener(Event_OnConnectInterrupt, OnConnectInterrupt);

            TcpListener listener = new TcpListener(IPAddress.Any, Port);
            listener.Start();

            Console.WriteLine("服务器已启动，等待客户端连接...");

            int clientID = 0;
            while (true)
            {
                if (mClients.Count < MaxClients)
                {
                    try
                    {
                        TcpClient client = await listener.AcceptTcpClientAsync();
                        CSConnect cSConnect = new CSConnect(client, clientID++);

                        Console.WriteLine("客户端已连接：" + client.Client.RemoteEndPoint);

                        mClients.Add(cSConnect);
                        cSConnect.Start();
                    }
                    catch (SocketException ex)
                    {
                        Console.WriteLine("接受客户端连接时发生SocketException：" + ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("已达到最大客户端数量，拒绝新的连接请求");
                }

                await Task.Delay(1000);
            }

        }

        public static void OnConnectInterrupt(object obj)
        {
            NetworkEventPackage package = (NetworkEventPackage)obj;

            var list = mClients.ToArray();
            for (int i = 0; i < list.Length; i++)
            {
                if (list[i].ID == package.ID && mClients.TryTake(out list[i]))
                {
                    list[i].Close();
                    Console.WriteLine($"检测到客户端断开连接: 客户端ID:{list[i].ID} RemoteEndPoint:{list[i].RemoteEndPoint}");
                }
            }
        }


    }
}
