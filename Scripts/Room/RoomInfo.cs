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

        public string CurrentGameTurnPlayerName { get; set; }

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
        /// 游戏阶段
        /// </summary>
        public GameStage GameStage { get; set; }

        public RoomInfo()
        {
            Chairs = new List<ChairInfo>();
            RoomName = string.Empty;
            RoomNub = -1;
            IsOpen = false;
            DisCardList = new List<CardType>();
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
                    Chairs[i].IdentityType = IdentityType.None;
                    Chairs[i].CharacterType = CharacterType.None;
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

        public UserData GetNextUserData(string userName)
        {
            int index = -1;
            for (int i = 0; i < Chairs.Count; i++)
            {
                if (Chairs[i].IsNull == false && Chairs[i].UserData != null && Chairs[i].UserData.Name == userName)
                {
                    index = i + 1;
                    break;
                }
            }

            if (index == -1) return null;

            if (index >= Chairs.Count) index = 0;
            for (int i = index; i < Chairs.Count; i++)
            {
                if (Chairs[i].IsNull == false && Chairs[i].UserData != null)
                {
                    return Chairs[i].UserData;
                }
            }
            return null;
        }

        public void GameStart()
        {
            CharacterList = Character.GetNewCharacterList();
            CardList = GameCard.GetNewCardList();
            DisCardList.Clear();
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

        public void Close()
        {
            IsOpen = false;
            RoomName = string.Empty;
            for (int i = 0; i < Chairs.Count; i++)
            {
                Chairs[i].Clear();
            }
        }

    }
}
