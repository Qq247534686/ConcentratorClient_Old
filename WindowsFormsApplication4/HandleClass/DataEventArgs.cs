using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using 集中器控制客户端.HandleClass.reportModels;

namespace 集中器控制客户端.HandleClass
{
    /// <summary>
    /// 事件传值时用于存数据的类
    /// </summary>
    public class DataEventArgs : EventArgs
    {
        public string data { get; set; }
        public DeviceData theTreeArrayData { get; set; }
        public Object obj { get; set; }
    }
}
