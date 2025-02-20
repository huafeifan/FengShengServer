using System.Collections.Generic;
using System.Linq;
using IdentityType = LoginServer.Game.IdentityType;
using CharacterType = LoginServer.Game.CharacterType;
using LoginServer.Game;
using System;
using System.Threading.Tasks;
using LoginServer.Room;

namespace FengShengServer
{
    public class Game
    {
        private CSConnect mCSConnect;
        private RoomInfo mRoomInfo;
        private List<ChairInfo> mChairList;
        private List<UserData> mUserList;
        private List<CSConnect> mConnectList;
        private Func<bool> mUpdateCache;
        private Task mGameTask;

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
            ProtosManager.Instance.AddProtosListener(mCSConnect.ID, CmdConfig.InformationTransmitReady_C2S, OnReceiveInformationTransmitReady);
            ProtosManager.Instance.AddProtosListener(mCSConnect.ID, CmdConfig.WaitInformationReceive_C2S, OnReceiveWaitInformationReceive);
            ProtosManager.Instance.AddProtosListener(mCSConnect.ID, CmdConfig.PlayHandCard_C2S, OnReceivePlayHandCard);
            ProtosManager.Instance.AddProtosListener(mCSConnect.ID, CmdConfig.TriggerCardEnd_C2S, OnReceiveTriggerCardEnd);
            ProtosManager.Instance.AddProtosListener(mCSConnect.ID, CmdConfig.AskUseShiPo_C2S, OnReceiveAskUseShiPo);
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
            ProtosManager.Instance.RemoveProtosListener(mCSConnect.ID, CmdConfig.InformationTransmitReady_C2S, OnReceiveInformationTransmitReady);
            ProtosManager.Instance.RemoveProtosListener(mCSConnect.ID, CmdConfig.WaitInformationReceive_C2S, OnReceiveWaitInformationReceive);
            ProtosManager.Instance.RemoveProtosListener(mCSConnect.ID, CmdConfig.PlayHandCard_C2S, OnReceivePlayHandCard);
            ProtosManager.Instance.RemoveProtosListener(mCSConnect.ID, CmdConfig.TriggerCardEnd_C2S, OnReceiveTriggerCardEnd);
            ProtosManager.Instance.RemoveProtosListener(mCSConnect.ID, CmdConfig.AskUseShiPo_C2S, OnReceiveAskUseShiPo);
        }

        /// <summary>
        /// 游戏阶段轮询
        /// </summary>
        private async void Update()
        {
            await Task.Delay(50);
            while (mRoomInfo.GameStageQueue.Count > 0)
            {
                await Task.Delay(100);
                if (mRoomInfo.PlayCardStageQueue.Count > 0)
                {
                    mUpdateCache = mRoomInfo.PlayCardStageQueue.Peek();
                    if (mUpdateCache.Invoke())
                    {
                        mRoomInfo.PlayCardStageQueue.Dequeue();
                    }
                    continue;
                }

                if (mRoomInfo.InformationStageQueue.Count > 0)
                {
                    mUpdateCache = mRoomInfo.InformationStageQueue.Peek();
                    if (mUpdateCache.Invoke())
                    {
                        mRoomInfo.InformationStageQueue.Dequeue();
                    }
                    continue;
                }

                if (mRoomInfo.GameStageQueue.Count > 0)
                {
                    mUpdateCache = mRoomInfo.GameStageQueue.Peek();
                    if (mUpdateCache.Invoke())
                    {
                        mRoomInfo.GameStageQueue.Dequeue();
                    }
                    continue;
                }
            }
        }

        #region GameStage
        /// <summary>
        /// 收到游戏开始协议
        /// </summary>
        private void OnReceiveGameStart(object obj)
        {
            var data = obj as LoginServer.Game.GameStart_C2S;
            if (data == null)
                return;

            mRoomInfo = RoomDataManager.Instance.GetRoomInfo(data.RoomNub);
            if (TrySendGameStartError()) return;
            mChairList = mRoomInfo.ChairListCache = mRoomInfo.Chairs.Where(c => !c.IsNull && c.UserData != null).ToList();
            mUserList = mRoomInfo.UserListCache = mRoomInfo.GetAllUserData();
            mConnectList = mRoomInfo.ConnectListCache = mUserList.Select(u => u.CSConnect).ToList();

            mRoomInfo.GameStart();
            if (mGameTask != null)
            {
                mGameTask.Dispose();
                mGameTask = null;
            }
            mGameTask = Task.Run(() => Update());

            //配置游戏阶段顺序
            //游戏开始
            mRoomInfo.GameStageQueue.Enqueue(GameStage1);
            //选身份
            mRoomInfo.GameStageQueue.Enqueue(GameStage2);
            //选角色
            mRoomInfo.GameStageQueue.Enqueue(GameStage3);
            //等待角色选择完成
            mRoomInfo.GameStageQueue.Enqueue(GameStage4);
            //发起始手牌
            mRoomInfo.GameStageQueue.Enqueue(GameStage5);
            //随机选择玩家,切换到他的回合
            mRoomInfo.GameStageQueue.Enqueue(GameStage6);
        }

        /// <summary>
        /// 游戏开始
        /// </summary>
        private bool GameStage1()
        {
            mRoomInfo.GameStage = GameStage.GameStart;
            var sendData = new LoginServer.Game.GameStart_S2C();
            sendData.Code = LoginServer.Game.GameStart_S2C.Types.Ret_Code.Success;
            sendData.GameCardCount = mRoomInfo.CardList.Count;
            ProtosManager.Instance.Multicast(mConnectList, CmdConfig.GameStart_S2C, sendData);
            return true;
        }

        /// <summary>
        /// 选身份
        /// </summary>
        private bool GameStage2()
        {
            mRoomInfo.GameStage = GameStage.IdentityChoose;
            List<IdentityType> identityList = Identity.GetIdentityList(mRoomInfo);
            for (int i = 0; i < mRoomInfo.GetChairCount(); i++)
            {
                UserData user = mRoomInfo.Chairs[i].UserData;
                if (user == null) continue;
                mRoomInfo.Chairs[i].Identity = Identity.GetIdentity(identityList[i]);
                var sendData = new LoginServer.Game.Identity_S2C();
                sendData.Identity = identityList[i];
                ProtosManager.Instance.Unicast(user.CSConnect, CmdConfig.Identity_S2C, sendData);
            }
            return true;
        }

        /// <summary>
        /// 选角色
        /// </summary>
        private bool GameStage3()
        {
            mRoomInfo.GameStage = GameStage.CharacterChoose;
            for (int i = 0; i < mRoomInfo.GetChairCount(); i++)
            {
                UserData user = mRoomInfo.Chairs[i].UserData;
                if (user == null) continue;
                var sendData = new LoginServer.Game.CharacterChooseList_S2C();
                sendData.Characters.AddRange(mRoomInfo.GetCharacterChooseList());
                ProtosManager.Instance.Unicast(user.CSConnect, CmdConfig.CharacterChooseList_S2C, sendData);
            }
            return true;
        }

        /// <summary>
        /// 等待角色选择完成
        /// </summary>
        private bool GameStage4()
        {
            return mChairList.Find(c => c.Character == null) == null;
        }

        /// <summary>
        /// 发起始手牌
        /// </summary>
        private bool GameStage5()
        {
            mRoomInfo.GameStage = GameStage.DealCards;
            SendInitialDealCards();
            for (int i = 0; i < mUserList.Count; i++)
            {
                SendHandCardCount(mUserList[i].Name, mRoomInfo.GetHandCount(mUserList[i].Name));
            }
            SendInformationCount();
            return true;
        }

        /// <summary>
        /// 随机选择玩家,切换到他的回合
        /// </summary>
        private bool GameStage6()
        {
            mRoomInfo.GameStage = GameStage.GameTurn;
            mRoomInfo.Data.GameTurnStart_C2S = null;
            ChooseRandomPlayerGameTurn();
            mRoomInfo.GameStageQueue.Enqueue(GameStage7);
            mRoomInfo.GameStageQueue.Enqueue(GameStage8);
            return true;
        }

        /// <summary>
        /// 等待玩家发送回合开始
        /// </summary>
        private bool GameStage7()
        {
            return mRoomInfo.Data.GameTurnStart_C2S != null && mRoomInfo.CurrentGameTurnPlayerName == mRoomInfo.Data.GameTurnStart_C2S.UserName;
        }

        /// <summary>
        /// 通知全桌某玩家回合开始
        /// </summary>
        private bool GameStage8()
        {
            mRoomInfo.GameStage = GameStage.GameTurnStart;
            mRoomInfo.CurrentPlayHandCardPlayerName = string.Empty;
            mRoomInfo.CurrentAskInformationReceivedPlayerName = string.Empty;
            mRoomInfo.Data.DealCards_C2S = null;
            mRoomInfo.Data.IsGameTurnOpertateEnd = false;
            SendGameTurnStart();
            mRoomInfo.GameStageQueue.Enqueue(GameStage9);
            mRoomInfo.GameStageQueue.Enqueue(GameStage10);
            return true;
        }

        /// <summary>
        /// 等待玩家发送抽牌
        /// </summary>
        private bool GameStage9()
        {
            return mRoomInfo.Data.DealCards_C2S != null && mRoomInfo.CurrentGameTurnPlayerName == mRoomInfo.Data.DealCards_C2S.UserName;
        }

        /// <summary>
        /// 通知全桌某玩家回合抽卡
        /// </summary>
        private bool GameStage10()
        {
            mRoomInfo.GameStage = GameStage.DealCards;
            mRoomInfo.Data.GameTurnOpertateEnd_C2S = null;
            var data = mRoomInfo.Data.DealCards_C2S;
            SendDealCard(data.UserName, data.Count);
            SendHandCardCount(data.UserName, mRoomInfo.GetHandCount(data.UserName));
            mRoomInfo.Data.PlayHandCard_C2S = null;
            mRoomInfo.InformationStageQueue.Enqueue(InformationStage9);
            mRoomInfo.GameStageQueue.Enqueue(GameStage11);
            mRoomInfo.GameStageQueue.Enqueue(GameStage12);
            return true;
        }

        /// <summary>
        /// 等待玩家回合操作结束
        /// </summary>
        private bool GameStage11()
        {
            return mRoomInfo.Data.IsGameTurnOpertateEnd || (mRoomInfo.Data.GameTurnOpertateEnd_C2S != null &&
                mRoomInfo.CurrentGameTurnPlayerName == mRoomInfo.Data.GameTurnOpertateEnd_C2S.UserName);
        }

        /// <summary>
        /// 通知全桌某玩家回合操作结束
        /// </summary>
        /// <returns></returns>
        private bool GameStage12()
        {
            mRoomInfo.GameStage = GameStage.GameTurnOpertateEnd;
            mRoomInfo.Data.GameTurnDisCard_C2S = null;
            SendGameTurnOpertateEnd();
            mRoomInfo.GameStageQueue.Enqueue(GameStage13);
            mRoomInfo.GameStageQueue.Enqueue(GameStage14);
            return true;
        }
        
        /// <summary>
        /// 等待玩家回合结束前弃牌
        /// </summary>
        private bool GameStage13()
        {
            return mRoomInfo.Data.GameTurnDisCard_C2S != null && mRoomInfo.CurrentGameTurnPlayerName == mRoomInfo.Data.GameTurnDisCard_C2S.UserName;
        }

        /// <summary>
        /// 通知全桌某玩家回合结束前弃牌
        /// </summary>
        private bool GameStage14()
        {
            mRoomInfo.GameStage = GameStage.GameTurnDisCard;
            mRoomInfo.Data.GameTurnEnd_C2S = null;
            SendGameTurnDisCard();
            var data = mRoomInfo.Data.GameTurnDisCard_C2S;
            SendHandCardCount(data.UserName, mRoomInfo.GetChair(data.UserName).HandCard.Count);
            mRoomInfo.GameStageQueue.Enqueue(GameStage15);
            mRoomInfo.GameStageQueue.Enqueue(GameStage16);
            return true;
        }

        /// <summary>
        /// 等待玩家回合结束
        /// </summary>
        private bool GameStage15()
        {
            return mRoomInfo.Data.GameTurnEnd_C2S != null && mRoomInfo.CurrentGameTurnPlayerName == mRoomInfo.Data.GameTurnEnd_C2S.UserName;
        }

        /// <summary>
        /// 通知全桌某玩家回合结束
        /// </summary>
        private bool GameStage16()
        {
            mRoomInfo.GameStage = GameStage.GameTurnEnd;
            SendGameTurnEnd();
            mRoomInfo.GameStageQueue.Enqueue(GameStage17);
            return true;
        }

        /// <summary>
        /// 切换到下一个玩家的回合
        /// </summary>
        private bool GameStage17()
        {
            mRoomInfo.Data.Clear();
            string userName = mRoomInfo.GetNextUserData(mRoomInfo.CurrentGameTurnPlayerName).Name;
            mRoomInfo.CurrentGameTurnPlayerName = userName;
            SendGameTurn(userName);
            mRoomInfo.GameStageQueue.Enqueue(GameStage7);
            mRoomInfo.GameStageQueue.Enqueue(GameStage8);
            return true;
        }

        /// <summary>
        /// 随机选择玩家开始
        /// </summary>
        private void ChooseRandomPlayerGameTurn()
        {
            //Random random = new Random();
            //int randomIndex = random.Next(0, userList.Count);
            int randomIndex = 0;
            string userName = mUserList[randomIndex].Name;
            mRoomInfo.CurrentGameTurnPlayerName = userName;
            SendGameTurn(userName);
        }

        /// <summary>
        /// 起始手牌
        /// </summary>
        private void SendInitialDealCards()
        {
            var sendData = new LoginServer.Game.DealCards_S2C();
            for (int i = 0; i < mUserList.Count; i++)
            {
                var dealCard = new LoginServer.Game.DealCards();
                dealCard.UserName = mUserList[i].Name;
                //dealCard.Cards.AddRange(roomInfo.DrawCards(userList[i].Name, 2));
                dealCard.Cards.AddRange(mRoomInfo.DrawCards(mUserList[i].Name, 6));
                sendData.DealCards.Add(dealCard);
            }
            sendData.RemainGameCardCount = mRoomInfo.CardList.Count;
            sendData.DisCardCount = mRoomInfo.DisCardList.Count;
            ProtosManager.Instance.Multicast(mConnectList, CmdConfig.DealCards_S2C, sendData);
        }

        /// <summary>
        /// 轮到某人的回合
        /// </summary>
        private void SendGameTurn(string userName)
        {
            var sendData = new LoginServer.Game.GameTurn_S2C();
            sendData.UserName = userName;
            ProtosManager.Instance.Multicast(mConnectList, CmdConfig.GameTurn_S2C, sendData);
        }

        /// <summary>
        /// 某人的回合开始
        /// </summary>
        private void SendGameTurnStart()
        {
            var sendData = new LoginServer.Game.GameTurnStart_S2C();
            sendData.UserName = mRoomInfo.CurrentGameTurnPlayerName;
            ProtosManager.Instance.Multicast(mConnectList, CmdConfig.GameTurnStart_S2C, sendData);
        }

        /// <summary>
        /// 操作结束
        /// </summary>
        private void SendGameTurnOpertateEnd()
        {
            if (mRoomInfo.Data.IsGameTurnOpertateEnd) return;
            mRoomInfo.Data.IsGameTurnOpertateEnd = true;
            var sendData = new LoginServer.Game.GameTurnOpertateEnd_S2C();
            sendData.UserName = mRoomInfo.CurrentGameTurnPlayerName;
            ProtosManager.Instance.Multicast(mConnectList, CmdConfig.GameTurnOpertateEnd_S2C, sendData);
        }

        /// <summary>
        /// 弃牌
        /// </summary>
        private void SendGameTurnDisCard()
        {
            var cards = mRoomInfo.Data.GameTurnDisCard_C2S.Cards;
            var indexs = mRoomInfo.Data.GameTurnDisCard_C2S.Indexs;
            var sendData = new LoginServer.Game.GameTurnDisCard_S2C();
            sendData.UserName = mRoomInfo.CurrentGameTurnPlayerName;
            for (int i = 0; i < cards.Count; i++)
            {
                bool isSuccess = mRoomInfo.DisCard(mRoomInfo.CurrentGameTurnPlayerName, cards[i]);
                if (isSuccess)
                {
                    sendData.Cards.Add(cards[i]);
                    sendData.Indexs.Add(indexs[i]);
                }
            }
            sendData.DisCardCount = mRoomInfo.DisCardList.Count;
            ProtosManager.Instance.Multicast(mConnectList, CmdConfig.GameTurnDisCard_S2C, sendData);
        }

        /// <summary>
        /// 回合结束
        /// </summary>
        private void SendGameTurnEnd()
        {
            var sendData = new LoginServer.Game.GameTurnEnd_S2C();
            ProtosManager.Instance.Multicast(mConnectList, CmdConfig.GameTurnEnd_S2C, sendData);
        }

        /// <summary>
        /// 手牌数量
        /// </summary>
        private void SendHandCardCount(string userName, int handCardCount)
        {
            var sendData = new LoginServer.Game.HandCardCount_S2C();
            sendData.UserName = userName;
            sendData.Count = handCardCount;
            ProtosManager.Instance.Multicast(mConnectList, CmdConfig.HandCardCount_S2C, sendData);
        }

        /// <summary>
        /// 情报数量
        /// </summary>
        private void SendInformationCount()
        {
            var sendData = new LoginServer.Game.InformationCount_S2C();
            for (int i = 0; i < mChairList.Count; i++)
            {
                SendInformationCount(mChairList[i]);
            }
        }

        /// <summary>
        /// 情报数量
        /// </summary>
        private void SendInformationCount(ChairInfo chair)
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
            ProtosManager.Instance.Multicast(mConnectList, CmdConfig.InformationCount_S2C, sendData);
        }

        /// <summary>
        /// 抽卡
        /// </summary>
        private void SendDealCard(string userName, int dealCardsCount)
        {
            for (int i = 0; i < mRoomInfo.GetChairCount(); i++)
            {
                UserData user = mRoomInfo.Chairs[i].UserData;
                if (user == null || user.Name != mRoomInfo.CurrentGameTurnPlayerName) continue;
                var sendData = new LoginServer.Game.DealCards_S2C();
                var dealCard = new LoginServer.Game.DealCards();
                dealCard.UserName = userName;
                dealCard.Cards.AddRange(mRoomInfo.DrawCards(userName, dealCardsCount));
                sendData.DealCards.Add(dealCard);
                sendData.RemainGameCardCount = mRoomInfo.CardList.Count;
                sendData.DisCardCount = mRoomInfo.DisCardList.Count;
                ProtosManager.Instance.Multicast(mConnectList, CmdConfig.DealCards_S2C, sendData);
                break;
            }
        }

        /// <summary>
        /// 游戏开始错误检测
        /// </summary>
        private bool TrySendGameStartError()
        {
            var sendData = new LoginServer.Game.GameStart_S2C();
            if (mRoomInfo == null)
            {
                sendData.Code = LoginServer.Game.GameStart_S2C.Types.Ret_Code.Failed;
                sendData.Msg = "房间信息不存在";
                ProtosManager.Instance.Unicast(mCSConnect, CmdConfig.GameStart_S2C, sendData);
                return true;
            }

            if (mRoomInfo.GetUserCount() == 0)
            {
                sendData.Code = LoginServer.Game.GameStart_S2C.Types.Ret_Code.Failed;
                sendData.Msg = "房间中无玩家";
                ProtosManager.Instance.Unicast(mCSConnect, CmdConfig.GameStart_S2C, sendData);
                return true;
            }

            int chairCount = mRoomInfo.GetChairCount();
            if (mRoomInfo.Chairs == null || chairCount == 0)
            {
                sendData.Code = LoginServer.Game.GameStart_S2C.Types.Ret_Code.Failed;
                sendData.Msg = "房间未提供座位";
                ProtosManager.Instance.Unicast(mCSConnect, CmdConfig.GameStart_S2C, sendData);
                return true;
            }

            if (!mRoomInfo.IsFull())
            {
                sendData.Code = LoginServer.Game.GameStart_S2C.Types.Ret_Code.Failed;
                sendData.Msg = "房间未满员";
                ProtosManager.Instance.Unicast(mCSConnect, CmdConfig.GameStart_S2C, sendData);
                return true;
            }

            for (int i = 0; i < chairCount; i++)
            {
                if (!mRoomInfo.Chairs[i].IsRoomOwner() && !mRoomInfo.Chairs[i].IsReady)
                {
                    sendData.Code = LoginServer.Game.GameStart_S2C.Types.Ret_Code.Failed;
                    sendData.Msg = $"玩家{mRoomInfo.Chairs[i].UserData.Name}未准备";
                    ProtosManager.Instance.Unicast(mCSConnect, CmdConfig.GameStart_S2C, sendData);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 收到角色选择协议
        /// </summary>
        private void OnReceiveCharacterChoose(object obj)
        {
            var data = obj as LoginServer.Game.CharacterChoose_C2S;
            if (data == null)
                return;

            mRoomInfo = UserDataManager.Instance.GetUserDataByConnectID(mCSConnect.ID).RoomInfo;
            mChairList = mRoomInfo.ChairListCache;
            mUserList = mRoomInfo.UserListCache;
            mConnectList = mRoomInfo.ConnectListCache;

            mRoomInfo.GetChair(data.UserName).Character = Character.GetCharacter(data.Character);
        }

        /// <summary>
        /// 收到玩家回合开始协议
        /// </summary>
        private void OnReceiveGameTurnStart(object obj)
        {
            mRoomInfo.Data.GameTurnStart_C2S = obj as LoginServer.Game.GameTurnStart_C2S;
        }

        /// <summary>
        /// 收到抽牌协议
        /// </summary>
        private void OnReceiveDealCards(object obj)
        {
            mRoomInfo.Data.DealCards_C2S = obj as LoginServer.Game.DealCards_C2S;
        }

        /// <summary>
        /// 收到玩家回合操作结束协议
        /// </summary>
        private void OnReceiveGameTurnOpertateEnd(object obj)
        {
            mRoomInfo.Data.GameTurnOpertateEnd_C2S = obj as LoginServer.Game.GameTurnOpertateEnd_C2S;
        }

        /// <summary>
        /// 收到回合弃牌协议
        /// </summary>
        private void OnReceiveGameTurnDisCard(object obj)
        {
            mRoomInfo.Data.GameTurnDisCard_C2S = obj as LoginServer.Game.GameTurnDisCard_C2S;
        }

        /// <summary>
        /// 收到玩家回合结束协议
        /// </summary>
        private void OnReceiveGameTurnEnd(object obj)
        {
            mRoomInfo.Data.GameTurnEnd_C2S = obj as LoginServer.Game.GameTurnEnd_C2S;
        }

        #endregion

        #region InformationStage
        /// <summary>
        /// 收到情报宣言
        /// </summary>
        private void OnReceiveInformationDeclaration(object obj)
        {
            mRoomInfo.Data.InformationDeclaration_C2S = obj as LoginServer.Game.InformationDeclaration_C2S;

            mRoomInfo.InformationStageQueue.Enqueue(InformationStage1);
        }

        /// <summary>
        /// 通知全桌,玩家的情报宣言
        /// </summary>
        private bool InformationStage1()
        {
            mRoomInfo.InformationStage = InformationStage.InformationDeclaration;
            var data = mRoomInfo.Data.InformationDeclaration_C2S;
            SendInformationDeclaration(data.UserName);
            mRoomInfo.PlayCardStageQueue.Enqueue(PlayCardStage1);
            mRoomInfo.PlayCardStageQueue.Enqueue(PlayCardStage2);
            mRoomInfo.InformationStageQueue.Enqueue(InformationStage2);
            mRoomInfo.Data.PlayHandCard_C2S = null;
            return true;
        }

        /// <summary>
        /// 通知全桌玩家情报可以开始传递
        /// </summary>
        private bool InformationStage2()
        {
            mRoomInfo.InformationStage = InformationStage.WaitInformationTransmit;
            mRoomInfo.Data.InformationTransmitReady_C2S = null;
            SendWaitInformationTransmit();
            mRoomInfo.InformationStageQueue.Enqueue(InformationStage3);
            mRoomInfo.InformationStageQueue.Enqueue(InformationStage4);
            mRoomInfo.InformationStageQueue.Enqueue(InformationStage5);
            return true;
        }

        /// <summary>
        /// 等待玩家选择情报并开始传递
        /// </summary>
        private bool InformationStage3()
        {
            return mRoomInfo.Data.InformationTransmitReady_C2S != null;
        }

        /// <summary>
        /// 通知全桌玩家传递的情报内容
        /// </summary>
        private bool InformationStage4()
        {
            mRoomInfo.InformationStage = InformationStage.InformationTransmit;
            var data = mRoomInfo.Data.InformationTransmitReady_C2S;
            mRoomInfo.InformationTransmit(data.FromUserName, data);
            var askUser = mRoomInfo.GetNextUserData(data.FromUserName, data.Transmit, data.Direction);
            mRoomInfo.CurrentAskInformationReceivedPlayerName = askUser.Name;
            SendInformationTransmitInfo(askUser.Name);
            SendHandCardCount(data.FromUserName, mRoomInfo.GetChair(data.FromUserName).HandCard.Count);
            return true;
        }

        /// <summary>
        /// 通知全桌玩家情报传递的对象
        /// </summary>
        private bool InformationStage5()
        {
            mRoomInfo.InformationStage = InformationStage.WaitInformationReceive;
            mRoomInfo.Data.WaitInformationReceive_C2S = null;
            SendInformationTransmit(mRoomInfo.CurrentAskInformationReceivedPlayerName);
            mRoomInfo.PlayCardStageQueue.Enqueue(PlayCardStage15);
            mRoomInfo.PlayCardStageQueue.Enqueue(PlayCardStage2);
            mRoomInfo.InformationStageQueue.Enqueue(InformationStage10);
            mRoomInfo.InformationStageQueue.Enqueue(InformationStage6);
            mRoomInfo.InformationStageQueue.Enqueue(InformationStage7);
            return true;
        }

        /// <summary>
        /// 等待收到情报传递到的玩家回应消息
        /// </summary>
        private bool InformationStage6()
        {
            return mRoomInfo.Data.WaitInformationReceive_C2S != null;
        }

        /// <summary>
        /// 收到情报传递到的玩家回复的消息。若接收情报则通知全桌玩家接收情报,若不接收情报则情报继续传递
        /// </summary>
        private bool InformationStage7()
        {
            var data = mRoomInfo.Data.WaitInformationReceive_C2S;
            if (data.IsReceive)
            {
                mRoomInfo.InformationStage = InformationStage.InformationReceive;
                SendReceiveInformation(data.UserName);
                mRoomInfo.PlayCardStageQueue.Enqueue(PlayCardStage16);
                mRoomInfo.PlayCardStageQueue.Enqueue(PlayCardStage2);
                mRoomInfo.InformationStageQueue.Enqueue(InformationStage8);
            }
            else
            {
                var askUser = mRoomInfo.GetNextUserData(data.UserName, mRoomInfo.InformationCard.Transmit, mRoomInfo.InformationCard.Direction);
                mRoomInfo.CurrentAskInformationReceivedPlayerName = askUser.Name;
                mRoomInfo.InformationStageQueue.Enqueue(InformationStage5);
            }
            return true;
        }

        /// <summary>
        /// 通知所有人情报接收成功,需要判断游戏是否结束
        /// </summary>
        private bool InformationStage8()
        {
            var data = mRoomInfo.Data.WaitInformationReceive_C2S;
            var chair = mRoomInfo.GetChair(mRoomInfo.CurrentAskInformationReceivedPlayerName);
            chair.ReceiveInformation(mRoomInfo.InformationCard.Card);
            SendReceiveInformationSuccess();
            SendInformationCount(chair);
            if (!GameComplete(chair))
            {
                SendGameTurnOpertateEnd();
            }
            return true;
        }

        /// <summary>
        /// 回合中等待出牌或者情报宣言的阶段
        /// </summary>
        private bool InformationStage9()
        {
            if (mRoomInfo.Data.InformationDeclaration_C2S != null)
            {
                return true;
            }

            if (mRoomInfo.Data.PlayHandCard_C2S != null)
            {
                mRoomInfo.PlayCardStageQueue.Enqueue(PlayCardStage7);
                mRoomInfo.InformationStageQueue.Enqueue(InformationStage9);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 询问是否接收情报
        /// </summary>
        private bool InformationStage10()
        {
            SendAskInformationReceive(mRoomInfo.CurrentAskInformationReceivedPlayerName);
            return true;
        }

        /// <summary>
        /// 通知全桌情报传递到哪了
        /// </summary>
        private void SendInformationTransmit(string userName)
        {
            var sendData = new LoginServer.Game.InformationTransmit_S2C();
            sendData.UserName = userName;
            ProtosManager.Instance.Multicast(mConnectList, CmdConfig.InformationTransmit_S2C, sendData);
        }

        /// <summary>
        /// 开始情报宣言
        /// </summary>
        private void SendInformationDeclaration(string userName)
        {
            var sendData = new LoginServer.Game.InformationDeclaration_S2C();
            sendData.UserName = userName;
            ProtosManager.Instance.Multicast(mConnectList, CmdConfig.InformationDeclaration_S2C, sendData);
        }

        /// <summary>
        /// 通知情报可以开始传递
        /// </summary>
        private void SendWaitInformationTransmit()
        {
            var sendData = new LoginServer.Game.WaitInformationTransmit_S2C();
            sendData.UserName = mRoomInfo.CurrentGameTurnPlayerName;
            ProtosManager.Instance.Multicast(mConnectList, CmdConfig.WaitInformationTransmit_S2C, sendData);
        }

        /// <summary>
        /// 通知要传递的情报的具体信息
        /// </summary>
        private void SendInformationTransmitInfo(string toUserName)
        {
            var data = mRoomInfo.Data.InformationTransmitReady_C2S;
            var sendData = new LoginServer.Game.InformationTransmitReady_S2C();
            sendData.FromUserName = data.FromUserName;
            sendData.Card = data.Card;
            sendData.HandCardIndex = data.HandCardIndex;
            sendData.ToUserName = toUserName;
            sendData.Transmit = data.Transmit;
            sendData.Direction = data.Direction;
            ProtosManager.Instance.Multicast(mConnectList, CmdConfig.InformationTransmitReady_S2C, sendData);
        }

        /// <summary>
        /// 询问是否接收情报
        /// </summary>
        private void SendAskInformationReceive(string userName)
        {
            var sendData = new LoginServer.Game.WaitInformationReceive_S2C();
            sendData.UserName = userName;
            ProtosManager.Instance.Multicast(mConnectList, CmdConfig.WaitInformationReceive_S2C, sendData);
        }

        /// <summary>
        /// 收到情报传递准备协议
        /// </summary>
        private void OnReceiveInformationTransmitReady(object obj)
        {
            mRoomInfo.Data.InformationTransmitReady_C2S = obj as LoginServer.Game.InformationTransmitReady_C2S;
        }

        /// <summary>
        /// 收到等待情报接收协议
        /// </summary>
        private void OnReceiveWaitInformationReceive(object obj)
        {
            mRoomInfo.Data.WaitInformationReceive_C2S = obj as LoginServer.Game.WaitInformationReceive_C2S;
        }

        /// <summary>
        /// 玩家准备接收情报通知
        /// </summary>
        private void SendReceiveInformation(string userName)
        {
            var sendData = new LoginServer.Game.InformationReceive_S2C();
            sendData.UserName = userName;
            ProtosManager.Instance.Multicast(mConnectList, CmdConfig.InformationReceive_S2C, sendData);
        }

        /// <summary>
        /// 通知玩家情报接收成功
        /// </summary>
        private void SendReceiveInformationSuccess()
        {
            var sendData = new LoginServer.Game.InformationReceiveSuccess_S2C();
            sendData.UserName = mRoomInfo.CurrentAskInformationReceivedPlayerName;
            sendData.Card = mRoomInfo.InformationCard.Card;
            ProtosManager.Instance.Multicast(mConnectList, CmdConfig.InformationReceiveSuccess_S2C, sendData);
        }

        /// <summary>
        /// 游戏结束
        /// </summary>
        /// <returns></returns>
        private bool GameComplete(ChairInfo chair)
        {
            if (chair.IsVictory(out VictoryType victoryType))
            {
                mRoomInfo.GameComplete();
                SendGameComplete(mRoomInfo.CurrentAskInformationReceivedPlayerName, victoryType);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 游戏结束
        /// </summary>
        private void SendGameComplete(string userName, VictoryType victoryType)
        {
            var sendData = new LoginServer.Game.GameComplete_S2C();
            if (victoryType == VictoryType.JunQing || victoryType == VictoryType.QianFu)
            {
                IdentityType type = victoryType == VictoryType.JunQing ? IdentityType.JunQing : IdentityType.QianFu;
                var list = mRoomInfo.Chairs.Where(c => c.UserData != null && !c.IsNull).ToList();
                foreach (var chair in list)
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
                var list = mRoomInfo.Chairs.Where(c => c.UserData != null && !c.IsNull).ToList();
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
            ProtosManager.Instance.Multicast(mConnectList, CmdConfig.GameComplete_S2C, sendData);
        }
        #endregion

        #region PlayCardStage
        /// <summary>
        /// 除了没有手牌的玩家和发出情报的玩家,其他人可以出牌
        /// </summary>
        private bool PlayCardStage1()
        {
            for (int i = 0; i < mChairList.Count; i++)
            {
                mRoomInfo.Chairs[i].IsSkip = 
                    mRoomInfo.Chairs[i].HandCard.Count == 0 || 
                    mRoomInfo.Chairs[i].UserData.Name == mRoomInfo.CurrentGameTurnPlayerName;
            }
            return true;
        }

        /// <summary>
        /// 若所有人都不出牌，则直接跳过。若有人未做选择则询问
        /// </summary>
        private bool PlayCardStage2()
        {
            if (mChairList.Find(c => c.IsSkip == false) == null)
            {
                return true;
            }

            mRoomInfo.CurrentPlayHandCardPlayerName = string.Empty;
            SendAskPlayHandCard();
            mRoomInfo.Data.PlayHandCardResponse_C2SQueue = new Queue<PlayHandCardResponse_C2S>();
            mRoomInfo.PlayCardStageQueue.Enqueue(PlayCardStage3);
            mRoomInfo.PlayCardStageQueue.Enqueue(PlayCardStage4);
            return true;
        }

        /// <summary>
        /// 等待询问是否需要出牌的玩家回应
        /// </summary>
        private bool PlayCardStage3()
        {
            return mRoomInfo.Data.PlayHandCardResponse_C2SQueue.Count > 0;
        }

        /// <summary>
        /// 若有玩家有出牌需求,则等待玩家出牌;若玩家没有出牌需求则状态置为跳过
        /// </summary>
        private bool PlayCardStage4()
        {
            var data = mRoomInfo.Data.PlayHandCardResponse_C2SQueue.Dequeue();
            mRoomInfo.GetChair(data.UserName).IsSkip = data.IsSkip;
            if (!data.IsSkip)
            {
                mRoomInfo.Data.PlayHandCard_C2S = null;
                SendWaitPlayerPlayHandCard(data.UserName);
                mRoomInfo.PlayCardStageQueue.Enqueue(PlayCardStage6);
                mRoomInfo.PlayCardStageQueue.Enqueue(PlayCardStage7);
                return true;
            }

            mRoomInfo.PlayCardStageQueue.Enqueue(PlayCardStage5);
            return true;

        }

        /// <summary>
        /// 若所有人都不出牌，则直接跳过。若有人未做选择则查询选择记录
        /// </summary>
        private bool PlayCardStage5()
        {
            if (mChairList.Find(c => c.IsSkip == false) == null)
            {
                return true;
            }

            mRoomInfo.PlayCardStageQueue.Enqueue(PlayCardStage3);
            mRoomInfo.PlayCardStageQueue.Enqueue(PlayCardStage4);
            return true;
        }

        /// <summary>
        /// 等待出牌结果
        /// </summary>
        private bool PlayCardStage6()
        {
            return mRoomInfo.Data.PlayHandCard_C2S != null;
        }

        /// <summary>
        /// 处理出牌结果;抢到出牌机会但不出牌,则再次询问剩余玩家是否有出牌需要;想要出牌但出牌失败则回复;出牌成功通知全桌
        /// </summary>
        private bool PlayCardStage7()
        {
            var data = mRoomInfo.Data.PlayHandCard_C2S;
            if (!data.IsPlayHandCard)
            {
                SendPlayerPlayHandCardEnd(data.UserName);
                mRoomInfo.GetChair(data.UserName).IsSkip = true;
                mRoomInfo.PlayCardStageQueue.Enqueue(PlayCardStage2);
                mRoomInfo.Data.PlayHandCard_C2S = null;
                return true;
            }

            ICard cardXiaoGuo = GameCard.GetCardXiaoGuo(data.Card.Xiaoguo);
            if (cardXiaoGuo == null)
            {
                SendPlayHandCardError("未配置的效果牌");
                mRoomInfo.Data.PlayHandCard_C2S = null;
                return true;
            }

            if (cardXiaoGuo.CheckUseCondition(mRoomInfo, out string errorMsg) == false)
            {
                SendPlayHandCardError(errorMsg);
                mRoomInfo.Data.PlayHandCard_C2S = null;
                return true;
            }

            if (!mRoomInfo.UseCard(data.UserName, data.Card))
            {
                SendPlayHandCardError("不存在的手牌");
                mRoomInfo.Data.PlayHandCard_C2S = null;
                return true;
            }

            mRoomInfo.WaitTriggerCard = new WaitTriggerCard()
            {
                UserName = data.UserName,
                CardXiaoGuo = cardXiaoGuo,
                CardInfo = data.Card,
                IsShiPo = false,
            };

            SendPlayHandCard(data.UserName, data.Card, data.HandCardIndex);
            SendHandCardCount(data.UserName, mRoomInfo.GetHandCount(data.UserName));
            mRoomInfo.CurrentPlayHandCardPlayerName = data.UserName;
            mRoomInfo.PlayCardStageQueue.Enqueue(PlayCardStage8);
            mRoomInfo.PlayCardStageQueue.Enqueue(PlayCardStage9);
            mRoomInfo.Data.PlayHandCard_C2S = null;
            return true;
        }

        /// <summary>
        /// 除了没有手牌的玩家和打出效果牌的玩家,其他人可以打出识破
        /// </summary>
        private bool PlayCardStage8()
        {
            for (int i = 0; i < mChairList.Count; i++)
            {
                if (mChairList[i].UserData.Name == mRoomInfo.CurrentPlayHandCardPlayerName ||
                    mChairList[i].HandCard.Count == 0)
                {
                    mChairList[i].IsUseShiPo = false;
                }
                else
                {
                    mChairList[i].IsUseShiPo = true;
                }
            }
            return true;
        }

        /// <summary>
        /// 前提:还没人打出识破,若所有人都不出识破，则直接跳过。若有人未做选择则询问
        /// </summary>
        private bool PlayCardStage9()
        {
            if (mChairList.Find(c => c.IsUseShiPo) == null)
            {
                mRoomInfo.PlayCardStageQueue.Enqueue(PlayCardStage13);
                return true;
            }

            SendAskShiPo();
            mRoomInfo.Data.AskUseShiPo_C2SQueue = new Queue<AskUseShiPo_C2S>();
            mRoomInfo.PlayCardStageQueue.Enqueue(PlayCardStage10);
            mRoomInfo.PlayCardStageQueue.Enqueue(PlayCardStage11);
            return true;
        }

        /// <summary>
        /// 等待收到识破回复
        /// </summary>
        private bool PlayCardStage10()
        {
            return mRoomInfo.Data.AskUseShiPo_C2SQueue.Count > 0;
        }

        /// <summary>
        /// 若有玩家打出识破,则处理识破结果;若玩家不打出识破则置状态为不打出识破
        /// </summary>
        private bool PlayCardStage11()
        {
            var data = mRoomInfo.Data.AskUseShiPo_C2SQueue.Dequeue();
            if (data.IsUse)
            {
                if (mRoomInfo.UseCard(data.UserName, data.Card))
                {
                    mRoomInfo.GetChair(data.UserName).IsUseShiPo = true;
                    mRoomInfo.CurrentPlayHandCardPlayerName = data.UserName;
                    mRoomInfo.WaitTriggerCard.IsShiPo = !mRoomInfo.WaitTriggerCard.IsShiPo;
                    SendPlayHandCard(data.UserName, data.Card, data.HandCardIndex);
                    SendHandCardCount(data.UserName, mRoomInfo.GetHandCount(data.UserName));
                    mRoomInfo.PlayCardStageQueue.Enqueue(PlayCardStage8);
                    mRoomInfo.PlayCardStageQueue.Enqueue(PlayCardStage9);
                    return true;
                }
                else
                {
                    SendPlayHandCardError("不存在的手牌");
                    mRoomInfo.GetChair(data.UserName).IsUseShiPo = false;
                    mRoomInfo.PlayCardStageQueue.Enqueue(PlayCardStage12);
                    return true;
                }
            }

            mRoomInfo.GetChair(data.UserName).IsUseShiPo = false;
            mRoomInfo.PlayCardStageQueue.Enqueue(PlayCardStage12);
            return true;

        }

        /// <summary>
        /// 前提:有人打出了识破,若所有人都不出识破，则直接跳过。若有人未做选择则查询记录
        /// </summary>
        private bool PlayCardStage12()
        {
            if (mChairList.Find(c => c.IsUseShiPo) == null)
            {
                mRoomInfo.PlayCardStageQueue.Enqueue(PlayCardStage13);
                return true;
            }

            mRoomInfo.PlayCardStageQueue.Enqueue(PlayCardStage10);
            mRoomInfo.PlayCardStageQueue.Enqueue(PlayCardStage11);
            return true;
        }

        /// <summary>
        /// 触发卡牌效果
        /// </summary>
        private bool PlayCardStage13()
        {
            if (mRoomInfo.WaitTriggerCard.IsShiPo)
            {
                SendTriggerCardResult();
                mRoomInfo.WaitTriggerCard = null;
                return true;
            }

            mRoomInfo.WaitTriggerCard.CardXiaoGuo.Trigger();
            SendTriggerCardResult();
            mRoomInfo.Data.TriggerCardEnd_C2S = null;
            mRoomInfo.PlayCardStageQueue.Enqueue(PlayCardStage14);
            return true;
        }

        /// <summary>
        /// 卡牌效果触发结束
        /// </summary>
        private bool PlayCardStage14()
        {
            return mRoomInfo.Data.TriggerCardEnd_C2S != null;
        }

        /// <summary>
        /// 除了没有手牌的玩家,其他人可以出牌
        /// </summary>
        private bool PlayCardStage15()
        {
            for (int i = 0; i < mChairList.Count; i++)
            {
                mRoomInfo.Chairs[i].IsSkip = mRoomInfo.Chairs[i].HandCard.Count == 0;
            }
            return true;
        }

        /// <summary>
        /// 除了没有手牌的玩家和接收情报的玩家,其他人可以出牌
        /// </summary>
        private bool PlayCardStage16()
        {
            for (int i = 0; i < mChairList.Count; i++)
            {
                mRoomInfo.Chairs[i].IsSkip =
                    mRoomInfo.Chairs[i].HandCard.Count == 0 ||
                    mRoomInfo.Chairs[i].UserData.Name == mRoomInfo.CurrentAskInformationReceivedPlayerName;
            }
            return true;
        }

        /// <summary>
        /// 通知玩家可以出牌
        /// </summary>
        private void SendAskPlayHandCard()
        {
            var sendData = new LoginServer.Game.AskPlayHandCard_S2C();
            for (int i = 0; i < mChairList.Count; i++)
            {
                if (mChairList[i].IsSkip == false)
                {
                    sendData.UserName.Add(mChairList[i].UserData.Name);
                }
            }
            ProtosManager.Instance.Multicast(mConnectList, CmdConfig.AskPlayHandCard_S2C, sendData);
        }

        /// <summary>
        /// 收到玩家出牌需求回复协议
        /// </summary>
        private void OnReceivePlayHandCardResponse(object obj)
        {
            var data = obj as LoginServer.Game.PlayHandCardResponse_C2S;
            if (data != null)
            {
                mRoomInfo.Data.PlayHandCardResponse_C2SQueue.Enqueue(data);
            }
        }

        /// <summary>
        /// 等待玩家出牌
        /// </summary>
        private void SendWaitPlayerPlayHandCard(string userName)
        {
            var sendData = new LoginServer.Game.PlayHandCardResponse_S2C();
            sendData.UserName = userName;
            sendData.IsSkip = false;
            ProtosManager.Instance.Multicast(mConnectList, CmdConfig.PlayHandCardResponse_S2C, sendData);
        }

        /// <summary>
        /// 收到出牌协议
        /// </summary>
        private void OnReceivePlayHandCard(object obj)
        {
            var data = obj as LoginServer.Game.PlayHandCard_C2S;
            if (data == null)
                return;

            if (mRoomInfo.Data.PlayHandCard_C2S == null)
            {
                mRoomInfo.Data.PlayHandCard_C2S = data;
            }
        }

        /// <summary>
        /// 打出手牌失败
        /// </summary>
        private void SendPlayHandCardError(string errorMsg)
        {
            var data = mRoomInfo.Data.PlayHandCard_C2S;
            var sendData = new LoginServer.Game.PlayHandCard_S2C();
            sendData.UserName = data.UserName;
            sendData.Card = data.Card;
            sendData.HandCardIndex = data.HandCardIndex;
            sendData.Code = PlayHandCard_S2C.Types.Ret_Code.Failed;
            sendData.Msg = errorMsg;
            var connect = UserDataManager.Instance.GetUserData(data.UserName).CSConnect;
            ProtosManager.Instance.Unicast(connect, CmdConfig.PlayHandCard_S2C, sendData);
        }

        /// <summary>
        /// 通知全桌某玩家出牌结束
        /// </summary>
        private void SendPlayerPlayHandCardEnd(string userName)
        {
            var sendData = new LoginServer.Game.PlayHandCardResponse_S2C();
            sendData.UserName = userName;
            sendData.IsSkip = true;
            ProtosManager.Instance.Multicast(mConnectList, CmdConfig.PlayHandCardResponse_S2C, sendData);
        }

        /// <summary>
        /// 打出手牌成功回复
        /// </summary>
        private void SendPlayHandCard(string userName, CardType card, int handCardIndex)
        {
            var sendData = new LoginServer.Game.PlayHandCard_S2C();
            sendData.UserName = userName;
            sendData.Card = card;
            sendData.HandCardIndex = handCardIndex;
            sendData.Code = PlayHandCard_S2C.Types.Ret_Code.Success;
            sendData.Msg = string.Empty;
            ProtosManager.Instance.Multicast(mConnectList, CmdConfig.PlayHandCard_S2C, sendData);
        }

        /// <summary>
        /// 发送询问识破协议
        /// </summary>
        private void SendAskShiPo()
        {
            var sendData = new LoginServer.Game.AskUseShiPo_S2C();
            for (int i = 0; i < mChairList.Count; i++)
            {
                if (mChairList[i].CanUseShiPo() && mChairList[i].IsUseShiPo)
                {
                    sendData.UserName.Add(mChairList[i].UserData.Name);
                }
            }
            ProtosManager.Instance.Multicast(mConnectList, CmdConfig.AskUseShiPo_S2C, sendData);
        }

        /// <summary>
        /// 收到是否使用识破的答复
        /// </summary>
        private void OnReceiveAskUseShiPo(object obj)
        {
            var data = obj as LoginServer.Game.AskUseShiPo_C2S;
            if (data == null)
                return;

            mRoomInfo.Data.AskUseShiPo_C2SQueue.Enqueue(data);
        }

        /// <summary>
        /// 收到卡牌触发效果结束协议
        /// </summary>
        private void OnReceiveTriggerCardEnd(object obj)
        {
            mRoomInfo.Data.TriggerCardEnd_C2S = obj as LoginServer.Game.TriggerCardEnd_C2S;
        }

        /// <summary>
        /// 发送卡牌触发结果
        /// </summary>
        private void SendTriggerCardResult()
        {
            var sendData = new LoginServer.Game.TriggerCardResult_S2C();
            sendData.UserName = mRoomInfo.WaitTriggerCard.UserName;
            sendData.IsTrigger = !mRoomInfo.WaitTriggerCard.IsShiPo;
            ProtosManager.Instance.Multicast(mConnectList, CmdConfig.TriggerCardResult_S2C, sendData);
        }
        #endregion

    }
}
