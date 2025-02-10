using System;

namespace FengShengServer
{
    public class ChairInfo
    {
        public int ChairID { get; set; }

        public UserData UserData { get; set; }

        public bool IsReady { get; set; }

        public bool IsRobot { get; set; }

        public bool IsNull { get; set; }

        public void Clear()
        {
            if (UserData != null)
            {
                UserData.RoomInfo = null;
                UserData.IdentityType = LoginServer.Game.IdentityType.None;
                UserData.CharacterType = LoginServer.Game.CharacterType.None;
                UserData = null;
            }

            IsReady = false;
            IsRobot = false;
            IsNull = true;
        }

        /// <summary>
        /// 是否是房主
        /// </summary>
        public bool IsRoomOwner()
        {
            return ChairID == 1;
        }
    }
}
