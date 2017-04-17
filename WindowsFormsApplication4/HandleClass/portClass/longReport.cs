using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 集中器控制客户端.HandleClass.portClass
{
    public class longReport
    {
        public byte HEAD_FIELD { get; set; }
        public byte CONTROL_FIELD { get; set; }
        public byte[] ADDRESS_FIELD { get; set; }
        public byte VALIDATE_FIELD { get; set; }
        public object ASDU_FIELD { get; set; }
        public byte REPORT_LENGTH { get; set; }
        public byte END_FIELD { get; set; }
        public string REPORT_TYPE { get; set; }
        public byte[] REPORT_CONTENT { get; set; }
    }

    //ASDU信息
    public class ASDURequest
    {
        public byte TI { get; set; } //类型标识符
        public byte VSQ { get; set; } //可变帧长限定词
        public byte[] COT { get; set; }//传送原因 2位
        public byte[] ASDU_PD { get; set; }//ASDU公共地址 2位
        public Object MSG_OBJ { get; set; }//信息对象

    }
}
