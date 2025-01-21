using System;
using System.Collections.Generic;

namespace FengShengServer
{
    public class ProtosListener
    {
        private Dictionary<uint, Action<byte[]>> mProtosListeners = new Dictionary<uint, Action<byte[]>>();

        public void AddListener(uint cmd, Action<byte[]> callBack)
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

        public void RemoveListener(uint cmd, Action<byte[]> callBack)
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

        public void TriggerEvent(uint cmd, byte[] data)
        {
            if (mProtosListeners.ContainsKey(cmd))
            {
                Action<byte[]> callBack = (Action<byte[]>)mProtosListeners[cmd].Clone();
                callBack.Invoke(data);
            }
        }

        public void Close()
        {
            mProtosListeners.Clear();
        }

    }
}
