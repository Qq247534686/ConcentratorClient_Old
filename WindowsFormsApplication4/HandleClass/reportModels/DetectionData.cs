using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace 集中器控制客户端.HandleClass.reportModels
{
    [Serializable]
    public class DetectionData
    {
        public double SettingNumber { get; set; }
        public string DeviceAddress { get; set; }
        public string DeviceName { get; set; }
        public string DeviceLine { get; set; }
        public List<DetectionDataArray> detectionDataArray { get; set; }
       
    }
     [Serializable]
    public class DetectionDataArray
    {
        public string DetectionName { get; set; }
        public string GroupName { get; set; }
        public string DetectionNumber { get; set; }
        public string DetectionRouteA { get; set; }
        public string DetectionRouteB { get; set; }
        public string DetectionRouteC { get; set; }
    }
}
