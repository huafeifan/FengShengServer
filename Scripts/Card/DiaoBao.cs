using System;
using LoginServer.Game;

namespace FengShengServer
{
    public class DiaoBao : ICard
    {
        private bool mIsComplete;
        private Game mGame;
        private RoomInfo mRoomInfo;
        private string mUserName;
        private UseDiaoBao_C2S UseDiaoBao_C2S;

        private Card_XiaoGuoType mCard = Card_XiaoGuoType.DiaoBao;
        public Card_XiaoGuoType GetCardXiaoGuoType()
        {
            return mCard;
        }

        public bool CheckUseCondition(RoomInfo roomInfo, out string errorMsg)
        {
            if (roomInfo.InformationStage != InformationStage.InformationTransmit ||
                roomInfo.CurrentAskInformationReceivedPlayerName != roomInfo.CurrentGameTurnPlayerName)
            {
                errorMsg = "只能在情报传回传出者时使用";
                return false;
            }

            if (roomInfo.InformationCard != null && roomInfo.InformationCard.Transmit == Card_TransmitType.WenBen)
            {
                errorMsg = "无法对文本情报使用";
                return false;
            }

            errorMsg = string.Empty;
            return true;
        }

        private void SendUseDiaoBao()
        {
            var sendData = new LoginServer.Game.UseDiaoBao_S2C();
            mUserName = mRoomInfo.WaitTriggerCard.UserName;
            sendData.UserName = mUserName;
            sendData.Card = mRoomInfo.WaitTriggerCard.CardInfo;
            ProtosManager.Instance.Multicast(mRoomInfo.ConnectListCache, CmdConfig.UseDiaoBao_S2C, sendData);
        }

        public void Trigger(Game game, params object[] args)
        {
            mIsComplete = false;
            UseDiaoBao_C2S = null;
            mGame = game;
            mRoomInfo = mGame.GetRoomInfo();
            SendUseDiaoBao();
            CSConnect connect = mRoomInfo.UserListCache.Find(user => user.Name == mUserName).CSConnect;
            ProtosManager.Instance.AddProtosListener(connect.ID, CmdConfig.UseDiaoBao_C2S, OnReceiveUseDiaoBao, 1);
            mRoomInfo.InformationStageQueue.Clear();
            mRoomInfo.InformationStageQueue.Enqueue(mGame.InformationStage5);
        }

        public bool IsComplete()
        {
            return mIsComplete;
        }

        private void OnReceiveUseDiaoBao(object obj)
        {
            UseDiaoBao_C2S = obj as UseDiaoBao_C2S;
            mRoomInfo.InformationTransmit(
                fromUserName: mRoomInfo.InformationCard.FromUserName,
                card: UseDiaoBao_C2S.Card,
                toUserName: mRoomInfo.GetNextUserData(mRoomInfo.CurrentGameTurnPlayerName, UseDiaoBao_C2S.Transmit, UseDiaoBao_C2S.Direction).Name,
                transmit: UseDiaoBao_C2S.Transmit,
                direction: UseDiaoBao_C2S.Direction);

            mRoomInfo.CurrentAskInformationReceivedPlayerName = mRoomInfo.InformationCard.ToUserName;
            mRoomInfo.PlayCardStageQueue.InsertFirst(DiaoBaoStage1);
            mIsComplete = true;
        }

        /// <summary>
        /// 清空出牌链
        /// </summary>
        private bool DiaoBaoStage1()
        {
            mRoomInfo.Data.PlayHandCard_C2S = null;
            mRoomInfo.WaitTriggerCard = null;
            mRoomInfo.PlayCardStageQueue.Clear();
            return true;
        }

    }

}
