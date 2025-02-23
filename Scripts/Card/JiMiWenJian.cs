using FengShengServer;
using LoginServer.Game;

public class JiMiWenJian : ICard
{
    private bool mIsComplete;
    private Game mGame;
    private RoomInfo mRoomInfo;
    private string mUserName;
    private UseJiMiWenJian_C2S UseJiMiWenJian_C2S;

    private Card_XiaoGuoType mCard = Card_XiaoGuoType.JiMiWenJian;
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

        int count = 0;
        for (int i = 0; i < roomInfo.ChairListCache.Count; i++)
        {
            count += roomInfo.ChairListCache[i].GetInformationCount(Card_ColorType.Red);
            count += roomInfo.ChairListCache[i].GetInformationCount(Card_ColorType.Blue);
            count += roomInfo.ChairListCache[i].GetInformationCount(Card_ColorType.RedBlue);
        }
        if (count < 4)
        {
            errorMsg = "场上真情报数量不足,无法使用";
            return false;
        }

        errorMsg = string.Empty;
        return true;
    }

    private void SendJiMiWenJian()
    {
        var sendData = new LoginServer.Game.UseJiMiWenJian_S2C();
        mUserName = mRoomInfo.WaitTriggerCard.UserName;
        sendData.UserName = mUserName;
        sendData.Card = mRoomInfo.WaitTriggerCard.CardInfo;
        ProtosManager.Instance.Multicast(mRoomInfo.ConnectListCache, CmdConfig.UseJiMiWenJian_S2C, sendData);
    }

    public void Trigger(Game game, params object[] args)
    {
        mIsComplete = false;
        UseJiMiWenJian_C2S = null;
        mGame = game;
        mRoomInfo = mGame.GetRoomInfo();
        SendJiMiWenJian();

        int count = 0;
        for (int i = 0; i < mRoomInfo.ChairListCache.Count; i++)
        {
            count += mRoomInfo.ChairListCache[i].GetInformationCount(Card_ColorType.Red);
            count += mRoomInfo.ChairListCache[i].GetInformationCount(Card_ColorType.Blue);
            count += mRoomInfo.ChairListCache[i].GetInformationCount(Card_ColorType.RedBlue);
        }
        mGame.SendDealCard(mUserName, count > 7 ? 3 : 2);
        mGame.SendHandCardCount();
        CSConnect connect = mRoomInfo.UserListCache.Find(user => user.Name == mUserName).CSConnect;
        ProtosManager.Instance.AddProtosListener(connect.ID, CmdConfig.UseJiMiWenJian_C2S, OnReceiveUseJiMiWenJian, 1);
    }

    public bool IsComplete()
    {
        return mIsComplete;
    }

    private void OnReceiveUseJiMiWenJian(object obj)
    {
        UseJiMiWenJian_C2S = obj as UseJiMiWenJian_C2S;
        mIsComplete = true;
    }

}