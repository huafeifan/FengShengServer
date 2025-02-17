using System.Collections.Generic;
using System.Linq;
using IdentityType = LoginServer.Game.IdentityType;
using CharacterType = LoginServer.Game.CharacterType;
using LoginServer.Game;
using Google.Protobuf.Collections;
using System;
using LoginServer.Room;
using System.Runtime.Remoting.Contexts;
using LoginServer.Login;

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
            ProtosManager.Instance.AddProtosListener(mCSConnect.ID, CmdConfig.GameTurnStart_C2S, OnReceiveGameTurnStart);
            ProtosManager.Instance.AddProtosListener(mCSConnect.ID, CmdConfig.DealCards_C2S, OnReceiveDealCards);
            ProtosManager.Instance.AddProtosListener(mCSConnect.ID, CmdConfig.GameTurnOpertateEnd_C2S, OnReceiveGameTurnOpertateEnd);
            ProtosManager.Instance.AddProtosListener(mCSConnect.ID, CmdConfig.GameTurnDisCard_C2S, OnReceiveGameTurnDisCard);
            ProtosManager.Instance.AddProtosListener(mCSConnect.ID, CmdConfig.GameTurnEnd_C2S, OnReceiveGameTurnEnd);
            ProtosManager.Instance.AddProtosListener(mCSConnect.ID, CmdConfig.InformationDeclaration_C2S, OnReceiveInformationDeclaration);
            ProtosManager.Instance.AddProtosListener(mCSConnect.ID, CmdConfig.PlayHandCardResponse_C2S, OnReceivePlayHandCardResponse);
            ProtosManager.Instance.AddProtosListener(mCSConnect.ID, CmdConfig.InformationTransmit_C2S, OnReceiveInformationTransmit);
            ProtosManager.Instance.AddProtosListener(mCSConnect.ID, CmdConfig.WaitInformationReceive_C2S, OnReceiveWaitInformationReceive);
            ProtosManager.Instance.AddProtosListener(mCSConnect.ID, CmdConfig.PlayHandCard_C2S, OnReceivePlayHandCard);
        }

        public void Close()
        {
            ProtosManager.Instance.RemoveProtosListener(mCSConnect.ID, CmdConfig.GameStart_C2S, OnReceiveGameStart);
            ProtosManager.Instance.RemoveProtosListener(mCSConnect.ID, CmdConfig.CharacterChoose_C2S, OnReceiveCharacterChoose);
            ProtosManager.Instance.RemoveProtosListener(mCSConnect.ID, CmdConfig.GameTurnStart_C2S, OnReceiveGameTurnStart);
            ProtosManager.Instance.RemoveProtosListener(mCSConnect.ID, CmdConfig.DealCards_C2S, OnReceiveDealCards);
            ProtosManager.Instance.RemoveProtosListener(mCSConnect.ID, CmdConfig.GameTurnOpertateEnd_C2S, OnReceiveGameTurnOpertateEnd);
            ProtosManager.Instance.RemoveProtosListener(mCSConnect.ID, CmdConfig.GameTurnDisCard_C2S, OnReceiveGameTurnDisCard);
            ProtosManager.Instance.RemoveProtosListener(mCSConnect.ID, CmdConfig.GameTurnEnd_C2S, OnReceiveGameTurnEnd);
            ProtosManager.Instance.RemoveProtosListener(mCSConnect.ID, CmdConfig.InformationDeclaration_C2S, OnReceiveInformationDeclaration);
            ProtosManager.Instance.RemoveProtosListener(mCSConnect.ID, CmdConfig.PlayHandCardResponse_C2S, OnReceivePlayHandCardResponse);
            ProtosManager.Instance.RemoveProtosListener(mCSConnect.ID, CmdConfig.InformationTransmit_C2S, OnReceiveInformationTransmit);
            ProtosManager.Instance.RemoveProtosListener(mCSConnect.ID, CmdConfig.WaitInformationReceive_C2S, OnReceiveWaitInformationReceive);
            ProtosManager.Instance.RemoveProtosListener(mCSConnect.ID, CmdConfig.PlayHandCard_C2S, OnReceivePlayHandCard);
        }

        /// <summary>
        /// 游戏开始
        /// </summary>
        private void SendGameStart(RoomInfo roomInfo, List<CSConnect> connectList)
        {
            var sendData = new LoginServer.Game.GameStart_S2C();
            sendData.Code = LoginServer.Game.GameStart_S2C.Types.Ret_Code.Success;
            sendData.GameCardCount = roomInfo.CardList.Count;
            ProtosManager.Instance.Multicast(connectList, CmdConfig.GameStart_S2C, sendData);
        }

        /// <summary>
        /// 选择身份
        /// </summary>
        private void IdentityChoose(RoomInfo roomInfo)
        {
            List<IdentityType> identityList = Identity.GetIdentityList(roomInfo);
            for (int i = 0; i < roomInfo.GetChairCount(); i++)
            {
                UserData user = roomInfo.Chairs[i].UserData;
                if (user == null) continue;
                roomInfo.Chairs[i].Identity = Identity.GetIdentity(identityList[i]);
                SendIdentityChoose(identityList[i], user.CSConnect);
            }
        }

        /// <summary>
        /// 发送身份选择
        /// </summary>
        private void SendIdentityChoose(IdentityType identity, CSConnect connect)
        {
            var sendData = new LoginServer.Game.Identity_S2C();
            sendData.Identity = identity;
            ProtosManager.Instance.Unicast(connect, CmdConfig.Identity_S2C, sendData);
        }

        /// <summary>
        /// 选择角色
        /// </summary>
        private void CharacterChoose(RoomInfo roomInfo)
        {
            for (int i = 0; i < roomInfo.GetChairCount(); i++)
            {
                UserData user = roomInfo.Chairs[i].UserData;
                if (user == null) continue;
                SendCharacterChoose(roomInfo.GetCharacterChooseList(), user.CSConnect);
            }
        }

        /// <summary>
        /// 发送角色选择
        /// </summary>
        private void SendCharacterChoose(List<CharacterType> characters, CSConnect connect)
        {
            var sendData = new LoginServer.Game.CharacterChooseList_S2C();
            sendData.Characters.AddRange(characters);
            ProtosManager.Instance.Unicast(connect, CmdConfig.CharacterChooseList_S2C, sendData);
        }

        /// <summary>
        /// 起始手牌
        /// </summary>
        private void SendInitialDealCards(RoomInfo roomInfo, List<UserData> userList, List<CSConnect> connectList)
        {
            var sendData = new LoginServer.Game.DealCards_S2C();
            for (int i = 0; i < userList.Count; i++)
            {
                var dealCard = new LoginServer.Game.DealCards();
                dealCard.UserName = userList[i].Name;
                dealCard.Cards.AddRange(roomInfo.DrawCards(userList[i].Name, 2));
                //dealCard.Cards.AddRange(roomInfo.DrawCards(userList[i].Name, 6));
                sendData.DealCards.Add(dealCard);
            }
            sendData.RemainGameCardCount = roomInfo.CardList.Count;
            sendData.DisCardCount = roomInfo.DisCardList.Count;
            ProtosManager.Instance.Multicast(connectList, CmdConfig.DealCards_S2C, sendData);
        }

        /// <summary>
        /// 轮到某人的回合
        /// </summary>
        private void SendGameTurn(string userName, RoomInfo roomInfo, List<CSConnect> connectList)
        {
            var sendData = new LoginServer.Game.GameTurn_S2C();
            sendData.UserName = userName;
            ProtosManager.Instance.Multicast(connectList, CmdConfig.GameTurn_S2C, sendData);
        }

        /// <summary>
        /// 某人的回合开始
        /// </summary>
        private void SendGameTurnStart(RoomInfo roomInfo)
        {
            var userList = roomInfo.GetAllUserData();
            var connectList = userList.Select(u => u.CSConnect).ToList();
            var sendData = new LoginServer.Game.GameTurnStart_S2C();
            sendData.UserName = roomInfo.CurrentGameTurnPlayerName;
            ProtosManager.Instance.Multicast(connectList, CmdConfig.GameTurnStart_S2C, sendData);
        }

        /// <summary>
        /// 操作结束
        /// </summary>
        private void SendGameTurnOpertateEnd(RoomInfo roomInfo, List<CSConnect> connectList)
        {
            var sendData = new LoginServer.Game.GameTurnOpertateEnd_S2C();
            sendData.UserName = roomInfo.CurrentGameTurnPlayerName;
            ProtosManager.Instance.Multicast(connectList, CmdConfig.GameTurnOpertateEnd_S2C, sendData);
        }

        /// <summary>
        /// 弃牌
        /// </summary>
        private void SendGameTurnDisCard(RoomInfo roomInfo, RepeatedField<CardType> cards, RepeatedField<int> indexs, List<CSConnect> connectList)
        {
            var sendData = new LoginServer.Game.GameTurnDisCard_S2C();
            sendData.UserName = roomInfo.CurrentGameTurnPlayerName;
            for (int i = 0; i < cards.Count; i++)
            {
                bool isSuccess = roomInfo.DisCard(roomInfo.CurrentGameTurnPlayerName, cards[i]);
                if (isSuccess)
                {
                    sendData.Cards.Add(cards[i]);
                    sendData.Indexs.Add(indexs[i]);
                }
            }
            sendData.DisCardCount = roomInfo.DisCardList.Count;
            ProtosManager.Instance.Multicast(connectList, CmdConfig.GameTurnDisCard_S2C, sendData);
        }

        /// <summary>
        /// 回合结束
        /// </summary>
        private void SendGameTurnEnd(RoomInfo roomInfo, List<CSConnect> connectList)
        {
            var sendData = new LoginServer.Game.GameTurnEnd_S2C();
            ProtosManager.Instance.Multicast(connectList, CmdConfig.GameTurnEnd_S2C, sendData);
        }

        /// <summary>
        /// 手牌数量
        /// </summary>
        private void SendHandCardCount(string userName, int handCardCount, List<CSConnect> connectList)
        {
            var sendData = new LoginServer.Game.HandCardCount_S2C();
            sendData.UserName = userName;
            sendData.Count = handCardCount;
            ProtosManager.Instance.Multicast(connectList, CmdConfig.HandCardCount_S2C, sendData);
        }

        /// <summary>
        /// 情报数量
        /// </summary>
        private void SendInformationCount(RoomInfo roomInfo, List<UserData> userList, List<CSConnect> connectList)
        {
            var sendData = new LoginServer.Game.InformationCount_S2C();
            for (int i = 0; i < userList.Count; i++)
            {
                var chair = roomInfo.GetChair(userList[i].Name);
                SendInformationCount(roomInfo, chair, connectList);
            }
        }

        /// <summary>
        /// 情报数量
        /// </summary>
        private void SendInformationCount(RoomInfo roomInfo, ChairInfo chair, List<CSConnect> connectList)
        {
            var sendData = new LoginServer.Game.InformationCount_S2C();
            sendData.Infos.Add(new InformationCount()
            {
                UserName = chair.UserData.Name,
                RedCount = chair.GetInformationCount(Card_ColorType.Red),
                BlueCount = chair.GetInformationCount(Card_ColorType.Blue),
                GrayCount = chair.GetInformationCount(Card_ColorType.Gray),
                RedBlueCount = chair.GetInformationCount(Card_ColorType.RedBlue),
            });
            ProtosManager.Instance.Multicast(connectList, CmdConfig.InformationCount_S2C, sendData);
        }

        /// <summary>
        /// 抽卡
        /// </summary>
        private void SendDealCard(RoomInfo roomInfo, string userName, int dealCardsCount, List<CSConnect> connectList)
        {
            for (int i = 0; i < roomInfo.GetChairCount(); i++)
            {
                UserData user = roomInfo.Chairs[i].UserData;
                if (user == null || user.Name != roomInfo.CurrentGameTurnPlayerName) continue;
                var sendData = new LoginServer.Game.DealCards_S2C();
                var dealCard = new LoginServer.Game.DealCards();
                dealCard.UserName = userName;
                dealCard.Cards.AddRange(roomInfo.DrawCards(userName, dealCardsCount));
                sendData.DealCards.Add(dealCard);
                sendData.RemainGameCardCount = roomInfo.CardList.Count;
                sendData.DisCardCount = roomInfo.DisCardList.Count;
                ProtosManager.Instance.Multicast(connectList, CmdConfig.DealCards_S2C, sendData);
                break;
            }
        }

        /// <summary>
        /// 游戏结束
        /// </summary>
        private void SendGameComplete(string userName, RoomInfo roomInfo, VictoryType victoryType, List<CSConnect> connectList)
        {
            var sendData = new LoginServer.Game.GameComplete_S2C();
            if (victoryType == VictoryType.JunQing || victoryType == VictoryType.QianFu)
            {
                IdentityType type = victoryType == VictoryType.JunQing ? IdentityType.JunQing : IdentityType.QianFu;
                var list = roomInfo.Chairs.Where(c => c.UserData != null && !c.IsNull).ToList();
                foreach(var chair in list)
                {
                    var item = new GameCompleteItem()
                    {
                        UserName = chair.UserData.Name,
                        Identity = chair.Identity.GetIdentity(),
                    };
                    if (item.Identity == type)
                    {
                        sendData.VictoryList.Add(item);
                    }
                    else
                    {
                        sendData.DefeateList.Add(item);
                    }
                }
            }
            else if (victoryType == VictoryType.PartTeGong)
            {
                var list = roomInfo.Chairs.Where(c => c.UserData != null && !c.IsNull).ToList();
                foreach (var chair in list)
                {
                    var item = new GameCompleteItem()
                    {
                        UserName = chair.UserData.Name,
                        Identity = chair.Identity.GetIdentity(),
                    };
                    if (item.Identity == IdentityType.TeGong && chair.IsVictory(out _))
                    {
                        sendData.VictoryList.Add(item);
                    }
                    else
                    {
                        sendData.DefeateList.Add(item);
                    }
                }
            }
            ProtosManager.Instance.Multicast(connectList, CmdConfig.GameComplete_S2C, sendData);
        }

        /// <summary>
        /// 通知玩家可以出牌
        /// </summary>
        private void SendAskPlayHandCard(RoomInfo roomInfo)
        {
            var sendData = new LoginServer.Game.AskPlayHandCard_S2C();
            var connectList = new List<CSConnect>();
            for (int i = 0; i < roomInfo.Chairs.Count; i++)
            {
                if (roomInfo.Chairs[i].IsSkip == false)
                {
                    sendData.UserName.Add(roomInfo.Chairs[i].UserData.Name);
                    connectList.Add(roomInfo.Chairs[i].UserData.CSConnect);
                }
            }
            ProtosManager.Instance.Multicast(connectList, CmdConfig.AskPlayHandCard_S2C, sendData);
        }

        /// <summary>
        /// 游戏开始错误检测
        /// </summary>
        private bool TrySendGameStartError(RoomInfo roomInfo)
        {
            var sendData = new LoginServer.Game.GameStart_S2C();
            if (roomInfo == null)
            {
                sendData.Code = LoginServer.Game.GameStart_S2C.Types.Ret_Code.Failed;
                sendData.Msg = "房间信息不存在";
                ProtosManager.Instance.Unicast(mCSConnect, CmdConfig.GameStart_S2C, sendData);
                return true;
            }

            if (roomInfo.GetUserCount() == 0)
            {
                sendData.Code = LoginServer.Game.GameStart_S2C.Types.Ret_Code.Failed;
                sendData.Msg = "房间中无玩家";
                ProtosManager.Instance.Unicast(mCSConnect, CmdConfig.GameStart_S2C, sendData);
                return true;
            }

            int chairCount = roomInfo.GetChairCount();
            if (roomInfo.Chairs == null || chairCount == 0)
            {
                sendData.Code = LoginServer.Game.GameStart_S2C.Types.Ret_Code.Failed;
                sendData.Msg = "房间未提供座位";
                ProtosManager.Instance.Unicast(mCSConnect, CmdConfig.GameStart_S2C, sendData);
                return true;
            }

            if (!roomInfo.IsFull())
            {
                sendData.Code = LoginServer.Game.GameStart_S2C.Types.Ret_Code.Failed;
                sendData.Msg = "房间未满员";
                ProtosManager.Instance.Unicast(mCSConnect, CmdConfig.GameStart_S2C, sendData);
                return true;
            }

            for (int i = 0; i < chairCount; i++)
            {
                if (!roomInfo.Chairs[i].IsRoomOwner() && !roomInfo.Chairs[i].IsReady)
                {
                    sendData.Code = LoginServer.Game.GameStart_S2C.Types.Ret_Code.Failed;
                    sendData.Msg = $"玩家{roomInfo.Chairs[i].UserData.Name}未准备";
                    ProtosManager.Instance.Unicast(mCSConnect, CmdConfig.GameStart_S2C, sendData);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 收到游戏开始协议
        /// </summary>
        private void OnReceiveGameStart(object obj)
        {
            var data = obj as LoginServer.Game.GameStart_C2S;
            if (data == null)
                return;

            var roomInfo = RoomDataManager.Instance.GetRoomInfo(data.RoomNub);
            if (TrySendGameStartError(roomInfo)) return;

            var sendData = new LoginServer.Game.GameStart_S2C();

            roomInfo.GameStart();
            roomInfo.GameStage = GameStage.WaitGameStart;

            if (roomInfo.GameStage == GameStage.WaitGameStart)
            {
                //获取多播连接列表
                var connectList = roomInfo.GetAllUserData().Select(u => u.CSConnect).ToList();
                SendGameStart(roomInfo, connectList);
                roomInfo.GameStage = GameStage.WaitIdentityChoose;
            }

            //服务端为游戏玩家随机抽取身份
            if (roomInfo.GameStage == GameStage.WaitIdentityChoose)
            {
                IdentityChoose(roomInfo);
                roomInfo.GameStage = GameStage.WaitCharacterChoose;
            }

            //服务端为游戏玩家随机抽取3张角色牌
            if (roomInfo.GameStage == GameStage.WaitCharacterChoose)
            {
                CharacterChoose(roomInfo);
            }

        }

        /// <summary>
        /// 收到角色选择协议
        /// </summary>
        private void OnReceiveCharacterChoose(object obj)
        {
            lock (this)
            {
                var data = obj as LoginServer.Game.CharacterChoose_C2S;
                if (data == null)
                    return;

                var roomInfo = UserDataManager.Instance.GetUserData(data.UserName).RoomInfo;
                roomInfo.GetChair(data.UserName).Character = Character.GetCharacter(data.Character);
                for (int i = 0; i < roomInfo.GetChairCount(); i++)
                {
                    if (roomInfo.Chairs[i].IsNull || roomInfo.Chairs[i].UserData == null)
                    {
                        continue;
                    }

                    if (roomInfo.Chairs[i].Character == null)
                    {
                        return;
                    }
                }

                //当所有玩家都选择完角色，开始发牌阶段
                roomInfo.GameStage = GameStage.WaitDealCards;

                var userList = roomInfo.GetAllUserData();
                var connectList = userList.Select(u => u.CSConnect).ToList();
                if (roomInfo.GameStage == GameStage.WaitDealCards)
                {
                    SendInitialDealCards(roomInfo, userList, connectList);
                    roomInfo.GameStage = GameStage.WaitGameTurn;
                    for (int i = 0; i < userList.Count; i++)
                    {
                        SendHandCardCount(userList[i].Name, roomInfo.GetHandCount(userList[i].Name), connectList);
                    }
                    SendInformationCount(roomInfo, userList, connectList);
                }

                //Random random = new Random();
                //int randomIndex = random.Next(0, userList.Count);
                int randomIndex = 0;
                string userName = userList[randomIndex].Name;
                if (roomInfo.GameStage == GameStage.WaitGameTurn)
                {
                    SendGameTurn(userName, roomInfo, connectList);
                    roomInfo.CurrentGameTurnPlayerName = userName;
                    roomInfo.GameStage = GameStage.WaitGameTurnStart;
                    roomInfo.InformationStage = InformationStage.WaitInformationDeclaration;
                }
            }

        }

        /// <summary>
        /// 收到玩家回合开始协议
        /// </summary>
        private void OnReceiveGameTurnStart(object obj)
        {
            var data = obj as LoginServer.Game.GameTurnStart_C2S;
            if (data == null)
                return;

            var roomInfo = UserDataManager.Instance.GetUserData(data.UserName).RoomInfo;
            if (roomInfo.CurrentGameTurnPlayerName == data.UserName && roomInfo.GameStage == GameStage.WaitGameTurnStart)
            {
                SendGameTurnStart(roomInfo);
                roomInfo.CurrentPlayHandCardPlayerName = string.Empty;
                roomInfo.CurrentAskInformationReceivedPlayerName = string.Empty;
                roomInfo.GameStage = GameStage.WaitDealCards;
            }
        }

        /// <summary>
        /// 收到抽牌协议
        /// </summary>
        private void OnReceiveDealCards(object obj)
        {
            var data = obj as LoginServer.Game.DealCards_C2S;
            if (data == null)
                return;

            var roomInfo = UserDataManager.Instance.GetUserData(data.UserName).RoomInfo;
            if (roomInfo.CurrentGameTurnPlayerName == data.UserName)
            {
                if (roomInfo.GameStage == GameStage.WaitDealCards)
                {
                    roomInfo.GameStage = GameStage.WaitGameTurnOpertateEnd;
                }

                var userList = roomInfo.GetAllUserData();
                var connectList = userList.Select(u => u.CSConnect).ToList();
                SendDealCard(roomInfo, roomInfo.CurrentGameTurnPlayerName, data.Count, connectList);
                SendHandCardCount(data.UserName, roomInfo.GetHandCount(data.UserName), connectList);
            }
        }

        /// <summary>
        /// 收到玩家回合操作结束协议
        /// </summary>
        private void OnReceiveGameTurnOpertateEnd(object obj)
        {
            var data = obj as LoginServer.Game.GameTurnOpertateEnd_C2S;
            if (data == null)
                return;

            var roomInfo = UserDataManager.Instance.GetUserData(data.UserName).RoomInfo;
            if (roomInfo.CurrentGameTurnPlayerName == data.UserName && roomInfo.GameStage == GameStage.WaitGameTurnOpertateEnd)
            {
                var userList = roomInfo.GetAllUserData();
                var connectList = userList.Select(u => u.CSConnect).ToList();
                SendGameTurnOpertateEnd(roomInfo, connectList);
                roomInfo.GameStage = GameStage.WaitGameTurnDisCard;
            }
        }

        /// <summary>
        /// 收到回合弃牌协议
        /// </summary>
        private void OnReceiveGameTurnDisCard(object obj)
        {
            var data = obj as LoginServer.Game.GameTurnDisCard_C2S;
            if (data == null)
                return;

            var roomInfo = UserDataManager.Instance.GetUserData(data.UserName).RoomInfo;
            if (roomInfo.CurrentGameTurnPlayerName == data.UserName && roomInfo.GameStage == GameStage.WaitGameTurnDisCard)
            {
                var userList = roomInfo.GetAllUserData();
                var connectList = userList.Select(u => u.CSConnect).ToList();
                SendGameTurnDisCard(roomInfo, data.Cards, data.Indexs, connectList);
                roomInfo.GameStage = GameStage.WaitGameTurnEnd;
                SendHandCardCount(data.UserName, roomInfo.GetChair(data.UserName).HandCard.Count, connectList);
            }
        }

        /// <summary>
        /// 收到玩家回合结束协议
        /// </summary>
        private void OnReceiveGameTurnEnd(object obj)
        {
            var data = obj as LoginServer.Game.GameTurnEnd_C2S;
            if (data == null)
                return;

            var roomInfo = UserDataManager.Instance.GetUserData(data.UserName).RoomInfo;
            var userList = roomInfo.GetAllUserData();
            var connectList = userList.Select(u => u.CSConnect).ToList();
            if (roomInfo.GameStage == GameStage.WaitGameTurnEnd)
            {
                SendGameTurnEnd(roomInfo, connectList);
                roomInfo.GameStage = GameStage.WaitGameTurn;
            }

            string userName = roomInfo.GetNextUserData(data.UserName).Name;
            if (roomInfo.GameStage == GameStage.WaitGameTurn)
            {
                SendGameTurn(userName, roomInfo, connectList);
                roomInfo.CurrentGameTurnPlayerName = userName;
                roomInfo.GameStage = GameStage.WaitGameTurnStart;
                roomInfo.InformationStage = InformationStage.WaitInformationDeclaration;
            }
        }

        /// <summary>
        /// 开始情报宣言
        /// </summary>
        private void SendInformationDeclaration(string userName, RoomInfo roomInfo)
        {
            var connectList = roomInfo.GetAllUserData().Select(u => u.CSConnect).ToList();
            var sendData = new LoginServer.Game.InformationDeclaration_S2C();
            sendData.UserName = userName;
            ProtosManager.Instance.Multicast(connectList, CmdConfig.InformationDeclaration_S2C, sendData);
        }

        /// <summary>
        /// 收到情报宣言
        /// </summary>
        private void OnReceiveInformationDeclaration(object obj)
        {
            var data = obj as LoginServer.Game.InformationDeclaration_C2S;
            if (data == null)
                return;

            var roomInfo = UserDataManager.Instance.GetUserData(data.UserName).RoomInfo;
            if (roomInfo.GameStage == GameStage.WaitGameTurnOpertateEnd &&
                roomInfo.InformationStage == InformationStage.WaitInformationDeclaration)
            {
                for (int i = 0; i < roomInfo.Chairs.Count; i++)
                {
                    if (roomInfo.Chairs[i].UserData == null || roomInfo.Chairs[i].IsNull ||
                        roomInfo.Chairs[i].UserData.Name == roomInfo.CurrentGameTurnPlayerName ||
                        roomInfo.Chairs[i].HandCard.Count == 0)
                    {
                        roomInfo.Chairs[i].IsSkip = true;
                    }
                    else
                    {
                        roomInfo.Chairs[i].IsSkip = false;
                    }
                }

                SendInformationDeclaration(data.UserName, roomInfo);
                roomInfo.PlayCardStage = PlayCardStage.WaitPlayerRequestHandCard;
                SendAskPlayHandCard(roomInfo);
            }
        }

        /// <summary>
        /// 等待玩家出牌
        /// </summary>
        private void SendWaitPlayerPlayHandCard(string userName, RoomInfo roomInfo)
        {
            var connectList = roomInfo.GetAllUserData().Select(u => u.CSConnect).ToList();
            var sendData = new LoginServer.Game.PlayHandCardResponse_S2C();
            sendData.UserName = userName;
            sendData.IsSkip = false;
            ProtosManager.Instance.Multicast(connectList, CmdConfig.PlayHandCardResponse_S2C, sendData);
        }

        /// <summary>
        /// 通知情报可以开始传递
        /// </summary>
        private void SendInformationTransmit(RoomInfo roomInfo, List<CSConnect> connectList)
        {
            var sendData = new LoginServer.Game.WaitInformationTransmit_S2C();
            sendData.UserName = roomInfo.CurrentGameTurnPlayerName;
            ProtosManager.Instance.Multicast(connectList, CmdConfig.WaitInformationTransmit_S2C, sendData);
        }

        /// <summary>
        /// 通知玩家情报接收成功
        /// </summary>
        private void SendReceiveInformationSuccess(RoomInfo roomInfo, List<CSConnect> connectList)
        {
            var sendData = new LoginServer.Game.InformationReceiveSuccess_S2C();
            sendData.UserName = roomInfo.CurrentAskInformationReceivedPlayerName;
            sendData.Card = roomInfo.InformationCard.Card;
            ProtosManager.Instance.Multicast(connectList, CmdConfig.InformationReceiveSuccess_S2C, sendData);
        }

        /// <summary>
        /// 通知某玩家出牌结束
        /// </summary>
        private void SendPlayerResponseHandCardLinkEnd(RoomInfo roomInfo)
        {
            var connectList = roomInfo.GetAllUserData().Select(u => u.CSConnect).ToList();
            var sendData = new LoginServer.Game.PlayHandCardResponse_S2C();
            sendData.UserName = roomInfo.CurrentPlayHandCardPlayerName;
            sendData.IsSkip = true;
            ProtosManager.Instance.Multicast(connectList, CmdConfig.PlayHandCardResponse_S2C, sendData);
        }

        /// <summary>
        /// 收到玩家出牌需求回复协议
        /// </summary>
        private void OnReceivePlayHandCardResponse(object obj)
        {
            var data = obj as LoginServer.Game.PlayHandCardResponse_C2S;
            if (data == null)
                return;

            var roomInfo = UserDataManager.Instance.GetUserData(data.UserName).RoomInfo;
            if (roomInfo.GameStage == GameStage.WaitGameTurnOpertateEnd)
            {
                //等待玩家请求出牌阶段时,玩家请求出牌,获得出牌机会
                if (roomInfo.PlayCardStage == PlayCardStage.WaitPlayerRequestHandCard && data.IsSkip == false)
                {
                    roomInfo.PlayCardStage = PlayCardStage.WaitPlayerPlayHandCard;
                    roomInfo.CurrentPlayHandCardPlayerName = data.UserName;
                    SendWaitPlayerPlayHandCard(data.UserName, roomInfo);
                    return;
                }

                if (roomInfo.CurrentPlayHandCardPlayerName == data.UserName && data.IsSkip == true)
                {
                    SendPlayerResponseHandCardLinkEnd(roomInfo);
                    roomInfo.CurrentPlayHandCardPlayerName = string.Empty;
                }

                var chair = roomInfo.GetChair(data.UserName);
                chair.IsSkip = true;
                chair = roomInfo.Chairs.Find(c => c.IsSkip == false);
                if (chair != null)
                {
                    //获得出牌机会的玩家跳过、不出牌时,询问其他玩家是否需要出牌
                    if (roomInfo.PlayCardStage == PlayCardStage.WaitPlayerPlayHandCard && data.IsSkip == true)
                    {
                        roomInfo.PlayCardStage = PlayCardStage.WaitPlayerRequestHandCard;
                        SendAskPlayHandCard(roomInfo);
                    }
                    else
                    {
                        Console.WriteLine(chair.UserData == null ? "null" : chair.UserData.Name);
                    }
                    return;
                }

                if (roomInfo.CurrentPlayHandCardPlayerName != string.Empty)
                {
                    return;
                }

                var connectList = roomInfo.GetAllUserData().Select(u => u.CSConnect).ToList();
                //所有玩家都已经跳过
                //情报宣言阶段 => 情报传递阶段
                if (roomInfo.InformationStage == InformationStage.WaitInformationDeclaration)
                {
                    roomInfo.InformationStage = InformationStage.WaitInformationTransmit;
                    SendInformationTransmit(roomInfo, connectList);
                }
                //情报接收阶段 => 情报接收成功 => 结算/操作结束
                else if (roomInfo.InformationStage == InformationStage.WaitInformationReceive)
                {
                    roomInfo.InformationStage = InformationStage.WaitEnd;

                    var receiveInformationChair = roomInfo.GetChair(roomInfo.CurrentAskInformationReceivedPlayerName);
                    receiveInformationChair.ReceiveInformation(roomInfo.InformationCard.Card);

                    SendReceiveInformationSuccess(roomInfo, connectList);
                    SendInformationCount(roomInfo, receiveInformationChair, connectList);
                    if (receiveInformationChair.IsVictory(out VictoryType victoryType))
                    {
                        SendGameComplete(roomInfo.CurrentAskInformationReceivedPlayerName, roomInfo, victoryType, connectList);
                    }
                    else
                    {
                        SendGameTurnOpertateEnd(roomInfo, connectList);
                        roomInfo.GameStage = GameStage.WaitGameTurnDisCard;
                    }
                }
            }

        }

        /// <summary>
        /// 通知要传递的情报的具体信息
        /// </summary>
        private void SendInformationTransmitInfo(string toUserName, RoomInfo roomInfo, InformationTransmit_C2S data, List<CSConnect> connectList)
        {
            var sendData = new LoginServer.Game.InformationTransmit_S2C();
            sendData.FromUserName = data.FromUserName;
            sendData.Card = data.Card;
            sendData.HandCardIndex = data.HandCardIndex;
            sendData.ToUserName = toUserName;
            sendData.Transmit = data.Transmit;
            sendData.Direction = data.Direction;
            ProtosManager.Instance.Multicast(connectList, CmdConfig.InformationTransmit_S2C, sendData);
        }

        /// <summary>
        /// 询问是否接收情报
        /// </summary>
        private void SendAskInformationReceive(string userName, List<CSConnect> connectList)
        {
            var sendData = new LoginServer.Game.WaitInformationReceive_S2C();
            sendData.UserName = userName;
            ProtosManager.Instance.Multicast(connectList, CmdConfig.WaitInformationReceive_S2C, sendData);
        }

        /// <summary>
        /// 收到情报传递协议
        /// </summary>
        private void OnReceiveInformationTransmit(object obj)
        {
            var data = obj as LoginServer.Game.InformationTransmit_C2S;
            if (data == null)
                return;

            var roomInfo = UserDataManager.Instance.GetUserData(data.FromUserName).RoomInfo;
            if (roomInfo.GameStage == GameStage.WaitGameTurnOpertateEnd &&
                roomInfo.InformationStage == InformationStage.WaitInformationTransmit)
            {
                roomInfo.InformationStage = InformationStage.WaitInformationReceive;
                roomInfo.InformationTransmit(data.FromUserName, data);

                var connectList = roomInfo.GetAllUserData().Select(u => u.CSConnect).ToList();
                var askUser = roomInfo.GetNextUserData(data.FromUserName, data.Transmit, data.Direction);
                roomInfo.CurrentAskInformationReceivedPlayerName = askUser.Name;

                SendInformationTransmitInfo(askUser.Name, roomInfo, data, connectList);
                SendAskInformationReceive(askUser.Name, connectList);
            }
        }

        /// <summary>
        /// 玩家准备接收情报通知
        /// </summary>
        private void SendReceiveInformation(string userName, List<CSConnect> connectList)
        {
            var sendData = new LoginServer.Game.InformationReceive_S2C();
            sendData.UserName = userName;
            ProtosManager.Instance.Multicast(connectList, CmdConfig.InformationReceive_S2C, sendData);
        }

        /// <summary>
        /// 收到等待情报接收协议
        /// </summary>
        private void OnReceiveWaitInformationReceive(object obj)
        {
            var data = obj as LoginServer.Game.WaitInformationReceive_C2S;
            if (data == null)
                return;

            var roomInfo = UserDataManager.Instance.GetUserData(data.UserName).RoomInfo;
            if (roomInfo.GameStage == GameStage.WaitGameTurnOpertateEnd &&
                roomInfo.InformationStage == InformationStage.WaitInformationReceive)
            {
                var connectList = roomInfo.GetAllUserData().Select(u => u.CSConnect).ToList();
                if (data.IsReceive)
                {
                    for (int i = 0; i < roomInfo.Chairs.Count; i++)
                    {
                        if (roomInfo.Chairs[i].UserData == null || roomInfo.Chairs[i].IsNull ||
                            roomInfo.Chairs[i].UserData.Name == roomInfo.CurrentAskInformationReceivedPlayerName ||
                            roomInfo.Chairs[i].HandCard.Count == 0)
                        {
                            roomInfo.Chairs[i].IsSkip = true;
                        }
                        else
                        {
                            roomInfo.Chairs[i].IsSkip = false;
                        }
                    }
                    SendReceiveInformation(data.UserName, connectList);
                    roomInfo.PlayCardStage = PlayCardStage.WaitPlayerRequestHandCard;
                    SendAskPlayHandCard(roomInfo);
                }
                else
                {
                    var askUser = roomInfo.GetNextUserData(data.UserName, roomInfo.InformationCard.Transmit, roomInfo.InformationCard.Direction);
                    roomInfo.CurrentAskInformationReceivedPlayerName = askUser.Name;
                    SendAskInformationReceive(askUser.Name, connectList);
                }
            }
        }

        /// <summary>
        /// 打出手牌失败
        /// </summary>
        private void SendPlayHandCardError(RoomInfo roomInfo, LoginServer.Game.PlayHandCard_C2S data)
        {
            var userData = UserDataManager.Instance.GetUserData(data.UserName);
            var sendData = new LoginServer.Game.PlayHandCard_S2C();
            sendData.UserName = data.UserName;
            sendData.Card = data.Card;
            sendData.HandCardIndex = data.HandCardIndex;
            sendData.Code = PlayHandCard_S2C.Types.Ret_Code.Failed;
            sendData.Msg = "不存在的手牌";
            ProtosManager.Instance.Unicast(userData.CSConnect, CmdConfig.PlayHandCard_S2C, sendData);
        }

        /// <summary>
        /// 打出手牌成功回复
        /// </summary>
        private void SendPlayHandCard(RoomInfo roomInfo, LoginServer.Game.PlayHandCard_C2S data, List<CSConnect> connectList)
        {
            var sendData = new LoginServer.Game.PlayHandCard_S2C();
            sendData.UserName = data.UserName;
            sendData.Card = data.Card;
            sendData.HandCardIndex = data.HandCardIndex;
            sendData.Code = PlayHandCard_S2C.Types.Ret_Code.Success;
            sendData.Msg = string.Empty;
            ProtosManager.Instance.Multicast(connectList, CmdConfig.PlayHandCard_S2C, sendData);
        }

        /// <summary>
        /// 收到出牌协议
        /// </summary>
        private void OnReceivePlayHandCard(object obj)
        {
            var data = obj as LoginServer.Game.PlayHandCard_C2S;
            if (data == null)
                return;

            var userData = UserDataManager.Instance.GetUserData(data.UserName);
            var roomInfo = userData.RoomInfo;
            if (roomInfo.GameStage == GameStage.WaitGameTurnOpertateEnd)
            {
                if (!roomInfo.UseCard(data.UserName, data.Card))
                {
                    SendPlayHandCardError(roomInfo, data);
                    return;
                }

                roomInfo.CurrentPlayHandCardPlayerName = string.Empty;
                for (int i = 0; i < roomInfo.Chairs.Count; i++)
                {
                    if (roomInfo.Chairs[i].IsNull == true || roomInfo.Chairs[i].UserData == null ||
                        roomInfo.Chairs[i].UserData.Name == data.UserName ||
                        roomInfo.Chairs[i].HandCard.Count == 0)
                    {
                        roomInfo.Chairs[i].IsSkip = true;
                    }
                    else
                    {
                        roomInfo.Chairs[i].IsSkip = false;
                    }
                }
                var connectList = roomInfo.GetAllUserData().Select(u => u.CSConnect).ToList();
                SendPlayHandCard(roomInfo, data, connectList);
                SendHandCardCount(data.UserName, roomInfo.GetHandCount(data.UserName), connectList);
                roomInfo.PlayCardStage = PlayCardStage.WaitPlayerRequestHandCard;
                SendAskPlayHandCard(roomInfo);
            }
        }

    }
}
