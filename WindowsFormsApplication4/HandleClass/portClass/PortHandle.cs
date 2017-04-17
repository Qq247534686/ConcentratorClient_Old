using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace 集中器控制客户端.HandleClass.portClass
{
    public class PortHandle
    {
        private SerialPort THE_SERIAL_PORT = null;
        public PortHandle(SerialPort THE_SERIAL_PORT)
        {
            this.THE_SERIAL_PORT = THE_SERIAL_PORT;
        }
        //请求参数格式...
        public void GetParamaters(byte[] bytes)
        {
            THE_SERIAL_PORT.Write(bytes, 0, bytes.Length);
        }
        //参数绑定控件（解析方法）
        internal void HandleData(byte[] reviceData)
        {
            //switch (switch_on)
            //{
            //    default:
            //}
        }
        //获取控件值
        internal void GetContorlsValue(byte[] reviceData)
        { 
            

        }
        internal void SendReporet(byte[] bytes)
        {
            THE_SERIAL_PORT.Write(bytes, 0, bytes.Length);
        }




    }
}
