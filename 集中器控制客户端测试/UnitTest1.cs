using System;
using 集中器控制客户端;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using 集中器控制客户端.HandleClass;
using System.Diagnostics;
/*---------------------暂不使用测试-----------------------------*/
namespace 集中器控制客户端测试
{
    [TestClass]
    public class UnitTest1
    {
        HandleTool theScoket =HandleTool.createHandleTool();
        [TestMethod]
        public void TestMethod1()
        {
            Debug.WriteLine("Hello");
            Assert.AreEqual((1 + 1), 2);//断言
          
            //Assert.Fail();
        }
        
    }
}
