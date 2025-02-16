using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoginServer.Game;

namespace FengShengServer
{
    public class TeGong : IIdentity
    {
        private IdentityType mIdentityType = IdentityType.TeGong;

        public IdentityType GetIdentity()
        {
            return mIdentityType;
        }

        public bool IsVictory(ChairInfo chair, out VictoryType victoryType)
        {
            victoryType = VictoryType.PartTeGong;
            int redCount = chair.GetInformationCount(Card_ColorType.Red);
            int blueCount = chair.GetInformationCount(Card_ColorType.Blue);
            int redBlueCount = chair.GetInformationCount(Card_ColorType.RedBlue);
            return (redCount + blueCount + redBlueCount) >= 6 || chair.Character.IsVictory(chair);
        }
    }

}
