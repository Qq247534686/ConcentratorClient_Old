using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 集中器控制客户端.HandleClass.reportModels
{
    public class TheRemoteDataArray {
       public byte[] DEVICE_ADDRESS { get; set; }
       public byte THE_COUNT { get; set; }
       public string THE_TYPE_NAME { get; set; }
       public List<TheRemoteData> THE_REMOTE_DATA { get; set; }
    }
    /// <summary>
    /// 遥信遥测对象
    /// </summary>
    public class TheRemoteData
    {
        public string RemoteName { get; set; }
        public string RemoteAddressStr { get; set; }
        public string RemoteValueStr { get; set; }
        public string LastestModifStr { get; set; }
        public byte[] RemoteValue { get; set; }
        public byte[] RemoteAddress { get; set; }
        public byte[] LastestModify { get; set; }
        public string RemoteTime { get; set; }
        public byte quility { get; set; }
    }
}
