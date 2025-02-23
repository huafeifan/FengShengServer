using FengShengServer;
using LoginServer.Game;

public class DiaoHuLiShan : ICard
{
    private bool mIsComplete;
    private Game mGame;
    private RoomInfo mRoomInfo;
    private string mUserName;
    private UseDiaoHuLiShan_C2S UseDiaoHuLiShan_C2S;

    private Card_XiaoGuoType mCard = Card_XiaoGuoType.DiaoHuLiShan;
    public Card_XiaoGuoType GetCardXiaoGuoType()
    {
        return mCard;
    }

    public bool CheckUseCondition(RoomInfo roomInfo, out string errorMsg)
    {
        var data = roomInfo.Data.PlayHandCard_C2S;
        if (data.UserName == roomInfo.CurrentAskInformationReceivedPlayerName)
        {
            errorMsg = "自己接收情报时无法使用";
            return false;
        }
        if (roomInfo.InformationStage != InformationStage.WaitInformationReceive)
        {
            errorMsg = "只能在玩家接收情报时使用";
            return false;
        }

        errorMsg = string.Empty;
        return true;
    }

    private void SendDiaoHuLiShan()
    {
        var sendData = new LoginServer.Game.UseDiaoHuLiShan_S2C();
        mUserName = mRoomInfo.WaitTriggerCard.UserName;
        sendData.UserName = mUserName;
        sendData.Card = mRoomInfo.WaitTriggerCard.CardInfo;
        ProtosManager.Instance.Multicast(mRoomInfo.ConnectListCache, CmdConfig.UseDiaoHuLiShan_S2C, sendData);
    }

    public void Trigger(Game game, params object[] args)
    {
        mIsComplete = false;
        UseDiaoHuLiShan_C2S = null;
        mGame = game;
        mRoomInfo = mGame.GetRoomInfo();
        SendDiaoHuLiShan();
        CSConnect connect = mRoomInfo.UserListCache.Find(user => user.Name == mUserName).CSConnect;
        ProtosManager.Instance.AddProtosListener(connect.ID, CmdConfig.UseDiaoHuLiShan_C2S, OnReceiveUseDiaoHuLiShan, 1);
        mRoomInfo.InformationStageQueue.Clear();
        mRoomInfo.InformationStageQueue.Enqueue(mGame.InformationStage5);
    }

    public bool IsComplete()
    {
        return mIsComplete;
    }

    private void OnReceiveUseDiaoHuLiShan(object obj)
    {
        UseDiaoHuLiShan_C2S = obj as UseDiaoHuLiShan_C2S;
        mRoomInfo.InformationTransmit(
            fromUserName: mRoomInfo.InformationCard.FromUserName, 
            card: mRoomInfo.InformationCard.Card, 
            toUserName: mRoomInfo.GetNextUserData(mRoomInfo.CurrentAskInformationReceivedPlayerName, mRoomInfo.InformationCard.Transmit, mRoomInfo.InformationCard.Direction).Name, 
            transmit: mRoomInfo.InformationCard.Transmit, 
            direction: mRoomInfo.InformationCard.Direction);

        mRoomInfo.CurrentAskInformationReceivedPlayerName = mRoomInfo.InformationCard.ToUserName;
        mRoomInfo.PlayCardStageQueue.InsertFirst(DiaoHuLiShanStage1);
        mIsComplete = true;
    }

    /// <summary>
    /// 清空出牌链
    /// </summary>
    private bool DiaoHuLiShanStage1()
    {
        mRoomInfo.PlayCardStageQueue.Clear();
        return true;
    }

}