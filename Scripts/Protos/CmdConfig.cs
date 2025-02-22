
namespace FengShengServer
{
    public class CmdConfig
    {
        /// <summary>
        /// ����Э���
        /// </summary>
        public const uint HeartBeat = 0x0000;

        /// <summary>
        /// ��½Э���
        /// </summary>
        public const uint Login_C2S = 0x0001;
        public const uint Login_S2C = 0x0001;

        /// <summary>
        /// �����б�Э���
        /// </summary>
        public const uint RoomList_C2S = 0x0100;
        public const uint RoomList_S2C = 0x0100;

        /// <summary>
        /// ��������Э���
        /// </summary>
        public const uint CreateRoom_C2S = 0x0101;
        public const uint CreateRoom_S2C = 0x0101;

        /// <summary>
        /// ���뷿��Э���
        /// </summary>
        public const uint EnterRoom_C2S = 0x0102;
        public const uint EnterRoom_S2C = 0x0102;

        /// <summary>
        /// �˳�����Э���
        /// </summary>
        public const uint ExitRoom_C2S = 0x0103;
        public const uint ExitRoom_S2C = 0x0103;

        /// <summary>
        /// ������Ϣ���Э���
        /// </summary>
        public const uint RoomInfoChange_S2C = 0x0104;

        /// <summary>
        /// ���󷿼�ȫ����ϢЭ���
        /// </summary>
        public const uint RequestRoomInfo_C2S = 0x0105;
        public const uint RequestRoomInfo_S2C = 0x0105;

        /// <summary>
        /// ���׼��Э���
        /// </summary>
        public const uint ReadyStatus_C2S = 0x0106;
        public const uint ReadyStatus_S2C = 0x0106;

        /// <summary>
        /// ��Ϸ��ʼЭ���
        /// </summary>
        public const uint GameStart_C2S = 0x0200;
        public const uint GameStart_S2C = 0x0200;

        /// <summary>
        /// ��ݷַ�Э���
        /// </summary>
        public const uint Identity_S2C = 0x0201;

        /// <summary>
        /// ��ɫѡ��Э���
        /// </summary>
        public const uint CharacterChoose_C2S = 0x0202;

        /// <summary>
        /// ��ɫѡ���б�Э���
        /// </summary>
        public const uint CharacterChooseList_S2C = 0x0203;

        /// <summary>
        /// ��Ϸ���Э���
        /// </summary>
        public const uint GameComplete_S2C = 0x0204;

        /// <summary>
        /// ����Э���
        /// </summary>
        public const uint DealCards_C2S = 0x0205;
        public const uint DealCards_S2C = 0x0205;

        /// <summary>
        /// �ֵ�ĳ�˵Ļغ�Э���
        /// </summary>
        public const uint GameTurn_S2C = 0x0206;

        /// <summary>
        /// ĳ�˻غϽ���Э���
        /// </summary>
        public const uint GameTurnEnd_C2S = 0x0207;
        public const uint GameTurnEnd_S2C = 0x0207;

        /// <summary>
        /// ĳ�˻غϿ�ʼЭ���
        /// </summary>
        public const uint GameTurnStart_C2S = 0x0208;
        public const uint GameTurnStart_S2C = 0x0208;

        /// <summary>
        /// ĳ�˻غϲ�������Э���
        /// </summary>
        public const uint GameTurnOpertateEnd_C2S = 0x0209;
        public const uint GameTurnOpertateEnd_S2C = 0x0209;

        /// <summary>
        /// ĳ�˻غ�����Э���
        /// </summary>
        public const uint GameTurnDisCard_C2S = 0x020A;
        public const uint GameTurnDisCard_S2C = 0x020A;

        /// <summary>
        /// ��������Э���
        /// </summary>
        //public const uint HandCardCount_C2S = 0x020B;
        public const uint HandCardCount_S2C = 0x020B;

        /// <summary>
        /// �鱨����Э���
        /// </summary>
        public const uint InformationDeclaration_C2S = 0x020C;
        public const uint InformationDeclaration_S2C = 0x020C;

        /// <summary>
        /// ��Ӧ�Ƿ����Э���
        /// </summary>
        public const uint PlayHandCardResponse_C2S = 0x020D;
        public const uint PlayHandCardResponse_S2C = 0x020D;

        /// <summary>
        /// �ȴ��鱨����Э���
        /// </summary>
        public const uint WaitInformationTransmit_S2C = 0x020E;

        /// <summary>
        /// �鱨����Э���
        /// </summary>
        public const uint InformationTransmitReady_C2S = 0x020F;
        public const uint InformationTransmitReady_S2C = 0x020F;

        /// <summary>
        /// �ȴ��鱨����Э���
        /// </summary>
        public const uint WaitInformationReceive_C2S = 0x0210;
        public const uint WaitInformationReceive_S2C = 0x0210;

        /// <summary>
        /// ����Э���
        /// </summary>
        public const uint PlayHandCard_C2S = 0x0211;
        public const uint PlayHandCard_S2C = 0x0211;

        /// <summary>
        /// �鱨����֪ͨЭ���
        /// </summary>
        public const uint InformationReceive_S2C = 0x0212;

        /// <summary>
        /// ѯ���Ƿ����Э���
        /// </summary>
        public const uint AskPlayHandCard_S2C = 0x0213;

        /// <summary>
        /// �鱨���ճɹ�֪ͨЭ���
        /// </summary>
        public const uint InformationReceiveSuccess_S2C = 0x0214;

        /// <summary>
        /// ֪ͨ�鱨�����鱨������Э���
        /// </summary>
        public const uint InformationTransmitSender_S2C = 0x0215;

        /// <summary>
        /// �鱨�����㲥Э���
        /// </summary>
        public const uint InformationCount_S2C = 0x0216;

        /// <summary>
        /// ѯ���Ƿ�ʹ��ʶ��Э���
        /// </summary>
        public const uint AskUseShiPo_C2S = 0x0218;
        public const uint AskUseShiPo_S2C = 0x0218;

        /// <summary>
        /// ����Ч���������Э���
        /// </summary>
        public const uint TriggerCardResult_S2C = 0x0219;

        /// <summary>
        /// �鱨����Э���
        /// </summary>
        public const uint InformationTransmit_S2C = 0x021A;

        /// <summary>
        /// ����Ч��Э���
        /// </summary>
        public const uint UseDiaoBao_C2S = 0x021B;
        public const uint UseDiaoBao_S2C = 0x021B;

        /// <summary>
        /// ����Ч��Э���
        /// </summary>
        public const uint UsePoYi_C2S = 0x021C;
        public const uint UsePoYi_S2C = 0x021C;
    }
}
