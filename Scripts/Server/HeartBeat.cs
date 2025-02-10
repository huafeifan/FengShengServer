using System;
using System.Timers;

namespace FengShengServer
{
    public class HeartBeat
    {
        /// <summary>
        /// �������
        /// </summary>
        private int mInterval;

        /// <summary>
        /// ������ʧ����
        /// </summary>
        private int mHeartBeatMissCount;

        /// <summary>
        /// ������ʧ�������ޣ��ﵽ���޶Ͽ�����
        /// </summary>
        private const int mHeartBeatMissCountLimit = 10;

        /// <summary>
        /// ��ǰʱ�������Ƿ�������
        /// </summary>
        private bool mHeartBeatFlag;

        /// <summary>
        /// �Ƿ��ӡ��־
        /// </summary>
        private bool mIsDebug;

        /// <summary>
        /// Ĭ����Ϣ��
        /// </summary>
        private byte[] mDefaultMessage = new byte[0];

        private Timer mTimer;
        private CSConnect mCSConnect;

        public HeartBeat()
        {
            mTimer = new Timer();
        }

        /// <summary>
        /// ���ü�ʱ��
        /// </summary>
        /// <param name="interval">ʱ����</param>
        public void SetTimer(int interval = 1000)
        {
            mInterval = interval;
            mTimer.Elapsed += OnTimedEvent;
            mTimer.AutoReset = true;
        }

        /// <summary>
        /// �������Ӷ���
        /// </summary>
        /// <param name="tcpClient"></param>
        public void SetCSConnect(CSConnect cs)
        {
            mCSConnect = cs;
        }

        /// <summary>
        /// ���õ�ǰʱ������������ʶ
        /// </summary>
        /// <param name="flag"></param>
        public void SetHeartBeatFlag(bool flag)
        {
            mHeartBeatFlag = flag;
        }

        /// <summary>
        /// �Ƿ��ӡ������־
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
                Console.WriteLine($"�ͻ���ID:{mCSConnect.ID} RemoteEndPoint:{mCSConnect.RemoteEndPoint} �����������ѿ���");
        }

        public void Close()
        {
            mHeartBeatFlag = false;
            mHeartBeatMissCount = 0;
            mTimer.Stop();
            mTimer.Enabled= false;
            mTimer.Elapsed -= OnTimedEvent;

            if (mIsDebug)
                Console.WriteLine($"�ͻ���ID:{mCSConnect.ID} RemoteEndPoint:{mCSConnect.RemoteEndPoint} �����������ѹر�");
        }

        public byte[] GetHeatBeatData()
        {
            return mDefaultMessage;
        }

    }
}
