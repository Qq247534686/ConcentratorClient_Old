using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 集中器控制客户端.HandleClass.reportModels
{
    public class ClientReport
    {
        public byte HEAD_FIELD { get; set; }
        public byte[] REPORT_LENGTH { get; set; }
        public byte CONTROL_FIELD { get; set; }
        //01 登录
        public object REPORT_BODY { get; set; }
        public byte VALIDATE_FIELD { get; set; }
        public byte END_FIELD { get; set; }
        
    }
}
