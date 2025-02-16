using System;
using System.Collections.Generic;
using System.Linq;
using Google.Protobuf;

namespace FengShengServer
{
    public class ProtosManager
    {
        private static ProtosManager mInstance;
        public static ProtosManager Instance
        {
            get
            {
                if (mInstance == null) 
                    mInstance = new ProtosManager();
                return mInstance;
            }
        }

        /// <summary>
        /// Э�����  ����ID�ţ�������
        /// </summary>
        private Dictionary<int, ProtosListener> mProtosListeners = new Dictionary<int, ProtosListener>();

        public void Start()
        {
            Console.WriteLine("Э�������������");
        }

        public void Close()
        {
            for (int i = 0; i < mProtosListeners.Count; i++)
            {
                mProtosListeners[i].Clear();
            }
            mProtosListeners.Clear();

            Console.WriteLine("Э��������ѹر�");
        }

        #region Э���¼���

        /// <summary>
        /// ע���������ID��Э���¼���
        /// </summary>
        /// <param name="connectID"></param>
        public void RegisterProtosListener(int connectID)
        {
            if (!mProtosListeners.ContainsKey(connectID))
            {
                mProtosListeners.Add(connectID, new ProtosListener());
            }
            else
            {
                mProtosListeners[connectID].Clear();
            }
        }

        public void RemoveProtosListener(int connectID)
        {
            if (mProtosListeners.ContainsKey(connectID))
            {
                mProtosListeners[connectID].Clear();
                mProtosListeners.Remove(connectID);
            }
        }

        public ProtosListener GetProtosListener(int connectID)
        {
            if (mProtosListeners.ContainsKey(connectID))
            {
                return mProtosListeners[connectID];
            }
            return null;
        }

        /// <summary>
        /// ���Э�����
        /// </summary>
        /// <param name="connectID">����ID</param>
        /// <param name="cmd">Э���</param>
        /// <param name="callBack"></param>
        public void AddProtosListener(int connectID, uint cmd, Action<object> callBack)
        {
            var listener = GetProtosListener(connectID);
            if (listener != null)
            {
                listener.AddListener(cmd, callBack);
            }
        }

        /// <summary>
        /// �Ƴ�Э�����
        /// </summary>
        /// <param name="connectID">����ID</param>
        /// <param name="cmd">Э���</param>
        /// <param name="callBack"></param>
        public void RemoveProtosListener(int connectID, uint cmd, Action<object> callBack)
        {
            var listener = GetProtosListener(connectID);
            if (listener != null)
            {
                listener.RemoveListener(cmd, callBack);
            }
        }

        /// <summary>
        /// ����Э��
        /// </summary>
        /// <param name="connectID">����ID</param>
        /// <param name="cmd">Э���</param>
        /// <param name="data">�ֽ���</param>
        public void TriggerProtos(int connectID, uint cmd, object data)
        {
            var listener = GetProtosListener(connectID);
            if (listener != null)
            {
                listener.TriggerEvent(cmd, data);
            }
        }

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="connect">����</param>
        /// <param name="cmd"></param>
        /// <param name="sendData"></param>
        public void Unicast(CSConnect connect, uint cmd, IMessage sendData)
        {
            if (connect == null) return;
            SenderManager.Instance.AddSendMessage(connect, cmd, sendData.ToByteArray(), true);
        }

        /// <summary>
        /// �ಥ����
        /// </summary>
        /// <param name="connectList">����</param>
        /// <param name="cmd"></param>
        /// <param name="sendData"></param>
        public void Multicast(List<CSConnect> connectList, uint cmd, IMessage sendData)
        {
            for (int i = 0; i < connectList.Count; i++)
            {
                Unicast(connectList[i], cmd, sendData);
            }
        }

        /// <summary>
        /// �ಥ����
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="sendData"></param>
        public void Broadcast(uint cmd, IMessage sendData)
        {
            var connectList = Server.Clients;
            for (int i = 0; i < connectList.Count; i++)
            {
                Unicast(connectList[i], cmd, sendData);
            }
        }

        #endregion

        #region Э��ת����

        /// <summary>
        /// Э��ת��
        /// </summary>
        /// <param name="connectID"></param>
        /// <param name="cmd"></param>
        /// <param name="bytes"></param>
        public void ProtosConvert(int connectID, uint cmd, byte[] bytes)
        {
            switch (cmd)
            {
                case CmdConfig.Login_C2S:
                    TriggerProtos(connectID, cmd, LoginServer.Login.Login_C2S.Parser.ParseFrom(bytes));
                    return;

                case CmdConfig.RoomList_C2S:
                    TriggerProtos(connectID, cmd, LoginServer.Room.RoomList_C2S.Parser.ParseFrom(bytes));
                    return;

                case CmdConfig.CreateRoom_C2S:
                    TriggerProtos(connectID, cmd, LoginServer.Room.CreateRoom_C2S.Parser.ParseFrom(bytes));
                    return;

                case CmdConfig.EnterRoom_C2S:
                    TriggerProtos(connectID, cmd, LoginServer.Room.EnterRoom_C2S.Parser.ParseFrom(bytes));
                    return;

                case CmdConfig.ExitRoom_C2S:
                    TriggerProtos(connectID, cmd, LoginServer.Room.ExitRoom_C2S.Parser.ParseFrom(bytes));
                    return;

                //case CmdConfig.RoomInfoChange_C2S:
                //    TriggerProtos(connectID, cmd, LoginServer.Room.RoomInfoChange_C2S.Parser.ParseFrom(bytes));
                //    return;

                case CmdConfig.RequestRoomInfo_C2S:
                    TriggerProtos(connectID, cmd, LoginServer.Room.RequestRoomInfo_C2S.Parser.ParseFrom(bytes));
                    return;

                case CmdConfig.ReadyStatus_C2S:
                    TriggerProtos(connectID, cmd, LoginServer.Room.ReadyStatus_C2S.Parser.ParseFrom(bytes));
                    return;

                case CmdConfig.GameStart_C2S:
                    TriggerProtos(connectID, cmd, LoginServer.Game.GameStart_C2S.Parser.ParseFrom(bytes));
                    return;

                //case CmdConfig.Identity_C2S:
                //    TriggerProtos(connectID, cmd, LoginServer.Game.Identity_C2S.Parser.ParseFrom(bytes));
                //    return;

                case CmdConfig.CharacterChoose_C2S:
                    TriggerProtos(connectID, cmd, LoginServer.Game.CharacterChoose_C2S.Parser.ParseFrom(bytes));
                    return;

                //case CmdConfig.GameComplete_C2S:
                //    TriggerProtos(connectID, cmd, LoginServer.Game.GameComplete_C2S.Parser.ParseFrom(bytes));
                //    return;

                case CmdConfig.DealCards_C2S:
                    TriggerProtos(connectID, cmd, LoginServer.Game.DealCards_C2S.Parser.ParseFrom(bytes));
                    return;

                //case CmdConfig.GameTurn_C2S:
                //    TriggerProtos(connectID, cmd, LoginServer.Game.GameTurn_C2S.Parser.ParseFrom(bytes));
                //    return;

                case CmdConfig.GameTurnEnd_C2S:
                    TriggerProtos(connectID, cmd, LoginServer.Game.GameTurnEnd_C2S.Parser.ParseFrom(bytes));
                    return;

                case CmdConfig.GameTurnStart_C2S:
                    TriggerProtos(connectID, cmd, LoginServer.Game.GameTurnStart_C2S.Parser.ParseFrom(bytes));
                    return;

                case CmdConfig.GameTurnOpertateEnd_C2S:
                    TriggerProtos(connectID, cmd, LoginServer.Game.GameTurnOpertateEnd_C2S.Parser.ParseFrom(bytes));
                    return;

                case CmdConfig.GameTurnDisCard_C2S:
                    TriggerProtos(connectID, cmd, LoginServer.Game.GameTurnDisCard_C2S.Parser.ParseFrom(bytes));
                    return;

                //case CmdConfig.HandCardCount_C2S:
                //    TriggerProtos(connectID, cmd, LoginServer.Game.HandCardCount_C2S.Parser.ParseFrom(bytes));
                //    return;

                case CmdConfig.InformationDeclaration_C2S:
                    TriggerProtos(connectID, cmd, LoginServer.Game.InformationDeclaration_C2S.Parser.ParseFrom(bytes));
                    return;

                case CmdConfig.InformationDeclarationResponse_C2S:
                    TriggerProtos(connectID, cmd, LoginServer.Game.InformationDeclarationResponse_C2S.Parser.ParseFrom(bytes));
                    return;

                //case CmdConfig.WaitInformationTransmit_C2S:
                //    TriggerProtos(connectID, cmd, LoginServer.Game.WaitInformationTransmit_C2S.Parser.ParseFrom(bytes));
                //    return;

                case CmdConfig.InformationTransmit_C2S:
                    TriggerProtos(connectID, cmd, LoginServer.Game.InformationTransmit_C2S.Parser.ParseFrom(bytes));
                    return;

                case CmdConfig.WaitInformationReceive_C2S:
                    TriggerProtos(connectID, cmd, LoginServer.Game.WaitInformationReceive_C2S.Parser.ParseFrom(bytes));
                    return;

                case CmdConfig.InformationDeclarationResponseEnd_C2S:
                    TriggerProtos(connectID, cmd, LoginServer.Game.InformationDeclarationResponseEnd_C2S.Parser.ParseFrom(bytes));
                    return;

                //case CmdConfig.InformationReceive_C2S:
                //    TriggerProtos(connectID, cmd, LoginServer.Game.InformationReceive_C2S.Parser.ParseFrom(bytes));
                //    return;

                case CmdConfig.InformationReceiveResponse_C2S:
                    TriggerProtos(connectID, cmd, LoginServer.Game.InformationReceiveResponse_C2S.Parser.ParseFrom(bytes));
                    return;

                //case CmdConfig.InformationReceiveSuccess_C2S:
                //    TriggerProtos(connectID, cmd, LoginServer.Game.InformationReceiveSuccess_C2S.Parser.ParseFrom(bytes));
                //    return;

                case CmdConfig.InformationReceiveResponseEnd_C2S:
                    TriggerProtos(connectID, cmd, LoginServer.Game.InformationReceiveResponseEnd_C2S.Parser.ParseFrom(bytes));
                    return;
            }
        }
        #endregion

    }
}
