using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 集中器控制客户端.HandleClass.reportModels
{
    /// <summary>
    /// 一组配置
    /// </summary>
    [Serializable]
    public class SettingGrop
    {
        #region 单例模式>持久化
        private static SettingGrop theSettingGrop = new SettingGrop();
        private SettingGrop() { }
        public static SettingGrop createSettingGrop()
        {
            return theSettingGrop;
        }
        #endregion
        /// <summary>
        /// 变电站
        /// </summary>
        public List<ConvertingStation> TheConvertingStation { get; set; }
        /// <summary>
        /// 母线
        /// </summary>
        public List<Generatrix> TheGeneratrix { get; set; }
        /// <summary>
        /// 线路
        /// </summary>
        public List<Circuit> TheCircuit { get; set; }
        /// <summary>
        /// 检测点
        /// </summary>
        public List<Detection> TheDetection { get; set; }
        /// <summary>
        /// 设备
        /// </summary>
        public List<DeviceDataSetting> TheDeviceDataSetting { get; set; }
        /// <summary>
        /// 定时参数
        /// </summary>
        public List<int> TheTimeSetting { get; set; }

        public Hitch TheHitch { get; set; }
        public void AddConvertingStationToArray()
        {

        }
        public void AddGeneratrixToArray(Generatrix theGeneratrix)
        {
            TheGeneratrix.Add(theGeneratrix);
        }
        public void AddCircuitToArray(Circuit theCircuit)
        {
            TheCircuit.Add(theCircuit);
        }
        public void AddDetectionToArray(Detection theDetection)
        {
            TheDetection.Add(theDetection);
        }
        public void AddDeviceDataSetting()
        {
            DeviceDataSetting addDeviceDataSetting = new DeviceDataSetting()
            {
                DeviceDataToID = "0",
                DeviceDataID = "default",
                DeviceDataName = "请选择"
            };
            TheDeviceDataSetting.Add(addDeviceDataSetting);
        }
        public void ClearArray()
        {
            try
            {
                if (ValidateData.ArrayIsNullOrZero(TheConvertingStation))
                {
                    TheConvertingStation.Clear();
                }
                if (ValidateData.ArrayIsNullOrZero(TheGeneratrix))
                {
                    TheGeneratrix.Clear();
                }
                if (ValidateData.ArrayIsNullOrZero(TheCircuit))
                {
                    TheCircuit.Clear();
                }
                if (ValidateData.ArrayIsNullOrZero(TheDetection))
                {
                    TheDetection.Clear();
                }
                if (ValidateData.ArrayIsNullOrZero(TheDeviceDataSetting))
                {
                    TheDeviceDataSetting.Clear();
                }
                if (ValidateData.ArrayIsNullOrZero(TheTimeSetting))
                {
                    TheTimeSetting.Clear();
                }
            }
            catch (Exception msg)
            {
                Log.LogWrite(msg);
            }
        }
    }
}
