
namespace FengShengServer
{
    public enum PlayCardStage
    {
        None,
        /// <summary>
        /// 等待玩家请求出牌
        /// </summary>
        WaitPlayerRequestHandCard,
        /// <summary>
        /// 等待玩家出牌
        /// </summary>
        WaitPlayerPlayHandCard,
    }
}
