using System;
using System.Collections.Generic;
using LoginServer.Game;

namespace FengShengServer
{
    public class ChairInfo
    {
        public int ChairID { get; set; }

        public UserData UserData { get; set; }

        public List<CardType> HandCard { get; set; } = new List<CardType>();

        public List<CardType> InformationCard { get; set; } = new List<CardType>();

        public IIdentity Identity { get; set; }

        public ICharacter Character { get; set; }

        public bool IsReady { get; set; }

        public bool IsRobot { get; set; }

        public bool IsNull { get; set; }

        public bool IsSkip { get; set; }

        public void Clear()
        {
            if (UserData != null)
            {
                UserData.RoomInfo = null;
                UserData = null;
            }

            Identity = null;
            Character = null;
            IsReady = false;
            IsRobot = false;
            IsNull = true;
            IsSkip = false;
            InformationCard.Clear();
            HandCard.Clear();
        }

        /// <summary>
        /// 是否是房主
        /// </summary>
        public bool IsRoomOwner()
        {
            return ChairID == 1;
        }

        public bool DisCard(string cardName)
        {
            for (int i = 0; i < HandCard.Count; i++) 
            { 
                if (HandCard[i].CardName == cardName)
                {
                    HandCard.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public void DealCard(CardType card)
        {
            HandCard.Add(card);
        }

        public void ReceiveInformation(CardType card)
        {
            InformationCard.Add(card);
        }

        public int GetInformationCount(Card_ColorType color)
        {
            int result = 0;
            for (int i = 0; i < InformationCard.Count; i++)
            {
                if (InformationCard[i].Color == color)
                {
                    result++;
                }
            }
            return result;
        }

        public bool IsVictory(out VictoryType victoryType)
        {
            return Identity.IsVictory(this, out victoryType);
        }
    }
}
