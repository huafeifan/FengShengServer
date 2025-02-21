using System;
using Google.Protobuf;
using System.Threading.Tasks;
using System.Reflection;
using System.Text;

namespace FengShengServer
{
    public class SenderPackage
    {
        private CSConnect mCSConnect;
        private uint mCmd;
        private byte[] mSendDataBytes;
        private IMessage mSendData;

        public bool IsLog;

        public SenderPackage(CSConnect connect, uint cmd, byte[] bytes, bool isLog)
        {
            SetData(connect, cmd, bytes, isLog);
        }

        public SenderPackage(CSConnect connect, uint cmd, IMessage sendData, bool isLog)
        {
            SetData(connect, cmd, sendData, isLog);
        }

        public void SetData(CSConnect connect, uint cmd, byte[] bytes, bool isLog)
        {
            mCSConnect = connect;
            mCmd = cmd;
            mSendDataBytes = bytes;
            mSendData = null;
            IsLog = isLog;
        }

        public void SetData(CSConnect connect, uint cmd, IMessage sendData, bool isLog)
        {
            mCSConnect = connect;
            mCmd = cmd;
            mSendDataBytes = null;
            mSendData = sendData;
            IsLog = isLog;
        }

        public Task WriteAsync()
        {
            try
            {
                if (mCSConnect != null && mCSConnect.TcpClient != null && mCSConnect.TcpClient.Connected)
                {
                    if (mSendData != null)
                    {
                        mSendDataBytes = mSendData.ToByteArray();
                    }
                    int len = mSendDataBytes.Length + 4;
                    byte b1 = (byte)((uint)len >> 8 & 0xFFu);
                    byte b2 = (byte)((uint)len & 0xFFu);
                    byte b3 = (byte)(mCmd >> 8 & 0xFFu);
                    byte b4 = (byte)(mCmd & 0xFFu);

                    byte[] bytes = new byte[len];
                    bytes[0] = b1;
                    bytes[1] = b2;
                    bytes[2] = b3;
                    bytes[3] = b4;
                    mSendDataBytes.CopyTo(bytes, 4);

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
            var user = UserDataManager.Instance.GetUserDataByConnectID(mCSConnect.ID);
            if (user == null)
            {
                return $"客户端ID:{mCSConnect.ID} RemoteEndPoint:{mCSConnect.RemoteEndPoint} Send 0x{mCmd:x4}, Length {mSendDataBytes.Length + 4}";
            }

            if (mSendData == null)
            {
                return $"用户名:{user.Name} Send 0x{mCmd:x4}, Length {mSendDataBytes.Length + 4}";
            }
            
            Type type = mSendData.GetType();
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            StringBuilder sb = new StringBuilder($"用户名:{user.Name} Send 0x{mCmd:x4}, Length {mSendDataBytes.Length + 4}\r\n");
            foreach (var property in properties) 
            {
                sb.AppendLine($"{property.Name}:{property.GetValue(mSendData)}");
            }
            return sb.ToString();
        }

    }
}
