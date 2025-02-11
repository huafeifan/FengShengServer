using System;
using System.Collections.Generic;
using System.Linq;
using IdentityType = LoginServer.Game.IdentityType;
using CharacterType = LoginServer.Game.CharacterType;

namespace FengShengServer
{
    public class Game
    {
        private CSConnect mCSConnect;
        private Identity mIdentity = new Identity();
        private Character mCharacter = new Character();
        private GameCard mGameCard = new GameCard();

        private bool mIsDebug;

        public void SetCSConnect(CSConnect cSConnect)
        {
            mCSConnect = cSConnect;
        }

        /// <summary>
        /// 是否打印游戏模块日志
        /// </summary>
        /// <param name="flag"></param>
        public void SetDebug(bool flag)
        {
            mIsDebug = flag;
        }

        public void Start()
        {
            ProtosManager.Instance.AddProtosListener(mCSConnect.ID, CmdConfig.GameStart_C2S, OnReceiveGameStart);
            ProtosManager.Instance.AddProtosListener(mCSConnect.ID, CmdConfig.CharacterChoose_C2S, OnReceiveCharacterChoose);
        }

        public void Close()
        {
            ProtosManager.Instance.RemoveProtosListener(mCSConnect.ID, CmdConfig.GameStart_C2S, OnReceiveGameStart);
            ProtosManager.Instance.RemoveProtosListener(mCSConnect.ID, CmdConfig.CharacterChoose_C2S, OnReceiveCharacterChoose);
        }

        private void OnReceiveGameStart(object obj)
        {
            var data = obj as LoginServer.Game.GameStart_C2S;
            if (data == null)
                return;

            var sendData = new LoginServer.Game.GameStart_S2C();

            var roomInfo = RoomDataManager.Instance.GetRoomInfo(data.RoomNub);
            if (roomInfo == null)
            {
                sendData.Code = LoginServer.Game.GameStart_S2C.Types.Ret_Code.Failed;
                sendData.Msg = "房间信息不存在";
                ProtosManager.Instance.Unicast(mCSConnect, CmdConfig.GameStart_S2C, sendData);
                return;
            }

            if (roomInfo.GetUserCount() == 0)
            {
                sendData.Code = LoginServer.Game.GameStart_S2C.Types.Ret_Code.Failed;
                sendData.Msg = "房间中无玩家";
                ProtosManager.Instance.Unicast(mCSConnect, CmdConfig.GameStart_S2C, sendData);
                return;
            }

            int chairCount = roomInfo.GetChairCount();
            if (roomInfo.Chairs == null || chairCount == 0)
            {
                sendData.Code = LoginServer.Game.GameStart_S2C.Types.Ret_Code.Failed;
                sendData.Msg = "房间未提供座位";
                ProtosManager.Instance.Unicast(mCSConnect, CmdConfig.GameStart_S2C, sendData);
                return;
            }

            if (!roomInfo.IsFull())
            {
                sendData.Code = LoginServer.Game.GameStart_S2C.Types.Ret_Code.Failed;
                sendData.Msg = "房间未满员";
                ProtosManager.Instance.Unicast(mCSConnect, CmdConfig.GameStart_S2C, sendData);
                return;
            }

            for (int i = 0; i < chairCount; i++)
            {
                if (!roomInfo.Chairs[i].IsRoomOwner() && !roomInfo.Chairs[i].IsReady)
                {
                    sendData.Code = LoginServer.Game.GameStart_S2C.Types.Ret_Code.Failed;
                    sendData.Msg = $"玩家{roomInfo.Chairs[i].UserData.Name}未准备";
                    ProtosManager.Instance.Unicast(mCSConnect, CmdConfig.GameStart_S2C, sendData);
                    return;
                }
            }

            //获取多播连接列表
            var connectList = roomInfo.GetAllUserData().Select(u => u.CSConnect).ToList();

            //多播游戏开始
            sendData.Code = LoginServer.Game.GameStart_S2C.Types.Ret_Code.Success;
            ProtosManager.Instance.Multicast(connectList, CmdConfig.GameStart_S2C, sendData);

            //服务端为游戏玩家随机抽取身份
            List<IdentityType> identityList = Identity.GetIdentityList(roomInfo);
            for (int i = 0; i < roomInfo.GetChairCount(); i++)
            {
                UserData user = roomInfo.Chairs[i].UserData;
                if (user == null) continue;
                user.IdentityType = identityList[i];
                CSConnect connect = user.CSConnect;

                var sendIdentityData = new LoginServer.Game.Identity_S2C();
                sendIdentityData.Identity = identityList[i];
                ProtosManager.Instance.Unicast(connect, CmdConfig.Identity_S2C, sendIdentityData);
            }

            //服务端为游戏玩家随机抽取3张角色牌
            mCharacter.Init();
            for (int i = 0; i < roomInfo.GetChairCount(); i++)
            {
                UserData user = roomInfo.Chairs[i].UserData;
                if (user == null) continue;
                CSConnect connect = user.CSConnect;

                var sendCharacterChooseListData = new LoginServer.Game.CharacterChooseList_S2C();
                sendCharacterChooseListData.Characters.AddRange(mCharacter.GetCharacterChooseList());
                ProtosManager.Instance.Unicast(connect, CmdConfig.CharacterChooseList_S2C, sendCharacterChooseListData);
            }
        }

        private void OnReceiveCharacterChoose(object obj)
        {
            lock (this)
            {
                var data = obj as LoginServer.Game.CharacterChoose_C2S;
                if (data == null)
                    return;

                var user = UserDataManager.Instance.GetUserData(data.UserName);
                user.CharacterType = data.Character;

                var roomInfo = user.RoomInfo;
                for (int i = 0; i < roomInfo.GetChairCount(); i++)
                {
                    if (roomInfo.Chairs[i].UserData == null)
                    {
                        continue;
                    }

                    if (roomInfo.Chairs[i].UserData.CharacterType == CharacterType.None)
                    {
                        return;
                    }
                }
                var sendData = new LoginServer.Game.DealCards_S2C();
                for (int i = 0; i < roomInfo.GetChairCount(); i++)
                {
                    CSConnect connect = roomInfo.Chairs[i].UserData.CSConnect;
                    ProtosManager.Instance.Unicast(connect, CmdConfig.DealCards_S2C, sendData);
                }

            }

        }

    }
}
