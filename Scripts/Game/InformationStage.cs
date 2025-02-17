
using LoginServer.Game;
using System.Collections.Generic;

namespace FengShengServer
{
    public enum InformationStage
    {
        None,
        /// <summary>
        /// 等待情报宣言
        /// </summary>
        WaitInformationDeclaration,
        /// <summary>
        /// 等待发出情报
        /// </summary>
        WaitInformationTransmit,
        /// <summary>
        /// 等待玩家接收情报
        /// </summary>
        WaitInformationReceive,
        /// <summary>
        /// 等待情报传递结束
        /// </summary>
        WaitEnd,
    }
}
