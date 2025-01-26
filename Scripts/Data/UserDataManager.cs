using System;
using System.Collections.Generic;

namespace FengShengServer
{
    public class UserDataManager
    {
        private static UserDataManager mInstance;
        public static UserDataManager Instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = new UserDataManager();
                return mInstance;
            }
        }

        private List<UserData> mUserList = new List<UserData>();
        public List<UserData> UserList { get { return mUserList; } }

        public void Start()
        {
            Console.WriteLine("用户数据管理器已启动");
        }

        public void Close()
        {
            mUserList.Clear();

            Console.WriteLine("用户数据管理器已关闭");
        }

        public void OnUserOffline(object obj)
        {
            if (obj == null) return;

            RemoveUser(obj as UserData);
        }

        public bool AddUser(UserData userData)
        {
            if (userData == null) return false;

            for (int i = 0; i < mUserList.Count; i++)
            {
                if (mUserList[i].Name == userData.Name)
                {
                    return false;
                }
            }

            mUserList.Add(userData);
            Console.WriteLine($"用户{userData.Name}已登录");
            return true;
        }

        public bool RemoveUser(UserData userData) 
        {
            if (userData == null) return false;

            bool result = mUserList.Remove(userData);
            if (result)
            {
                Console.WriteLine($"用户{userData.Name}已退出");
            }
            return result;
        }

        public UserData GetUserData(string name)
        {
            for (int i = 0; i < mUserList.Count; i++)
            {
                if (mUserList[i].Name == name)
                {
                    return mUserList[i];
                }
            }
            return null;
        }

        public UserData GetUserDataByConnectID(int connectID)
        {
            for (int i = 0; i < mUserList.Count; i++)
            {
                if (mUserList[i].CSConnect != null &&
                    mUserList[i].CSConnect.ID == connectID)
                {
                    return mUserList[i];
                }
            }
            return null;
        }

        public List<UserData> GetUserDataByRoomNub(int roomNub)
        {
            List<UserData> result = new List<UserData>();
            for (int i = 0; i < mUserList.Count; i++)
            {
                if (mUserList[i].RoomInfo != null &&
                    mUserList[i].RoomInfo.RoomNub == roomNub)
                {
                    result.Add(mUserList[i]);
                }
            }
            return result;
        }

    }
}
