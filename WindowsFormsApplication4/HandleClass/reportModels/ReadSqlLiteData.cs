using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 集中器控制客户端.HandleClass.reportModels
{
    public class ReadSqlLiteData
    {
        /// <summary>
        /// 设备地址
        /// </summary>
        public string theAddress { get; set; }
        /// <summary>
        /// 数据地址
        /// </summary>
        public string theDataAddress { get; set; }
        /// <summary>
        /// 属于的路线
        /// </summary>
        public string theBelong { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string theNumber { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string theValue { get; set; }
    }
}
