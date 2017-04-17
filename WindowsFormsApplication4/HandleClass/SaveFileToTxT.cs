using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace 集中器控制客户端.HandleClass
{
    public static class SaveFileToTxT
    {

        public static void printByteFile(byte[] data, string fileName, bool ascii)
        {

            string needToPrint = string.Empty;
            for (int i = 0; i < data.Length; i++)
            {
                needToPrint += data[i].ToString("X2") + " ";
            }
            StreamWriter sw = new StreamWriter(fileName, false);
            if (ascii)
            {
                sw.Write(System.Text.Encoding.ASCII.GetString(data));
            }
            else
            {
                sw.Write(needToPrint);
            }
            sw.Close();
            sw.Dispose();
        }
        public static void printStringFile(string formatStr,string data, string fileName, string code)
        {
            if (formatStr == "Date")
            {
                formatStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t";
            }
            StreamWriter sw = new StreamWriter(fileName, true,Encoding.GetEncoding(code));
            sw.WriteLine(formatStr+data);
            sw.Close();
            sw.Dispose();
        }
        public static string  ReadStringFile(string fileName)
        {
            string str = "";
            StreamReader sw = new StreamReader(fileName);
            str = sw.ReadToEnd();
            sw.Close();
            sw.Dispose();
            return str;
        }
    }
}
