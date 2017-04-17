using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 集中器控制客户端.HandleClass.reportModels;

namespace 集中器控制客户端.HandleClass.reportModels
{
    /// <summary>
    /// 变电站
    /// </summary>
    [Serializable]
    public class ConvertingStation 
    {
        public bool stationChecked = true;
        public string stationID { get; set; }
        public string stationName { get; set; }
        public string stationRemarks { get; set; }
    }
    /// <summary>
    /// 母线
    /// </summary>
    [Serializable]
    public class Generatrix 
    {
        
        public bool GeneratrixCheck = false;
        public string GeneratrixID { get; set; }
        public string GeneratrixName { get; set; }
        public string GeneratrixNumber { get; set; }
        public string GeneratrixToName { get; set; }
        public string GeneratrixToID { get; set; }
    }
    /// <summary>
    /// 线路
    /// </summary>
    [Serializable]
    public class Circuit 
    {
        public bool CircuitCheck = false;
        public string CircuitID { get; set; }
        public string CircuitName { get; set; }
        public string CircuitNumber { get; set; }
        public string CircuitToName { get; set; }
        public string CircuitToID{ get; set; }
    }
    /// <summary>
    /// 检测点
    /// </summary>
    [Serializable]
    public class Detection 
    {
        public bool DetectionCheck = false;
        public string DetectionID { get; set; }
        public string DetectionName { get; set; }
        public string DetectionNumber { get; set; }
        public string RemoteA { get; set; }
        public string RemoteB { get; set; }
        public string RemoteC { get; set; }
        public string DetectionToName { get; set; }
        /// <summary>
        /// 所属路线的ID
        /// </summary>
        public string DetectionToID { get; set; }
        public string DetectionToSecondName { get; set; }
        /// <summary>
        /// 所属路线的前一个ID
        /// </summary>
        public string DetectionToSecondID { get; set; }

    }
    /// <summary>
    /// 设备
    /// </summary>
    [Serializable]
    public class DeviceDataSetting 
    {
        public bool DeviceDataCheck = false;
        public string DeviceDataID { get; set; }
        public string DeviceDataName { get; set; }
        public string LastDateTime { get; set; }
        public string TheRemoteA { get; set; }
        public string RemoteAToID { get; set; }
        public string TheRemoteB { get; set; }
        public string RemoteBToID { get; set; }
        public string TheRemoteC { get; set; }
        public string RemoteCToID { get; set; }
        public string DeviceDataToID { get; set; }

    }
   
}
