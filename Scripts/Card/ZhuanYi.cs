using FengShengServer;
using LoginServer.Game;

public class ZhuanYi : ICard
{
    private bool mIsComplete;
    private Game mGame;
    private RoomInfo mRoomInfo;
    private string mUserName;
    private UseZhuanYi_C2S UseZhuanYi_C2S;

    private Card_XiaoGuoType mCard = Card_XiaoGuoType.ZhuanYi;
    public Card_XiaoGuoType GetCardXiaoGuoType()
    {
        return mCard;
    }

    public bool CheckUseCondition(RoomInfo roomInfo, out string errorMsg)
    {
        var data = roomInfo.Data.PlayHandCard_C2S;

        if (roomInfo.CurrentAskInformationReceivedPlayerName != data.UserName)
        {
            errorMsg = "只能在情报传递到自己时使用";
            return false;
        }

        if (roomInfo.InformationStage != InformationStage.InformationTransmit)
        {
            errorMsg = "只能在情报传递时使用";
            return false;
        }

        if (data.TargetUserName == data.UserName)
        {
            errorMsg = "转移不能以自己为目标";
            return false;
        }

        errorMsg = string.Empty;
        return true;
    }

    private void SendZhuanYi()
    {
        var sendData = new LoginServer.Game.UseZhuanYi_S2C();
        mUserName = mRoomInfo.WaitTriggerCard.UserName;
        sendData.UserName = mUserName;
        sendData.Card = mRoomInfo.WaitTriggerCard.CardInfo;
        ProtosManager.Instance.Multicast(mRoomInfo.ConnectListCache, CmdConfig.UseZhuanYi_S2C, sendData);
    }

    public void Trigger(Game game, params object[] args)
    {
        mIsComplete = false;
        UseZhuanYi_C2S = null;
        mGame = game;
        mRoomInfo = mGame.GetRoomInfo();
        SendZhuanYi();
        var chair = mRoomInfo.GetChair(mRoomInfo.WaitTriggerCard.TargetUserName);
        chair.ZhuanYiFlag = true;
        CSConnect connect = mRoomInfo.UserListCache.Find(user => user.Name == mUserName).CSConnect;
        ProtosManager.Instance.AddProtosListener(connect.ID, CmdConfig.UseZhuanYi_C2S, OnReceiveUseZhuanYi, 1);
        mRoomInfo.InformationStageQueue.Clear();
        mRoomInfo.InformationStageQueue.Enqueue(mGame.InformationStage5);
    }

    public bool IsComplete()
    {
        return mIsComplete;
    }

    private void OnReceiveUseZhuanYi(object obj)
    {
        UseZhuanYi_C2S = obj as UseZhuanYi_C2S;
        mRoomInfo.InformationTransmit(
                fromUserName: mRoomInfo.InformationCard.FromUserName,
                card: mRoomInfo.InformationCard.Card,
                toUserName: mRoomInfo.WaitTriggerCard.TargetUserName,
                transmit: mRoomInfo.InformationCard.Transmit,
                direction: mRoomInfo.InformationCard.Direction);

        mRoomInfo.CurrentAskInformationReceivedPlayerName = mRoomInfo.InformationCard.ToUserName;
        mRoomInfo.PlayCardStageQueue.InsertFirst(ZhuanYiStage1);
        mIsComplete = true;
    }

    /// <summary>
    /// 清空出牌链
    /// </summary>
    private bool ZhuanYiStage1()
    {
        mRoomInfo.Data.PlayHandCard_C2S = null;
        mRoomInfo.WaitTriggerCard = null;
        mRoomInfo.PlayCardStageQueue.Clear();
        return true;
    }

}