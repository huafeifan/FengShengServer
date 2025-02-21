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
            mRoomInfo.InformationStageQueue.InsertFirst(DiaoBaoStage1);
        }

        public bool IsComplete()
        {
            return mIsComplete;
        }

        private void OnReceiveUseDiaoBao(object obj)
        {
            UseDiaoBao_C2S = obj as UseDiaoBao_C2S;
            var data = UseDiaoBao_C2S;
            var askUser = mRoomInfo.GetNextUserData(mRoomInfo.CurrentGameTurnPlayerName, data.Transmit, data.Direction);
            data.ToUserName = askUser.Name;
            mRoomInfo.InformationTransmit(data);
            //回合结束不再询问其他玩家是否出牌
            for (int i = 0; i < mRoomInfo.ChairListCache.Count; i++)
            {
                mRoomInfo.ChairListCache[i].IsSkip = true;
            }
            mIsComplete = true;
        }

        /// <summary>
        /// 等待客户端使用调包的消息,通知全桌玩家情报传递的对象
        /// </summary>
        private bool DiaoBaoStage1()
        {
            if (UseDiaoBao_C2S == null)
            {
                return false;
            }

            mRoomInfo.InformationStage = InformationStage.InformationTransmit;
            mRoomInfo.CurrentAskInformationReceivedPlayerName = mRoomInfo.InformationCard.ToUserName;
            mGame.SendInformationTransmit(mRoomInfo.CurrentAskInformationReceivedPlayerName);
            mRoomInfo.Data.WaitInformationReceive_C2S = null;
            mRoomInfo.PlayCardStageQueue.Enqueue(DiaoBaoStage2);
            return true;
        }

        /// <summary>
        /// 除了没有手牌的玩家和发出调包的玩家,其他人可以出牌
        /// </summary>
        private bool DiaoBaoStage2()
        {
            for (int i = 0; i < mRoomInfo.ChairListCache.Count; i++)
            {
                mRoomInfo.ChairListCache[i].IsSkip =
                    mRoomInfo.ChairListCache[i].HandCard.Count == 0 ||
                    mRoomInfo.ChairListCache[i].UserData.Name == mUserName;
            }
            mRoomInfo.PlayCardStageQueue.Enqueue(mGame.PlayCardStage2);
            return true;
        }

    }

}
