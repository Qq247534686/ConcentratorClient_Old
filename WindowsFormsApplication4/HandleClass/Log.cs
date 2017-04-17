using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace 集中器控制客户端.HandleClass
{
    /// <summary>
    /// 日志类
    /// </summary>
    public class Log
    {
        private static ILog logger = LogManager.GetLogger("errorMsg");
        /// <summary>
        /// 记录错误或者异常
        /// </summary>
        /// <param name="msg"></param>
        public static void LogWrite(Exception msg)
        {
            logger.Error(msg.ToString() + "\r\n");
        }
        public static void LogWrite(string msg)
        {
            logger.Error(msg.ToString() + "\r\n");
        }
    }
}
