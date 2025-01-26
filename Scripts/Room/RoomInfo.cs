using System;
using System.Collections.Generic;

namespace FengShengServer
{
    public class RoomInfo
    {
        public string RoomName { get; set; }

        public int RoomNub { get; set; }

        public bool IsOpen { get; set; }

        public List<ChairInfo> Chairs { get; set; }

        public RoomInfo()
        {
            Chairs = new List<ChairInfo>();
            RoomName = string.Empty;
            RoomNub = -1;
            IsOpen = false;
        }

        public int GetChairCount()
        {
            return Chairs.Count;
        }

        public int GetUserCount()
        {
            int count = 0;
            for (int i = 0; i < Chairs.Count; i++)
            {
                if (!Chairs[i].IsNull && !Chairs[i].IsRobot && Chairs[i].UserData != null)
                {
                    count++;
                }
            }
            return count;
        }

        public int GetRobotCount()
        {
            int count = 0;
            for (int i = 0; i < Chairs.Count; i++)
            {
                if (!Chairs[i].IsNull && Chairs[i].IsRobot)
                {
                    count++;
                }
            }
            return count;
        }

        public void InitChairs(int chairCounts)
        {
            Chairs.Clear();
            for (int i = 1; i <= chairCounts; i++)
            {
                ChairInfo chair = new ChairInfo();
                chair.ChairID = i;
                chair.Clear();
                Chairs.Add(chair);
            }
        }

        public bool IsFull()
        {
            for (int i = 0; i < Chairs.Count; i++)
            {
                if (Chairs[i].IsNull)
                {
                    return false;
                }
            }
            return true;
        }

        public void AddUser(UserData user)
        {
            for (int i = 0; i < Chairs.Count; i++)
            {
                if (Chairs[i].IsNull)
                {
                    Chairs[i].IsNull = false;
                    Chairs[i].IsReady = false;
                    Chairs[i].IsRobot = false;

                    Chairs[i].UserData = user;
                    user.RoomInfo = this;
                    break;
                }
            }
        }

        public bool RemoveUser(string userName)
        {
            for (int i = 0; i < Chairs.Count; i++)
            {
                if (Chairs[i].IsNull == false &&
                    Chairs[i].UserData != null &&
                    Chairs[i].UserData.Name == userName)
                {
                    Chairs[i].Clear();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否是房主
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool IsRoomOwner(string userName)
        {
            if (Chairs[0].UserData != null && Chairs[0].UserData.Name == userName)
            {
                return true;
            }
            return false;
        }

        public List<UserData> GetAllUserData()
        {
            List<UserData> result = new List<UserData>();
            for (int i = 0; i < Chairs.Count; i++)
            {
                if (Chairs[i].IsNull == false && Chairs[i].UserData != null)
                    result.Add(Chairs[i].UserData);
            }
            return result;
        }

        public void Close()
        {
            IsOpen = false;
            RoomName = string.Empty;
            for (int i = 0; i < Chairs.Count; i++)
            {
                Chairs[i].Clear();
            }
        }

    }
}
