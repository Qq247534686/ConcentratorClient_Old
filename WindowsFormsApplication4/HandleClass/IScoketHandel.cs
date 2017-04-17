using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 集中器控制客户端.HandleClass.reportModels;

namespace 集中器控制客户端.HandleClass
{
    /// <summary>
    /// Socket基本操作功能的接口
    /// </summary>
    public interface IScoketHandel
    {
        bool stopSocket();//停止
        void ReceiveInfo();//接收
        bool sendInfo(byte[] info);//发送
        bool createSocket(UserInfo theUserInfo);//创建
    }
}
