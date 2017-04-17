using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace WindowsFormsApplication4.SocketDemo
{
    public static class SocketServer
    {
        public static string MySocketServer(string iPStr, int portStr)
        {
            ///创建终结点（EndPoint）
            IPAddress ip = IPAddress.Parse(iPStr);//把ip地址字符串转换为IPAddress类型的实例
            IPEndPoint ipe = new IPEndPoint(ip, portStr);//用指定的端口和ip初始化IPEndPoint类的新实例
            ///创建socket并开始监听
            Console.WriteLine("开始创建Socket对象...");
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建一个socket对像，如果用udp协议，则要用SocketType.Dgram类型的套接字
            Console.WriteLine("开始为Socket绑定EndPoint对象...");
            s.Bind(ipe);//绑定EndPoint对像（2000端口和ip地址）
            Console.WriteLine("开起监听...");
            s.Listen(10);//开始监听
            ///接受到client连接，为此连接建立新的socket，并接受信息
            Console.WriteLine("等待客户端响应...");
            Socket temp = s.Accept();//为新建连接创建新的socket
            string recvStr = string.Empty;
            byte[] recvBytes = new byte[1024*1024];
            int bytes;
            bytes = temp.Receive(recvBytes, recvBytes.Length, 0);//从客户端接受信息
            recvStr += Encoding.GetEncoding("GB2312").GetString(recvBytes, 0, bytes);
            Console.WriteLine("输出客户端传的数据："+recvStr);//把客户端传来的信息显示出来
            string sendStr = "200";
            byte[] bs = Encoding.ASCII.GetBytes(sendStr);
            temp.Send(bs, bs.Length, 0);//返回信息给客户端
            Console.WriteLine("接收成功，返回客户端："+sendStr);
            temp.Close();
            s.Close();//重要用完记得关闭
            Console.ReadLine();
            return recvStr;
        }
        /***********************发送文件***********************/
        private const int BufferSize = 1024;
        public static void Send(string ipAddress,int port, string path)
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipInfo=new IPEndPoint(IPAddress.Parse(ipAddress),port);
            sock.Connect(ipInfo);
            using (FileStream reader = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                long send = 0L,length = reader.Length;
                sock.Send(BitConverter.GetBytes(length));//发送文件总长度
                string fileName = Path.GetFileName(path);
                sock.Send(Encoding.Default.GetBytes(fileName));//发送文件名
                Console.WriteLine("Sending file:" + fileName + ".Plz wait...");
                byte[] buffer = new byte[BufferSize];
                int read, sent;
                //断点发送 在这里判断设置reader.Position即可
                while ((read = reader.Read(buffer, 0, BufferSize)) != 0)
                {
                    sent = 0;
                    while ((sent += sock.Send(buffer, sent, read, SocketFlags.None)) < read)
                    {
                        send += (long)sent;
                        //Console.WriteLine("Sent " + send + "/" + length + ".");//进度
                    }
                }
                Console.WriteLine("Send finish.");
            }
        }
 
        public static void Receive(IPEndPoint ip, string path)
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sock.Bind(ip);
            sock.Listen(1);
            Socket client = sock.Accept();
            byte[] buffer = new byte[BufferSize];
            client.Receive(buffer);
            long receive = 0L, length = BitConverter.ToInt64(buffer, 0);
            string fileName = Encoding.Default.GetString(buffer, 0, client.Receive(buffer));
            Console.WriteLine("Receiveing file:" + fileName + ".Plz wait...");
            using (FileStream writer = new FileStream(Path.Combine(path, fileName), FileMode.Create, FileAccess.Write, FileShare.None))
            {
                int received;
                //断点接受 在这里判断设置writer.Position即可
                while (receive < length)
                {
                    received = client.Receive(buffer);
                    writer.Write(buffer, 0, received);
                    writer.Flush();
                    receive += (long)received;
                    //Console.WriteLine("Received " + receive + "/" + length + ".");//进度
                }
            }
            Console.WriteLine("Receive finish.");
        }
    }
}
