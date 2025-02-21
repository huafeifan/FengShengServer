
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
        /// 身份选择
        /// </summary>
        IdentityChoose,
        /// <summary>
        /// 身份选择
        /// </summary>
        CharacterChoose,
        /// <summary>
        /// 发牌
        /// </summary>
        DealCards,
        /// <summary>
        /// 轮到某人
        /// </summary>
        GameTurn,
        /// <summary>
        /// 回合开始
        /// </summary>
        GameTurnStart,
        /// <summary>
        /// 回合操作结束
        /// </summary>
        GameTurnOpertateEnd,
        /// <summary>
        /// 回合弃牌
        /// </summary>
        GameTurnDisCard,
        /// <summary>
        /// 回合结束
        /// </summary>
        GameTurnEnd,
    }
}
