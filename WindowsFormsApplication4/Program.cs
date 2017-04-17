using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using log4net;
using System.Configuration;
using System.Reflection;
namespace 集中器控制客户端
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            log4net.Config.XmlConfigurator.Configure();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string assemblyName= ConfigurationManager.AppSettings["AssemblyName"].ToString();
            string classmName = ConfigurationManager.AppSettings["ClassmName"].ToString();
            Assembly AssemblySystemName = Assembly.Load(assemblyName);
            Type type = AssemblySystemName.GetType(assemblyName + "." + classmName);
            Application.Run(Activator.CreateInstance(type) as Form);
        }
    }
}
