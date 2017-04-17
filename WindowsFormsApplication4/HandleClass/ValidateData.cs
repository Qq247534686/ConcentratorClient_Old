using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 集中器控制客户端.HandleClass
{
    public class ValidateData
    {
        /// <summary>
        /// 验证数组是否不为null或者数组长度为0
        /// </summary>
        /// <typeparam name="T">数组类型</typeparam>
        /// <param name="theArray">要传入的数组</param>
        /// <returns></returns>
        public static bool ArrayIsNullOrZero<T>(List<T> theArray)
        {
            return theArray == null || theArray.Count <= 0 ? false : true;
        }
        public static bool ArrayIsNullOrObject<T>(T obj)
        {
            return obj == null? false : true;
        }
        public static bool StringIsNullOrZero(string str)
        {
            return String.IsNullOrWhiteSpace(str) == true ? false : true;
        }
        public static bool CellIsNullOrZero(DataGridViewCell cell)
        {
            return String.IsNullOrEmpty(cell.Value.ToString()) == true ? false : true;
        }
        public static bool ValidateString(string str)
        {
            bool isTrue = false;
            if (Regex.IsMatch(str, @"^[\u4e00-\u9fa5|\d|a-zA-Z]+$"))
            {
                isTrue = true;
            }
            return isTrue;
        }
    }
}
