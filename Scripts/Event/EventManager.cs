using System;
using System.Collections.Generic;

namespace FengShengServer
{
    public class EventManager
    {
        public const string Event_OnConnectInterrupt = "OnConnectInterrupt";
        public const string Event_OnUserStatusChange = "OnUserStatusChange";

        private static EventManager mInstance;
        public static EventManager Instance
        {
            get
            {
                if (mInstance == null) 
                    mInstance = new EventManager();
                return mInstance;
            }
        }

        /// <summary>
        /// 常规监听 事件名，事件回调
        /// </summary>
        private Dictionary<string, Action<System.Object>> mEventListeners = new Dictionary<string, Action<System.Object>>();

        public void Start()
        {
            Console.WriteLine("事件管理器已启动");
        }

        public void Close()
        {
            mEventListeners.Clear();

            Console.WriteLine("事件管理器已关闭");
        }

        #region 常规事件组

        public void AddListener(string eventName, Action<System.Object> callBack)
        {
            if (mEventListeners.ContainsKey(eventName))
            {
                mEventListeners[eventName] += callBack;
            }
            else
            {
                mEventListeners.Add(eventName, callBack);
            }
        }

        public void RemoveListener(string eventName, Action<System.Object> callBack)
        {
            if (mEventListeners.ContainsKey(eventName))
            {
                mEventListeners[eventName] -= callBack;
                if (mEventListeners[eventName] == null)
                {
                    mEventListeners.Remove(eventName);
                }
            }
        }

        public void TriggerEvent(string eventName, object arg)
        {
            if (mEventListeners.ContainsKey(eventName))
            {
                Action<System.Object> callBack = mEventListeners[eventName];
                callBack.Invoke(arg);
            }
        }
        #endregion
    }
}
