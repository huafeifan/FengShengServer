
namespace FengShengServer
{
    public class UserData
    {
        public string Name { get; set; }

        public RoomInfo RoomInfo { get; set; }

        public CSConnect CSConnect { get; set; }

        /// <summary>
        /// 房间信息和用户信息相互关联
        /// </summary>
        /// <param name="roomInfo"></param>
        public void SetRoomInfo(RoomInfo roomInfo)
        {
            RoomInfo = roomInfo;
            roomInfo.AddUser(this);
        }
    }
}
