using System;
using System.Collections.Generic;

namespace FengShengServer
{
    public class ProtosListener
    {
        private Dictionary<uint, Action<object>> mProtosListeners = new Dictionary<uint, Action<object>>();

        public void AddListener(uint cmd, Action<object> callBack)
        {
            if (mProtosListeners.ContainsKey(cmd))
            {
                mProtosListeners[cmd] += callBack;
            }
            else
            {
                mProtosListeners.Add(cmd, callBack);
            }
        }

        public void RemoveListener(uint cmd, Action<object> callBack)
        {
            if (mProtosListeners.ContainsKey(cmd))
            {
                mProtosListeners[cmd] -= callBack;
                if (mProtosListeners[cmd] == null)
                {
                    mProtosListeners.Remove(cmd);
                }
            }
        }

        public void TriggerEvent(uint cmd, object data)
        {
            if (mProtosListeners.ContainsKey(cmd))
            {
                Action<object> callBack = (Action<object>)mProtosListeners[cmd].Clone();
                callBack.Invoke(data);
            }
        }

        public void Clear()
        {
            mProtosListeners.Clear();
        }

    }
}
