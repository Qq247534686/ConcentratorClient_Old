using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 集中器控制客户端.HandleClass.reportModels
{
    public class ResponseValueData
    {
        /// <summary>
        /// 设备地址
        /// </summary>
        public byte[] DEVICE_ADDRESS { get; set; }
        /// <summary>
        /// 参数类型
        /// </summary>
        public byte VALUE_TYPE { get; set; }
        /// <summary>
        /// 参数条目
        /// </summary>
        public byte VALUE_COUNT { get; set; }
        public List<ResponseValue> theResponseValueArray { get; set; }

    }
    public class ResponseValue {
        /// <summary>
        /// 参数地址
        /// </summary>
        public byte[] VALUE_ADDRESS { get; set; }
        /// <summary>
        /// 参数值
        /// </summary>
        public byte[] VALUE { get; set; }
        /// <summary>
        /// 参数长度
        /// </summary>
        public byte VALUE_LENGTH { get; set; }

    
    
    }
}
