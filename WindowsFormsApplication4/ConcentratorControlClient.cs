using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using 集中器控制客户端.HandleClass.reportModels;
using 集中器控制客户端.HandleClass;
using System.Collections;
using System.Threading;
using System.Data;
namespace 集中器控制客户端
{
    public delegate void FallBackEvent(IpInputBox box, int flag);
    public partial class ConcentratorControlClient : Form
    {
        //192.168.1.76
        //8888

        public ConcentratorControlClient()
        {
            InitializeComponent();
        }

        #region 变量
        private HandleTool theScoket;//实例化Scoket对象;
        private HandelControls theHandelControls;
        private SettingGrop theSettingGrop;
        KeyValueLength theKeyValueLength = new KeyValueLength();
        private int nowRowIndex = -1;
        private int selectRowIndex = 0;
        OpaqueCommand cmd = new OpaqueCommand();//加载类
        List<string> ControlsTag = new List<string>();//窗体控件的Tag值
        List<string> ControlsValue = new List<string>();//窗体控件的Value(显示的)值
        private IpInputBox ipBox = new IpInputBox(false);
        private IpInputBox ipBox2 = new IpInputBox(false);
        private string DeviceAddress = "";//当前选中的设备实时地址
        private string isDeviceAddress = "";//
        CreateSetting theCreateSettingForm;
        #endregion
        //载入窗体时运行的方法和绑定的委托，事件
        private void WindowLoading()
        {

            theScoket = HandleTool.createHandleTool();
            theHandelControls = HandelControls.createHandelControls();//处理控件的赋值
            theCreateSettingForm = new CreateSetting();
            theSettingGrop = SettingGrop.createSettingGrop();
            //默认窗体下拉框选中第一个
            comboBox1.SelectedIndex = comboBox2.SelectedIndex = comboBox3.SelectedIndex = comboBox4.SelectedIndex = comboBox42.SelectedIndex = comboBox43.SelectedIndex = comboBox44.SelectedIndex = comboBox45.SelectedIndex = comboBox46.SelectedIndex = comboBox5.SelectedIndex = comboBox6.SelectedIndex = com_CatalogCollection.SelectedIndex = comboBox7.SelectedIndex = 0;
            //添加特定的IP框
            SettingFour.Controls.Add(ipBox);
            ipBox.Tag = "8073";//特定IP框格式
            ipBox.Location = new Point(139, 52);//107, 71
            SettingFour.Controls.Add(ipBox2);
            ipBox2.Tag = "8075";
            ipBox2.Location = new Point(642, 52);//642, 52
            timerUpdateTime.Start(); //显示页面时间
            theHandelControls.CreateFileViewColumns(fileDataGridView);//创建接收文件的DataGridView列
            theHandelControls.CreateDeviceViewColumns(deviceDataGridView); //创建设备信息的DataGridView列
            theHandelControls.CreateLoadML(); //创建初始化存文件数据的目录
            theHandelControls.AddTelemeteringViewRowsData(TelemeteringView);//初始化显示遥测表格数据
            theHandelControls.AddRemoteViewViewRowsData(RemoteView);//初始化显示遥信表格数据
            theScoket.sendReceiveLogHandler += theScoket_sendReceiveLogHandler;//显示实时报文
            theScoket.loginHandler += theScoket_loginHandler;//登录处理
            theScoket.getTreeDataHandler += theScoket_getTreeDataHandler;//绑定设备数据到左侧DataGridView
            theScoket.theRemoteDataArrayHandler += theScoket_theRemoteDataArrayHandler;//显示遥信表格数据
            theScoket.theTelemeteringHandler += theScoket_theTelemeteringHandler;//显示遥测表格数据
            theScoket.convertingStationHandel += theScoket_convertingStationHandel;//接收变电站
            theScoket.generatrixHandel += theScoket_generatrixHandel;//接收母线
            theScoket.circuitHandel += theScoket_circuitHandel;//接收线路
            theScoket.timeParameterHandel += theScoket_timeParameterHandel;//设定定时数据
            theScoket.detectionHandel += theScoket_detectionHandel;//接收检测点
            theScoket.theHitchHandler += theScoket_theHitchHandler;//故障检测点
            theScoket.deviceSettingHandel += theScoket_deviceSettingHandel;//维护界面
            theScoket.ShowLoding += theScoket_ShowLoding;//显示Loding
            theScoket.HideLoding += theScoket_HideLoding;//隐藏Loding
            theScoket.theResponseValueHandler += theScoket_theResponseValueHandler;//在控件上显示值
            theScoket.summonDirectoryHandler += theScoket_summonDirectoryHandler;//加载文件信息到文件表格中
            theScoket.updateDirectoryHandler += theScoket_updateDirectoryHandler;//更新文件表格的文件状态"未下载","已下载","读取中"
            //for (int i = 6001; i < 6021; i++) { comboBox6.Items.Add(i.ToString()); }//遥控地址6001~6020
        }

        void theScoket_theHitchHandler(object sender, EventArgs e)
        {
            DataEventArgs logDataEventArg = (DataEventArgs)e;
            Hitch theHitch = logDataEventArg.obj as Hitch;
            theSettingGrop.TheHitch = theHitch;
        }

        #region 维护界面代码
        void theScoket_deviceSettingHandel(object sender, EventArgs e)
        {
            DataEventArgs logDataEventArg = (DataEventArgs)e;
            List<DeviceDataSetting> theDeviceDataSettingArray = logDataEventArg.obj as List<DeviceDataSetting>;
            Action<List<DeviceDataSetting>> theDeviceDataSettingMehod = bindDeviceDataSettingArray<DeviceDataSetting>;
            this.Invoke(theDeviceDataSettingMehod, new object[] { theDeviceDataSettingArray });
        }

        void theScoket_detectionHandel(object sender, EventArgs e)
        {
            DataEventArgs logDataEventArg = (DataEventArgs)e;
            List<Detection> theDetectionArray = logDataEventArg.obj as List<Detection>;
            Action<List<Detection>> theDetectionMehod = bindDetectionArray<Detection>;
            this.Invoke(theDetectionMehod, new object[] { theDetectionArray });
        }
        private void bindDetectionArray<T>(List<Detection> theDetectionArray)
        {
            if (ValidateData.ArrayIsNullOrZero<Detection>(theDetectionArray))
            {
                theSettingGrop.TheDetection = theDetectionArray;
                Action<List<Detection>> theDetectionMehod = theCreateSettingForm.bindDetection;
                theDetectionMehod.Invoke(theSettingGrop.TheDetection);
            }
        }
        private void bindDeviceDataSettingArray<T>(List<DeviceDataSetting> theDeviceDataSettingArray)
        {
            if (ValidateData.ArrayIsNullOrZero<DeviceDataSetting>(theDeviceDataSettingArray))
            {
                theSettingGrop.TheDeviceDataSetting = theDeviceDataSettingArray;
                //LoadHostDeviceMethod(theDeviceDataSettingArray);
                Action<List<DeviceDataSetting>> theDeviceDataSettingMehod = theCreateSettingForm.bindDeviceDataSetting;
                theDeviceDataSettingMehod.Invoke(theSettingGrop.TheDeviceDataSetting);

            }
        }
        void theScoket_circuitHandel(object sender, EventArgs e)
        {
            DataEventArgs logDataEventArg = (DataEventArgs)e;
            List<Circuit> theCircuitArray = logDataEventArg.obj as List<Circuit>;
            Action<List<Circuit>> theCircuitMehod = bindCircuitArray<Circuit>;
            this.Invoke(theCircuitMehod, new object[] { theCircuitArray });
        }
        private void bindCircuitArray<T>(List<Circuit> theCircuitArray)
        {

            if (ValidateData.ArrayIsNullOrZero<Circuit>(theCircuitArray))
            {
                theSettingGrop.TheCircuit = theCircuitArray;
                Action<List<Circuit>> theCircuitMehod = theCreateSettingForm.bindCircuit;
                theCircuitMehod.Invoke(theSettingGrop.TheCircuit);
            }

        }
        void theScoket_generatrixHandel(object sender, EventArgs e)
        {
            DataEventArgs logDataEventArg = (DataEventArgs)e;
            List<Generatrix> theGeneratrixArray = logDataEventArg.obj as List<Generatrix>;
            Action<List<Generatrix>> theGeneratrixMehod = bindGeneratrixArray<Generatrix>;
            this.Invoke(theGeneratrixMehod, new object[] { theGeneratrixArray });
        }
        private void bindGeneratrixArray<T>(List<Generatrix> theGeneratrixArray)
        {
            if (ValidateData.ArrayIsNullOrZero<Generatrix>(theGeneratrixArray))
            {
                theSettingGrop.TheGeneratrix = theGeneratrixArray;

                Action<List<Generatrix>> theGeneratrixMehod = theCreateSettingForm.bindGeneratrix;
                theGeneratrixMehod.Invoke(theSettingGrop.TheGeneratrix);
            }

        }
        private void bindTimeSettingArray<T>(List<int> theTimeSetting)
        {
            if (ValidateData.ArrayIsNullOrZero<int>(theTimeSetting))
            {
                theSettingGrop.TheTimeSetting = theTimeSetting;
                Action<List<int>> theGeneratrixMehod = theCreateSettingForm.bindTimeSetting;
                theGeneratrixMehod.Invoke(theSettingGrop.TheTimeSetting);
            }

        }
        void theScoket_convertingStationHandel(object sender, EventArgs e)
        {
            DataEventArgs logDataEventArg = (DataEventArgs)e;
            List<ConvertingStation> theConvertingStationArray = logDataEventArg.obj as List<ConvertingStation>;
            Action<List<ConvertingStation>> theConvertingStationMehod = bindConvertingStationArray<ConvertingStation>;
            this.Invoke(theConvertingStationMehod, new object[] { theConvertingStationArray });
        }
        private void bindConvertingStationArray<T>(List<ConvertingStation> theConvertingStationArray)
        {
            if (ValidateData.ArrayIsNullOrZero<ConvertingStation>(theConvertingStationArray))
            {
                theSettingGrop.TheConvertingStation = theConvertingStationArray;
                Action<List<ConvertingStation>> theConvertingStationMehod = theCreateSettingForm.bindConvertingStation;
                theConvertingStationMehod.Invoke(theSettingGrop.TheConvertingStation);

            }

        }
        #endregion

        #region 窗体载入事件
        private void ConcentratorControlClient_Load(object sender, EventArgs e)
        {
            //cmd.ShowOpaqueLayer(fileDataGridView, 125, true);
            //cmd.HideOpaqueLayer();
            try
            {
                WindowLoading();
                //LoadHostDevice();

                ReadXmlSetting();
            }
            catch (Exception msg)
            {
                Log.LogWrite(msg);
            }

        }
        /// <summary>
        /// 设定值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void theScoket_theResponseValueHandler(object sender, EventArgs e)
        {
            DataEventArgs logDataEventArg = (DataEventArgs)e;
            ResponseValueData theResponseValueData = logDataEventArg.obj as ResponseValueData;
            Action<object> remoteAndTelemetering = SetControlValue;
            this.Invoke(remoteAndTelemetering, new object[] { theResponseValueData });
        }
        //根据地址来选中操作的Group
        private void SetControlValue(object obj)
        {
            ResponseValueData theResponseValueArray = obj as ResponseValueData;
            string contorlName = string.Empty;
            switch (theResponseValueArray.VALUE_TYPE)
            {
                case 0x01: contorlName = "SettingOne"; break;
                case 0x02: contorlName = "SettingTow"; break;
                case 0x03: contorlName = "SettingThree"; break;
                case 0x04: contorlName = "SettingFour"; break;
                case 0x05: contorlName = "SettingFive"; break;
                case 0x06: contorlName = "SettingSix"; break;
                case 0x07: contorlName = "SettingSeven"; break;
                case 0x08: contorlName = "SettingEight"; break;
                case 0x09: contorlName = "SettingNine"; break;
                default: break;
            }
            if (contorlName == "SettingOne")
            {
                SettingOneAttribute(theResponseValueArray.theResponseValueArray[0].VALUE);
                return;
            }
            for (int i = 0; i < theResponseValueArray.theResponseValueArray.Count; i++)
            {
                string address = ByteWithString.byteToHexStr(theResponseValueArray.theResponseValueArray[i].VALUE_ADDRESS).ToLower();
                SetControlValueMethod(theResponseValueArray.theResponseValueArray[i].VALUE, address, contorlName);
            }
        }

        private void SettingOneAttribute(byte[] bytes)
        {
            string firstStr = Convert.ToString(bytes[1], 2).PadLeft(8, '0');
            string secondStr = Convert.ToString(bytes[0], 2).PadLeft(8, '0');
            textBox233.Text = firstStr[7].ToString() == "1" ? "忙指示:busy" : "忙指示:normal";
            textBox232.Text = firstStr[6].ToString() == "1" ? "TCP状态:online" : "TCP状态:offline";
            textBox231.Text = firstStr[5].ToString() == "1" ? "FTP状态:online" : "FTP状态:offline";
            textBox228.Text = firstStr[4].ToString() == "1" ? "登录状态:loginning" : "登录状态:free";
            textBox230.Text = firstStr[3].ToString() == "1" ? "发送状态:sending" : "发送状态:free";
            textBox229.Text = firstStr[2].ToString() == "1" ? "接收状态:sending" : "接收状态:free";
            textBox227.Text = secondStr[7].ToString() == "1" ? "模块检测:正常" : "模块检测:异常";
            textBox226.Text = secondStr[6].ToString() == "1" ? "SIM卡检测:正常" : "SIM卡检测:异常";
            textBox225.Text = secondStr[5].ToString() == "1" ? "GSM服务:正常" : "GSM服务:异常";
            textBox131.Text = secondStr[4].ToString() == "1" ? "GPRS服务:正常" : "GPRS服务:异常";
            secondStr = secondStr[1].ToString() + secondStr[2].ToString() + secondStr[3].ToString();
            int signalVal = Convert.ToInt32(secondStr, 2);
            label179.Text = signalVal.ToString();
            progressBar1.Value = signalVal;
        }
        /// <summary>
        /// 控件赋值
        /// </summary>
        /// <param name="responseValue"></param>
        /// <param name="address"></param>
        /// <param name="contorlName"></param>
        private void SetControlValueMethod(byte[] responseValue, string address, string contorlName)
        {
            try
            {
                Control Controls = tabControl2.Controls.Find(contorlName, true)[0];
                foreach (Control item in Controls.Controls)
                {
                    if (item is TextBox)
                    {
                        TextBox textBox = item as TextBox;
                        if (textBox.Tag.ToString() == address)
                        {
                            if (address == "8073")
                            {
                                ipBox.IpAddress = theHandelControls.ToCodeValue(address, responseValue).Replace(" ", "");
                                ipBox.UpDateIpaddress();
                            }
                            else if (address == "8075")
                            {
                                ipBox2.IpAddress = theHandelControls.ToCodeValue(address, responseValue).Replace(" ", "");
                                ipBox2.UpDateIpaddress();
                            }
                            else
                            {
                                textBox.Text = theHandelControls.ToCodeValue(address, responseValue).Replace(" ", "");
                            }
                        }
                    }
                    else if (item is ComboBox)
                    {
                        ComboBox comBox = item as ComboBox;
                        if (comBox.Tag.ToString() == address && !ByteWithString.compareArray(responseValue, new byte[] { 0xff, 0xff }))
                        {
                            string selectValue = theHandelControls.ToCodeValue(address, responseValue);
                            byte[] bytes = ByteWithString.strToToHexByte(selectValue);
                            comBox.SelectedIndex = bytes[bytes.Length - 1] + 1;
                        }
                    }
                }
            }
            catch (Exception msg)
            {
                Log.LogWrite(msg);
            }
        }
        /// <summary>
        /// 遥测事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void theScoket_theTelemeteringHandler(object sender, EventArgs e)
        {
            DataEventArgs logDataEventArg = (DataEventArgs)e;
            TheRemoteDataArray theRemoteDataArray = logDataEventArg.obj as TheRemoteDataArray;
            Action<List<TheRemoteData>, byte[]> remoteAndTelemetering = RunTelemeteringMethod;
            this.Invoke(remoteAndTelemetering, new object[] { theRemoteDataArray.THE_REMOTE_DATA, theRemoteDataArray.DEVICE_ADDRESS });
        }

        /// <summary>
        /// 显示遥测数据
        /// </summary>
        /// <param name="obj"></param>
        private void RunTelemeteringMethod(List<TheRemoteData> obj, byte[] theDeviceAddress)
        {
            List<TheRemoteData> theRemoteData = obj;
            if (theDeviceAddress != null && ByteWithString.byteToHexStr(theDeviceAddress) == DeviceAddress)
            {
                for (int i = 0; i < TelemeteringView.Rows.Count; i++)
                {
                    for (int j = 0; j < theRemoteData.Count; j++)
                    {
                        string strRemoteName = TelemeteringView.Rows[i].Cells["TRemoteName"].Value.ToString();
                        string strTag = TelemeteringView.Rows[i].Cells["TRemoteName"].Tag.ToString();
                        string address = ByteWithString.byteToHexStr(theRemoteData[j].RemoteAddress).ToLower();

                        theRemoteData[j].RemoteName = theHandelControls.ToSixString(theRemoteData[j].RemoteAddress, 0);
                        if (strRemoteName == theRemoteData[j].RemoteName && strTag == address)
                        {
                            TelemeteringView.Rows[i].Cells["TRemoteValueStr"].Value = ((theRemoteData[j].RemoteValue[1] << 8) | theRemoteData[j].RemoteValue[0]).ToString();
                            TelemeteringView.Rows[i].Cells["TLastestModifStr"].Value = theRemoteData[j].quility.ToString("X2").ToLower();
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 显示遥信数据
        /// </summary>
        /// <param name="obj"></param>
        private void RunRemoteMethod(List<TheRemoteData> obj, byte[] theDeviceAddress)
        {

            List<TheRemoteData> theRemoteData = obj;
            if (theDeviceAddress != null && ByteWithString.byteToHexStr(theDeviceAddress) == DeviceAddress)
            {
                for (int i = 0; i < RemoteView.Rows.Count; i++)
                {
                    for (int j = 0; j < theRemoteData.Count; j++)
                    {
                        string strRemoteName = RemoteView.Rows[i].Cells["RemoteName"].Value.ToString();
                        string strTag = RemoteView.Rows[i].Cells["RemoteName"].Tag.ToString();
                        if (strRemoteName == theRemoteData[j].RemoteName && strTag == theRemoteData[j].RemoteAddressStr)
                        {
                            RemoteView.Rows[i].Cells["RemoteValueStr"].Value = theRemoteData[j].RemoteValueStr;
                            if (theRemoteData[j].LastestModifStr == null || theRemoteData[j].LastestModifStr.Length == 0)
                            {
                                RemoteView.Rows[i].Cells["LastestModifStr"].Value = "";
                            }
                            else
                            {
                                RemoteView.Rows[i].Cells["LastestModifStr"].Value = theRemoteData[j].LastestModifStr;
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 遥信事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void theScoket_theRemoteDataArrayHandler(object sender, EventArgs e)
        {
            DataEventArgs logDataEventArg = (DataEventArgs)e;
            TheRemoteDataArray theRemoteDataArray = logDataEventArg.obj as TheRemoteDataArray;
            theRemoteDataArray.THE_REMOTE_DATA = theHandelControls.UpdateTheRemoteDataArray(theRemoteDataArray.THE_REMOTE_DATA);
            Action<List<TheRemoteData>, byte[]> remoteAndTelemetering = RunRemoteMethod;
            this.Invoke(remoteAndTelemetering, new object[] { theRemoteDataArray.THE_REMOTE_DATA, theRemoteDataArray.DEVICE_ADDRESS });
        }

        void theScoket_HideLoding(object sender, EventArgs e)
        {
            Action theHideOpaqueLayer = cmd.HideOpaqueLayer;
            this.Invoke(theHideOpaqueLayer);
        }

        void theScoket_ShowLoding(object sender, EventArgs e)
        {
            Action<Control, int, bool> theShowOpaqueLayer = cmd.ShowOpaqueLayer;
            this.Invoke(theShowOpaqueLayer, new object[] { this, 125, true });
        }
        /// <summary>
        /// 设置文件读取状态
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="showTxt"></param>
        private void SettingDataGridText(int rowIndex, string showTxt, string updateTime)
        {

            string pullPathStr = fileDataGridView.Rows[rowIndex].Cells["fileName"].Value.ToString();
            if (!pullPathStr.TrimEnd().EndsWith(".dat"))
            {

                if (com_CatalogCollection.SelectedItem.ToString().Equals("历史负荷数据"))
                {
                    fileDataGridView.Rows[rowIndex].Cells["towFileBtn"].Value = "二进制文件下载";
                }
                else
                {
                    fileDataGridView.Rows[rowIndex].Cells["towFileBtn"].Value = "---";
                }
                fileDataGridView.Rows[rowIndex].Cells["sixFileBtn"].Value = "---";
                fileDataGridView.Rows[rowIndex].Cells["ImgBtn"].Value = "---";
            }
            if (string.IsNullOrEmpty(updateTime))
            {
                fileDataGridView.Rows[rowIndex].Cells["filestate"].Value = showTxt;
            }
            else
            {
                fileDataGridView.Rows[rowIndex].Cells["filestate"].Value = showTxt;
                fileDataGridView.Rows[rowIndex].Cells["updateFileData"].Value = updateTime;
            }

        }
        /// <summary>
        /// cfg文件回写的方法
        /// </summary>
        /// <param name="theDatPayh"></param>
        private void returnFileWithCFG(string theDatPayh)
        {
            theDatPayh = pullPath(2, false) + @"/" + theDatPayh;
            string cfgStr = HandleWaveData.WriteCFG(theDatPayh);
            if (!string.IsNullOrEmpty(cfgStr))
            {
                string newFileName = theDatPayh.Replace(".dat", ".cfg");
                SaveFileToTxT.printByteFile(Encoding.ASCII.GetBytes(cfgStr), pullPath(2, false) + @"/" + newFileName, true);
            }
        }
        private void ReturnWrite(cycleDataArray theCycleDataArray, byte[] bufferHex)
        {
            List<byte> listArrayByte = new List<byte>();
            List<int> listArray = new List<int>();
            int avg = 0;
            for (int i = 0; i < theCycleDataArray.cycleDataListI.Count; i++)
            {
                int pl = (int)((theCycleDataArray.cycleDataListI[i][0].absValue / 50 - 1) * 0.03 + 1);//频率
                avg += pl;
                for (int j = 0; j < bufferHex.Length; j++)
                {
                    listArrayByte.Add(Convert.ToByte((Convert.ToByte(bufferHex[j] & 0xff) * pl)));
                    listArrayByte.Add(Convert.ToByte((Convert.ToByte(bufferHex[j] >> 8 & 0xff) * pl)));
                }
            }
            avg = (int)(avg / 3);
            for (int i = 0; i < theCycleDataArray.cycleDataListZero.Count; i++)
            {
                for (int j = 0; j < bufferHex.Length; j++)
                {
                    listArrayByte.Add(Convert.ToByte((Convert.ToByte(bufferHex[j] & 0xff) * avg)));
                    listArrayByte.Add(Convert.ToByte((Convert.ToByte(bufferHex[j] >> 8 & 0xff) * avg)));
                }

            }
            string datStr = ByteWithString.byteToHexStrAppend(listArrayByte.ToArray(), " ");
            SaveFileToTxT.printStringFile("", datStr, "XXX.dat", "UTF-8");
        }

        /// <summary>
        /// 获得选中的目录
        /// </summary>
        /// <returns></returns>
        private string GetComBoxItemsValue()
        {
            return com_CatalogCollection.SelectedItem.ToString();
        }
        /// <summary>
        /// 更新文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void theScoket_updateDirectoryHandler(object sender, EventArgs e)
        {
            DataEventArgs logDataEventArg = (DataEventArgs)e;
            CatalogFiles theCatalogFiles = logDataEventArg.obj as CatalogFiles;
            Func<string> myCatalogCollectionItems = GetComBoxItemsValue;
            string catalogName = this.Invoke(myCatalogCollectionItems).ToString();//文件目录
            string address = ByteWithString.byteToHexStr(theCatalogFiles.DeviceAddress);//设备地址
            string fileName = Encoding.ASCII.GetString(theCatalogFiles.FILE_NAME);
            Func<int, bool, string> myPath = pullPath;
            string filePath = this.Invoke(myPath, new object[] { 1, true }).ToString() + "/" + fileName;
            string fileString = ByteWithString.byteToHexStr(theCatalogFiles.FILE_CONTENT);
            if (fileName.ToLower().EndsWith(".dat") || fileName.ToLower().EndsWith(".cfg"))
            {
                filePath = ConfigurationManager.AppSettings["FilePath_FileCatalog"].ToString() + @"/" + address + @"/录波文件";
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                filePath += @"/" + fileName;
            }
            StreamWriter sw = new StreamWriter(filePath, false);
            sw.Write(fileString);
            sw.Dispose();
            sw.Close();
            string updateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Action<int, string, string> myDataGridRowsText = SettingDataGridText;
            if (nowRowIndex>=0)
            {
                this.Invoke(myDataGridRowsText, new object[] { nowRowIndex, "已下载", updateTime });
            }
            FileInfo filelInfo = new FileInfo(filePath);
            if (filelInfo.Exists)
            {
                for (int i = 0; i < fileDataGridView.Rows.Count; i++)
                {
                    if (fileDataGridView.Rows[i].Cells["fileName"].Value.ToString().Equals(filelInfo.Name) && (fileName.ToLower().EndsWith(".dat") || fileName.ToLower().EndsWith(".cfg")))
                    {
                        fileDataGridView.Rows[i].Cells["updateFileData"].Value = updateTime;
                        fileDataGridView.Rows[i].Cells["filestate"].Value = "已下载";
                        if (fileName.ToLower().EndsWith(".dat"))
                        {
                            fileDataGridView.Rows[i].Cells["soureFileDown"].Value = "源文件下载";
                            fileDataGridView.Rows[i].Cells["sixFileBtn"].Value = "十六进制文件下载";
                        }
                       
                    }
                }
                
            }
        }

        /// <summary>
        /// 把数据绑定到表格
        /// </summary>
        /// <param name="theCatalog"></param>
        private void TheBindData(TheCatalog theCatalog)
        {
            fileDataGridView.Rows.Clear();
            theCatalog.CATALOG_NAME = theHandelControls.GetConBoxItem(com_CatalogCollection.SelectedItem.ToString());
            theHandelControls.GetDataGridViewInfo(theCatalog, fileDataGridView);
        }
        /// <summary>
        /// 获得选设备的地址
        /// </summary>
        /// <returns></returns>
        private byte[] GetTagToByteArray()
        {
            List<byte> bytes = new List<byte>();
            try
            {
                if (!string.IsNullOrEmpty(DeviceAddress))
                {
                    bytes = ByteWithString.strToToHexByte(DeviceAddress).ToList();
                }
                else
                {
                    bytes.Add(0x00);
                }
            }
            catch (Exception msg)
            {
                Log.LogWrite(msg);
            }
            return bytes.ToArray();
        }
        /// <summary>
        /// 显示文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void theScoket_summonDirectoryHandler(object sender, EventArgs e)
        {
            DataEventArgs logDataEventArg = (DataEventArgs)e;
            TheCatalog theCatalog = logDataEventArg.obj as TheCatalog;
            // 获得选设备的地址
            Func<byte[]> getTreeTag = GetTagToByteArray;
            byte[] bytes = this.Invoke(getTreeTag) as byte[];
            if (bytes != null && ByteWithString.compareArray(theCatalog.DEVICE_ADDRESS, bytes))
            {
                Action<TheCatalog> myBindDataToObject = TheBindData;
                this.Invoke(myBindDataToObject, new object[] { theCatalog });
            }
        }

        #endregion



        #region 登录处理
        /// <summary>
        /// 登录处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void theScoket_loginHandler(object sender, EventArgs e)
        {
            DataEventArgs dataEventArgs = (DataEventArgs)e;
            if (dataEventArgs.data == "1")
            {
                toolTip1.ToolTipTitle = "已登录";
                pic_User.Image = Image.FromFile(@"Img/user_24px_508421_easyicon.net.png");
                //0x23,0x24,0x25,0x26,0x00
                byte[] bytes = new byte[] { 0x00 };
                theScoket.theSocketSend<byte[]>(0x34, bytes.ToArray());

            }
            else
            {
                toolTip1.ToolTipTitle = "未登录";
                pic_User.Image = Image.FromFile(@"Img/offline_user_24px_508417_easyicon.net.png");
                theScoket.stopSocket();
                HandelControls.BtnRestore(this, StringInfo.btn_OK, StringInfo.Img_NO);
                HandelControls.Msg("登录失败", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        #endregion



        #region 召唤目录
        private void button2_Click(object sender, EventArgs e)
        {
            byte[] bys = GetTagToByteArray();
            if (bys != null && bys.Length > 1)
            {
                byte[] bytes_Address = bys;
                byte[] bytes_CatalogName = theHandelControls.GetConBoxItem(com_CatalogCollection.SelectedItem.ToString());
                theHandelControls.ToTheCatalog(0x04, bytes_Address, bytes_CatalogName);
            }
        }
        #endregion




        #region 单元格单击事件
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
                {
                    selectRowIndex = e.RowIndex;
                    string selectDir = com_CatalogCollection.SelectedItem.ToString();
                    string fileName = fileDataGridView.Rows[e.RowIndex].Cells["fileName"].Value.ToString();
                    string pullPathFile = pullPath(1, true) + @"/" + fileName;//完整的文件路径
                    FileInfo fs = new FileInfo(pullPathFile);
                    HandleWaveData theHandleWaveData = new HandleWaveData();
                    ChartForm theChartForm = new ChartForm();
                    ChartNewView theChartNewView = new ChartNewView();
                    Action<WaveData, string, byte[]> chartValue = theChartForm.theSetValueMethod;
                    Action<List<PortChartData>,List<string>, string, byte[]> chartValueNew = theChartNewView.theSetDataMethod;
                    if (fileDataGridView.Rows[e.RowIndex].Cells["soureFileDown"].Selected)//源文件下载
                    {
                        if (fileDataGridView.Rows[e.RowIndex].Cells["filestate"].Value.ToString() == "未下载")
                        {
                            DownOrUpdateFiles(fileName);
                            SettingDataGridText(e.RowIndex, "读取中", "");
                            nowRowIndex = e.RowIndex;
                        }
                        if (File.Exists(pullPathFile) && fileDataGridView.Rows[e.RowIndex].Cells["filestate"].Value.ToString() == "已下载")
                        {
                            if (!fileName.Contains(".dat"))//单独处理.dat文件
                            {
                                using (StreamReader sr = new StreamReader(pullPathFile))
                                {
                                    string showText = Encoding.ASCII.GetString(ByteWithString.strToToHexByte(sr.ReadToEnd()));
                                    sr.Close();
                                    NotePadUtil.OpenNotepad(showText.Replace("\0", "").Replace(" ", ""));
                                }
                            }
                            else
                            {
                                System.Diagnostics.Process.Start("notepad.exe", pullPathFile);
                            }
                        }
                    }
                    else if (fileDataGridView.Rows[e.RowIndex].Cells["ImgBtn"].Selected)//波形图
                    {
                        if (!File.Exists(Path.GetFullPath(pullPathFile.Replace(".dat", ".cfg"))))
                        {
                            MessageBox.Show("请先下载此文件的cfg文件");
                            return;
                        }
                        if (fileName.Contains(".dat") && selectDir == "录波文件")//单独处理.dat文件
                        {
                            //chartValue.Invoke(theHandelControls.ChartShow(pullPathFile), pullPathFile, GetTagToByteArray());
                            //chartValue.Invoke(theHandelControls.ChartShow(pullPathFile), pullPathFile, GetTagToByteArray());
                            List<string> listName = new List<string>();
                            List<PortChartData> listData = theHandelControls.ChartShowNew(pullPathFile, out listName);
                            chartValueNew.Invoke(listData, listName, pullPathFile, GetTagToByteArray());
                            theChartNewView.ShowDialog();

                        }
                    }
                    else if (fileDataGridView.Rows[e.RowIndex].Cells["towFileBtn"].Selected)//二进制文件下载
                    {

                        if (fileDataGridView.Rows[e.RowIndex].Cells["filestate"].Value.ToString() == "已下载")
                        {

                            if (fileName.Contains(".dat") && selectDir == "录波文件")//单独处理.dat文件
                            {
                                WaveData theWaveData = theHandleWaveData.binaryFileSerialize(ByteWithString.FileStreamTobyteArray(pullPathFile));
                                byte[] buffer = theHandleWaveData.waveDateUnSerialize(theWaveData);
                                SaveFileToTxT.printByteFile(buffer, pullPathFile + ".Binary", false);
                                NotePadUtil.OpenNotepad(ByteWithString.byteToHexStr(buffer));
                            }
                            else
                            {
                                if (com_CatalogCollection.SelectedItem.ToString().Equals("历史负荷数据"))
                                {
                                    NotePadUtil.OpenNotepadFile(pullPathFile);
                                }
                            }
                        }


                    }
                    else if (fileDataGridView.Rows[e.RowIndex].Cells["sixFileBtn"].Selected)//十六进制文件下载
                    {
                        if (fileDataGridView.Rows[e.RowIndex].Cells["filestate"].Value.ToString() == "已下载")
                        {
                            if (fileName.Contains(".dat") && selectDir == "录波文件")//单独处理.dat文件
                            {
                                string txt = "";
                                if (!File.Exists(pullPathFile + ".Hex"))
                                {
                                    WaveData theWaveData = theHandleWaveData.binaryFileSerialize(ByteWithString.FileStreamTobyteArray(pullPathFile));
                                    theWaveData = theHandleWaveData.hexadecimalFileSerialize(theWaveData);
                                    byte[] bufferHex = theHandleWaveData.waveDateUnSerialize(theWaveData);
                                    txt = ByteWithString.byteToHexStrAppend(bufferHex, " ");
                                }
                                else
                                {
                                    txt = SaveFileToTxT.ReadStringFile(pullPathFile + ".Hex");
                                }
                                NotePadUtil.OpenNotepad(txt);
                            }

                        }
                    }
                    else if (fileDataGridView.Rows[e.RowIndex].Cells["updateFile"].Selected)
                    {
                        byte[] bys = GetTagToByteArray();
                        if (bys != null && bys.Length >= 1)
                        {
                            SettingDataGridText(e.RowIndex, "读取中", "");
                            DownOrUpdateFiles(fileName);
                            nowRowIndex = e.RowIndex;
                        }
                    }
                }
            }
            catch (Exception msg)
            {
                Log.LogWrite(msg);
            }
        }
        private void DownOrUpdateFiles(string fileName)
        {
            byte[] bys = GetTagToByteArray();
            if (bys.Length > 1)
            {
                CatalogFiles catalogFiles = new CatalogFiles();
                catalogFiles.DeviceAddress = bys;//地址
                catalogFiles.FILE_NAME = Encoding.ASCII.GetBytes(fileName);//文件名
                catalogFiles.FILE_LENGTH = (byte)catalogFiles.FILE_NAME.Length;//文件长度
                theHandelControls.GetFileData(catalogFiles);
            }
        }

        #endregion

        #region 绑定设备数据到左侧DataGridView
        void theScoket_getTreeDataHandler(object sender, EventArgs e)
        {
            DataEventArgs dataEventArgs = (DataEventArgs)e;
            DeviceData theTreeArrayData = dataEventArgs.theTreeArrayData;
            Action<DeviceData> getListData = UpdateDeviceInfo;
            this.Invoke(getListData, new object[] { theTreeArrayData });
        }
        #endregion


        #region 更新树节点
        private void UpdateDeviceInfo(DeviceData listData)
        {
            deviceDataGridView.Rows.Clear();
            for (int i = 0; i < listData.theDeviceData.Count; i++)
            {
                deviceDataGridView.Rows.Add();
                string name = ByteWithString.byteToHexStr(listData.theDeviceData[i].DeviceAddress);
                deviceDataGridView.Rows[i].Cells["deviceAddress"].Value = name;
                deviceDataGridView.Rows[i].Cells["deviceStatus"].Value = listData.theDeviceData[i].DeviceStatue >= 1 ? "在线" : "离线";
            }
            theHandelControls.cellColor(deviceDataGridView, DeviceAddress);
        }

        #endregion

        void theScoket_sendReceiveLogHandler(object sender, EventArgs e)
        {
            DataEventArgs dataEventArgs = (DataEventArgs)e;
            ReportContentText theReportContent = dataEventArgs.obj as ReportContentText;
            StringBuilder msgInfo = new StringBuilder();
            string strMsg = theReportContent.ReportType > 0 ? "接收:" : "发送:";
            string theReportTxt = ByteWithString.byteToHexStrAppend(theReportContent.ReportContent, " ");
            string dateTimeStr = "时间:" + ByteWithString.ConvertCP56TIME2aToDateTime(theReportContent.ReportDateTime).ToString("yyyy-MM-dd HH:mm:ss");
            msgInfo.AppendFormat("{0}{1}\r\n{2}\r\n", strMsg, theReportTxt, dateTimeStr);
            Action<object> remoteAndTelemetering = Info_TxtShow;
            this.Invoke(remoteAndTelemetering, new object[] { msgInfo });
        }
        private void Info_TxtShow(object obj)
        {
            StringBuilder strMsg = obj as StringBuilder;
            Info_Txt.Text = strMsg.ToString() + "\r\n" + Info_Txt.Text;
        }
        /// <summary>
        /// 连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Connect_Click(object sender, EventArgs e)
        {
            theHandelControls.Loging(this, theSettingGrop);
        }

        public void ss()
        {
            btn_Connect.Text = "正在断开...";
        }


        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Empty_Click(object sender, EventArgs e)
        {
            Info_Txt.Text = string.Empty;
        }
        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 集中器控制客户端_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                SaveXmlSetting();
            }
            catch (Exception msg)
            {

                Log.LogWrite(msg);
            }
            finally
            {
                Application.ExitThread();
                Application.Exit();
            }
        }
        private void ReadXmlSetting()
        {
            XDocument xmlFile = XDocument.Load(@"SaveCommand.xml");
            List<XElement> nodes = xmlFile.Root.Elements().ToList();
            for (int i = 0; i < nodes.Count; i++)
            {
                XElement node = nodes[i];
                Control theControl = groupBox6.Controls.Find(node.Attribute("contorlName").Value, true)[0];
                switch (node.Attribute("type").Value)
                {
                    case "bool":
                        CheckBox theCheckBox = theControl as CheckBox;
                        theCheckBox.Checked = Boolean.Parse(node.Attribute("value").Value);
                        break;
                    case "int":
                        NumericUpDown theNumericUpDown = theControl as NumericUpDown;
                        theNumericUpDown.Value = int.Parse(node.Attribute("value").Value);
                        break;
                    default: break;
                }
            }
        }
        private void SaveXmlSetting()
        {
            XDocument xmlFile = XDocument.Load(@"SaveCommand.xml");
            List<XElement> nodes = xmlFile.Root.Elements().ToList();
            string isChecked = checkBox1.Checked ? "true" : "false";
            XElement xe = nodes.Find(u => u.Value == "程序开始时总召") as XElement;
            xe.SetAttributeValue("value", isChecked);
            xmlFile.Save(@"SaveCommand.xml");
        }





        #region 显示日期
        private void timer2_Tick(object sender, EventArgs e)
        {
            lab_time.Text = theHandelControls.DayOfWeek(DateTime.Now.DayOfWeek.ToString());
        }
        #endregion



        /// <summary>
        /// 刷新左侧的ListBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            theScoket.theSocketSend<byte[]>(0x08, null);
        }

        private void button42_Click(object sender, EventArgs e)
        {
            if (IP_Txt.Text == "192.168.1.76")
            {
                IP_Txt.Text = "116.226.184.235";
            }
            else
            {
                IP_Txt.Text = "192.168.1.76";
            }
            //192.168.1.72
        }
        /// <summary>
        /// 下拉框更新表格数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void com_CatalogCollection_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (com_CatalogCollection.SelectedIndex >= 0)
            {
                byte[] bys = GetTagToByteArray();
                if (!com_CatalogCollection.SelectedItem.ToString().Equals("---请选择---"))
                {
                    com_CatalogCollection.Items.Remove("---请选择---");
                }
                if (bys != null && bys.Length > 1)
                {

                    //当前目录是否存在和有文件
                    DirectoryInfo directoryInfo = new DirectoryInfo(pullPath(1, true));
                    if (directoryInfo.Exists && directoryInfo.GetFiles().Length > 0)
                    {
                        FileInfo[] fileArray = directoryInfo.GetFiles();
                        TheCatalog theTheCatalog = new TheCatalog();
                        theTheCatalog.DEVICE_ADDRESS = bys;
                        theTheCatalog.CATALOG_NAME = theHandelControls.GetConBoxItem(com_CatalogCollection.SelectedItem.ToString());
                        theTheCatalog.THE_CATALOG_FILES = new List<CatalogFiles>();
                        for (int i = 0; i < fileArray.Length; i++)
                        {
                            CatalogFiles theCatalogFiles = new CatalogFiles();
                            string filterFile = fileArray[i].Name;
                            if (filterFile.Contains(".Hex") || filterFile.Contains(".Binary"))
                            {
                                continue;
                            }
                            theCatalogFiles.FILE_NAME = Encoding.ASCII.GetBytes(fileArray[i].Name);
                            theTheCatalog.THE_CATALOG_FILES.Add(theCatalogFiles);
                        }
                        fileDataGridView.Rows.Clear();
                        theHandelControls.GetDataGridViewInfo(theTheCatalog, fileDataGridView);
                    }
                    else
                    {
                        fileDataGridView.Rows.Clear();
                    }
                }
            }

        }
        /// <summary>
        /// 获得文件的相对路径或绝对路径
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        private string pullPath(int number, bool flag)
        {
            string fileSavePath = "";//父级目录
            switch (number)
            {
                case 1: fileSavePath = ConfigurationManager.AppSettings["FilePath_FileCatalog"].ToString(); break;
                case 2: fileSavePath = ConfigurationManager.AppSettings["FilePath_CreateFile"].ToString(); break;
                case 3: fileSavePath = ConfigurationManager.AppSettings["FilePath_SettingTest"].ToString(); break;
                default: break;
            }
            string address = ByteWithString.byteToHexStr(GetTagToByteArray());//获得选中地址
            string catalog = "";  //获得选中目录
            string pullPath = "";//拼接路径
            if (flag == true)
            {
                catalog = com_CatalogCollection.SelectedItem.ToString();
                pullPath = fileSavePath + "/" + address + "/" + catalog;
            }
            else
            {
                pullPath = fileSavePath + "/" + address;
            }
            return pullPath;
        }
        /// <summary>
        /// 获取最新的遥测数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTelemeteringData_Click(object sender, EventArgs e)
        {


        }



        private void button9_Click(object sender, EventArgs e)
        {
            sendResponse("01");
        }

        private void button41_Click(object sender, EventArgs e)
        {
            sendResponse("02");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sendResponse("03");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sendResponse("04");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            sendResponse("05");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            sendResponse("06");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            sendResponse("07");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            sendResponse("08");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            sendResponse("09");
        }
        private void button18_Click(object sender, EventArgs e)
        {
            sendRequst("SettingOne", "01");
        }
        private void button10_Click(object sender, EventArgs e)
        {
            sendRequst("SettingTow", "02");
        }
        private void button11_Click(object sender, EventArgs e)
        {
            sendRequst("SettingThree", "03");

        }

        private void button12_Click(object sender, EventArgs e)
        {
            sendRequst("SettingFour", "04");
        }
        private void button16_Click(object sender, EventArgs e)
        {

            sendRequst("SettingFive", "05");
        }
        private void button17_Click(object sender, EventArgs e)
        {
            sendRequst("SettingSix", "06");
        }
        private void button15_Click(object sender, EventArgs e)
        {
            sendRequst("SettingSeven", "07");
        }

        private void button14_Click(object sender, EventArgs e)
        {

            sendRequst("SettingEight", "08");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            sendRequst("SettingNine", "09");
        }
        private List<string> GetControlTagAndValue(string contorlName, out List<string> contorlsValue)
        {
            List<string> controlsTag = new List<string>();
            contorlsValue = new List<string>();
            try
            {
                Control Controls = tabControl2.Controls.Find(contorlName, true)[0];
                List<Control> ControlArray = new List<Control>();
                foreach (Control item in Controls.Controls)
                {
                    ControlArray.Add(item);
                }
                ControlArray = ControlArray.OrderBy(u => u.TabIndex).ToList<Control>();
                foreach (Control item in ControlArray)
                {
                    if (item is TextBox)
                    {
                        TextBox textBox = item as TextBox;
                        if (theHandelControls.ValidateLength(contorlName, textBox.Text.Replace(" ", "")))
                        {
                            if (textBox.Tag.ToString() == "8073")
                            {

                                controlsTag.Add(textBox.Tag.ToString());
                                contorlsValue.Add(ipBox.IpAddressString);
                            }
                            else if (textBox.Tag.ToString() == "8075")
                            {
                                controlsTag.Add(textBox.Tag.ToString());
                                contorlsValue.Add(ipBox2.IpAddressString);
                            }
                            else
                            {
                                controlsTag.Add(textBox.Tag.ToString());
                                contorlsValue.Add(textBox.Text);
                            }

                        }
                    }
                    else if (item is ComboBox)
                    {
                        ComboBox comboBox = item as ComboBox;
                        controlsTag.Add(comboBox.Tag.ToString());
                        if (comboBox.SelectedIndex > 0)
                        {
                            if (contorlName == "SettingThree" || contorlName == "SettingFive")
                            {
                                contorlsValue.Add("00" + (comboBox.SelectedIndex - 1).ToString("X2"));
                            }
                            else
                            {
                                
                                contorlsValue.Add((comboBox.SelectedIndex-1).ToString("X2"));
                            }

                        }
                        else
                        {
                            contorlsValue.Add("");
                        }
                    }
                }
            }
            catch (Exception msg)
            {
                Log.LogWrite(msg);
            }
            return controlsTag;
        }
        /// <summary>
        /// 消息
        /// </summary>
        List<string> MsgList = new List<string>();
        private void showMsgInfo(List<string> msg)
        {
            string str = "";
            if (msg.Count > 0)
            {
                for (int i = 0; i < MsgList.Count; i++)
                {
                    str += MsgList[i] + ",";
                }
                MessageBox.Show("【" + str.TrimEnd(',') + "】输入有错", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }
        /// <summary>
        /// 设置文本框值验证
        /// </summary>
        /// <param name="contorlName"></param>
        /// <returns></returns>
        private bool ValidateGroupContorls(string contorlName)
        {
            int countErroe = 0;
            if (MessageBox.Show("是否确认设置？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.No)
            {
                return false;
            }
            bool falg = true;
            try
            {
                MsgList.Clear();
                Control Controls = tabControl2.Controls.Find(contorlName, true)[0];
                List<Control> ControlArray = new List<Control>();
                foreach (Control item in Controls.Controls)
                {
                    ControlArray.Add(item);
                }
                ControlArray = ControlArray.OrderBy(u => u.TabIndex).ToList<Control>();
                foreach (Control item in ControlArray)
                {
                    if (item is TextBox)
                    {
                        TextBox textBox = item as TextBox;
                        textBox.BackColor = Color.White;
                        int length = theKeyValueLength.SearchKey(textBox.Tag.ToString());
                        switch (theHandelControls.theKeyValueAddress.SearchKey(textBox.Tag.ToString()))
                        {
                            case "ASCII":
                                int toASCIILength = theHandelControls.StringConvertByte(textBox.Tag.ToString(), textBox.Text).Length;
                                if (toASCIILength > length)
                                {
                                    textBox.BackColor = Color.Red;
                                    MsgList.Add(textBox.Text);
                                    countErroe++;
                                    falg = false;
                                    continue;
                                }
                                break;
                            case "DEC":
                                int toDECLength = theHandelControls.StringConvertByte(textBox.Tag.ToString(), textBox.Text).Length;
                                if (toDECLength > length)
                                {
                                    textBox.BackColor = Color.Red;
                                    MsgList.Add(textBox.Text);
                                    countErroe++;
                                    falg = false;
                                    continue;
                                }
                                break;
                            case "IP":
                                IpInputBox theIpInputBox = textBox as IpInputBox;
                                int toIPLength = theHandelControls.StringConvertByte(theIpInputBox.Tag.ToString(), theIpInputBox.IpAddress).Length;
                                if (toIPLength > length)
                                {
                                    theIpInputBox.BackColor = Color.Red;
                                    MsgList.Add(theIpInputBox.IpAddress);
                                    falg = false;
                                    countErroe++;
                                    continue;
                                }
                                break;
                            case "HEX":
                                if (!Regex.IsMatch(textBox.Text, @"[a-fA-F0-9]{" + (length * 2).ToString() + "}"))
                                {
                                    textBox.BackColor = Color.Red;
                                    MsgList.Add(textBox.Text);
                                    falg = false;
                                    countErroe++;
                                    continue;
                                }
                                else
                                {
                                    int textLength = theHandelControls.StringConvertByte(textBox.Tag.ToString(), textBox.Text).Length;
                                    if (length != textLength)
                                    {
                                        textBox.BackColor = Color.Red;
                                        MsgList.Add(textBox.Text);
                                        countErroe++;
                                        falg = false;
                                    }
                                }
                                break;
                            default: break;
                        }
                    }
                    else if (item is ComboBox)
                    {
                        ComboBox comboBox = item as ComboBox;
                        if (comboBox.SelectedIndex == 0)
                        {
                            countErroe++;
                            falg = false;
                        }
                    }
                }
            }
            catch (Exception msg)
            {
                Log.LogWrite(msg);
            }
            if(countErroe>0)
            {
                falg=false;
                MessageBox.Show("请确认参数不为空？", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
             falg=true;
            }
            return falg;
        }
        /// <summary>
        /// 封装获取控件值的方法
        /// </summary>
        /// <param name="code"></param>
        private void sendResponse(string code)
        {
            if (deviceDataGridView.Rows.Count > 0)
            {
                theHandelControls.RequestDeviceStar(GetTagToByteArray(), code);
            }

        }
        //封装设置控件值的方法
        private void sendRequst(string groupName, string groupNumber)
        {
            if (ValidateGroupContorls(groupName))
            {
                ControlsTag = GetControlTagAndValue(groupName, out ControlsValue);
                theHandelControls.ResponseSettingParameter(GetTagToByteArray(), groupNumber, ControlsTag, ControlsValue);
            }
            //else
            //{
            //    showMsgInfo(MsgList);
            //}
        }
        //封装遥控的方法
        private void sendControl(byte commandByte, byte index)
        {
            byte[] address = GetTagToByteArray();
            if (address != null && address.Length > 1)
            {
                byte[] btyArray = ByteWithString.strToToHexByte(comboBox6.SelectedItem.ToString());
                List<byte> listArray = new List<byte>();
                listArray = address.ToList();
                listArray = listArray.Concat(btyArray.ToList()).ToList();
                listArray.Add(index);
                theHandelControls.ToTheCatalog(commandByte, address, listArray.ToArray());
            }
        }
        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox5.SelectedIndex > 0)
            {
                comboBox5.Items.Remove("---请选择---");
            }
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox6.SelectedIndex > 0)
            {
                comboBox6.Items.Remove("---请选择---");
            }
        }



        private void button18_Click_1(object sender, EventArgs e)
        {
            byte[] bys = GetTagToByteArray();
            if (bys != null && bys.Length > 1)
            {
                byte bty = theHandelControls.GetConInstrueBoxItem(comboBox5.SelectedItem.ToString());
                if (bty != 0xbb)
                {
                    theScoket.theSocketSend<byte[]>(bty, bys);
                }
            }

        }
        //遥控预制
        private void button19_Click(object sender, EventArgs e)
        {
            byte command = ByteWithString.strToToHexByte(comboBox7.SelectedItem.ToString())[0];
            sendControl(0x17, command);
        }
        //遥控执行
        private void button23_Click(object sender, EventArgs e)
        {
            byte command = ByteWithString.strToToHexByte(comboBox7.SelectedItem.ToString())[0];
            sendControl(0x18, command);
        }

        private void button22_Click(object sender, EventArgs e)
        {

        }


        private int none = -1;
        //每次单击的格子会刷新设备地址
        private void deviceDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (deviceDataGridView.Rows.Count > 0 && e.RowIndex >= 0)
            {
                DeviceAddress = deviceDataGridView.Rows[e.RowIndex].Cells["deviceAddress"].Value.ToString();
                if (none < 0)
                {
                    isDeviceAddress = deviceDataGridView.Rows[0].Cells["deviceAddress"].Value.ToString();
                    none = 100;
                }
                if (DeviceAddress != isDeviceAddress)
                {
                    //开始清理所有的控件数据...
                    clearAllData();
                    isDeviceAddress = DeviceAddress;
                    nowRowIndex = -1;
                }
            }
        }
        /// <summary>
        /// 清理控件数据
        /// </summary>
        public void clearAllData()
        {
            ipBox.IpAddress = "...";
            ipBox.UpDateIpaddress();
            ipBox2.IpAddress = "...";
            ipBox2.UpDateIpaddress();
            HandelControls.ClearTelemeteringAndRemote(TelemeteringView, RemoteView);//清理遥测遥信
            GropPabelClearSettingOne();
            HandelControls.GropPabelClear(SettingTow);
            HandelControls.GropPabelClear(SettingThree);
            HandelControls.GropPabelClear(SettingFour);
            HandelControls.GropPabelClear(SettingFive);
            HandelControls.GropPabelClear(SettingSix);
            HandelControls.GropPabelClear(SettingSeven);
            HandelControls.TabPageClear(SettingEight);
            HandelControls.TabPageClear(SettingNine);
            fileDataGridView.Rows.Clear();
        }

        private void GropPabelClearSettingOne()
        {
            textBox233.Text = "忙指示:";
            textBox232.Text = "TCP状态:";
            textBox231.Text = "FTP状态:";
            textBox228.Text = "登录状态:";
            textBox230.Text = "发送状态:";
            textBox229.Text = "接收状态:";
            textBox227.Text = "模块检测:";
            textBox226.Text = "SIM卡检测:";
            textBox225.Text = "GSM服务:";
            textBox131.Text = "GPRS服务:";
            label179.Text = "0";
            progressBar1.Value = 0;
        }
        private void button24_Click(object sender, EventArgs e)
        {
            theCreateSettingForm.Text = "线路参数设置";
            theCreateSettingForm.ShowDialog();
        }


        private void button27_Click(object sender, EventArgs e)
        {
            RouteMap theRouteMap = new RouteMap();
            theRouteMap.Text = "线路故障拓扑图";
            theRouteMap.ShowDialog();
        }

        void theScoket_timeParameterHandel(object sender, EventArgs e)
        {
            DataEventArgs logDataEventArg = (DataEventArgs)e;
            List<int> listIntArray = logDataEventArg.obj as List<int>;
            Action<List<int>> theGeneratrixMehod = bindTimeSettingArray<int>;
            this.Invoke(theGeneratrixMehod, new object[] { listIntArray });
        }

        private void button29_Click(object sender, EventArgs e)
        {

            OpenFileDialog excel_file = new OpenFileDialog();
            if (DialogResult.OK == excel_file.ShowDialog(this))
            {
                if (HandelControls.Msg("是否确定上传？", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    FileStream fs = new FileStream(excel_file.FileName, FileMode.Open, FileAccess.Read);
                    List<byte> contList = new List<byte>();
                    contList = contList.Concat(GetTagToByteArray()).ToList();//设备地址
                    byte[] infoBytes = new byte[(long)fs.Length];
                    fs.Read(infoBytes, 0, infoBytes.Length);
                    fs.Close();
                    if (theScoket.SOCKET_SEND != null && theScoket.SOCKET_SEND.Connected)
                    {
                        contList = contList.Concat(infoBytes).ToList();
                        theScoket.theSocketSend<byte[]>(0x39, contList.ToArray());
                    }
                    else
                    {
                        HandelControls.Msg("上传失败，请检查是否连接成功", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
        }

        private void button22_Click_1(object sender, EventArgs e)
        {
            PortSetting thePortSetting = new PortSetting();
            thePortSetting.ShowDialog();
        }

    }
}
