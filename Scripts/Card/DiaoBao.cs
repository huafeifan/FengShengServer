using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoginServer.Game;

namespace FengShengServer
{
    public class DiaoBao : ICard
    {
        private Card_XiaoGuoType mCard = Card_XiaoGuoType.DiaoBao;
        public Card_XiaoGuoType GetCardXiaoGuoType()
        {
            return mCard;
        }

        public bool CheckUseCondition(RoomInfo roomInfo, out string errorMsg)
        {
            //if (roomInfo.InformationStage == InformationStage.WaitInformationReceive &&
            //    roomInfo.CurrentAskInformationReceivedPlayerName == roomInfo.CurrentGameTurnPlayerName)
            //{
                errorMsg = string.Empty;
                return true;
            //}

            //errorMsg = "错误:只能在情报传回传出者时使用";
            //return false;
        }

        public void Trigger()
        {

        }
    }

}
