using FengShengServer;
using LoginServer.Game;

public class JieHuo : ICard
{
    private bool mIsComplete;
    private Game mGame;
    private RoomInfo mRoomInfo;
    private string mUserName;
    private UseJieHuo_C2S UseJieHuo_C2S;

    private Card_XiaoGuoType mCard = Card_XiaoGuoType.JieHuo;
    public Card_XiaoGuoType GetCardXiaoGuoType()
    {
        return mCard;
    }

    public bool CheckUseCondition(RoomInfo roomInfo, out string errorMsg)
    {
        var data = roomInfo.Data.PlayHandCard_C2S;
        if (data.UserName == roomInfo.CurrentGameTurnPlayerName)
        {
            errorMsg = "只能在他人回合使用";
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

    private void SendJieHuo()
    {
        var sendData = new LoginServer.Game.UseJieHuo_S2C();
        mUserName = mRoomInfo.WaitTriggerCard.UserName;
        sendData.UserName = mUserName;
        sendData.Card = mRoomInfo.WaitTriggerCard.CardInfo;
        ProtosManager.Instance.Multicast(mRoomInfo.ConnectListCache, CmdConfig.UseJieHuo_S2C, sendData);
    }

    public void Trigger(Game game, params object[] args)
    {
        mIsComplete = false;
        UseJieHuo_C2S = null;
        mGame = game;
        mRoomInfo = mGame.GetRoomInfo();
        SendJieHuo();
        CSConnect connect = mRoomInfo.UserListCache.Find(user => user.Name == mUserName).CSConnect;
        ProtosManager.Instance.AddProtosListener(connect.ID, CmdConfig.UseJieHuo_C2S, OnReceiveUseJieHuo, 1);
        mRoomInfo.InformationStageQueue.Clear();
        mRoomInfo.InformationStageQueue.Enqueue(mGame.InformationStage5);
    }

    public bool IsComplete()
    {
        return mIsComplete;
    }

    private void OnReceiveUseJieHuo(object obj)
    {
        UseJieHuo_C2S = obj as UseJieHuo_C2S;
        mRoomInfo.InformationTransmit(
            fromUserName: mRoomInfo.InformationCard.FromUserName, 
            card: mRoomInfo.InformationCard.Card, 
            toUserName: UseJieHuo_C2S.UserName, 
            transmit: mRoomInfo.InformationCard.Transmit, 
            direction: mRoomInfo.InformationCard.Direction);

        mRoomInfo.CurrentAskInformationReceivedPlayerName = UseJieHuo_C2S.UserName;
        mRoomInfo.PlayCardStageQueue.InsertFirst(JieHuoStage1);
        mIsComplete = true;
    }

    /// <summary>
    /// 清空出牌链
    /// </summary>
    private bool JieHuoStage1()
    {
        mRoomInfo.Data.PlayHandCard_C2S = null;
        mRoomInfo.WaitTriggerCard = null;
        mRoomInfo.PlayCardStageQueue.Clear();
        return true;
    }

}