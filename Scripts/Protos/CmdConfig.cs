
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
        public const uint DealCards_C2S = 0x0205;
        public const uint DealCards_S2C = 0x0205;

        /// <summary>
        /// 轮到某人的回合协议号
        /// </summary>
        public const uint GameTurn_S2C = 0x0206;

        /// <summary>
        /// 某人回合结束协议号
        /// </summary>
        public const uint GameTurnEnd_C2S = 0x0207;
        public const uint GameTurnEnd_S2C = 0x0207;

        /// <summary>
        /// 某人回合开始协议号
        /// </summary>
        public const uint GameTurnStart_C2S = 0x0208;
        public const uint GameTurnStart_S2C = 0x0208;

        /// <summary>
        /// 某人回合操作结束协议号
        /// </summary>
        public const uint GameTurnOpertateEnd_C2S = 0x0209;
        public const uint GameTurnOpertateEnd_S2C = 0x0209;

        /// <summary>
        /// 某人回合弃牌协议号
        /// </summary>
        public const uint GameTurnDisCard_C2S = 0x020A;
        public const uint GameTurnDisCard_S2C = 0x020A;

        /// <summary>
        /// 手牌数量协议号
        /// </summary>
        //public const uint HandCardCount_C2S = 0x020B;
        public const uint HandCardCount_S2C = 0x020B;

        /// <summary>
        /// 情报宣言协议号
        /// </summary>
        public const uint InformationDeclaration_C2S = 0x020C;
        public const uint InformationDeclaration_S2C = 0x020C;

        /// <summary>
        /// 响应是否出牌协议号
        /// </summary>
        public const uint PlayHandCardResponse_C2S = 0x020D;
        public const uint PlayHandCardResponse_S2C = 0x020D;

        /// <summary>
        /// 等待情报发出协议号
        /// </summary>
        public const uint WaitInformationTransmit_S2C = 0x020E;

        /// <summary>
        /// 情报传出协议号
        /// </summary>
        public const uint InformationTransmitReady_C2S = 0x020F;
        public const uint InformationTransmitReady_S2C = 0x020F;

        /// <summary>
        /// 等待情报接受协议号
        /// </summary>
        public const uint WaitInformationReceive_C2S = 0x0210;
        public const uint WaitInformationReceive_S2C = 0x0210;

        /// <summary>
        /// 出牌协议号
        /// </summary>
        public const uint PlayHandCard_C2S = 0x0211;
        public const uint PlayHandCard_S2C = 0x0211;

        /// <summary>
        /// 情报接收通知协议号
        /// </summary>
        public const uint InformationReceive_S2C = 0x0212;

        /// <summary>
        /// 询问是否出牌协议号
        /// </summary>
        public const uint AskPlayHandCard_S2C = 0x0213;

        /// <summary>
        /// 情报接收成功通知协议号
        /// </summary>
        public const uint InformationReceiveSuccess_S2C = 0x0214;

        /// <summary>
        /// 通知情报传回情报传出者协议号
        /// </summary>
        public const uint InformationTransmitSender_S2C = 0x0215;

        /// <summary>
        /// 情报数量广播协议号
        /// </summary>
        public const uint InformationCount_S2C = 0x0216;

        /// <summary>
        /// 询问是否使用识破协议号
        /// </summary>
        public const uint AskUseShiPo_C2S = 0x0218;
        public const uint AskUseShiPo_S2C = 0x0218;

        /// <summary>
        /// 卡牌效果触发结果协议号
        /// </summary>
        public const uint TriggerCardResult_S2C = 0x0219;

        /// <summary>
        /// 情报传递协议号
        /// </summary>
        public const uint InformationTransmit_S2C = 0x021A;

        /// <summary>
        /// 调包效果协议号
        /// </summary>
        public const uint UseDiaoBao_C2S = 0x021B;
        public const uint UseDiaoBao_S2C = 0x021B;

        /// <summary>
        /// 破译效果协议号
        /// </summary>
        public const uint UsePoYi_C2S = 0x021C;
        public const uint UsePoYi_S2C = 0x021C;

        /// <summary>
        /// 烧毁效果协议号
        /// </summary>
        public const uint UseShaoHui_C2S = 0x021D;
        public const uint UseShaoHui_S2C = 0x021D;

        /// <summary>
        /// 增援效果协议号
        /// </summary>
        public const uint UseZengYuan_C2S = 0x021E;
        public const uint UseZengYuan_S2C = 0x021E;

        /// <summary>
        /// 机密文件效果协议号
        /// </summary>
        public const uint UseJiMiWenJian_C2S = 0x021F;
        public const uint UseJiMiWenJian_S2C = 0x021F;

        /// <summary>
        /// 锁定效果协议号
        /// </summary>
        public const uint UseSuoDing_C2S = 0x0220;
        public const uint UseSuoDing_S2C = 0x0220;

        /// <summary>
        /// 截获效果协议号
        /// </summary>
        public const uint UseJieHuo_C2S = 0x0221;
        public const uint UseJieHuo_S2C = 0x0221;
    }
}
