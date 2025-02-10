using System;
using System.Timers;

namespace FengShengServer
{
    public class HeartBeat
    {
        /// <summary>
        /// 心跳间隔
        /// </summary>
        private int mInterval;

        /// <summary>
        /// 心跳丢失次数
        /// </summary>
        private int mHeartBeatMissCount;

        /// <summary>
        /// 心跳丢失次数上限，达到上限断开连接
        /// </summary>
        private const int mHeartBeatMissCountLimit = 10;

        /// <summary>
        /// 当前时间间隔内是否有心跳
        /// </summary>
        private bool mHeartBeatFlag;

        /// <summary>
        /// 是否打印日志
        /// </summary>
        private bool mIsDebug;

        /// <summary>
        /// 默认消息包
        /// </summary>
        private byte[] mDefaultMessage = new byte[0];

        private Timer mTimer;
        private CSConnect mCSConnect;

        public HeartBeat()
        {
            mTimer = new Timer();
        }

        /// <summary>
        /// 设置计时器
        /// </summary>
        /// <param name="interval">时间间隔</param>
        public void SetTimer(int interval = 1000)
        {
            mInterval = interval;
            mTimer.Elapsed += OnTimedEvent;
            mTimer.AutoReset = true;
        }

        /// <summary>
        /// 传入连接对象
        /// </summary>
        /// <param name="tcpClient"></param>
        public void SetCSConnect(CSConnect cs)
        {
            mCSConnect = cs;
        }

        /// <summary>
        /// 设置当前时间间隔内心跳标识
        /// </summary>
        /// <param name="flag"></param>
        public void SetHeartBeatFlag(bool flag)
        {
            mHeartBeatFlag = flag;
        }

        /// <summary>
        /// 是否打印心跳日志
        /// </summary>
        /// <param name="flag"></param>
        public void SetDebug(bool flag)
        {
            mIsDebug = flag;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            if (mHeartBeatFlag) 
            {
                mHeartBeatMissCount = 0;
                SetHeartBeatFlag(false);
            }
            else
            {
                mHeartBeatMissCount++;
            }

            if (mHeartBeatMissCount >= mHeartBeatMissCountLimit)
            {
                EventManager.Instance.TriggerEvent(EventManager.Event_OnConnectInterrupt, new NetworkEventPackage() { ID = mCSConnect.ID });
            }
        }

        public void Start()
        {
            mHeartBeatFlag = false;
            mHeartBeatMissCount = 0;
            mTimer.Interval = mInterval;
            mTimer.Enabled = true;
            mTimer.Start();

            if (mIsDebug)
                Console.WriteLine($"客户端ID:{mCSConnect.ID} RemoteEndPoint:{mCSConnect.RemoteEndPoint} 心跳处理器已开启");
        }

        public void Close()
        {
            mHeartBeatFlag = false;
            mHeartBeatMissCount = 0;
            mTimer.Stop();
            mTimer.Enabled= false;
            mTimer.Elapsed -= OnTimedEvent;

            if (mIsDebug)
                Console.WriteLine($"客户端ID:{mCSConnect.ID} RemoteEndPoint:{mCSConnect.RemoteEndPoint} 心跳处理器已关闭");
        }

        public byte[] GetHeatBeatData()
        {
            return mDefaultMessage;
        }

    }
}
