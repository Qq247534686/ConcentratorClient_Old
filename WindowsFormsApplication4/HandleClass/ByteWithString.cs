using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 集中器控制客户端.HandleClass
{
    /// <summary>
    /// 十六进制和字符串的互操作
    /// </summary>
    public static class ByteWithString
    {
        //public static Queue<byte[]> queueArray = new Queue<byte[]>();

        public static List<byte[]> queueArray = new List<byte[]>();
        #region 十六进制转汉字
        /// <summary>
        /// 十六进制转汉字
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static string hexToChainCode(string hex)　//１６进制转汉字
        {

            // 需要将 hex 转换成 byte 数组。
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                try
                {
                    // 每两个字符是一个 byte。
                    bytes[i] = byte.Parse(hex.Substring(i * 2, 2),
                    System.Globalization.NumberStyles.HexNumber);
                }
                catch (Exception msg)
                {
                    Log.LogWrite(msg);
                }
            }
            // 获得 GB2312，Chinese Simplified。
            System.Text.Encoding chs = System.Text.Encoding.GetEncoding("GB2312");
            return chs.GetString(bytes);
        }

        #endregion
        /// <summary>
        /// 字符串转16进制字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] strToToHexByte(string hexString)
        {
            
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return returnBytes;
        }
        /// <summary>
        /// 字节数组转16进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr.ToLower();
        }
        public static string byteToHexStrAppend(byte[] bytes, string str)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2") + str;
                }
            }
            return returnStr.TrimEnd().ToLower();
        }
        /// <summary>
        /// 比较字节数组是否相等
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool compareArray(byte[] a, byte[] b)
        {
            bool bo = false;
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == b[i])
                    bo = true;
                else
                {
                    bo = false;
                    break;
                }
            }
            return bo;
        }
        /// <summary>
        /// 转两位字节
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static List<int> ByteArrayToInt(byte[] bytes)
        {
            List<int> listInt = new List<int>();
            int i = 0;
            while (i < bytes.Length)
            {
                short s = 0;   //一个16位整形变量，初值为 0000 0000 0000 0000
                s = (short)(s ^ bytes[i + 1]);  //将b1赋给s的低8位
                s = (short)(s << 8);  //s的低8位移动到高8位
                s = (short)(s ^ bytes[i]);
                listInt.Add(s);
                i += 2;
            }
            return listInt;
        }
        /// <summary>
        /// 读取文件流转存到字节数组
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] FileStreamTobyteArray(string filePath)
        {
            byte[] buffer;
            using (StreamReader sr = new StreamReader(filePath))
            {
                filePath = sr.ReadToEnd();
                buffer = ByteWithString.strToToHexByte(filePath.Replace(" ", ""));
            }
            return buffer;
        }
        /// <summary>
        /// 指定的字节数组转时间
        /// </summary>
        /// <param name="dateTimeByte"></param>
        /// <returns></returns>
        public static DateTime ConvertCP56TIME2aToDateTime(byte[] dateTimeByte)
        {

            DateTime dt = new DateTime();
            string year = DateTime.Now.Year.ToString().Substring(0, 2) + dateTimeByte[6].ToString();
            string month = dateTimeByte[5].ToString();
            string day = (dateTimeByte[4] & 0x1f).ToString();
            string hour = dateTimeByte[3].ToString();
            string minute = dateTimeByte[2].ToString();
            string milliSecond = ((dateTimeByte[1] << 8 | dateTimeByte[0]) % 1000).ToString();
            string second = ((dateTimeByte[1] << 8 | dateTimeByte[0]) / 1000).ToString();
            dt = DateTime.Parse(year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + second + "." + milliSecond);
            return dt;
        }


        /// <summary>
        /// 字节数组转特定编码
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string bytetoCodeString(byte[] bytes, string encodingName)
        {
            return Encoding.GetEncoding(encodingName).GetString(bytes);
        }
        public static byte[] encodeingToByte(string info, string code)
        {
            return Encoding.GetEncoding(code).GetBytes(info);

        }
        public static byte[] returnByteLength(string str)
        {
            str = str.PadLeft(8, '0');
            return ByteWithString.strToToHexByte(str);
        }
        public static byte[] returnByteLength(string str, int count)
        {
            str = str.PadLeft(count, '0');
            return ByteWithString.strToToHexByte(str);
        }
        /// <summary>
        /// 整数转四位字节并且高位在前
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] intTo4Byte(int data)
        {
            byte[] sr = new byte[4];
            sr[0] = Convert.ToByte(data >> 24 & 0xff);
            sr[1] = Convert.ToByte(data >> 16 & 0xff);
            sr[2] = Convert.ToByte(data >> 8 & 0xff);
            sr[3] = Convert.ToByte(data & 0xff);
            return sr;
        }
        public static byte[] comBoxSelectValueToByte(ComboBox theComboBox)
        {
            byte[] bty = new byte[4] { 0x00, 0x00, 0x00, 0x00 };

            return bty;
        }
        /// <summary>
        /// 断帧
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        internal static void SetResultArray(byte[] result)
        {
           
            try
            {
                if (result.Length > 0)
                {
                    if (result[0] != 0x03) return;

                    int contentLength = 0, nowIndex = 1;
                    while (result.Length > contentLength)
                    {
                        contentLength += BytesSerialize.returnByteLengthRight(result.Skip(nowIndex).Take(4).ToArray());
                        queueArray.Add(result.Skip(nowIndex - 1).Take(contentLength).ToArray());
                        nowIndex += contentLength+1;
                    }
                }
            }
            catch (Exception msg)
            {
                Log.LogWrite(msg);
            }
        }
    }
}
