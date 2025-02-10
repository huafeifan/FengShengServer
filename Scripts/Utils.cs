using System;
using System.Collections.Generic;

namespace FengShengServer
{
    public static class Utils
    {
        /// <summary>
        /// 洗牌
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void FisherYatesShuffle<T>(this List<T> list)
        {
            Random rnd = new Random();
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = rnd.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]); // 使用元组解构来交换元素
            }

        }
    }

}
