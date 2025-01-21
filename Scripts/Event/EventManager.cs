using System;
using System.Collections.Generic;

namespace FengShengServer
{
    public class EventManager
    {
        public const string Event_OnConnectInterrupt = "OnConnectInterrupt";
        public const string Event_OnUserOffline = "OnUserOffline";

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

        private Dictionary<string, Action<System.Object>> mEventListeners = new Dictionary<string, Action<System.Object>>();

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

    }
}
