using FengShengServer;
using LoginServer.Game;

public class ZengYuan : ICard
{
    private bool mIsComplete;
    private Game mGame;
    private RoomInfo mRoomInfo;
    private string mUserName;
    private UseZengYuan_C2S UseZengYuan_C2S;

    private Card_XiaoGuoType mCard = Card_XiaoGuoType.ZengYuan;
    public Card_XiaoGuoType GetCardXiaoGuoType()
    {
        return mCard;
    }

    public bool CheckUseCondition(RoomInfo roomInfo, out string errorMsg)
    {
        var data = roomInfo.Data.PlayHandCard_C2S;
        if (data.UserName != roomInfo.CurrentGameTurnPlayerName)
        {
            errorMsg = "只能在自己回合中使用";
            return false;
        }

        errorMsg = string.Empty;
        return true;
    }

    private void SendZengYuan()
    {
        var sendData = new LoginServer.Game.UseZengYuan_S2C();
        mUserName = mRoomInfo.WaitTriggerCard.UserName;
        sendData.UserName = mUserName;
        sendData.Card = mRoomInfo.WaitTriggerCard.CardInfo;
        ProtosManager.Instance.Multicast(mRoomInfo.ConnectListCache, CmdConfig.UseZengYuan_S2C, sendData);
    }

    public void Trigger(Game game, params object[] args)
    {
        mIsComplete = false;
        UseZengYuan_C2S = null;
        mGame = game;
        mRoomInfo = mGame.GetRoomInfo();
        SendZengYuan();
        int count = 1 + mRoomInfo.GetChair(mUserName).GetInformationCount(Card_ColorType.Gray);
        mGame.SendDealCard(mUserName, count);
        mGame.SendHandCardCount();
        CSConnect connect = mRoomInfo.UserListCache.Find(user => user.Name == mUserName).CSConnect;
        ProtosManager.Instance.AddProtosListener(connect.ID, CmdConfig.UseZengYuan_C2S, OnReceiveUseZengYuan, 1);
    }

    public bool IsComplete()
    {
        return mIsComplete;
    }

    private void OnReceiveUseZengYuan(object obj)
    {
        UseZengYuan_C2S = obj as UseZengYuan_C2S;
        mIsComplete = true;
    }

}