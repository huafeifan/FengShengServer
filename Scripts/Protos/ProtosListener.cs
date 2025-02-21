using System;
using System.Collections.Generic;

namespace FengShengServer
{
    public class ProtosListener
    {
        private Dictionary<uint, ProtosListenerObject> mProtosListeners = new Dictionary<uint, ProtosListenerObject>();

        public void AddListener(uint cmd, Action<object> callBack)
        {
            if (mProtosListeners.ContainsKey(cmd))
            {
                mProtosListeners[cmd].Action += callBack;
            }
            else
            {
                var listener = new ProtosListenerObject();
                listener.RemainCount = int.MaxValue;
                listener.Action = callBack;
                mProtosListeners.Add(cmd, listener);
            }
        }

        public void AddListener(uint cmd, Action<object> callBack, int listenerCount)
        {
            if (mProtosListeners.ContainsKey(cmd))
            {
                mProtosListeners[cmd].Action += callBack;
            }
            else
            {
                var listener = new ProtosListenerObject();
                listener.RemainCount = listenerCount;
                listener.Action = callBack;
                mProtosListeners.Add(cmd, listener);
            }
        }

        public void RemoveListener(uint cmd, Action<object> callBack)
        {
            if (mProtosListeners.ContainsKey(cmd))
            {
                mProtosListeners[cmd].Action -= callBack;
                if (mProtosListeners[cmd].Action == null)
                {
                    mProtosListeners.Remove(cmd);
                }
            }
        }

        public void TriggerEvent(uint cmd, object data)
        {
            if (mProtosListeners.ContainsKey(cmd))
            {
                Action<object> callBack = (Action<object>)mProtosListeners[cmd].Action.Clone();
                mProtosListeners[cmd].RemainCount--;
                if (mProtosListeners[cmd].RemainCount <= 0)
                {
                    mProtosListeners.Remove(cmd);
                }
                callBack.Invoke(data);
            }
        }

        public void Clear()
        {
            mProtosListeners.Clear();
        }

    }
}
