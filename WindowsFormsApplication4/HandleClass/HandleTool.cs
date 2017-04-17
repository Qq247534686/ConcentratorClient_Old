using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using log4net;
using 集中器控制客户端.HandleClass.reportModels;

namespace 集中器控制客户端.HandleClass
{
    public class HandleTool : BytesSerialize, IScoketHandel
    {
        #region 定义所需字段
        bool FLAG = false;//socket是否连接成功
        public string USER_NAME;//用户名
        public string PASSWORD;//密码
        private Thread BEGIN_RUN_SOCKET;//接收服务器数据的线程
        private Thread REVICE_DATA;//接收服务器数据的线程
        private Thread ROLL_REPORT_THREAD;//Handel Reporte

        public Socket SOCKET_SEND { get; set; }//socket对象
        private DataEventArgs logDataEventArg = new DataEventArgs();

        private List<byte[]> reports = new List<byte[]>();

        public event EventHandler sendReceiveLogHandler;//接收
        public event EventHandler loginHandler;
        public event EventHandler summonDirectoryHandler;
        public event EventHandler updateDirectoryHandler;
        public event EventHandler theRemoteDataArrayHandler;
        public event EventHandler theTelemeteringHandler;
        public event EventHandler theResponseValueHandler;
        public event EventHandler theHitchHandler;
        public event EventHandler timeParameterHandel;
        public event EventHandler getTreeDataHandler;//绑定树
        public event EventHandler convertingStationHandel;//实现变电站数据事件回调
        public event EventHandler generatrixHandel;//实现变母线数据事件回调
        public event EventHandler circuitHandel;//实现变线路数据事件回调
        public event EventHandler detectionHandel;//实现变检测点数据事件回调
        public event EventHandler deviceSettingHandel;//实现设备数据事件回调
        public event EventHandler ShowLoding;
        public event EventHandler HideLoding;
        #endregion
        #region 单例模式
        private static readonly HandleTool theScoket = new HandleTool();
        private HandleTool() { }
        public static HandleTool createHandleTool()
        {
            return theScoket;
        }
        #endregion

        #region Socket接收服务器数据,在返回处理后的数据运行指定的事件
        /// <summary>
        /// Socket接收
        /// </summary>
        public void ReceiveInfo()
        {
            while (true)
            {
                try
                {
                    byte[] result = new byte[1024 * 1024 * 3];
                    int receiveLength = SOCKET_SEND.Receive(result);
                    if (receiveLength == 0)
                    {
                        stopSocket();
                        break;
                    }
                    result = result.Skip(0).Take(receiveLength).ToArray();
                    //string dd = ByteWithString.byteToHexStr(result);
                    //Log.LogWrite("REC:"+dd);
                    //登录标识...

                    if (ByteWithString.byteToHexStrAppend(result, "").Equals("1049FFFF4716".ToLower()))
                    {
                        Thread.Sleep(500);
                        LoginReport theLoginReport = new LoginReport();
                        theLoginReport.USERNAME = Encoding.ASCII.GetBytes(USER_NAME);
                        theLoginReport.USERNAME_LENGTH = (byte)theLoginReport.USERNAME.Length;
                        theLoginReport.PASSWORD = Encoding.ASCII.GetBytes(PASSWORD);
                        theLoginReport.PASSWORD_LENGTH = (byte)theLoginReport.PASSWORD.Length;
                        theSocketSend(0x01, theLoginReport);
                    }
                    else
                    {
                        //处理报文...
                        reportSel(result);
                    }
                }
                catch (Exception msg)
                {
                    Log.LogWrite(msg);
                    break;

                }
            }
        }
        #endregion

        #region 创建Socket对象
        /// <summary>
        /// 创建Socket
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool createSocket(UserInfo theUserInfo)
        {
            //开线程防止连接假死...
             ToThreadCreateSocket(theUserInfo);
            
            //BEGIN_RUN_SOCKET = new Thread(ToThreadCreateSocket);
            //BEGIN_RUN_SOCKET.IsBackground = true;
            //BEGIN_RUN_SOCKET.Start(theUserInfo);
            return FLAG;
        }
        public  void ToThreadCreateSocket(object obj)
        {
            UserInfo theUserInfo = obj as UserInfo;
            try
            {
                FLAG = false;
                USER_NAME = theUserInfo.USER_NAME;
                PASSWORD = theUserInfo.PASSWORD;
                SOCKET_SEND = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IAsyncResult result = SOCKET_SEND.BeginConnect(IPAddress.Parse(theUserInfo.IP_ADDRESS), int.Parse(theUserInfo.PORT_NUMBER), null, null);
                bool success = result.AsyncWaitHandle.WaitOne(5000, true);
                if (!success || !SOCKET_SEND.Connected)
                {
                    SOCKET_SEND.Close();
                    //MessageBox.Show("connect failed!");
                }
                else
                {
                    REVICE_DATA = new Thread(ReceiveInfo);
                    REVICE_DATA.IsBackground = true;
                    REVICE_DATA.Start();
                    //线程是否已在运行...
                    if (ROLL_REPORT_THREAD == null || !ROLL_REPORT_THREAD.IsAlive)
                    {
                        ROLL_REPORT_THREAD = new Thread(AnalyzeData);
                        ROLL_REPORT_THREAD.IsBackground = true;
                        ROLL_REPORT_THREAD.Start();
                    }
                    FLAG = true;
                }


            }
            catch (Exception msg)
            {
                CloseThread();
                Log.LogWrite(msg);
            }

        }


        private void AnalyzeData()
        {
            while (true)
            {
                try
                {
                    if (reports.Count > 0)
                    {
                        ClientReport theClientReport = clientReportSerialize(reports[0]);
                        reports.RemoveAt(0);
                        switch (theClientReport.CONTROL_FIELD)
                        {
                            case 0x02://登录
                                logDataEventArg.data = (theClientReport.REPORT_BODY as returnLoginInfo).isOk.ToString();
                                loginHandler(this, logDataEventArg);
                                break;
                            case 0x03://获得设备
                                logDataEventArg.theTreeArrayData = theClientReport.REPORT_BODY as DeviceData;
                                getTreeDataHandler(this, logDataEventArg);
                                break;
                            case 0x05://获得目录
                                logDataEventArg.obj = theClientReport.REPORT_BODY as TheCatalog;
                                summonDirectoryHandler(this, logDataEventArg);
                                break;
                            case 0x07://下载文件
                                logDataEventArg.obj = theClientReport.REPORT_BODY as CatalogFiles;
                                updateDirectoryHandler(this, logDataEventArg);
                                break;
                            case 0x10://不带时标的遥信标数据
                                logDataEventArg.obj = theClientReport.REPORT_BODY as TheRemoteDataArray;
                                theRemoteDataArrayHandler(this, logDataEventArg);
                                break;
                            case 0x11://带时标的遥信标数据
                                logDataEventArg.obj = theClientReport.REPORT_BODY as TheRemoteDataArray;
                                theRemoteDataArrayHandler(this, logDataEventArg);
                                break;
                            case 0x12://遥测表数据
                                logDataEventArg.obj = theClientReport.REPORT_BODY as TheRemoteDataArray;
                                theTelemeteringHandler(this, logDataEventArg);
                                break;
                            case 0x14://参数设定
                                logDataEventArg.obj = theClientReport.REPORT_BODY as ResponseValueData;
                                theResponseValueHandler(this, logDataEventArg);
                                break;
                            case 0x0a://服务器报文
                                logDataEventArg.obj = theClientReport.REPORT_BODY as ReportContentText;
                                sendReceiveLogHandler(this, logDataEventArg);
                                break;
                            case 0x23://接收变电站
                                logDataEventArg.obj = theClientReport.REPORT_BODY as List<ConvertingStation>;
                                convertingStationHandel(this, logDataEventArg);
                                break;
                            case 0x24://接收母线
                                logDataEventArg.obj = theClientReport.REPORT_BODY as List<Generatrix>;
                                generatrixHandel(this, logDataEventArg);
                                break;
                            case 0x25://接收线路
                                logDataEventArg.obj = theClientReport.REPORT_BODY as List<Circuit>;
                                circuitHandel(this, logDataEventArg);
                                break;
                            case 0x26://接收检测点
                                logDataEventArg.obj = theClientReport.REPORT_BODY as List<Detection>;
                                detectionHandel(this, logDataEventArg);
                                break;
                            case 0x33://接收设备
                                logDataEventArg.obj = theClientReport.REPORT_BODY as List<DeviceDataSetting>;
                                deviceSettingHandel(this, logDataEventArg);
                                break;
                            case 0x35://返回定时执行参数
                                logDataEventArg.obj = theClientReport.REPORT_BODY as List<int>;
                                timeParameterHandel(this, logDataEventArg);
                                break;
                            case 0x38://故障检测点id
                                logDataEventArg.obj = theClientReport.REPORT_BODY as Hitch;
                                theHitchHandler(this, logDataEventArg);
                                break;
                        }
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
                catch
                {
                    continue;
                }
            }
        }
        #endregion

        #region 断开Socket与服务器的交互
        /// <summary>
        /// Socket停止
        /// </summary>
        /// <returns></returns>
        public bool stopSocket()
        {
            try
            {
                if (SOCKET_SEND.Connected)
                {
                    SOCKET_SEND.Shutdown(SocketShutdown.Both);
                    SOCKET_SEND.Close();
                    CloseThread();
                    return true;
                }
            }
            catch (Exception msg)
            {
                SOCKET_SEND.Close();
                CloseThread();
                Log.LogWrite(msg);
            }
            return false;
        }
        #endregion

        #region 向服务器发送数据
        /// <summary>
        /// Socket发送
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public bool sendInfo(byte[] info)
        {
            try
            {
                SOCKET_SEND.Send(info);
                //Log.LogWrite("Send:"+ByteWithString.byteToHexStr(info));
                return true;
            }
            catch (Exception msg)
            {
                Log.LogWrite(msg);

            }
            return false;
        }
        #endregion

        #region 封装自定义格式的指定格式的发送给服务器
        public bool theSocketSend<T>(byte bty, T t)
        {
            return sendInfo(theScoket.clientReportUnSerialize(HandelControls.CreateClientReport<T>(bty, t)));
        }
        #endregion

        private void reportSel(byte[] data)
        {
            while (true)
            {
                if (data.Length == 0) { break; }
                if (data[0] != 0x03) { break; }
                int reportLength = BytesSerialize.returnByteLengthRight(data.Skip(1).Take(4).ToArray());
                if (reportLength > data.Length) break;
                byte[] report = data.Skip(0).Take(reportLength).ToArray();
                reports.Add(report);
                data = data.Skip(reportLength).Take(data.Length - reportLength).ToArray();
            }

        }
        /// <summary>
        /// 关闭线程
        /// </summary>
        public void CloseThread()
        {
            if (REVICE_DATA != null && REVICE_DATA.IsAlive)
            {
                REVICE_DATA.Abort();
            }
            if (ROLL_REPORT_THREAD != null && ROLL_REPORT_THREAD.IsAlive)
            {
                ROLL_REPORT_THREAD.Abort();
            }
        }

    }
}
