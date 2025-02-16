using System.Collections.Generic;
using System.Linq;
using IdentityType = LoginServer.Game.IdentityType;
using CharacterType = LoginServer.Game.CharacterType;
using LoginServer.Game;
using Google.Protobuf.Collections;
using System;
using LoginServer.Room;

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
            ProtosManager.Instance.AddProtosListener(mCSConnect.ID, CmdConfig.InformationDeclarationResponse_C2S, OnReceiveInformationDeclarationResponse);
            ProtosManager.Instance.AddProtosListener(mCSConnect.ID, CmdConfig.InformationDeclarationResponseEnd_C2S, OnReceiveInformationDeclarationResponseEnd);
            ProtosManager.Instance.AddProtosListener(mCSConnect.ID, CmdConfig.InformationTransmit_C2S, OnReceiveInformationTransmit);
            ProtosManager.Instance.AddProtosListener(mCSConnect.ID, CmdConfig.WaitInformationReceive_C2S, OnReceiveWaitInformationReceive);
            ProtosManager.Instance.AddProtosListener(mCSConnect.ID, CmdConfig.InformationReceiveResponse_C2S, OnReceiveInformationReceiveResponse);
            ProtosManager.Instance.AddProtosListener(mCSConnect.ID, CmdConfig.InformationReceiveResponseEnd_C2S, OnReceiveInformationReceiveResponseEnd);
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
            ProtosManager.Instance.RemoveProtosListener(mCSConnect.ID, CmdConfig.InformationDeclarationResponse_C2S, OnReceiveInformationDeclarationResponse);
            ProtosManager.Instance.RemoveProtosListener(mCSConnect.ID, CmdConfig.InformationDeclarationResponseEnd_C2S, OnReceiveInformationDeclarationResponseEnd);
            ProtosManager.Instance.RemoveProtosListener(mCSConnect.ID, CmdConfig.InformationTransmit_C2S, OnReceiveInformationTransmit);
            ProtosManager.Instance.RemoveProtosListener(mCSConnect.ID, CmdConfig.WaitInformationReceive_C2S, OnReceiveWaitInformationReceive);
            ProtosManager.Instance.RemoveProtosListener(mCSConnect.ID, CmdConfig.InformationReceiveResponse_C2S, OnReceiveInformationReceiveResponse);
            ProtosManager.Instance.RemoveProtosListener(mCSConnect.ID, CmdConfig.InformationReceiveResponseEnd_C2S, OnReceiveInformationReceiveResponseEnd);
        }

        private void SendGameStart(RoomInfo roomInfo, List<CSConnect> connectList)
        {
            var sendData = new LoginServer.Game.GameStart_S2C();
            //多播游戏开始
            sendData.Code = LoginServer.Game.GameStart_S2C.Types.Ret_Code.Success;
            sendData.GameCardCount = roomInfo.CardList.Count;
            ProtosManager.Instance.Multicast(connectList, CmdConfig.GameStart_S2C, sendData);

            roomInfo.GameStage = GameStage.WaitIdentityChoose;
        }

        private void SendIdentityChoose(RoomInfo roomInfo)
        {
            List<IdentityType> identityList = Identity.GetIdentityList(roomInfo);
            for (int i = 0; i < roomInfo.GetChairCount(); i++)
            {
                UserData user = roomInfo.Chairs[i].UserData;
                if (user == null) continue;
                roomInfo.Chairs[i].Identity = Identity.GetIdentity(identityList[i]);
                CSConnect connect = user.CSConnect;

                var sendData = new LoginServer.Game.Identity_S2C();
                sendData.Identity = identityList[i];
                ProtosManager.Instance.Unicast(connect, CmdConfig.Identity_S2C, sendData);
            }
            roomInfo.GameStage = GameStage.WaitCharacterChoose;
        }

        private void SendCharacterChoose(RoomInfo roomInfo)
        {
            for (int i = 0; i < roomInfo.GetChairCount(); i++)
            {
                UserData user = roomInfo.Chairs[i].UserData;
                if (user == null) continue;
                CSConnect connect = user.CSConnect;

                var sendData = new LoginServer.Game.CharacterChooseList_S2C();
                sendData.Characters.AddRange(roomInfo.GetCharacterChooseList());
                ProtosManager.Instance.Unicast(connect, CmdConfig.CharacterChooseList_S2C, sendData);
            }
        }

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
            roomInfo.GameStage = GameStage.WaitGameTurn;
        }

        private void SendGameTurn(string userName, RoomInfo roomInfo, List<CSConnect> connectList)
        {
            var sendData = new LoginServer.Game.GameTurn_S2C();
            sendData.UserName = userName;
            ProtosManager.Instance.Multicast(connectList, CmdConfig.GameTurn_S2C, sendData);
            roomInfo.CurrentGameTurnPlayerName = userName;
            roomInfo.GameStage = GameStage.WaitGameTurnStart;
            roomInfo.InformationStage = InformationStage.WaitInformationDeclaration;
        }

        private void SendGameTurnStart(RoomInfo roomInfo)
        {
            var userList = roomInfo.GetAllUserData();
            var connectList = userList.Select(u => u.CSConnect).ToList();
            var sendData = new LoginServer.Game.GameTurnStart_S2C();
            sendData.UserName = roomInfo.CurrentGameTurnPlayerName;
            ProtosManager.Instance.Multicast(connectList, CmdConfig.GameTurnStart_S2C, sendData);
            roomInfo.GameStage = GameStage.WaitDealCards;
        }

        private void SendGameTurnDealCards(RoomInfo roomInfo, List<UserData> userList, List<CSConnect> connectList, int dealCardsCount)
        {
            for (int i = 0; i < roomInfo.GetChairCount(); i++)
            {
                UserData user = roomInfo.Chairs[i].UserData;
                if (user == null || user.Name != roomInfo.CurrentGameTurnPlayerName) continue;
                var sendData = new LoginServer.Game.DealCards_S2C();
                var dealCard = new LoginServer.Game.DealCards();
                dealCard.UserName = roomInfo.CurrentGameTurnPlayerName;
                dealCard.Cards.AddRange(roomInfo.DrawCards(roomInfo.CurrentGameTurnPlayerName, dealCardsCount));
                sendData.DealCards.Add(dealCard);
                sendData.RemainGameCardCount = roomInfo.CardList.Count;
                sendData.DisCardCount = roomInfo.DisCardList.Count;
                ProtosManager.Instance.Multicast(connectList, CmdConfig.DealCards_S2C, sendData);
                roomInfo.GameStage = GameStage.WaitGameTurnOpertateEnd;
                break;
            }
        }

        private void SendGameTurnOpertateEnd(RoomInfo roomInfo, List<CSConnect> connectList)
        {
            var sendData = new LoginServer.Game.GameTurnOpertateEnd_S2C();
            sendData.UserName = roomInfo.CurrentGameTurnPlayerName;
            ProtosManager.Instance.Multicast(connectList, CmdConfig.GameTurnOpertateEnd_S2C, sendData);
            roomInfo.GameStage = GameStage.WaitGameTurnDisCard;
        }

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
            roomInfo.GameStage = GameStage.WaitGameTurnEnd;
        }

        private void SendGameTurnEnd(RoomInfo roomInfo, List<CSConnect> connectList)
        {
            var sendData = new LoginServer.Game.GameTurnEnd_S2C();
            ProtosManager.Instance.Multicast(connectList, CmdConfig.GameTurnEnd_S2C, sendData);
            roomInfo.GameStage = GameStage.WaitGameTurn;
        }

        private void SendHandCardCount(string userName, int handCardCount, List<CSConnect> connectList)
        {
            var sendData = new LoginServer.Game.HandCardCount_S2C();
            sendData.UserName = userName;
            sendData.Count = handCardCount;
            ProtosManager.Instance.Multicast(connectList, CmdConfig.HandCardCount_S2C, sendData);
        }

        private void SendInformationCount(RoomInfo roomInfo, List<UserData> userList, List<CSConnect> connectList)
        {
            var sendData = new LoginServer.Game.InformationCount_S2C();
            for (int i = 0; i < userList.Count; i++) 
            {
                var chair = roomInfo.GetChair(userList[i].Name);
                SendInformationCount(roomInfo, chair, connectList);
            }
        }

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

        private void OnInformationReceiveSuccess(RoomInfo roomInfo, List<CSConnect> connectList)
        {
            var sendData = new LoginServer.Game.InformationReceiveSuccess_S2C();
            sendData.UserName = roomInfo.CurrentAskInformationReceivedPlayerName;
            sendData.Card = roomInfo.InformationCard.Card;
            if (roomInfo.InformationStage == InformationStage.WaitInformationReceiveResponse)
            {
                roomInfo.InformationStage = InformationStage.WaitEnd;

                var chair = roomInfo.GetChair(sendData.UserName);
                chair.ReceiveInformation(roomInfo.InformationCard.Card);

                ProtosManager.Instance.Multicast(connectList, CmdConfig.InformationReceiveSuccess_S2C, sendData);
                SendInformationCount(roomInfo, chair, connectList);

                if (chair.IsVictory(out VictoryType victoryType))
                {
                    SendGameComplete(sendData.UserName, roomInfo, victoryType, connectList);
                }
                else
                {
                    SendGameTurnOpertateEnd(roomInfo, connectList);
                }
            }
        }

        private void SendGameComplete(string userName, RoomInfo roomInfo, VictoryType victoryType, List<CSConnect> connectList)
        {
            var gameCompleteData = new LoginServer.Game.GameComplete_S2C();
            if (victoryType == VictoryType.JunQing)
            {
                for (int i = 0; i < roomInfo.Chairs.Count; i++)
                {
                    if (roomInfo.Chairs[i].UserData == null || roomInfo.Chairs[i].IsNull)
                        continue;

                    if (roomInfo.Chairs[i].Identity.GetIdentity() == IdentityType.JunQing)
                    {
                        gameCompleteData.VictoryList.Add(new GameCompleteItem()
                        {
                            UserName = roomInfo.Chairs[i].UserData.Name,
                            Identity = roomInfo.Chairs[i].Identity.GetIdentity(),
                        });
                    }
                    else
                    {
                        gameCompleteData.DefeateList.Add(new GameCompleteItem()
                        {
                            UserName = roomInfo.Chairs[i].UserData.Name,
                            Identity = roomInfo.Chairs[i].Identity.GetIdentity(),
                        });
                    }
                }
            }
            else if (victoryType == VictoryType.QianFu)
            {
                for (int i = 0; i < roomInfo.Chairs.Count; i++)
                {
                    if (roomInfo.Chairs[i].UserData == null || roomInfo.Chairs[i].IsNull)
                        continue;

                    if (roomInfo.Chairs[i].Identity.GetIdentity() == IdentityType.QianFu)
                    {
                        gameCompleteData.VictoryList.Add(new GameCompleteItem()
                        {
                            UserName = roomInfo.Chairs[i].UserData.Name,
                            Identity = roomInfo.Chairs[i].Identity.GetIdentity(),
                        });
                    }
                    else
                    {
                        gameCompleteData.DefeateList.Add(new GameCompleteItem()
                        {
                            UserName = roomInfo.Chairs[i].UserData.Name,
                            Identity = roomInfo.Chairs[i].Identity.GetIdentity(),
                        });
                    }
                }
            }
            else if (victoryType == VictoryType.PartTeGong)
            {
                for (int i = 0; i < roomInfo.Chairs.Count; i++)
                {
                    if (roomInfo.Chairs[i].UserData == null || roomInfo.Chairs[i].IsNull)
                        continue;

                    if (roomInfo.Chairs[i].Identity.GetIdentity() == IdentityType.TeGong && roomInfo.Chairs[i].IsVictory(out _))
                    {
                        gameCompleteData.VictoryList.Add(new GameCompleteItem()
                        {
                            UserName = roomInfo.Chairs[i].UserData.Name,
                            Identity = roomInfo.Chairs[i].Identity.GetIdentity(),
                        });
                    }
                    else
                    {
                        gameCompleteData.DefeateList.Add(new GameCompleteItem()
                        {
                            UserName = roomInfo.Chairs[i].UserData.Name,
                            Identity = roomInfo.Chairs[i].Identity.GetIdentity(),
                        });
                    }
                }
            }
            else if (victoryType == VictoryType.Single)
            {
                for (int i = 0; i < roomInfo.Chairs.Count; i++)
                {
                    if (roomInfo.Chairs[i].UserData == null || roomInfo.Chairs[i].IsNull)
                        continue;

                    if (roomInfo.Chairs[i].UserData.Name == userName)
                    {
                        gameCompleteData.VictoryList.Add(new GameCompleteItem()
                        {
                            UserName = roomInfo.Chairs[i].UserData.Name,
                            Identity = roomInfo.Chairs[i].Identity.GetIdentity(),
                        });
                    }
                    else
                    {
                        gameCompleteData.DefeateList.Add(new GameCompleteItem()
                        {
                            UserName = roomInfo.Chairs[i].UserData.Name,
                            Identity = roomInfo.Chairs[i].Identity.GetIdentity(),
                        });
                    }
                }
            }
            ProtosManager.Instance.Multicast(connectList, CmdConfig.GameComplete_S2C, gameCompleteData);
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
            roomInfo.GameStart();
            roomInfo.GameStage = GameStage.WaitGameStart;

            if (roomInfo.GameStage == GameStage.WaitGameStart)
            {
                //获取多播连接列表
                var connectList = roomInfo.GetAllUserData().Select(u => u.CSConnect).ToList();
                SendGameStart(roomInfo, connectList);
            }


            //服务端为游戏玩家随机抽取身份
            if (roomInfo.GameStage == GameStage.WaitIdentityChoose)
            {
                SendIdentityChoose(roomInfo);
            }

            //服务端为游戏玩家随机抽取3张角色牌
            if (roomInfo.GameStage == GameStage.WaitCharacterChoose)
            {
                SendCharacterChoose(roomInfo);
            }

        }

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
                    for (int i = 0; i < userList.Count; i++)
                    {
                        SendHandCardCount(userList[i].Name, roomInfo.GetHandCount(userList[i].Name), connectList);
                    }
                    SendInformationCount(roomInfo, userList, connectList);
                }

                Random random = new Random();
                int randomIndex = random.Next(0, userList.Count);
                //int randomIndex = 0;
                string userName = userList[randomIndex].Name;
                if (roomInfo.GameStage == GameStage.WaitGameTurn)
                {
                    SendGameTurn(userName, roomInfo, connectList);
                }
            }

        }

        private void OnReceiveGameTurnStart(object obj)
        {
            var data = obj as LoginServer.Game.GameTurnStart_C2S;
            if (data == null)
                return;

            var roomInfo = UserDataManager.Instance.GetUserData(data.UserName).RoomInfo;
            if (roomInfo.CurrentGameTurnPlayerName == data.UserName && roomInfo.GameStage == GameStage.WaitGameTurnStart)
            {
                SendGameTurnStart(roomInfo);
            }
        }

        private void OnReceiveDealCards(object obj)
        {
            var data = obj as LoginServer.Game.DealCards_C2S;
            if (data == null)
                return;

            var roomInfo = UserDataManager.Instance.GetUserData(data.UserName).RoomInfo;
            if (roomInfo.CurrentGameTurnPlayerName == data.UserName && roomInfo.GameStage == GameStage.WaitDealCards)
            {
                var userList = roomInfo.GetAllUserData();
                var connectList = userList.Select(u => u.CSConnect).ToList();
                SendGameTurnDealCards(roomInfo, userList, connectList, data.Count);
                SendHandCardCount(data.UserName, roomInfo.GetHandCount(data.UserName), connectList);
            }
        }

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

                if (roomInfo.GetHandCount(roomInfo.CurrentGameTurnPlayerName) == 0)
                {
                    SendDealCard(roomInfo, roomInfo.CurrentGameTurnPlayerName, 1, connectList);
                }
            }
        }

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
                SendHandCardCount(data.UserName, roomInfo.GetChair(data.UserName).HandCard.Count, connectList);
            }
        }

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
            }

            string userName = roomInfo.GetNextUserData(data.UserName).Name;
            if (roomInfo.GameStage == GameStage.WaitGameTurn)
            {
                SendGameTurn(userName, roomInfo, connectList);
            }
        }

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
                    roomInfo.Chairs[i].IsSkip =
                        (roomInfo.Chairs[i].UserData == null ||
                        roomInfo.Chairs[i].IsNull ||
                        roomInfo.Chairs[i].UserData.Name == roomInfo.CurrentGameTurnPlayerName);
                }

                var connectList = roomInfo.GetAllUserData().Select(u => u.CSConnect).ToList();
                var sendData = new LoginServer.Game.InformationDeclaration_S2C();
                sendData.UserName = data.UserName;
                ProtosManager.Instance.Multicast(connectList, CmdConfig.InformationDeclaration_S2C, sendData);
                roomInfo.InformationStage = InformationStage.WaitInformationDeclarationResponse;
            }
        }

        private void OnReceiveInformationDeclarationResponse(object obj)
        {
            var data = obj as LoginServer.Game.InformationDeclarationResponse_C2S;
            if (data == null)
                return;

            var roomInfo = UserDataManager.Instance.GetUserData(data.UserName).RoomInfo;
            if (roomInfo.GameStage == GameStage.WaitGameTurnOpertateEnd &&
                roomInfo.InformationStage == InformationStage.WaitInformationDeclarationResponse)
            {
                var chair = roomInfo.GetChair(data.UserName);
                if (chair != null)
                {
                    chair.IsSkip = !data.IsResponse;
                }

                //所有玩家都已经跳过
                chair = roomInfo.Chairs.Find(c => c.IsSkip == false);
                if (chair == null)
                {
                    var connectList = roomInfo.GetAllUserData().Select(u => u.CSConnect).ToList();
                    var sendData = new LoginServer.Game.WaitInformationTransmit_S2C();
                    sendData.UserName = roomInfo.CurrentGameTurnPlayerName;
                    if (roomInfo.InformationStage == InformationStage.WaitInformationDeclarationResponse)
                    {
                        roomInfo.InformationStage = InformationStage.WaitInformationTransmit;
                        ProtosManager.Instance.Multicast(connectList, CmdConfig.WaitInformationTransmit_S2C, sendData);
                    }
                }
                else
                {
                    Console.WriteLine(chair.UserData == null ? "null" : chair.UserData.Name);
                }
            }

        }

        private void OnReceiveInformationDeclarationResponseEnd(object obj)
        {
            var data = obj as LoginServer.Game.InformationDeclarationResponseEnd_C2S;
            if (data == null)
                return;

            var roomInfo = RoomDataManager.Instance.GetRoomInfo(data.RoomNub);
            if (roomInfo.GameStage == GameStage.WaitGameTurnOpertateEnd &&
                roomInfo.InformationStage == InformationStage.WaitInformationDeclarationResponse)
            {
                var connectList = roomInfo.GetAllUserData().Select(u => u.CSConnect).ToList();
                var sendData = new LoginServer.Game.WaitInformationTransmit_S2C();
                sendData.UserName = roomInfo.CurrentGameTurnPlayerName;
                if (roomInfo.InformationStage == InformationStage.WaitInformationDeclarationResponse)
                {
                    roomInfo.InformationStage = InformationStage.WaitInformationTransmit;
                    ProtosManager.Instance.Multicast(connectList, CmdConfig.WaitInformationTransmit_S2C, sendData);
                }
            }

        }

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
                var sendInformationTransmitData = new LoginServer.Game.InformationTransmit_S2C();
                var askUser = roomInfo.GetNextUserData(data.FromUserName, data.Transmit, data.Direction);
                roomInfo.CurrentAskInformationReceivedPlayerName = askUser.Name;
                sendInformationTransmitData.FromUserName = data.FromUserName;
                sendInformationTransmitData.Card = data.Card;
                sendInformationTransmitData.HandCardIndex = data.HandCardIndex;
                sendInformationTransmitData.ToUserName = askUser.Name;
                sendInformationTransmitData.Transmit = data.Transmit;
                sendInformationTransmitData.Direction = data.Direction;
                ProtosManager.Instance.Multicast(connectList, CmdConfig.InformationTransmit_S2C, sendInformationTransmitData);

                var sendWaitInformationReceiveData = new LoginServer.Game.WaitInformationReceive_S2C();
                sendWaitInformationReceiveData.UserName = askUser.Name;
                ProtosManager.Instance.Multicast(connectList, CmdConfig.WaitInformationReceive_S2C, sendWaitInformationReceiveData);
            }
        }

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
                    roomInfo.InformationStage = InformationStage.WaitInformationReceiveResponse;
                    for (int i = 0; i < roomInfo.Chairs.Count; i++)
                    {
                        roomInfo.Chairs[i].IsSkip =
                            (roomInfo.Chairs[i].UserData == null ||
                            roomInfo.Chairs[i].IsNull ||
                            roomInfo.Chairs[i].UserData.Name == roomInfo.CurrentAskInformationReceivedPlayerName);
                    }

                    var sendData = new LoginServer.Game.InformationReceive_S2C();
                    sendData.UserName = data.UserName;
                    ProtosManager.Instance.Multicast(connectList, CmdConfig.InformationReceive_S2C, sendData);
                }
                else
                {
                    var sendData = new LoginServer.Game.WaitInformationReceive_S2C();
                    var askUser = roomInfo.GetNextUserData(data.UserName, roomInfo.InformationCard.Transmit, roomInfo.InformationCard.Direction);
                    roomInfo.CurrentAskInformationReceivedPlayerName = askUser.Name;
                    sendData.UserName = askUser.Name;
                    ProtosManager.Instance.Multicast(connectList, CmdConfig.WaitInformationReceive_S2C, sendData);
                }
            }
        }

        private void OnReceiveInformationReceiveResponse(object obj)
        {
            var data = obj as LoginServer.Game.InformationReceiveResponse_C2S;
            if (data == null)
                return;

            var roomInfo = UserDataManager.Instance.GetUserData(data.UserName).RoomInfo;
            if (roomInfo.GameStage == GameStage.WaitGameTurnOpertateEnd &&
                roomInfo.InformationStage == InformationStage.WaitInformationReceiveResponse)
            {
                var chair = roomInfo.GetChair(data.UserName);
                if (chair != null)
                {
                    chair.IsSkip = !data.IsResponse;
                }

                //所有玩家都已经跳过
                chair = roomInfo.Chairs.Find(c => c.IsSkip == false);
                if (chair == null)
                {
                    var connectList = roomInfo.GetAllUserData().Select(u => u.CSConnect).ToList();
                    OnInformationReceiveSuccess(roomInfo, connectList);
                }
                else
                {
                    Console.WriteLine(chair.UserData == null ? "null" : chair.UserData.Name);
                }
            }
        }

        private void OnReceiveInformationReceiveResponseEnd(object obj)
        {
            var data = obj as LoginServer.Game.InformationReceiveResponseEnd_C2S;
            if (data == null)
                return;

            var roomInfo = RoomDataManager.Instance.GetRoomInfo(data.RoomNub);
            if (roomInfo.GameStage == GameStage.WaitGameTurnOpertateEnd &&
                roomInfo.InformationStage == InformationStage.WaitInformationReceiveResponse)
            {
                var connectList = roomInfo.GetAllUserData().Select(u => u.CSConnect).ToList();
                OnInformationReceiveSuccess(roomInfo, connectList);
            }
        }

    }
}
