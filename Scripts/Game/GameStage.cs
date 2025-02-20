
using LoginServer.Game;
using System.Collections.Generic;

namespace FengShengServer
{
    public enum GameStage
    {
        None,
        /// <summary>
        /// 游戏开始
        /// </summary>
        GameStart,
        /// <summary>
        /// 等待身份选择
        /// </summary>
        IdentityChoose,
        /// <summary>
        /// 等待身份选择
        /// </summary>
        CharacterChoose,
        /// <summary>
        /// 等待发牌
        /// </summary>
        DealCards,
        /// <summary>
        /// 等待轮到某人
        /// </summary>
        GameTurn,
        /// <summary>
        /// 等待回合开始
        /// </summary>
        GameTurnStart,
        /// <summary>
        /// 等待回合操作结束
        /// </summary>
        GameTurnOpertateEnd,
        /// <summary>
        /// 等待回合弃牌
        /// </summary>
        GameTurnDisCard,
        /// <summary>
        /// 等待回合结束
        /// </summary>
        GameTurnEnd,
    }
}
