using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 集中器控制客户端.HandleClass.reportModels
{
    public class ReportContentText
    {
        public byte ReportType { get; set; }
        public byte[] ReportDateTime { get; set; }
        public byte[] ReportContent { get; set; }
    }
}
