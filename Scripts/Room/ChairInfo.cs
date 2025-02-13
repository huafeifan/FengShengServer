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

        public IdentityType IdentityType { get; set; }

        public CharacterType CharacterType { get; set; } = CharacterType.None;

        public bool IsReady { get; set; }

        public bool IsRobot { get; set; }

        public bool IsNull { get; set; }

        public void Clear()
        {
            if (UserData != null)
            {
                UserData.RoomInfo = null;
                UserData = null;
            }

            IdentityType = IdentityType.None;
            CharacterType = CharacterType.None;
            IsReady = false;
            IsRobot = false;
            IsNull = true;
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
    }
}
