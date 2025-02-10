using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Runtime.CompilerServices;

namespace FengShengServer
{
    public class SenderManager
    {
        private static SenderManager mInstance;
        public static SenderManager Instance
        {
            get
            {
                if (mInstance == null) 
                    mInstance = new SenderManager();
                return mInstance;
            }
        }

        private CancellationTokenSource mCts;
        /// <summary>
        /// ���Ͱ������
        /// </summary>
        private Queue<SenderPackage> mSenderPool = new Queue<SenderPackage>();
        /// <summary>
        /// ���Ͱ�����
        /// </summary>
        private Queue<SenderPackage> mSenderList = new Queue<SenderPackage>();

        public void Start()
        {
            mCts = new CancellationTokenSource();
            _ = Task.Run(() => SendMessageAsync());
            Console.WriteLine("��Ϣ���͹�����������");
        }

        public void Close()
        {
            if (mCts != null)
            {
                mCts.Cancel();
            }
            mSenderPool.Clear();
            mSenderList.Clear();
            Console.WriteLine("��Ϣ���͹������ѹر�");
        }

        public void AddSendMessage(CSConnect connect, uint cmd, byte[] bytes, bool isLog)
        {
            mSenderList.Enqueue(GetSenderPackage(connect, cmd, bytes, isLog));
        }

        private SenderPackage GetSenderPackage(CSConnect connect, uint cmd, byte[] bytes, bool isLog)
        {
            if (mSenderPool.Count == 0)
            {
                return new SenderPackage(connect, cmd, bytes, isLog);
            }

            var result = mSenderPool.Dequeue();
            result.SetData(connect, cmd, bytes, isLog);
            return result;
        }

        private async Task SendMessageAsync()
        {
            while (!mCts.IsCancellationRequested)
            {
                try
                {
                    if (mSenderList.Count == 0)
                    {
                        await Task.Delay(100);
                    }
                    else
                    {
                        var sendPackage = mSenderList.Dequeue();
                        await sendPackage.WriteAsync();
                        if (sendPackage.IsLog)
                        {
                            Console.WriteLine(sendPackage.GetLog());
                        }
                        mSenderPool.Enqueue(sendPackage);
                        await Task.Delay(1);
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine("��������ʱ����IOException��" + ex.Message);
                    break;
                }
                catch (OperationCanceledException)
                {
                    // ��������£���ȡ�����Ʊ�����ʱ�����׳����쳣
                    break;
                }
            }
        }

    }
}
