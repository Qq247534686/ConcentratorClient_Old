using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 集中器控制客户端.HandleClass.portClass
{
    //文件信息对象
    public class file_message_obj
    {

        public byte[] MSG_ADDR { get; set; }//信息体地址 2位
        public byte MSG_PA_TYPE { get; set; }//附加数据包类型
        public object MSG_PA { get; set; }//附加数据包

    }

    //心跳信息对象
    public class heart_message_obj
    {

        public byte[] MSG_ADDR { get; set; }//信息体地址 2位
        public byte[] MSG_ELEMENT { get; set; }//信息对象元素
    }

    //遥信信息对象
    public class tele_message_obj {
        public byte[] MSG_ADDR { get; set; }//信息体地址 2位
        public byte[] QUALITY_POINT { get; set; }//带品质描述词的单/双点信息
        public byte[] TIME_MARK { get; set; }//时标
    }

    //总召信息对象
    public class total_call_obj{
        public byte[] MSG_ADDR { get; set; }//信息体地址 2位
        public byte MSG_ELEMENT { get; set; }//信息对象元素
    }

    //总召遥信遥测对象
    public class total_call_tele_message_obj
    {
        public byte[] MSG_ADDR { get; set; }//信息体地址 2位
        public byte[] MSG_ELEMENT { get; set; }//信息对象元素
    }

    //对时信息对象
    public class timing_obj {
        public byte[] MSG_ADDR { get; set; }//信息体地址 2位
        public byte[] MSG_ELEMENT { get; set; }//信息对象元素
    }

    //遥控预置信息对象
    public class ready_made_obj {
        public byte[] MSG_ADDR { get; set; }//信息体地址
        public byte[] MSG_ELEMENT { get; set; }//信息对象元素
    }

    //遥控信息对象
    public class remote_control_obj {
        public byte[] MSG_ADDR { get; set; }//信息体地址
        public byte MSG_ELEMENT { get; set; }//信息对象元素
    }

    //读取参数信息对象
    public class read_config_obj
    {
        public byte[] MSG_ADDR { get; set; }//信息体地址
        public byte[] MSG_ELEMENT { get; set; }//信息对象元素
    }

    //设置参数信息对象
    public class set_config_obj
    {
        public byte[] MSG_ADDR { get; set; }//信息体地址
        public byte[] VALUE_LENGTH { get; set; }//信息体地址
        public byte[] MSG_ELEMENT { get; set; }//信息对象元素
    }

    //故障事件信息
    public class fault_msg_obj {
        public byte TELE_MESSAGE_TYPE { get; set; }//遥信类型
        public byte[] FAULT_TELE_POINT { get; set; }//故障遥信点号
        public byte TELE_MESSAGE_VALUE { get; set; }//遥信值
        public byte[] FAULT_CP56_TIME { get; set; }//故障时刻时标
        public byte REMOTE_METER_NUM { get; set; }//遥测信息数量
        public List<fault_msg_item> FAULT_MSG_ITEMS { get; set; }//故障信息
    }

    //故障信息
    public class fault_msg_item {
        public byte[] REMOTE_METER_ADDR { get; set; }//故障信息体地址遥测
        public byte[] REMOTE_METER_TIME_VALUE { get; set; }//故障时刻数据
    }
}
