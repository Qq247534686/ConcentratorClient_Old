using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 集中器控制客户端.HandleClass.reportModels
{
    public class DeviceData
    {
      public  List<DeviceDataInfo> theDeviceData{get;set;}
    }
    public class DeviceDataInfo
    {
        public byte[] DeviceName { get; set; }
        public byte[] DeviceAddress { get; set; }
        public byte DeviceStatue { get; set; }
        public byte[] DeviceLastOnline { get; set; }
    }
    public class DeviceDataInfoToStr
    {
        public string DeviceName { get; set; }
        public byte[] DeviceAddress { get; set; }
    }
}
