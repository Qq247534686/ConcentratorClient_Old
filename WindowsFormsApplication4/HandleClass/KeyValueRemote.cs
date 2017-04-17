using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 集中器控制客户端.HandleClass
{
    /// <summary>
    /// 遥信类
    /// </summary>
   public class KeyValueRemote
    {
        /// <summary>
        /// 初始遥信表格数据
        /// </summary>
       public Dictionary<string, string> dic = new Dictionary<string, string>();
       public KeyValueRemote()
        {
            dic.Add("0001", "汇集单元设备状态");
            dic.Add("0002", "汇集单元电池低电压告警");
            dic.Add("0011", "第 1 路 A 相采集单元通信状态");
            dic.Add("0012", "第 1 路 A 相瞬时性短路");
            dic.Add("0013", "第 1 路 A 相永久性短路");
            dic.Add("0014", "第 1 路 A 相接地");
            dic.Add("0015", "第 1 路 A 相温度超限");
            dic.Add("0016", "第 1 路 A 相电流超限");
            dic.Add("0017", "第 1 路 A 相电池欠压告警");
            dic.Add("0021", "第 1 路 B 相采集单元通信状态");
            dic.Add("0022", "第 1 路 B 相瞬时性短路");
            dic.Add("0023", "第 1 路 B 相永久性短路");
            dic.Add("0024", "第 1 路 B 相接地");
            dic.Add("0025", "第 1 路 B 相温度超限");
            dic.Add("0026", "第 1 路 B 相电流超限");
            dic.Add("0027", "第 1 路 B 相电池欠压告警");
            dic.Add("0031", "第 1 路 C 相采集单元通信状态");
            dic.Add("0032", "第 1 路 C 相瞬时性短路");
            dic.Add("0033", "第 1 路 C 相永久性短路");
            dic.Add("0034", "第 1 路 C 相接地");
            dic.Add("0035", "第 1 路 C 相温度超限");
            dic.Add("0036", "第 1 路 C 相电流超限");
            dic.Add("0037", "第 1 路 C 相电池欠压告警");
            dic.Add("0041", "第 2 路 A 相采集单元通信状态");
            dic.Add("0042", "第 2 路 A 相瞬时性短路");
            dic.Add("0043", "第 2 路 A 相永久性短路");
            dic.Add("0044", "第 2 路 A 相接地");
            dic.Add("0045", "第 2 路 A 相温度超限");
            dic.Add("0046", "第 2 路 A 相电流超限");
            dic.Add("0047", "第 2 路 A 相电池欠压告警");
            dic.Add("0051", "第 2 路 B 相采集单元通信状态");
            dic.Add("0052", "第 2 路 B 相瞬时性短路");
            dic.Add("0053", "第 2 路 B 相永久性短路");
            dic.Add("0054", "第 2 路 B 相接地");
            dic.Add("0055", "第 2 路 B 相温度超限");
            dic.Add("0056", "第 2 路 B 相电流超限");
            dic.Add("0057", "第 2 路 B 相电池欠压告警");
            dic.Add("0061", "第 2 路 C 相采集单元通信状态");
            dic.Add("0062", "第 2 路 C 相瞬时性短路");
            dic.Add("0063", "第 2 路 C 相永久性短路");
            dic.Add("0064", "第 2 路 C 相接地");
            dic.Add("0065", "第 2 路 C 相温度超限");
            dic.Add("0066", "第 2 路 C 相电流超限");
            dic.Add("0067", "第 2 路 C 相电池欠压告警");
            dic.Add("0071", "第 3 路 A 相采集单元通信状态");
            dic.Add("0072", "第 3 路 A 相瞬时性短路");
            dic.Add("0073", "第 3 路 A 相永久性短路");
            dic.Add("0074", "第 3 路 A 相接地");
            dic.Add("0075", "第 3 路 A 相温度超限");
            dic.Add("0076", "第 3 路 A 相电流超限");
            dic.Add("0077", "第 3 路 A 相电池欠压告警");
            dic.Add("0081", "第 3 路 B 相采集单元通信状态");
            dic.Add("0082", "第 3 路 B 相瞬时性短路");
            dic.Add("0083", "第 3 路 B 相永久性短路");
            dic.Add("0084", "第 3 路 B 相接地");
            dic.Add("0085", "第 3 路 B 相温度超限");
            dic.Add("0086", "第 3 路 B 相电流超限");
            dic.Add("0087", "第 3 路 B 相电池欠压告警");
            dic.Add("0091", "第 3 路 C 相采集单元通信状态");
            dic.Add("0092", "第 3 路 C 相瞬时性短路");
            dic.Add("0093", "第 3 路 C 相永久性短路");
            dic.Add("0094", "第 3 路 C 相接地");
            dic.Add("0095", "第 3 路 C 相温度超限");
            dic.Add("0096", "第 3 路 C 相电流超限");
            dic.Add("0097", "第 3 路 C 相电池欠压告警");
        }
        public string SearchKey(string keyName)
        { 
            string str=string.Empty;
            dic.TryGetValue(keyName,out str);
            return str;
        }
    }
}
