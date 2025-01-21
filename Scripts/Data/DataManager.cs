using System;
using System.Collections.Generic;

namespace FengShengServer
{
    public class DataManager
    {
        private static DataManager mInstance;
        public static DataManager Instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = new DataManager();
                return mInstance;
            }
        }

        private List<UserData> mUserList = new List<UserData>();

        public void Start()
        {
            EventManager.Instance.AddListener(EventManager.Event_OnUserOffline, OnUserOffline);
        }

        public void Close()
        {
            EventManager.Instance.RemoveListener(EventManager.Event_OnUserOffline, OnUserOffline);
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

    }
}
