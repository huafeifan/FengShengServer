using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using Google.Protobuf;

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
        /// 发送包对象池
        /// </summary>
        private Queue<SenderPackage> mSenderPool = new Queue<SenderPackage>();
        /// <summary>
        /// 发送包队列
        /// </summary>
        private Queue<SenderPackage> mSenderList = new Queue<SenderPackage>();

        public void Start()
        {
            mCts = new CancellationTokenSource();
            _ = Task.Run(() => SendMessageAsync());
            Console.WriteLine("消息发送管理器已启动");
        }

        public void Close()
        {
            if (mCts != null)
            {
                mCts.Cancel();
            }
            mSenderPool.Clear();
            mSenderList.Clear();
            Console.WriteLine("消息发送管理器已关闭");
        }

        public void AddSendMessage(CSConnect connect, uint cmd, byte[] bytes, bool isLog)
        {
            mSenderList.Enqueue(GetSenderPackage(connect, cmd, bytes, isLog));
        }

        public void AddSendMessage(CSConnect connect, uint cmd, IMessage sendData, bool isLog)
        {
            mSenderList.Enqueue(GetSenderPackage(connect, cmd, sendData, isLog));
        }

        private SenderPackage GetSenderPackage(CSConnect connect, uint cmd, byte[] bytes, bool isLog)
        {
            if (mSenderPool.Count == 0)
            {
                return new SenderPackage(connect, cmd, bytes, isLog);
            }

            var result = mSenderPool.Dequeue();
            if (result == null) 
            {
                return GetSenderPackage(connect, cmd, bytes, isLog);
            }

            result.SetData(connect, cmd, bytes, isLog);
            return result;
        }

        private SenderPackage GetSenderPackage(CSConnect connect, uint cmd, IMessage sendData, bool isLog)
        {
            if (mSenderPool.Count == 0)
            {
                return new SenderPackage(connect, cmd, sendData, isLog);
            }

            var result = mSenderPool.Dequeue();
            if (result == null)
            {
                return GetSenderPackage(connect, cmd, sendData, isLog);
            }

            result.SetData(connect, cmd, sendData, isLog);
            return result;
        }

        private async Task SendMessageAsync()
        {
            while (!mCts.IsCancellationRequested)
            {
                if (mSenderList.Count == 0)
                {
                    await Task.Delay(100);
                }
                else
                {
                    await Task.Delay(10);
                    var sendPackage = mSenderList.Dequeue();
                    try
                    {
                        if (sendPackage == null)
                        {
                            continue;
                        }

                        var wait = sendPackage.WriteAsync();
                        if (wait == null)
                        {
                            mSenderPool.Enqueue(sendPackage);
                            continue;
                        }

                        await wait;
                        if (sendPackage.IsLog)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(sendPackage.GetLog());
                            Console.ResetColor();
                        }
                        mSenderPool.Enqueue(sendPackage);
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine("发送数据时发生IOException：" + ex.Message);
                    }
                    catch (OperationCanceledException)
                    {
                        // 正常情况下，当取消令牌被请求时，会抛出此异常
                    }
                }

            }
        }

    }
}
