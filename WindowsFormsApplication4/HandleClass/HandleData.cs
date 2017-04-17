using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;
using 集中器控制客户端.Model;
using log4net;
using System.Web.Script.Serialization;
namespace 集中器控制客户端.Class
{
    public class HandleData
    {
        /// <summary>
        /// json转对象
        /// </summary>
        /// <param name="json">json数据</param>
        /// <returns></returns>
        public static T JsonToObject<T>(string json)
        {
            JavaScriptSerializer toJson = new JavaScriptSerializer();
            return toJson.Deserialize<T>(json);
        }
        /// <summary>
        /// json转集合
        /// </summary>
        /// <param name="json">json数据</param>
        /// <returns></returns>
        public static List<T> JsonToTreeInfoData<T>(string json)
        {
            JavaScriptSerializer toJson = new JavaScriptSerializer();
            List<T> treeInfoData = new List<T>();
            try
            {
                treeInfoData = toJson.Deserialize<List<T>>(json);
            }
            catch (Exception msg)
            {
                ILog logger = LogManager.GetLogger("errorMsg");
                logger.Error(msg.ToString() + "\r\n");
            }
            return treeInfoData;
        }
        /// <summary>
        /// 对象转json
        /// </summary>
        /// <param name="json">对象</param>
        /// <returns></returns>
        public static string ObjectToJson<T>(T json)
        {
            JavaScriptSerializer toJson = new JavaScriptSerializer();
            return toJson.Serialize(json);
        }

    }
}
