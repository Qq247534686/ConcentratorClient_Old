using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace WindowsFormsApplication4.SocketDemo
{
    public static class SocketClient
    {
        public static string MySocketClient(string iPStr, int portStr)
        {
            ///创建终结点EndPoint
            IPAddress ip = IPAddress.Parse(iPStr);
            //IPAddress ipp = new IPAddress("127.0.0.1");
            IPEndPoint ipe = new IPEndPoint(ip, portStr);//把ip和端口转化为IPEndpoint实例

            ///创建socket并连接到服务器
            Socket c = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建Socket
            topHere: Console.WriteLine("正在进行连接…");
            try
            {
                c.Connect(ipe);//连接到服务器
            }
            catch
            {
                Console.WriteLine("未找到服务器正在进行重连...");
                goto topHere;
            }
            Console.Clear();
           Console.WriteLine("连接成功…");
            ///向服务器发送信息
            Console.WriteLine("请输入发送的内容(输入ESC回车退出输入)：");
            string str=string.Empty;
            while (true)
            {
                string sc=Console.ReadLine();
                if (sc.Equals("ESC"))
                {
                    break;
                }
                str += sc;
            }
            byte[] bs = Encoding.GetEncoding("GB2312").GetBytes(str);//把字符串编码为字节
            c.Send(bs, bs.Length, 0);//发送信息
            Console.WriteLine("发送成功！！！");
            ///接受从服务器返回的信息
            string recvStr = string.Empty;
            byte[] recvBytes = new byte[1024*1024];
            int bytes;
            bytes = c.Receive(recvBytes, recvBytes.Length, 0);//从服务器端接受返回信息
            recvStr = Encoding.GetEncoding("GB2312").GetString(recvBytes, 0, bytes);
            Console.WriteLine("服务器返回状态码为："+recvStr);//显示服务器返回信息
            ///一定记着用完socket后要关闭
            c.Close();
            return recvStr;
        }
    }
}
