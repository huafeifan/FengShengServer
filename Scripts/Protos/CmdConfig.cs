
namespace FengShengServer
{
    public class CmdConfig
    {
        /// <summary>
        /// 心跳协议号
        /// </summary>
        public const uint HeartBeat = 0x0000;

        /// <summary>
        /// 登陆协议号
        /// </summary>
        public const uint Login_C2S = 0x0001;
        public const uint Login_S2C = 0x0001;

        /// <summary>
        /// 房间列表协议号
        /// </summary>
        public const uint RoomList_C2S = 0x0100;
        public const uint RoomList_S2C = 0x0100;

        /// <summary>
        /// 创建房间协议号
        /// </summary>
        public const uint CreateRoom_C2S = 0x0101;
        public const uint CreateRoom_S2C = 0x0101;

        /// <summary>
        /// 进入房间协议号
        /// </summary>
        public const uint EnterRoom_C2S = 0x0102;
        public const uint EnterRoom_S2C = 0x0102;

        /// <summary>
        /// 退出房间协议号
        /// </summary>
        public const uint ExitRoom_C2S = 0x0103;
        public const uint ExitRoom_S2C = 0x0103;

        /// <summary>
        /// 房间信息变更协议号
        /// </summary>
        public const uint RoomInfoChange_S2C = 0x0104;

    }
}
