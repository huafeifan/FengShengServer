using FengShengServer;
using LoginServer.Game;

public class ShaoHui : ICard
{
    private bool mIsComplete;
    private Game mGame;
    private RoomInfo mRoomInfo;
    private string mUserName;
    private UseShaoHui_C2S UseShaoHui_C2S;

    private Card_XiaoGuoType mCard = Card_XiaoGuoType.ShaoHui;
    public Card_XiaoGuoType GetCardXiaoGuoType()
    {
        return mCard;
    }

    public bool CheckUseCondition(RoomInfo roomInfo, out string errorMsg)
    {
        var data = roomInfo.Data.PlayHandCard_C2S;
        if (roomInfo.GetChair(data.UserName).GetInformationCount(Card_ColorType.Gray) <= 0)
        {
            errorMsg = "没有假情报";
            return false;
        }

        errorMsg = string.Empty;
        return true;
    }

    private void SendUseShaoHui()
    {
        var sendData = new LoginServer.Game.UseShaoHui_S2C();
        mUserName = mRoomInfo.WaitTriggerCard.UserName;
        sendData.UserName = mUserName;
        sendData.Card = mRoomInfo.WaitTriggerCard.CardInfo;
        ProtosManager.Instance.Multicast(mRoomInfo.ConnectListCache, CmdConfig.UseShaoHui_S2C, sendData);
    }

    public void Trigger(Game game, params object[] args)
    {
        mIsComplete = false;
        UseShaoHui_C2S = null;
        mGame = game;
        mRoomInfo = mGame.GetRoomInfo();
        SendUseShaoHui();
        mRoomInfo.GetChair(mUserName).RemoveInformation(Card_ColorType.Gray);
        mGame.SendInformationCount();
        CSConnect connect = mRoomInfo.UserListCache.Find(user => user.Name == mUserName).CSConnect;
        ProtosManager.Instance.AddProtosListener(connect.ID, CmdConfig.UseShaoHui_C2S, OnReceiveUseShaoHui, 1);
    }

    public bool IsComplete()
    {
        return mIsComplete;
    }

    private void OnReceiveUseShaoHui(object obj)
    {
        UseShaoHui_C2S = obj as UseShaoHui_C2S;
        mIsComplete = true;
    }

}