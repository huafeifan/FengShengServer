using FengShengServer;
using LoginServer.Game;

public class ShiPo : ICard
{
    private Card_XiaoGuoType mCard = Card_XiaoGuoType.ShiPo;
    public Card_XiaoGuoType GetCardXiaoGuoType()
    {
        return mCard;
    }

    public bool CheckUseCondition(RoomInfo roomInfo, out string errorMsg)
    {
        errorMsg = "只能在玩家打出效果牌后使用";
        return false;
    }

    public void Trigger(Game game, params object[] args)
    {

    }

    public bool IsComplete()
    {
        return true;
    }

}