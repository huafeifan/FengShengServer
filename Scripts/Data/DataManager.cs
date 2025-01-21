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
            return true;
        }

    }
}
