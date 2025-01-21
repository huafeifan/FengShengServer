
using System;
using System.Diagnostics;
using Google.Protobuf;

namespace FengShengServer
{
    public class Login
    {
        private CSConnect mCSConnect;
        private UserData mUserData;

        public void SetCSConnect(CSConnect cSConnect)
        {
            mCSConnect = cSConnect;
        }

        public void Start()
        {
            mCSConnect.ProtosListener.AddListener(CmdConfig.Login_C2S, OnReceiveLogin);
        }

        public void Close()
        {
            if (mUserData != null) 
            {
                EventManager.Instance.TriggerEvent(EventManager.Event_OnUserOffline, mUserData);
            }

            mCSConnect.ProtosListener.RemoveListener(CmdConfig.Login_C2S, OnReceiveLogin);
        }

        public void OnReceiveLogin(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return;
            var receiveData = LoginServer.Login.Login_C2S.Parser.ParseFrom(bytes);
            if (receiveData == null) return;

            mUserData = new UserData()
            {
                Name = receiveData.Name
            };

            bool isSuccess = DataManager.Instance.AddUser(mUserData);
            LoginServer.Login.Login_S2C sendData = new LoginServer.Login.Login_S2C();

            if (isSuccess)
            {
                sendData.Name = mUserData.Name;
                sendData.Code = LoginServer.Login.Login_S2C.Types.Ret_Code.Success;
                sendData.Msg = "登录成功";
            }
            else 
            {
                Console.WriteLine($"用户 {mUserData.Name} 仍在在线用户列表中");
                //EventManager.Instance.TriggerEvent(Server.Event_OnConnectInterrupt, new NetworkEventPackage() { ID = mCSConnect.ID });
                //sendData.Name = receiveData.Name;
                //sendData.Code = LoginServer.Login.Login_S2C.Types.Ret_Code.Online;
                //sendData.Msg = "上一处登录已经踢下线";
            }
            mCSConnect.Sender.SendMessage(CmdConfig.Login_S2C, sendData.ToByteArray());
        }
    }
}
