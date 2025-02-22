using FengShengServer;
using LoginServer.Game;

public class SuoDing : ICard
{
    private bool mIsComplete;
    private Game mGame;
    private RoomInfo mRoomInfo;
    private string mUserName;
    private UseSuoDing_C2S UseSuoDing_C2S;

    private Card_XiaoGuoType mCard = Card_XiaoGuoType.SuoDing;
    public Card_XiaoGuoType GetCardXiaoGuoType()
    {
        return mCard;
    }

    public bool CheckUseCondition(RoomInfo roomInfo, out string errorMsg)
    {
        if (roomInfo.InformationStage != InformationStage.InformationTransmit)
        {
            errorMsg = "只能在情报传递时使用";
            return false;
        }

        errorMsg = string.Empty;
        return true;
    }

    private void SendSuoDing()
    {
        var sendData = new LoginServer.Game.UseSuoDing_S2C();
        mUserName = mRoomInfo.WaitTriggerCard.UserName;
        sendData.UserName = mUserName;
        sendData.Card = mRoomInfo.WaitTriggerCard.CardInfo;
        ProtosManager.Instance.Multicast(mRoomInfo.ConnectListCache, CmdConfig.UseSuoDing_S2C, sendData);
    }

    public void Trigger(Game game, params object[] args)
    {
        mIsComplete = false;
        UseSuoDing_C2S = null;
        mGame = game;
        mRoomInfo = mGame.GetRoomInfo();
        SendSuoDing();
        var chair = mRoomInfo.GetChair(mRoomInfo.CurrentAskInformationReceivedPlayerName);
        chair.CanRefuse = false;
        CSConnect connect = mRoomInfo.UserListCache.Find(user => user.Name == mUserName).CSConnect;
        ProtosManager.Instance.AddProtosListener(connect.ID, CmdConfig.UseZengYuan_C2S, OnReceiveUseZengYuan, 1);
    }

    public bool IsComplete()
    {
        return mIsComplete;
    }

    private void OnReceiveUseZengYuan(object obj)
    {
        UseSuoDing_C2S = obj as UseSuoDing_C2S;
        mIsComplete = true;
    }

}