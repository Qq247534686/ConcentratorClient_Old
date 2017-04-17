using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 集中器控制客户端.HandleClass.reportModels
{
    public class LoginReport
    {
        public byte USERNAME_LENGTH { get; set; }
        public byte[] USERNAME { get; set; }
        public byte PASSWORD_LENGTH { get; set; }
        public byte[] PASSWORD { get; set; }
        
    }
    public class returnLoginInfo
    {
        public byte isOk { get; set; }
    }
}
