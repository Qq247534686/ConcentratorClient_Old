using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 集中器控制客户端.HandleClass.reportModels
{
    public class WriteFile
    {
        public byte[] DEVICE_ADDRESS { get; set; }
        public byte FILE_LENGTH { get; set; }
        public byte[] FILE_NAME { get; set; }
        public byte[] FILE_COUNT { get; set; }
    }
}
