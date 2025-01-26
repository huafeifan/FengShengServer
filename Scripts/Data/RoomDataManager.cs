using System;
using System.Collections.Generic;

namespace FengShengServer
{
    public class RoomDataManager
    {
        private static RoomDataManager mInstance;
        public static RoomDataManager Instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = new RoomDataManager();
                return mInstance;
            }
        }

        //RoomNub, RoomInfo
        private Dictionary<int, RoomInfo> mRoomList = new Dictionary<int, RoomInfo>(100);
        public Dictionary<int, RoomInfo> RoomList { get { return mRoomList; } }

        public void Start()
        {
            for (int i = 1; i <= 100; i++)
            {
                mRoomList.Add(i, null);
            }

            TryAddRoom("2025.1.23", 4, out _);
            TryAddRoom("哦还有", 5, out _);
            TryAddRoom("nice", 6, out _);
            TryAddRoom("hello", 4, out _);

            Console.WriteLine("房间数据管理器已启动");
        }

        public void Close()
        {
            mRoomList.Clear();

            Console.WriteLine("房间数据管理器已关闭");
        }

        public bool TryAddRoom(string roomName, int chairCount, out RoomInfo roomInfo)
        {
            int key = -1;
            foreach (var room in mRoomList)
            {
                if (room.Value == null || room.Value.IsOpen == false)
                {
                    key = room.Key;
                    break;
                }
            }

            if (key != -1)
            {
                if (mRoomList[key] == null)
                {
                    mRoomList[key] = new RoomInfo();
                }

                if (mRoomList[key].IsOpen == false)
                {
                    mRoomList[key].RoomName = roomName;
                    mRoomList[key].RoomNub = key;
                    mRoomList[key].InitChairs(chairCount);
                    mRoomList[key].IsOpen = true;
                    roomInfo = mRoomList[key];
                    return true;
                }
            }
            roomInfo = null;
            return false;
        }

        public void CloseRoom(int roomNub)
        {
            RoomInfo roomInfo = GetRoomInfo(roomNub);
            if (roomInfo != null)
            {
                roomInfo.Close();
            }
        }

        public RoomInfo GetRoomInfo(int roomNub)
        {
            if (mRoomList.ContainsKey(roomNub))
            {
                return mRoomList[roomNub];
            }
            return null;
        }

        /// <summary>
        /// 将RoomInfo注入消息格式中
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        public void RoomInfoConvert(RoomInfo arg1, LoginServer.Room.RoomInfo arg2)
        {
            arg2.RoomNub = arg1.RoomNub;
            arg2.RoomName = arg1.RoomName;
            arg2.ChairCount = arg1.GetChairCount();
            arg2.UserCount = arg1.GetUserCount();
            arg2.RobotCount = arg1.GetRobotCount();
        }

    }
}
