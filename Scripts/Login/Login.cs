using System;

namespace FengShengServer
{
    public class Login
    {
        private CSConnect mCSConnect;

        private bool mIsDebug;

        public void SetCSConnect(CSConnect cSConnect)
        {
            mCSConnect = cSConnect;
        }

        /// <summary>
        /// 是否打印登录模块日志
        /// </summary>
        /// <param name="flag"></param>
        public void SetDebug(bool flag)
        {
            mIsDebug = flag;
        }

        public void Start()
        {
            ProtosManager.Instance.AddProtosListener(mCSConnect.ID, CmdConfig.Login_C2S, OnReceiveLogin);
        }

        public void Close()
        {
            ProtosManager.Instance.RemoveProtosListener(mCSConnect.ID, CmdConfig.Login_C2S, OnReceiveLogin);
        }

        public void OnReceiveLogin(object obj)
        {
            LoginServer.Login.Login_S2C sendData = new LoginServer.Login.Login_S2C();

            var receiveData = obj as LoginServer.Login.Login_C2S;
            if (receiveData == null)
            {
                sendData.User = null;
                sendData.Code = LoginServer.Login.Login_S2C.Types.Ret_Code.Failed;
                sendData.Msg = "数据不完整,请重新登录";
                ProtosManager.Instance.Unicast(mCSConnect, CmdConfig.Login_S2C, sendData);
                return;
            }

            var userData = new UserData()
            {
                Name = receiveData.Name,
                RoomInfo = null,
                CSConnect = mCSConnect,
                Status = UserStatus.Online
            };
            mCSConnect.UserData = userData;

            bool isSuccess = UserDataManager.Instance.AddUser(userData);

            var sendUserData = new LoginServer.Login.UserData() { Name = userData.Name };
            if (isSuccess)
            {
                sendData.User = sendUserData;
                sendData.Code = LoginServer.Login.Login_S2C.Types.Ret_Code.Success;
                sendData.Msg = "登录成功";
            }
            else 
            {
                Console.WriteLine($"用户 {userData.Name} 仍在在线用户列表中");
                sendData.User = sendUserData;
                sendData.Code = LoginServer.Login.Login_S2C.Types.Ret_Code.Failed;
                sendData.Msg = "名称重复";
            }
            ProtosManager.Instance.Unicast(mCSConnect, CmdConfig.Login_S2C, sendData);
            EventManager.Instance.TriggerEvent(EventManager.Event_OnUserStatusChange, userData);
        }
    }
}
