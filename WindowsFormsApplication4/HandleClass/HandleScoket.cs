using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Ninject;
namespace 集中器控制客户端.Class
{
    public class HandleScoket
    {
        private static Socket socketSend;
        /// <summary>
        /// 单列模式，线程的唯一性
        /// </summary>
        public static void GetScoket()
        {
            IDisposable iDisposable = (IDisposable)CallContext.GetData("IDisposable");
            if (iDisposable == null)
            {
                //依赖注入(DI)，控制反转(Ioc)
                IKernel ker=new StandardKernel();
                ker.Bind<IDisposable>().To<Socket>();
                iDisposable = ker.Get<IDisposable>();
                CallContext.SetData("IDisposable", iDisposable);
            }
            socketSend = (Socket)iDisposable;
        }


    }
}
