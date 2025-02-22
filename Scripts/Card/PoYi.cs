using FengShengServer;
using LoginServer.Game;

public class PoYi : ICard
{
    private bool mIsComplete;
    private Game mGame;
    private RoomInfo mRoomInfo;
    private string mUserName;
    private UsePoYi_C2S UsePoYi_C2S;

    private Card_XiaoGuoType mCard = Card_XiaoGuoType.PoYi;
    public Card_XiaoGuoType GetCardXiaoGuoType()
    {
        return mCard;
    }

    public bool CheckUseCondition(RoomInfo roomInfo, out string errorMsg)
    {
        if (roomInfo.InformationCard == null)
        {
            errorMsg = "没有情报可以破译";
            return false;
        }

        if (roomInfo.InformationCard.Transmit == Card_TransmitType.WenBen)
        {
            errorMsg = "文本情报不需要破译";
            return false;
        }

        errorMsg = string.Empty;
        return true;
    }

    private void SendUsePoYi()
    {
        var sendData = new LoginServer.Game.UsePoYi_S2C();
        mUserName = mRoomInfo.WaitTriggerCard.UserName;
        sendData.UserName = mUserName;
        sendData.Card = mRoomInfo.WaitTriggerCard.CardInfo;
        ProtosManager.Instance.Multicast(mRoomInfo.ConnectListCache, CmdConfig.UsePoYi_S2C, sendData);
    }

    public void Trigger(Game game, params object[] args)
    {
        mIsComplete = false;
        UsePoYi_C2S = null;
        mGame = game;
        mRoomInfo = mGame.GetRoomInfo();
        SendUsePoYi();
        CSConnect connect = mRoomInfo.UserListCache.Find(user => user.Name == mUserName).CSConnect;
        ProtosManager.Instance.AddProtosListener(connect.ID, CmdConfig.UsePoYi_C2S, OnReceiveUsePoYi, 1);
    }

    public bool IsComplete()
    {
        return mIsComplete;
    }

    private void OnReceiveUsePoYi(object obj)
    {
        UsePoYi_C2S = obj as UsePoYi_C2S;
        mIsComplete = true;
    }

}