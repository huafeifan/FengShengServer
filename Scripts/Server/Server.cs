using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FengShengServer
{
    public class Server
    {

        /// <summary>
        /// 端口号
        /// </summary>
        private const int Port = 8000;

        /// <summary>
        /// 最大连接数
        /// </summary>
        private const int MaxClients = 10;

        /// <summary>
        /// 客户端连接列表
        /// </summary>
        private static List<CSConnect> mClients = new List<CSConnect>();
        public static List<CSConnect> Clients { get { return mClients.ToList(); } }

        public static async Task Main(string[] args)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, Port);
            listener.Start();

            UserDataManager.Instance.Start();
            RoomDataManager.Instance.Start();
            EventManager.Instance.Start();
            ProtosManager.Instance.Start();
            SenderManager.Instance.Start();

            EventManager.Instance.AddListener(EventManager.Event_OnConnectInterrupt, OnConnectInterrupt);

            Console.WriteLine("服务器已启动，等待客户端连接...");

            int clientID = 0;
            while (true)
            {
                if (mClients.Count < MaxClients)
                {
                    try
                    {
                        TcpClient client = await listener.AcceptTcpClientAsync();

                        CSConnect cSConnect = new CSConnect(client, ++clientID);

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

            for (int i = 0; i < mClients.Count; i++)
            {
                if (mClients[i].ID == package.ID)
                {
                    if (mClients[i].UserData != null)
                    {
                        mClients[i].UserData.Status = UserStatus.Offline;
                        EventManager.Instance.TriggerEvent(EventManager.Event_OnUserStatusChange, mClients[i].UserData);
                    }
                    mClients[i].Close();
                    Console.WriteLine($"检测到客户端断开连接: 客户端ID:{mClients[i].ID} RemoteEndPoint:{mClients[i].RemoteEndPoint}");
                    mClients.RemoveAt(i);
                    break;
                }
            }
        }

    }
}
