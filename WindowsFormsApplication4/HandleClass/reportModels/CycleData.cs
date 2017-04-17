using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 集中器控制客户端.HandleClass.reportModels
{
    [Serializable]
    public class CycleData
    {
        public double overZeroPoint { get; set; }//过零点（相位）
        public double rootValue { get; set; }//方差（有效值）
        public int maxValue { get; set; }//最大值
        public int minValue { get; set; }//最小值
        public double absValue { get; set; }//频率
    }
    [Serializable]
    public class cycleDataArray {
        public List<List<CycleData>> cycleDataListI { get; set; }
        public List<List<CycleData>> cycleDataListU { get; set; }
        public List<List<CycleData>> cycleDataListZero { get; set; }
    }
}
