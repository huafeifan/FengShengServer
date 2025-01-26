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
                UserData = null;
            }

            IsReady = false;
            IsRobot = false;
            IsNull = true;
        }
    }
}
