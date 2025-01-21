
using System;

namespace FengShengServer
{
    public class Login
    {
        private CSConnect mCSConnect;

        public void SetCSConnect(CSConnect cSConnect)
        {
            mCSConnect = cSConnect;
        }

        public void Start()
        {
            mCSConnect.ProtosListener.AddListener(CmdConfig.Login, OnReceiveLogin);
        }

        public void Close()
        {
            mCSConnect.ProtosListener.RemoveListener(CmdConfig.Login, OnReceiveLogin);
        }

        public void OnReceiveLogin(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return;
            var data = LoginServer.Login.Login_C2S.Parser.ParseFrom(bytes);
            if (data == null) return;

            UserData userData = new UserData()
            {
                Name = data.Name
            };

            Console.WriteLine(data.Name);
            bool isSuccess = DataManager.Instance.AddUser(userData);
            if (isSuccess)
            {
                //mCSConnect.Sender.SendMessage(CmdConfig.Login, new byte[]);
            }
        }
    }
}
