using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoginServer.Game;

namespace FengShengServer
{
    public interface ICard
    {
        Card_XiaoGuoType GetCardXiaoGuoType();
        bool CheckUseCondition(RoomInfo roomInfo, out string errorMsg);

        void Trigger(Game game, params object[] args);

        bool IsComplete();
    }
}
