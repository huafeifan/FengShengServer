
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

        /// <summary>
        /// 请求房间全部信息协议号
        /// </summary>
        public const uint RequestRoomInfo_C2S = 0x0105;
        public const uint RequestRoomInfo_S2C = 0x0105;

        /// <summary>
        /// 玩家准备协议号
        /// </summary>
        public const uint ReadyStatus_C2S = 0x0106;
        public const uint ReadyStatus_S2C = 0x0106;

        /// <summary>
        /// 游戏开始协议号
        /// </summary>
        public const uint GameStart_C2S = 0x0200;
        public const uint GameStart_S2C = 0x0200;

        /// <summary>
        /// 身份分发协议号
        /// </summary>
        public const uint Identity_S2C = 0x0201;

        /// <summary>
        /// 角色选择协议号
        /// </summary>
        public const uint CharacterChoose_C2S = 0x0202;

        /// <summary>
        /// 角色选择列表协议号
        /// </summary>
        public const uint CharacterChooseList_S2C = 0x0203;

        /// <summary>
        /// 游戏完成协议号
        /// </summary>
        public const uint GameComplete_S2C = 0x0204;

        /// <summary>
        /// 发牌协议号
        /// </summary>
        public const uint DealCards_S2C = 0x0205;
    }
}
