using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoginServer.Game;

namespace FengShengServer
{
    public class QianFu : IIdentity
    {
        private IdentityType mIdentityType = IdentityType.QianFu;

        public IdentityType GetIdentity()
        {
            return mIdentityType;
        }

        public bool IsVictory(ChairInfo chair, out VictoryType victoryType)
        {
            victoryType = VictoryType.QianFu;
            int redCount = chair.GetInformationCount(Card_ColorType.Red);
            int redBlueCount = chair.GetInformationCount(Card_ColorType.RedBlue);
            return (redCount + redBlueCount) >= 3;
        }
    }

}
