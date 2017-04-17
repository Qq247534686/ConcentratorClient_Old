using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 集中器控制客户端.HandleClass.reportModels
{
    public class PortChartData
    {
        public string PointName { get; set; }
        public int PointNowNumber { get; set; }
        public  int V_X { get; set; }
        public List<int> H_Y { get; set; }
    }
    public class PortChartLineData
    {
        public string PointName { get; set; }
        public int PointNowNumber { get; set; }
        public List<int> Xvalues { get; set; }
        public List<int> Yvalues { get; set; }
    }


}
