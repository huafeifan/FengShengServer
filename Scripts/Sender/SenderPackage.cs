using System;
using System.Threading.Tasks;

namespace FengShengServer
{
    public class SenderPackage
    {
        private CSConnect mCSConnect;
        private uint mCmd;
        private byte[] mSendData;

        public bool IsLog;

        public SenderPackage(CSConnect connect, uint cmd, byte[] bytes, bool isLog)
        {
            SetData(connect, cmd, bytes, isLog);
        }

        public void SetData(CSConnect connect, uint cmd, byte[] bytes, bool isLog)
        {
            mCSConnect = connect;
            mCmd = cmd;
            mSendData = bytes;
            IsLog = isLog;
        }

        public Task WriteAsync()
        {
            try
            {
                if (mCSConnect != null && mCSConnect.TcpClient != null && mCSConnect.TcpClient.Connected)
                {
                    int len = mSendData.Length + 4;
                    byte b1 = (byte)((uint)len >> 8 & 0xFFu);
                    byte b2 = (byte)((uint)len & 0xFFu);
                    byte b3 = (byte)(mCmd >> 8 & 0xFFu);
                    byte b4 = (byte)(mCmd & 0xFFu);

                    byte[] bytes = new byte[len];
                    bytes[0] = b1;
                    bytes[1] = b2;
                    bytes[2] = b3;
                    bytes[3] = b4;
                    mSendData.CopyTo(bytes, 4);

                    var steam = mCSConnect.TcpClient.GetStream();
                    return steam.WriteAsync(bytes, 0, bytes.Length);
                }
                else
                {
                    Console.WriteLine($"客户端ID:{mCSConnect.ID} RemoteEndPoint:{mCSConnect.RemoteEndPoint} 连接已断开,消息{mCmd:x4}发送失败");
                    return default(Task);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"客户端ID:{mCSConnect.ID} RemoteEndPoint:{mCSConnect.RemoteEndPoint} Exception {e}");
                return default(Task);
            }
        }

        public string GetLog()
        {
             return $"客户端ID:{mCSConnect.ID} RemoteEndPoint:{mCSConnect.RemoteEndPoint} Send 0x{mCmd:x4}, Length {mSendData.Length + 4}";
        }

    }
}
