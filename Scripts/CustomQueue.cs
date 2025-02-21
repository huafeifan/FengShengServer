using System.Collections.Generic;

namespace FengShengServer
{
    public class CustomQueue<T>
    {
        private Queue<T> mQueue;
        public int Count { get { return mQueue.Count; } }

        public CustomQueue()
        {
            mQueue = new Queue<T>();
        }

        public T Peek()
        {
            return mQueue.Peek();
        }

        public void Enqueue(T t)
        {
            mQueue.Enqueue(t);
        }

        public T Dequeue()
        {
            return mQueue.Dequeue();
        }

        public void Clear() 
        {
            mQueue.Clear();
        }

        public void InsertFirst(T t)
        {
            var temp = new Queue<T>();
            temp.Enqueue(t);
            while (mQueue.Count > 0)
            {
                temp.Enqueue(mQueue.Dequeue());
            }
            mQueue = temp;
        }

        public void InsertFirst(Queue<T> t)
        {
            var temp = new Queue<T>();
            while (t.Count > 0)
            {
                temp.Enqueue(t.Dequeue());
            }
            while (mQueue.Count > 0)
            {
                temp.Enqueue(mQueue.Dequeue());
            }
            mQueue = temp;
        }


    }
}
