
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

    }
}
