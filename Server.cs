using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FengShengServer
{
    public class Server
    {
        private const int Port = 8000;
        private const int MaxClients = 10;
        private static ConcurrentBag<TcpClient> mClients = new ConcurrentBag<TcpClient>();
        public static async Task Main(string[] args)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, Port);
            listener.Start();
            Console.WriteLine("服务器已启动，等待客户端连接...");

            while (true)
            {
                if (mClients.Count < MaxClients) 
                {
                    try
                    {

                        TcpClient client = await listener.AcceptTcpClientAsync();
                        mClients.Add(client);
                        Console.WriteLine("客户端已连接：" + client.Client.RemoteEndPoint);
                        _ = Task.Run(() => HandleClientAsync(client));
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
            }

        }

        private static async Task HandleClientAsync(TcpClient client) 
        {
            try
            {
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead;
                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Console.WriteLine("收到来自" + client.Client.RemoteEndPoint + "的消息:" + message);

                        byte[] response = Encoding.UTF8.GetBytes("消息已接收");
                        await stream.WriteAsync(response, 0, response.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("处理客户端时出错:" + ex.Message);
            }
            finally
            {
                client.Close();
                Console.WriteLine("客户端已断开连接" + client.Client.RemoteEndPoint);
                mClients.TryTake(out _);
            }
        }
    }
}
