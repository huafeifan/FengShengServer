using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoginServer.Game;

namespace FengShengServer
{
    public class JunQing : IIdentity
    {
        private IdentityType mIdentityType = IdentityType.JunQing;

        public IdentityType GetIdentity()
        {
            return mIdentityType;
        }

        public bool IsVictory(ChairInfo chair, out VictoryType victoryType)
        {
            victoryType = VictoryType.JunQing;
            int blueCount = chair.GetInformationCount(Card_ColorType.Blue);
            int redBlueCount = chair.GetInformationCount(Card_ColorType.RedBlue);
            return (blueCount + redBlueCount) >= 3;
        }
    }

}
