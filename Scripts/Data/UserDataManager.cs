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
            EventManager.Instance.AddListener(EventManager.Event_OnUserStatusChange, OnUserStatusChange);

            Console.WriteLine("用户数据管理器已启动");
            Test();
        }

        public void Close()
        {
            mUserList.Clear();

            EventManager.Instance.AddListener(EventManager.Event_OnUserStatusChange, OnUserStatusChange);

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

        private void OnUserStatusChange(object obj)
        {
            if (obj != null && obj is UserData)
            {
                var userData = (UserData)obj;
                if (userData != null && userData.Status == UserStatus.Offline)
                {
                    RemoveUser(userData);
                }
            }
        }

        private void Test()
        {
            AddUser(new UserData()
            {
                CSConnect = null,
                Name = "test1",
                RoomInfo = null,
                Status = UserStatus.Online
            });
            Console.WriteLine("用户test1已被添加到用户列表");

            AddUser(new UserData()
            {
                CSConnect = null,
                Name = "test2",
                RoomInfo = null,
                Status = UserStatus.Online
            });
            Console.WriteLine("用户test2已被添加到用户列表");

            AddUser(new UserData()
            {
                CSConnect = null,
                Name = "test3",
                RoomInfo = null,
                Status = UserStatus.Online
            });
            Console.WriteLine("用户test3已被添加到用户列表");

            AddUser(new UserData()
            {
                CSConnect = null,
                Name = "test4",
                RoomInfo = null,
                Status = UserStatus.Online
            });
            Console.WriteLine("用户test4已被添加到用户列表");

            AddUser(new UserData()
            {
                CSConnect = null,
                Name = "test5",
                RoomInfo = null,
                Status = UserStatus.Online
            });
            Console.WriteLine("用户test5已被添加到用户列表");

            AddUser(new UserData()
            {
                CSConnect = null,
                Name = "test6",
                RoomInfo = null,
                Status = UserStatus.Online
            });
            Console.WriteLine("用户test6已被添加到用户列表");

            AddUser(new UserData()
            {
                CSConnect = null,
                Name = "test7",
                RoomInfo = null,
                Status = UserStatus.Online
            });
            Console.WriteLine("用户test7已被添加到用户列表");

            AddUser(new UserData()
            {
                CSConnect = null,
                Name = "test8",
                RoomInfo = null,
                Status = UserStatus.Online
            });
            Console.WriteLine("用户test8已被添加到用户列表");
        }

    }
}
