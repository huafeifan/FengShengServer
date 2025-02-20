using System;
using System.Collections.Generic;
using System.Linq;
using LoginServer.Game;

namespace FengShengServer
{
    public class RoomInfo
    {
        public string RoomName { get; set; }

        public int RoomNub { get; set; }

        public bool IsOpen { get; set; }

        public List<ChairInfo> Chairs { get; set; }

        /// <summary>
        /// 当前游戏回合的玩家名称
        /// </summary>
        public string CurrentGameTurnPlayerName { get; set; }

        /// <summary>
        /// 当前要询问是否接受情报的玩家名称
        /// </summary>
        public string CurrentAskInformationReceivedPlayerName { get; set; }

        /// <summary>
        /// 当前出牌的玩家名称
        /// </summary>
        public string CurrentPlayHandCardPlayerName { get; set; }

        /// <summary>
        /// 角色池
        /// </summary>
        public List<CharacterType> CharacterList { get; set; }

        /// <summary>
        /// 卡池
        /// </summary>
        public List<CardType> CardList { get; set; }

        /// <summary>
        /// 弃牌堆
        /// </summary>
        public List<CardType> DisCardList { get; set; }

        /// <summary>
        /// 当前正在传递的情报牌
        /// </summary>
        public InformationTransmitReady_C2S InformationCard { get; set; }

        /// <summary>
        /// 等待触发的卡牌效果
        /// </summary>
        public WaitTriggerCard WaitTriggerCard { get; set; }

        /// <summary>
        /// 游戏阶段
        /// </summary>
        public GameStage GameStage { get; set; }

        /// <summary>
        /// 游戏阶段队列
        /// </summary>
        public Queue<Func<bool>> GameStageQueue { get; set; }

        /// <summary>
        /// 情报阶段
        /// </summary>
        public InformationStage InformationStage { get; set; }

        /// <summary>
        /// 情报阶段队列
        /// </summary>
        public Queue<Func<bool>> InformationStageQueue { get; set; }

        /// <summary>
        /// 出牌阶段队列
        /// </summary>
        public Queue<Func<bool>> PlayCardStageQueue { get; set; }

        /// <summary>
        /// 协议数据
        /// </summary>
        public GameProtoData Data { get; set; }

        /// <summary>
        /// 缓存数据
        /// </summary>
        public List<ChairInfo> ChairListCache { get; set; }
        public List<UserData> UserListCache { get; set; }
        public List<CSConnect> ConnectListCache { get; set; }

        public RoomInfo()
        {
            Chairs = new List<ChairInfo>();
            RoomName = string.Empty;
            RoomNub = -1;
            IsOpen = false;
            DisCardList = new List<CardType>();
            GameStageQueue = new Queue<Func<bool>>();
            InformationStageQueue = new Queue<Func<bool>>();
            PlayCardStageQueue = new Queue<Func<bool>>();
        }

        public int GetChairCount()
        {
            return Chairs.Count;
        }
        
        public int GetUserCount()
        {
            int count = 0;
            for (int i = 0; i < Chairs.Count; i++)
            {
                if (!Chairs[i].IsNull && !Chairs[i].IsRobot && Chairs[i].UserData != null)
                {
                    count++;
                }
            }
            return count;
        }

        public int GetRobotCount()
        {
            int count = 0;
            for (int i = 0; i < Chairs.Count; i++)
            {
                if (!Chairs[i].IsNull && Chairs[i].IsRobot)
                {
                    count++;
                }
            }
            return count;
        }

        public void InitChairs(int chairCounts)
        {
            Chairs.Clear();
            for (int i = 1; i <= chairCounts; i++)
            {
                ChairInfo chair = new ChairInfo();
                chair.ChairID = i;
                chair.Clear();
                Chairs.Add(chair);
            }
        }

        public bool IsFull()
        {
            for (int i = 0; i < Chairs.Count; i++)
            {
                if (Chairs[i].IsNull)
                {
                    return false;
                }
            }
            return true;
        }

        public ChairInfo GetChair(string userName)
        {
            return Chairs.Find(c => !c.IsNull && c.UserData != null && c.UserData.Name == userName);
        }

        public int GetHandCount(string userName)
        {
            return GetChair(userName).HandCard.Count;
        }

        public void AddUser(UserData user)
        {
            for (int i = 0; i < Chairs.Count; i++)
            {
                if (Chairs[i].IsNull)
                {
                    Chairs[i].IsNull = false;
                    Chairs[i].IsReady = false;
                    Chairs[i].IsRobot = false;

                    Chairs[i].UserData = user;
                    Chairs[i].Identity = null;
                    Chairs[i].Character = null;
                    Chairs[i].HandCard.Clear();
                    user.RoomInfo = this;
                    break;
                }
            }
        }

        public bool RemoveUser(string userName)
        {
            for (int i = 0; i < Chairs.Count; i++)
            {
                if (Chairs[i].IsNull == false &&
                    Chairs[i].UserData != null &&
                    Chairs[i].UserData.Name == userName)
                {
                    Chairs[i].Clear();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否是房主
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool IsRoomOwner(string userName)
        {
            return Chairs[0].UserData != null && Chairs[0].UserData.Name == userName;
        }

        public List<UserData> GetAllUserData()
        {
            return Chairs.Where(c => !c.IsNull && c.UserData != null).Select(s => s.UserData).ToList();
        }

        public UserData GetNextUserData(string userName, Card_TransmitType transmit = Card_TransmitType.MiDian, Card_DirectionType direction = Card_DirectionType.Ni)
        {
            if (transmit == Card_TransmitType.MiDian || transmit == Card_TransmitType.WenBen)
            {
                if (direction == Card_DirectionType.Ni)
                {
                    int index = -100;
                    for (int i = 0; i < Chairs.Count; i++)
                    {
                        if (Chairs[i].IsNull == false && Chairs[i].UserData != null && Chairs[i].UserData.Name == userName)
                        {
                            index = i - 1;
                            break;
                        }
                    }

                    if (index == -100) return null;

                    if (index == -1)
                        index = Chairs.Count - 1;

                    for (int i = index; i >= 0; i--)
                    {
                        if (Chairs[i].IsNull == false && Chairs[i].UserData != null)
                        {
                            return Chairs[i].UserData;
                        }
                    }
                }
                else if (direction == Card_DirectionType.Shun)
                {
                    int index = -100;
                    for (int i = 0; i < Chairs.Count; i++)
                    {
                        if (Chairs[i].IsNull == false && Chairs[i].UserData != null && Chairs[i].UserData.Name == userName)
                        {
                            index = i + 1;
                            break;
                        }
                    }

                    if (index == -100) return null;

                    if (index == Chairs.Count)
                        index = 0;

                    for (int i = index; i < Chairs.Count; i++)
                    {
                        if (Chairs[i].IsNull == false && Chairs[i].UserData != null)
                        {
                            return Chairs[i].UserData;
                        }
                    }
                }
            }
            else if (transmit == Card_TransmitType.ZhiDa)
            {
                if (userName == InformationCard.FromUserName)
                {
                    return Chairs.Find(c => c.IsNull == false && c.UserData != null && c.UserData.Name == InformationCard.ToUserName).UserData;
                }
                else if (userName == InformationCard.ToUserName)
                {
                    return Chairs.Find(c => c.IsNull == false && c.UserData != null && c.UserData.Name == InformationCard.FromUserName).UserData;
                }
            }
            
            return null;
        }

        public void GameStart()
        {
            CharacterList = Character.GetNewCharacterList();
            CardList = GameCard.GetNewCardList();
            Data = new GameProtoData();
            DisCardList.Clear();
            CurrentGameTurnPlayerName = string.Empty;
            CurrentAskInformationReceivedPlayerName = string.Empty;
            CurrentPlayHandCardPlayerName = string.Empty;
            InformationCard = null;
            for (int i = 0; i < Chairs.Count; i++)
            {
                Chairs[i].Character = null;
                Chairs[i].Identity = null;
                Chairs[i].HandCard.Clear();
                Chairs[i].InformationCard.Clear();
                Chairs[i].IsReady = false;
            }

            GameStage = GameStage.None;
            GameStageQueue.Clear();
            InformationStage = InformationStage.None;
            InformationStageQueue.Clear();
            PlayCardStageQueue.Clear();
        }

        public void GameComplete()
        {
            GameStageQueue.Clear();
            InformationStageQueue.Clear();
            PlayCardStageQueue.Clear();
        }

        public List<CharacterType> GetCharacterChooseList()
        {
            return CharacterList.GetRange(0, 3);
        }

        /// <summary>
        /// 抽多张牌
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<CardType> DrawCards(string userName, int count)
        {
            var chair = GetChair(userName);
            List<CardType> result = new List<CardType>(count);
            for (int i = 0; i < count; i++)
            {
                var card = DrawCard();
                chair.DealCard(card);
                result.Add(card);
            }
            return result;
        }

        /// <summary>
        /// 抽一张牌
        /// </summary>
        /// <returns></returns>
        public CardType DrawCard()
        {
            if (CardList.Count == 0)
            {
                CardList = DisCardList;
                CardList.FisherYatesShuffle();
                DisCardList.Clear();
            }

            int index = CardList.Count - 1;
            CardType result = CardList[index];
            CardList.RemoveAt(index);
            return result;
        }

        /// <summary>
        /// 弃一张牌
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="cards"></param>
        public bool DisCard(string userName, CardType card)
        {
            var chair = GetChair(userName);
            bool isSuccess = chair.DisCard(card.CardName);
            if (isSuccess)
            {
                DisCardList.Add(card);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 消耗卡牌
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="card"></param>
        public bool UseCard(string userName, CardType card)
        {
            var chair = GetChair(userName);
            bool isSuccess = chair.DisCard(card.CardName);
            if (isSuccess)
            {
                DisCardList.Add(card);
            }
            return isSuccess;
        }

        /// <summary>
        /// 发出情报
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="card"></param>
        public bool InformationTransmit(string userName, InformationTransmitReady_C2S card)
        {
            var chair = GetChair(userName);
            bool isSuccess = chair.DisCard(card.Card.CardName);
            InformationCard = card;
            return isSuccess;
        }

        public void Close()
        {
            IsOpen = false;
            RoomName = string.Empty;
            for (int i = 0; i < Chairs.Count; i++)
            {
                Chairs[i].Clear();
            }
            DisCardList.Clear();
            CurrentGameTurnPlayerName = string.Empty;
            CurrentAskInformationReceivedPlayerName = string.Empty;
            CurrentPlayHandCardPlayerName = string.Empty;
            InformationCard = null;
            GameStage = GameStage.None;
            GameStageQueue.Clear();
            InformationStage = InformationStage.None;
            InformationStageQueue.Clear();
            PlayCardStageQueue.Clear();
        }

    }
}
