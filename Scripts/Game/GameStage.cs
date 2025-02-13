
using LoginServer.Game;
using System.Collections.Generic;

namespace FengShengServer
{
    public enum GameStage
    {
        None,
        /// <summary>
        /// 等待游戏开始
        /// </summary>
        WaitGameStart,
        /// <summary>
        /// 等待身份选择
        /// </summary>
        WaitIdentityChoose,
        /// <summary>
        /// 等待身份选择
        /// </summary>
        WaitCharacterChoose,
        /// <summary>
        /// 等待发牌
        /// </summary>
        WaitDealCards,
        /// <summary>
        /// 等待轮到某人
        /// </summary>
        WaitGameTurn,
        /// <summary>
        /// 等待回合开始
        /// </summary>
        WaitGameTurnStart,
        /// <summary>
        /// 等待回合操作结束
        /// </summary>
        WaitGameTurnOpertateEnd,
        /// <summary>
        /// 等待回合弃牌
        /// </summary>
        WaitGameTurnDisCard,
        /// <summary>
        /// 等待回合结束
        /// </summary>
        WaitGameTurnEnd,
    }
}
