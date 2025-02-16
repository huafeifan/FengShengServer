using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoginServer.Game;

namespace FengShengServer
{
    public interface IIdentity
    {
        IdentityType GetIdentity();

        bool IsVictory(ChairInfo chair, out VictoryType victoryType);
    }
}
