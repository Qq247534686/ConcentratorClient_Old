using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 集中器控制客户端.HandleClass
{
    /// <summary>
    /// 遥测类
    /// </summary>
    public class KeyValueTelemetering
    {
        /// <summary>
        /// 初始遥测表格数据
        /// </summary>
        public Dictionary<string, string> dic = new Dictionary<string, string>();
        public KeyValueTelemetering()
        {
            dic.Add("4001", "汇集单元电池电压");
            dic.Add("4011", "第 1 路负荷电流 Ia");
            dic.Add("4012", "第 1 路负荷电流 Ib");
            dic.Add("4013", "第 1 路负荷电流 Ic");
            dic.Add("4014", "预留");
            dic.Add("4015", "第 1 路温度 a");
            dic.Add("4016", "第 1 路温度 b");
            dic.Add("4017", "第 1 路温度 c");
            dic.Add("4018", "预留");
            dic.Add("4019", "第 1 路采集单元 a 电池电压");
            dic.Add("401a", "第 1 路采集单元 b 电池电压");
            dic.Add("401b", "第 1 路采集单元 c 电池电压");
            dic.Add("401c", "预留");
            dic.Add("401d", "预留");
            dic.Add("401e", "预留");
            dic.Add("401f", "预留");
            dic.Add("4021", "第 2 路负荷电流 Ia");
            dic.Add("4022", "第 2 路负荷电流 Ib");
            dic.Add("4023", "第 2 路负荷电流 Ic");
            dic.Add("4024", "预留");
            dic.Add("4025", "第 2 路温度 a");
            dic.Add("4026", "第 2 路温度 b");
            dic.Add("4027", "第 2 路温度 c");
            dic.Add("4028", "预留");
            dic.Add("4029", "第 2 路采集单元 a 电池电压");
            dic.Add("402a", "第 2 路采集单元 b 电池电压");
            dic.Add("402b", "第 2 路采集单元 c 电池电压");
            dic.Add("402c", "预留");
            dic.Add("402d", "预留");
            dic.Add("402e", "预留");
            dic.Add("402f", "预留");
            dic.Add("4031", "第 3 路负荷电流 Ia");
            dic.Add("4032", "第 3 路负荷电流 Ib");
            dic.Add("4033", "第 3 路负荷电流 Ic");
            dic.Add("4034", "预留");
            dic.Add("4035", "第 3 路温度 a");
            dic.Add("4036", "第 3 路温度 b");
            dic.Add("4037", "第 3 路温度 c");
            dic.Add("4038", "预留");
            dic.Add("4039", "第 3 路采集单元 a 电池电压");
            dic.Add("403a", "第 3 路采集单元 b 电池电压");
            dic.Add("403b", "第 3 路采集单元 c 电池电压");
            dic.Add("403c", "预留");
            dic.Add("403d", "预留");
            dic.Add("403e", "预留");
            dic.Add("403f", "预留");
        }
        public string SearchKey(string keyName)
        {
            string str=string.Empty;
            dic.TryGetValue(keyName,out str);
            return str;
        }
    }
}
