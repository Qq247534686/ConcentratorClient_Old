using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Xml;
using System.Xml.Serialization;
using System.Configuration;
using 集中器控制客户端.HandleClass.reportModels;
namespace 集中器控制客户端.HandleClass
{
    /// <summary>
    /// Xml序列化与反序列化
    /// </summary>
    public class XmlUtil
    {
        #region 反序列化
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>
        public static object Deserialize(Type type, string xml)
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xmldes = new XmlSerializer(type);
                    return xmldes.Deserialize(sr);
                }
            }
            catch (Exception e)
            {
                Log.LogWrite(e);
                return null;
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static object Deserialize(Type type, Stream stream)
        {
            XmlSerializer xmldes = new XmlSerializer(type);
            return xmldes.Deserialize(stream);
        }
        #endregion

        #region 序列化
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static bool Serializer(Type type, object obj, string name)
        {
            bool flag = false;
            string splStr = name;
            if (name.Contains("Data"))
            {
                splStr = name.Replace("Data", "");
            }
            string strName = ConfigurationManager.AppSettings["FilePath_SettingTest"].ToString() + @"/" + splStr;
            MemoryStream Stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(type);
            try
            {
                //序列化对象
                xml.Serialize(Stream, obj);
                if (!Directory.Exists(strName))
                {
                    Directory.CreateDirectory(strName);
                }
                Stream.Position = 0;
                StreamReader sr = new StreamReader(Stream);
                string str = sr.ReadToEnd();
                byte[] myByte = System.Text.Encoding.UTF8.GetBytes(str);
                string theFileName = strName + @"/" + name + ".xml";
                if (File.Exists(theFileName))
                {
                    File.Delete(theFileName);
                }
                using (FileStream fsWrite = new FileStream(theFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    fsWrite.Write(myByte, 0, myByte.Length);
                };
                sr.Close();
                Stream.Close();
                flag=true;
            }
            catch (Exception msg)
            {
                flag = false;
                Log.LogWrite(msg);
            }
            return flag;
        }
       
        #endregion
    }
}