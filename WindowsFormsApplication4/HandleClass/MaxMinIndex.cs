using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 集中器控制客户端.HandleClass
{
    public static class MaxMinIndex
    {

        /// <summary>
        /// 最大值
        /// </summary>
        /// <param name="pa"></param>
        /// <returns></returns>
        public static int GetMaxAndIndex(int[] pa)
        {
            int index = -1;//定义变量存最大值的索引
            double temp;
            if (pa.Length != 0)
            {
                temp = pa[0];
                index = 0;
                for (int i = 0; i < pa.Length; i++)
                {
                    if (temp < pa[i])
                    {
                        index = i;
                        temp = pa[i];
                    }
                }
            }
            return index;
        }
        /// <summary>
        /// 最小值
        /// </summary>
        /// <param name="pa"></param>
        /// <returns></returns>
        public static int GetMinAndIndex(int[] pa)
        {
            int index = -1;//定义变量存最小值的索引
            double temp;
            if (pa.Length != 0)
            {
                temp = pa[0];
                index = 0;
                for (int i = 0; i < pa.Length; i++)
                {
                    if (temp > pa[i])
                    {
                        index = i;
                        temp = pa[i];
                    }
                }
            }
            return index;
        }

    }
}
