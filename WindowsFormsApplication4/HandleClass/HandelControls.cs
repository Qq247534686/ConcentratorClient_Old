using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using 集中器控制客户端.HandleClass.reportModels;
using System.Text.RegularExpressions;
using System.Data;
using System.Threading;

namespace 集中器控制客户端.HandleClass
{
    /// <summary>
    /// 对控件进行赋值操作
    /// </summary>
    public class HandelControls
    {
        #region 初始化变量
        private HandleTool theScoket = HandleTool.createHandleTool();
        KeyValueRemote theKeyValueRemote = new KeyValueRemote();
        KeyValueTelemetering theKeyValueTelemetering = new KeyValueTelemetering();
        public KeyValueAddress theKeyValueAddress = new KeyValueAddress();
        SQLiteHelper theSQLiteHelper = new SQLiteHelper();
        private string filePath = ConfigurationManager.AppSettings["FilePath_FileCatalog"].ToString();//存放的目录
        #endregion

        #region 验证登陆
        /// <summary>
        /// 验证是否登陆成功
        /// </summary>
        /// <param name="getFromObject"></param>
        /// <param name="theSettingGrop"></param>
        /// <returns></returns>
        public bool Loging(ConcentratorControlClient getFromObject, SettingGrop theSettingGrop)
        {
            bool isLogin = false;
            if (getFromObject.btn_Connect.Text == StringInfo.btn_OK)
            {
                if (getFromObject.user_Txt.Text.Trim().Length <= 0 || getFromObject.Pws_Txt.Text.Trim().Length <= 0)
                {
                    HandelControls.Msg("用户名密码不能为空", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return isLogin;
                }
                if (!Regex.IsMatch(getFromObject.IP_Txt.Text, @"((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)") || !Regex.IsMatch(getFromObject.Port_Txt.Text.Trim(), @"^[0-9]{4,10}$"))
                {
                    HandelControls.Msg("连地址或端口不正确", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return isLogin;
                }
                getFromObject.btn_Connect.Enabled = false;
                getFromObject.btn_Connect.Text = "连接中...";
                UserInfo theUserInfo = new UserInfo()
                {
                    IP_ADDRESS = getFromObject.IP_Txt.Text,
                    PORT_NUMBER = getFromObject.Port_Txt.Text,
                    USER_NAME = getFromObject.user_Txt.Text,
                    PASSWORD = getFromObject.Pws_Txt.Text
                };
                if (theScoket.createSocket(theUserInfo))
                {
                    isLogin = true;
                    BtnRestore(getFromObject, StringInfo.btn_NO, StringInfo.Img_OK);
                    getFromObject.btn_Connect.Enabled = true;
                    //开始总召
                    //if (checkBox1.Checked)
                    //{
                    //    byte[] bys = GetTagToByteArray();
                    //    if (bys != null && bys.Length > 1)
                    //    {
                    //        theScoket.theSocketSend<byte[]>(0x09, bys);
                    //    }
                    //}
                }
                else
                {
                    BtnRestore(getFromObject, StringInfo.btn_OK, StringInfo.Img_NO);
                    getFromObject.btn_Connect.Enabled = true;
                    if (theSettingGrop != null)
                    {
                        theSettingGrop.ClearArray();
                    }
                    theScoket.CloseThread();
                    HandelControls.Msg("连接失败，请检查服务器是否开启", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
            else
            {
                if (MessageBox.Show("是否确定断开", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {

                    if (theSettingGrop != null)
                    {
                        theSettingGrop.ClearArray();
                    }
                    theScoket.CloseThread();
                    cellStaue(getFromObject.deviceDataGridView);
                    BtnRestore(getFromObject, StringInfo.btn_NO_Ing, StringInfo.Img_OK);
                    getFromObject.btn_Connect.Enabled = false;
                    theScoket.stopSocket();
                    getFromObject.clearAllData();
                    Thread.Sleep(3000);
                    getFromObject.btn_Connect.Enabled = true;
                    BtnRestore(getFromObject, StringInfo.btn_OK, StringInfo.Img_NO);
                }
            }
            return isLogin;

        }
        /// <summary>
        /// 断开
        /// </summary>
        /// <param name="btnTxt"></param>
        /// <param name="imgName"></param>
        public static void BtnRestore(ConcentratorControlClient getFromObject, String btnTxt, String imgName)
        {
            getFromObject.btn_Connect.Text = btnTxt;
            getFromObject.pic_StateImg.Image = Image.FromFile(@"Img/" + imgName);
            getFromObject.pic_User.Image = Image.FromFile(@"Img/offline_user_24px_508417_easyicon.net.png");
        }
        //当点击“断开”连接按钮时默认全部设备离线
        public void cellStaue(DataGridView deviceDataGridView)
        {
            for (int i = 0; i < deviceDataGridView.Rows.Count; i++)
            {
                if (deviceDataGridView.Rows[i].Cells["deviceStatus"].Value.ToString() != "未知")
                {
                    deviceDataGridView.Rows[i].Cells["deviceStatus"].Value = "离线";
                    deviceDataGridView.Rows[i].Cells["deviceStatus"].Style.BackColor = Color.Gray;
                }

            }
        }
        #endregion

        #region 单例模式
        private static readonly HandelControls theHandelControls = new HandelControls();
        private HandelControls() { }
        public static HandelControls createHandelControls()
        {
            return theHandelControls;
        }
        #endregion

        #region 创建列
        public void CreateFileViewColumns(DataGridView fileDataGridView)
        {
            fileDataGridView.Columns.Add("fileName", "文件名");
            fileDataGridView.Columns.Add("filestate", "文件状态");
            fileDataGridView.Columns.Add("soureFileDown", "源文件下载");
            fileDataGridView.Columns.Add("towFileBtn", "二进制文件下载");
            fileDataGridView.Columns.Add("sixFileBtn", "十六进制文件下载");
            fileDataGridView.Columns.Add("ImgBtn", "波形图");
            fileDataGridView.Columns.Add("updateFileData", "最近更新日期");
            fileDataGridView.Columns.Add("updateFile", "更新文件");
            fileDataGridView.Columns["towFileBtn"].Visible = false;
            fileDataGridView.Columns["sixFileBtn"].Width = 120;
            fileDataGridView.Columns["updateFile"].SortMode = DataGridViewColumnSortMode.NotSortable;
            fileDataGridView.Columns["soureFileDown"].SortMode = DataGridViewColumnSortMode.NotSortable;
            fileDataGridView.Columns["sixFileBtn"].SortMode = DataGridViewColumnSortMode.NotSortable;
            fileDataGridView.Columns["towFileBtn"].SortMode = DataGridViewColumnSortMode.NotSortable;
            fileDataGridView.Columns["ImgBtn"].SortMode = DataGridViewColumnSortMode.NotSortable;
        }
        public void CreateDeviceViewColumns(DataGridView deviceDataGridView)
        {
            deviceDataGridView.Columns.Add("deviceLine", "路线");
            deviceDataGridView.Columns.Add("deviceAddress", "地址");
            deviceDataGridView.Columns.Add("deviceName", "设备");
            deviceDataGridView.Columns["deviceName"].Visible = false;
            deviceDataGridView.Columns.Add("deviceStatus", "状态");
            deviceDataGridView.Columns["deviceLine"].Visible = false;
            deviceDataGridView.Columns["deviceLine"].SortMode = DataGridViewColumnSortMode.NotSortable;
            deviceDataGridView.Columns["deviceName"].SortMode = DataGridViewColumnSortMode.NotSortable;
            deviceDataGridView.Columns["deviceAddress"].SortMode = DataGridViewColumnSortMode.NotSortable;
            deviceDataGridView.Columns["deviceStatus"].SortMode = DataGridViewColumnSortMode.NotSortable;

        }
        #endregion

        #region 创建初始化存文件数据的目录
        //创建初始化存文件数据的目录
        public void CreateLoadML()
        {
            List<string> strList = new List<string>();
            strList.Add(ConfigurationManager.AppSettings["FilePath_FileCatalog"].ToString());
            strList.Add(ConfigurationManager.AppSettings["FilePath_CreateFile"].ToString());
            strList.Add(ConfigurationManager.AppSettings["FilePath_SettingTest"].ToString());
            strList.ForEach((u) =>
            {
                if (!Directory.Exists(u))
                {
                    Directory.CreateDirectory(u);
                }
            });
        }
        #endregion

        #region 判断设备状态从而改变“在线”，“离线”的颜色
        //判断设备状态从而改变“在线”，“离线”的颜色
        public void cellColor(DataGridView dt, string selectAddress)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                switch (dt.Rows[i].Cells["deviceStatus"].Value.ToString())
                {
                    case "在线": dt.Rows[i].Cells["deviceStatus"].Style.BackColor = Color.Green; break;
                    case "离线": dt.Rows[i].Cells["deviceStatus"].Style.BackColor = Color.Gray; break;
                    default: break;
                }
                if (dt.Rows[i].Cells["deviceAddress"].Value.ToString().Equals(selectAddress))
                {
                    dt.Rows[0].Selected = false;
                    dt.Rows[i].Selected = true;
                }
            }
        }
        #endregion

        #region 获得下拉框选中目录对应的十六进制编码
        /// <summary>
        /// 获得下拉框选中目录对应的十六进制编码
        /// </summary>
        /// <param name="comSelectValue"></param>
        /// <returns></returns>
        public byte[] GetConBoxItem(string comSelectValue)
        {
            byte[] bytes = new byte[4];
            switch (comSelectValue)
            {
                case "历史负荷数据":
                    bytes = new byte[4] { 0x06, 0x68, 0x00, 0x00 };
                    break;
                case "Log文件":
                    bytes = new byte[4] { 0x07, 0x68, 0x00, 0x00 };
                    break;
                case "录波文件":
                    bytes = new byte[4] { 0x0a, 0x68, 0x00, 0x00 };
                    break;
                case "预留1":
                    bytes = new byte[4] { 0x08, 0x68, 0x00, 0x00 };
                    break;
                case "预留2":
                    bytes = new byte[4] { 0x09, 0x68, 0x00, 0x00 };
                    break;
                default:
                    break;
            }
            return bytes;
        }
        #endregion

        #region 文件表格
        /// <summary>
        /// 设置表格属性
        /// </summary>
        /// <param name="theCatalog"></param>
        /// <param name="dt"></param>
        public void GetDataGridViewInfo(TheCatalog theCatalog, DataGridView dt)
        {
            try
            {
                string deviceAdddress = ByteWithString.byteToHexStrAppend(theCatalog.DEVICE_ADDRESS, "");//地址
                string catalogName = GetConBoxItemToBytes(theCatalog.CATALOG_NAME);//选择的文件目录
                string deviceCatalog = filePath + "/" + deviceAdddress + "/" + catalogName;
                if (!Directory.Exists(deviceCatalog))
                {
                    DirectoryInfo theDirectory = Directory.CreateDirectory(deviceCatalog);
                }
                if (theCatalog != null && theCatalog.THE_CATALOG_FILES != null && theCatalog.THE_CATALOG_FILES.Count > 0)
                {
                    for (int i = 0; i < theCatalog.THE_CATALOG_FILES.Count; i++)
                    {

                        dt.Rows.Add();
                        //当前文件名
                        string fileName = Encoding.ASCII.GetString(theCatalog.THE_CATALOG_FILES[i].FILE_NAME);
                        if (fileName.Contains(".Hex") || fileName.Contains(".Binary"))
                        {
                            continue;
                        }
                        //文件的绝对路径
                        string filePathName = deviceCatalog + "/" + fileName;
                        //当前目录
                        DirectoryInfo directoryInfo = new DirectoryInfo(deviceCatalog);
                        //当前目录下的所有文件
                        FileInfo[] fileInfo = directoryInfo.GetFiles();//获得所有文件
                        //当前目录下是否存在此文件
                        FileInfo isHaveFile = (from a in fileInfo where a.Name == fileName select a).FirstOrDefault<FileInfo>();
                        if (isHaveFile == null)//不存在
                        {
                            dt.Rows[i].Cells["filestate"].Value = "未下载";
                            dt.Rows[i].Cells["updateFileData"].Value = "------";
                        }
                        else
                        {
                            dt.Rows[i].Cells["filestate"].Value = "已下载";
                            string dataFormat = "yyyy-MM-dd HH:mm:ss";
                            dt.Rows[i].Cells["updateFileData"].Value = File.GetLastWriteTime(filePathName).ToString(dataFormat);
                        }

                        dt.Rows[i].Cells["fileName"].Value = fileName;
                        dt.Rows[i].Cells["filestate"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        if (dt.Rows[i].Cells["filestate"].Value.ToString() == "读取中")
                        {
                            dt.Rows[i].Cells["filestate"].Style.ForeColor = Color.Gray;
                        }
                        DataGridViewLinkCell soureFileDown = new DataGridViewLinkCell();
                        SetCellLinkColor(soureFileDown);
                        soureFileDown.Value = "源文件下载";
                        DataGridViewLinkCell towFileBtn = new DataGridViewLinkCell();
                        SetCellLinkColor(towFileBtn);
                        DataGridViewLinkCell sixFileBtn = new DataGridViewLinkCell();
                        SetCellLinkColor(sixFileBtn);
                        DataGridViewLinkCell ImgBtn = new DataGridViewLinkCell();
                        SetCellLinkColor(ImgBtn);
                        DataGridViewButtonCell updateFile = new DataGridViewButtonCell();
                        updateFile.Style.ForeColor = Color.White;
                        updateFile.Style.BackColor = Color.Black;
                        updateFile.Value = "更新文件";
                        updateFile.FlatStyle = FlatStyle.Popup;
                        if (!fileName.Contains(".dat"))
                        {
                            ImgBtn.Value = "---";
                            sixFileBtn.Value = "---";
                            if (catalogName.Equals("历史负荷数据"))
                            {
                                towFileBtn.Value = "二进制文件下载";
                            }
                            else
                            {
                                towFileBtn.Value = "---";
                            }

                        }
                        else
                        {
                            ImgBtn.Value = "波形图";
                            sixFileBtn.Value = "十六进制文件下载";
                            towFileBtn.Value = "二进制文件下载";
                        }
                        dt.Rows[i].Cells["soureFileDown"] = soureFileDown;
                        dt.Rows[i].Cells["soureFileDown"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dt.Rows[i].Cells["towFileBtn"] = towFileBtn;
                        dt.Rows[i].Cells["towFileBtn"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dt.Rows[i].Cells["sixFileBtn"] = sixFileBtn;
                        dt.Rows[i].Cells["sixFileBtn"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dt.Rows[i].Cells["ImgBtn"] = ImgBtn;
                        dt.Rows[i].Cells["ImgBtn"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dt.Rows[i].Cells["updateFile"] = updateFile;
                        dt.Rows[i].Cells["updateFile"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        HideGridViewColumn(catalogName, dt);//要隐藏的列明
                    }

                }
            }
            catch (Exception msg)
            {
                Log.LogWrite(msg);
            }

        }

        private string GetConBoxItemToBytes(byte[] bytes)
        {
            string CatalogName = string.Empty;
            byte[] historicalLoadData = new byte[4] { 0x06, 0x68, 0x00, 0x00 };
            byte[] LogFile = new byte[4] { 0x07, 0x68, 0x00, 0x00 };
            byte[] recordFile = new byte[4] { 0x0a, 0x68, 0x00, 0x00 };
            byte[] setAsideFirst = new byte[4] { 0x08, 0x68, 0x00, 0x00 };
            byte[] setAsideSecond = new byte[4] { 0x09, 0x68, 0x00, 0x00 };
            if (ByteWithString.compareArray(bytes, historicalLoadData))
            {
                CatalogName = "历史负荷数据";
            }
            else if (ByteWithString.compareArray(bytes, LogFile))
            {
                CatalogName = "Log文件";
            }
            else if (ByteWithString.compareArray(bytes, recordFile))
            {
                CatalogName = "录波文件";
            }
            else if (ByteWithString.compareArray(bytes, setAsideFirst))
            {
                CatalogName = "预留1";
            }
            else if (ByteWithString.compareArray(bytes, setAsideSecond))
            {
                CatalogName = "预留2";
            }
            return CatalogName;
        }
        /// <summary>
        /// 隐藏或者显示列
        /// </summary>
        /// <param name="catalogName">下拉框选中的目录</param>
        /// <param name="dt">要操作的表格</param>
        private void HideGridViewColumn(string catalogName, DataGridView dt)
        {
            switch (catalogName)
            {
                case "历史负荷数据":
                    dt.Columns["towFileBtn"].Visible = true;
                    dt.Columns["sixFileBtn"].Visible = true;
                    dt.Columns["ImgBtn"].Visible = true;
                    break;
                case "Log文件":
                    dt.Columns["towFileBtn"].Visible = false;
                    dt.Columns["sixFileBtn"].Visible = false;
                    dt.Columns["ImgBtn"].Visible = false;
                    break;
                case "录波文件":
                    dt.Columns["towFileBtn"].Visible = true;
                    dt.Columns["sixFileBtn"].Visible = true;
                    dt.Columns["ImgBtn"].Visible = true;
                    break;
                case "预留1":
                    dt.Columns["towFileBtn"].Visible = true;
                    dt.Columns["sixFileBtn"].Visible = true;
                    dt.Columns["ImgBtn"].Visible = true;
                    break;
                case "预留2":
                    dt.Columns["towFileBtn"].Visible = true;
                    dt.Columns["sixFileBtn"].Visible = true;
                    dt.Columns["ImgBtn"].Visible = true;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 表格样式
        /// </summary>
        /// <param name="linkCell"></param>
        private void SetCellLinkColor(DataGridViewLinkCell linkCell)
        {
            linkCell.LinkColor = Color.Blue;
            linkCell.ActiveLinkColor = Color.Red;
            linkCell.VisitedLinkColor = Color.Blue;
        }
        #endregion

        #region 发送格式
        /// <summary>
        /// 目录发送格式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        public static ClientReport CreateClientReport<T>(byte content, T body)
        {
            ClientReport theClientReport = new ClientReport();
            theClientReport.HEAD_FIELD = 0x03;
            theClientReport.REPORT_LENGTH = new byte[] { 0x00, 0x00, 0x00, 0x00 };
            theClientReport.CONTROL_FIELD = content;
            theClientReport.REPORT_BODY = body;
            theClientReport.END_FIELD = 0x16;
            return theClientReport;
        }
        #endregion

        #region 发送要召唤的目录信息
        /// <summary>
        /// 发送要召唤的目录信息
        /// </summary>
        /// <param name="DeviceAddress"></param>
        /// <param name="CatalogName"></param>
        public void ToTheCatalog(byte commamdByte, byte[] DeviceAddress, byte[] CatalogName)
        {
            try
            {
                TheCatalog theCatalog = new TheCatalog();
                theCatalog.CATALOG_NAME = CatalogName;
                theCatalog.DEVICE_ADDRESS = DeviceAddress;
                byte[] bytes = theScoket.clientReportUnSerialize(CreateClientReport<TheCatalog>(commamdByte, theCatalog));
                if (bytes.Length > 0)
                {
                    theScoket.sendInfo(bytes);
                }
            }
            catch (Exception msg)
            {
                Log.LogWrite(msg);
            }
        }

        #endregion

        #region 下载目录中的文件
        public void GetFileData(CatalogFiles catalogFiles)
        {
            theScoket.sendInfo(theScoket.clientReportUnSerialize(HandelControls.CreateClientReport<CatalogFiles>(0x06, catalogFiles)));
        }
        #endregion

        #region 获得图表所需的数据
        /// <summary>
        /// 获得图表所需的数据
        /// </summary>
        /// <param name="pullPathFile">文件路径</param>
        /// <returns></returns>
        public WaveData ChartShow(string pullPathFile)
        {
            HandleWaveData theHandleWaveData = new HandleWaveData();
            return theHandleWaveData.hexadecimalFileSerialize(theHandleWaveData.binaryFileSerialize(ByteWithString.FileStreamTobyteArray(pullPathFile)));
        }
        #endregion

        #region 返回解析后存入时间的对象
        internal List<TheRemoteData> UpdateTheRemoteDataArray(List<TheRemoteData> theRemoteData)
        {
            for (int i = 0; i < theRemoteData.Count; i++)
            {
                theRemoteData[i].RemoteName = ToSixString(theRemoteData[i].RemoteAddress, 1);
                theRemoteData[i].RemoteValueStr = ByteWithString.byteToHexStr(theRemoteData[i].RemoteValue);
                theRemoteData[i].RemoteAddressStr = ByteWithString.byteToHexStr(theRemoteData[i].RemoteAddress);
                if (theRemoteData[i].LastestModify != null && theRemoteData[i].LastestModify.Length > 0)
                {
                    theRemoteData[i].LastestModifStr = ByteWithString.ConvertCP56TIME2aToDateTime(theRemoteData[i].LastestModify).ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            return theRemoteData;





        }
        #endregion

        #region 遥信遥测表格
        public string ToSixString(byte[] p, int number)
        {

            string strKeyValue = ByteWithString.byteToHexStr(p);
            if (number >= 1)
            {
                strKeyValue = theKeyValueRemote.SearchKey(strKeyValue);//遥信
            }
            else
            {
                strKeyValue = theKeyValueTelemetering.SearchKey(strKeyValue);//遥测
            }
            return strKeyValue;
        }
        internal void AddTelemeteringViewRowsData(DataGridView TelemeteringView)
        {
            int index = 0;
            foreach (var item in theKeyValueTelemetering.dic)
            {
                TelemeteringView.Rows.Add();
                TelemeteringView.Rows[index].Cells["TRemoteName"].Value = item.Value;
                TelemeteringView.Rows[index].Cells["TRemoteName"].Tag = item.Key;
                TelemeteringView.Rows[index].Cells["TRemoteValueStr"].Value = "";
                TelemeteringView.Rows[index].Cells["TLastestModifStr"].Value = "";
                index++;
            }
        }

        internal void AddRemoteViewViewRowsData(DataGridView RemoteView)
        {
            int index = 0;
            foreach (var item in theKeyValueRemote.dic)
            {
                RemoteView.Rows.Add();
                RemoteView.Rows[index].Cells["RemoteName"].Value = item.Value;
                RemoteView.Rows[index].Cells["RemoteName"].Tag = item.Key;
                RemoteView.Rows[index].Cells["RemoteValueStr"].Value = "";
                RemoteView.Rows[index].Cells["LastestModifStr"].Value = "";
                index++;
            }
        }
        #endregion

        #region 获得设备状态
        /// <summary>
        /// 获得设备状态 0x01
        /// </summary>
        /// <param name="p"></param>
        internal void RequestDeviceStar(byte[] p, string code)
        {
            ClientReport theClientReport = CreateClientReport<string>(0x13, ByteWithString.byteToHexStr(p) + "|" + code);
            byte[] bytes = theScoket.clientReportUnSerialize(theClientReport);
            if (bytes.Length > 0)
            {
                theScoket.sendInfo(bytes);
            }
        }
        #endregion

        #region 处理控件地址的数据
        private byte[] addZeroToBytes(string address, byte[] responseValue)
        {
            List<byte> responseValueList = responseValue.ToList();
            switch (address)
            {
                case "8050":
                    responseValueList.AddRange(appendByte(responseValueList.Count, 20));
                    break;
                case "8077":
                    responseValueList.AddRange(appendByte(responseValueList.Count, 16));
                    break;
                case "8078":
                    responseValueList.AddRange(appendByte(responseValueList.Count, 16));
                    break;
                case "8079":
                    responseValueList.AddRange(appendByte(responseValueList.Count, 16));
                    break;
                default: break;

            }
            return responseValueList.ToArray();
        }
        private List<byte> appendByte(int minCount, int count)
        {
            List<byte> responseValueList = new List<byte>();
            for (int i = minCount; i < count; i++)
            {
                responseValueList.Add(0x00);
            }
            return responseValueList;
        }
        internal string ToCodeValue(string address, byte[] responseValue)
        {
            string retValue = "";
            switch (theKeyValueAddress.SearchKey(address))
            {
                case "ASCII":
                    retValue = Encoding.ASCII.GetString(responseValue);
                    break;
                case "HEX":
                    retValue = ByteWithString.byteToHexStrAppend(responseValue, " ");
                    break;
                case "DEC":
                    retValue = (responseValue[1] << 8 | responseValue[0]).ToString();
                    break;
                case "IP":
                    retValue = responseValue[0].ToString() + "." + responseValue[1].ToString() + "." + responseValue[2].ToString() + "." + responseValue[3].ToString();
                    break;
                default: break;

            }
            return retValue;
        }
        public byte[] StringConvertByte(string address, string responseValue)
        {
            //address = "8080"; responseValue = "96 11";
            responseValue = responseValue.Replace(" ", "");
            List<byte> bytes = new List<byte>();
            switch (theKeyValueAddress.SearchKey(address))
            {
                case "ASCII":
                    bytes = Encoding.ASCII.GetBytes(responseValue).ToList();
                    bytes = addZeroToBytes(address, bytes.ToArray()).ToList();
                    break;
                case "HEX":
                    for (int i = 0; i < responseValue.Length / 2; i++)
                    {
                        byte toHex = Convert.ToByte(Convert.ToInt32(responseValue.Substring(i * 2, 2), 16));
                        bytes.Add(toHex);
                    }
                    break;
                case "DEC":
                    bytes.Add(Convert.ToByte(int.Parse(responseValue) & 0xff));
                    bytes.Add(Convert.ToByte(int.Parse(responseValue) >> 8));

                    break;
                case "IP":
                    string[] responseValueStrs = responseValue.Split('.');
                    for (int i = 0; i < responseValueStrs.Length; i++)
                    {
                        bytes.Add(Convert.ToByte(int.Parse(responseValueStrs[i].ToString())));
                    }
                    break;
                default: break;

            }
            return bytes.ToArray();
        }
        #endregion

        #region 向服务器发送通过验证的设定参数值
        /// <summary>
        /// 发送设定的参数
        /// </summary>
        /// <param name="address"></param>
        /// <param name="groupNumber"></param>
        /// <param name="parameterCount"></param>
        /// <param name="addressArray"></param>
        /// <param name="valueArray"></param>
        public bool ResponseSettingParameter(byte[] address, string groupNumber, List<string> addressArray, List<string> valueArray)
        {
            bool flag = true;
            try
            {
                ResponseValueData theResponseValueData = new ResponseValueData();
                theResponseValueData.DEVICE_ADDRESS = address;
                theResponseValueData.VALUE_TYPE = ByteWithString.strToToHexByte(groupNumber)[0];
                theResponseValueData.VALUE_COUNT = (byte)addressArray.Count;
                theResponseValueData.theResponseValueArray = new List<ResponseValue>();
                for (int i = 0; i < addressArray.Count; i++)
                {
                    byte[] theBytes = ByteWithString.strToToHexByte(addressArray[i]);
                    ResponseValue theResponseValue = new ResponseValue();
                    theResponseValue.VALUE_ADDRESS = new byte[] { theBytes[1], theBytes[0] };
                    theResponseValue.VALUE = StringConvertByte(addressArray[i], valueArray[i]);
                    theResponseValue.VALUE_LENGTH = (byte)theResponseValue.VALUE.Length;
                    theResponseValueData.theResponseValueArray.Add(theResponseValue);
                }
                ClientReport theClientReport = CreateClientReport<ResponseValueData>(0x15, theResponseValueData);
                byte[] bytes = theScoket.clientReportUnSerialize(theClientReport);
                if (bytes.Length > 0)
                {
                    theScoket.sendInfo(bytes);
                }

            }
            catch (Exception msg)
            {
                Log.LogWrite(msg);
            }
            return flag;
        }
        /// <summary>
        /// 验证值
        /// </summary>
        /// <param name="contorlName"></param>
        /// <param name="textLength"></param>
        /// <returns></returns>
        public bool ValidateLength(string contorlName, string textLength)
        {
            bool flag = true;
            switch (contorlName)
            {
                case "SettingThree":
                    if (textLength.Length <= 0 && textLength.Length % 2 != 0)
                    {
                        flag = false;
                    }
                    break;
                case "SettingFive":
                    if (textLength.Length <= 0 && textLength.Length % 2 != 0)
                    {
                        flag = false;
                    }
                    break;
                case "SettingSix":
                    if (textLength.Length <= 0 && textLength.Length % 2 != 0)
                    {
                        flag = false;
                    }
                    break;
                case "SettingSeven":
                    if (textLength.Length <= 0 && textLength.Length % 2 != 0)
                    {
                        flag = false;
                    }
                    break;
                case "SettingEight":
                    if (textLength.Length <= 0 && textLength.Length % 2 != 0)
                    {
                        flag = false;
                    }
                    break;
                case "SettingNine":
                    if (textLength.Length <= 0 && textLength.Length % 2 != 0)
                    {
                        flag = false;
                    }
                    break;
            }
            return flag;
        }
        #endregion

        #region 发送：总召，对时，实时遥测指令
        public byte GetConInstrueBoxItem(string comSelectValue)
        {
            byte bytes = 0xbb;
            switch (comSelectValue)
            {
                case "总召":
                    bytes = 0x09;
                    break;
                case "对时":
                    bytes = 0x20;
                    break;
                case "实时遥测":
                    bytes = 0x19;
                    break;
                default:
                    break;
            }
            return bytes;
        }
        #endregion

        #region 消息提示
        /// <summary>
        /// 显示提示消息
        /// </summary>
        /// <param name="msg"></param>
        public static void Msg(string msg)
        {
            MessageBox.Show(msg);
        }
        public static DialogResult Msg(string msg, MessageBoxButtons msgButtons, MessageBoxIcon msgIco)
        {
            return MessageBox.Show(msg, "提示", msgButtons, msgIco);

        }

        internal List<PortChartData> ChartShowNew(string pullPathFile,out List<string> listName)
        {
            List<PortChartData> theListPortChartData = new List<PortChartData>();
            int Index = 1;
            listName = new List<string>();
            int nowIndex = 0;
            try
            {
                string newPath = pullPathFile.Replace(".dat", ".cfg");
                using (StreamReader sr = new StreamReader(newPath))
                {
                    sr.ReadLine();
                    int myCount = int.Parse(Regex.Match(sr.ReadLine().Split(',')[1].Trim(), @"^[0-9]+").Value);
                    for (int i = 0; i < myCount; i++)
                    {
                        listName.Add(sr.ReadLine().Split(',')[1]);
                    }
                    using (FileStream sf = new FileStream(Path.GetFullPath(pullPathFile), FileMode.Open))
                    {
                        byte[] bytes = new byte[sf.Length];
                        sf.Read(bytes, 0, bytes.Length);
                        int count = bytes.Length / 48;
                        List<PortChartLineData> lines = new List<PortChartLineData>();
                        for (nowIndex = 0; nowIndex < count; nowIndex++)
                        {
                            List<byte> bs = bytes.Skip(nowIndex * 48).Take(myCount * 2 + 8).ToList();
                            PortChartData thePortChartData = new PortChartData();
                            thePortChartData.V_X = BytesSerialize.returnByteLengthRightLeft(bs.Skip(4).Take(4).ToArray());//竖坐标X
                            thePortChartData.H_Y = (ByteWithString.ByteArrayToInt(bs.Skip(8).Take(myCount * 2).ToArray()));//竖坐标Y
                            theListPortChartData.Add(thePortChartData);
                        }

                    }
                }
            }
            catch(Exception r) {
                int ss=nowIndex;
                int dddv = Index;
                //string ss = "";
            }
            return theListPortChartData;
        }

        




        #endregion

        #region 封装删除的方法
        /// <summary>
        /// 封装删除的方法
        /// </summary>
        /// <param name="dt">操作的表名</param>
        /// <param name="name">选中的列明</param>
        /// <param name="byt">操作标识</param>
        /// <param name="msg">要提示的消息</param>
        public void deleteSelectRow(DataGridView dt, string name, byte byt, string msg)
        {
            if (dt.SelectedRows.Count <= 0)
            {
                HandelControls.Msg("请选择要删除的数据");
                return;
            }
            if (HandelControls.Msg(msg, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                List<byte> bytes = new List<byte>();
                bytes.Add(byt);
                string id = dt.SelectedRows[0].Cells[name].Value.ToString();
                bytes.AddRange(ByteWithString.intTo4Byte(int.Parse(id)).ToList());
                theScoket.theSocketSend<byte[]>(0x32, bytes.ToArray());
            }
        }
        #endregion

        #region 删除遥信遥测表数据
        internal static void ClearTelemeteringAndRemote(DataGridView TelemeteringView, DataGridView RemoteView)
        {
            for (int i = 0; i < TelemeteringView.Rows.Count; i++)
            {
                TelemeteringView.Rows[i].Cells["TRemoteValueStr"].Value = "";
                TelemeteringView.Rows[i].Cells["TLastestModifStr"].Value = "";
            }
            for (int i = 0; i < RemoteView.Rows.Count; i++)
            {
                RemoteView.Rows[i].Cells["RemoteValueStr"].Value = "";
                RemoteView.Rows[i].Cells["LastestModifStr"].Value = "";
            }
        }
        #endregion

        #region 清理Group内的控件
        internal static void GropPabelClear(GroupBox SettingTow)
        {
            foreach (Control item in SettingTow.Controls)
            {
                if (item is TextBox)
                {
                    TextBox txb = item as TextBox;
                    txb.Text = "";

                }
                else if (item is ComboBox)
                {
                    ComboBox com = item as ComboBox;
                    com.SelectedIndex = 0;
                }
            }

        }
        #endregion

        #region 清理TabPage内的控件
        internal static void TabPageClear(TabPage SettingTow)
        {
            foreach (Control item in SettingTow.Controls)
            {
                if (item is TextBox)
                {
                    TextBox txb = item as TextBox;
                    txb.Text = "";

                }
                else if (item is ComboBox)
                {
                    ComboBox com = item as ComboBox;
                    com.SelectedIndex = 0;
                }
            }

        }
        #endregion

        #region 将遥测遥信数据存入sqlLite数据库
        internal void SaveDataAddOrUpdate(object obj)
        {
            try
            {
                ThreadData theThreadData = obj as ThreadData;
                //DataGridView dt, string deviceAddress, string name
                ReadSqlLiteData theReadData = new ReadSqlLiteData();
                for (int i = 0; i < theThreadData.THE_DATA_GRID_VIEW.Rows.Count; i++)
                {
                    switch (theThreadData.THE_NAME)
                    {
                        case "遥信":
                            theReadData.theAddress = theThreadData.THE_DEVICE_ADDRESS;
                            theReadData.theDataAddress = theThreadData.THE_DATA_GRID_VIEW.Rows[i].Cells["RemoteName"].Tag.ToString();//名称
                            theReadData.theNumber = theThreadData.THE_DATA_GRID_VIEW.Rows[i].Cells["RemoteValueStr"].Value.ToString();//数值
                            theReadData.theValue = theThreadData.THE_DATA_GRID_VIEW.Rows[i].Cells["LastestModifStr"].Value.ToString();//时标
                            UpdataOrAddNowData(theReadData, "遥信");
                            break;
                        case "遥测":
                            theReadData.theAddress = theThreadData.THE_DEVICE_ADDRESS;
                            theReadData.theDataAddress = theThreadData.THE_DATA_GRID_VIEW.Rows[i].Cells["TRemoteName"].Tag.ToString();//名称
                            theReadData.theNumber = theThreadData.THE_DATA_GRID_VIEW.Rows[i].Cells["TRemoteValueStr"].Value.ToString();//数值
                            theReadData.theValue = theThreadData.THE_DATA_GRID_VIEW.Rows[i].Cells["TLastestModifStr"].Value.ToString();//描述
                            UpdataOrAddNowData(theReadData, "遥测");
                            break;
                        default: break;
                    }
                }
            }
            catch
            {
            }

        }

        public void UpdataOrAddNowData(ReadSqlLiteData theReadData, string name)
        {
            string line = "";
            string sql = "";
            switch (name)
            {
                case "遥测":
                    line = BelongTelemeteringLine(theReadData.theDataAddress);
                    sql = string.Format("SELECT count (*) FROM Telemetering WHERE deviceAddress = \"{0}\" AND telemeteringAddress = \"{1}\"", theReadData.theAddress, theReadData.theDataAddress);
                    if (int.Parse(theSQLiteHelper.ExecuteScalar(sql, null).ToString()) > 0)
                    {
                        sql = string.Format("update Telemetering set numberValue=\"{0}\",theDescribe=\"{1}\" WHERE deviceAddress = \"{2}\" AND telemeteringAddress = \"{3}\"", theReadData.theNumber, theReadData.theValue, theReadData.theAddress, theReadData.theDataAddress);
                    }
                    else
                    {
                        sql = string.Format("insert into Telemetering(deviceAddress,telemeteringAddress,belongLine,numberValue,theDescribe) values (\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\")", theReadData.theAddress, theReadData.theDataAddress, line, theReadData.theNumber, theReadData.theValue);
                    }
                    break;
                case "遥信":
                    line = BelongRemoteLine(theReadData.theDataAddress);
                    sql = string.Format("SELECT count (*) FROM Remote WHERE deviceAddress = \"{0}\" AND remoteAddress = \"{1}\"", theReadData.theAddress, theReadData.theDataAddress);
                    if (int.Parse(theSQLiteHelper.ExecuteScalar(sql, null).ToString()) > 0)
                    {
                        sql = string.Format("update Remote set numberValue=\"{0}\",theCharacteristic=\"{1}\" WHERE deviceAddress = \"{2}\" AND remoteAddress = \"{3}\"", theReadData.theNumber, theReadData.theValue, theReadData.theAddress, theReadData.theDataAddress);
                    }
                    else
                    {
                        sql = string.Format("insert into Remote(deviceAddress,remoteAddress,belongLine,numberValue,theCharacteristic) values (\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\")", theReadData.theAddress, theReadData.theDataAddress, line, theReadData.theNumber, theReadData.theValue);
                    }
                    break;
                default: break;
            }
            theSQLiteHelper.ExecuteNonQuery(sql, null);
        }

        private string BelongTelemeteringLine(string str)
        {
            string line = "";
            str = str.Substring(0, 3);
            switch (str)
            {
                case "401":
                    line = "1";
                    break;
                case "402":
                    line = "2";
                    break;
                case "403":
                    line = "3";
                    break;
                default: break;
            }
            return line;
        }

        private string BelongRemoteLine(string str)
        {
            string line = "";
            int nub = int.Parse(str.Substring(2, 2));
            if (nub <= 37)
            {
                line = "1";
            }
            else if (nub >= 41 && nub <= 67)
            {
                line = "2";
            }
            else if (nub >= 68 && nub <= 97)
            {
                line = "3";
            }
            return line;
        }
        #endregion

        #region  #region 读取SQLLite数据库的遥测数据
        public static List<ReadSqlLiteData> returnRemoteArray(string deviceDataID, string str)
        {
            SQLiteHelper theSQLiteHelper = new SQLiteHelper();
            string sql = string.Format("select * from Remote where deviceAddress=\"{0}\" and belongLine=\"{1}\"", deviceDataID, str);
            List<ReadSqlLiteData> readSqlLiteDataArray = new List<ReadSqlLiteData>();
            DataTable dt = theSQLiteHelper.ExecuteDataTable(sql, null);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ReadSqlLiteData theReadSqlLiteData = new ReadSqlLiteData();
                theReadSqlLiteData.theAddress = dt.Rows[i]["deviceAddress"].ToString();
                theReadSqlLiteData.theDataAddress = dt.Rows[i]["remoteAddress"].ToString();
                theReadSqlLiteData.theBelong = dt.Rows[i]["belongLine"].ToString();
                theReadSqlLiteData.theNumber = dt.Rows[i]["numberValue"].ToString();
                theReadSqlLiteData.theValue = dt.Rows[i]["theCharacteristic"].ToString();
                readSqlLiteDataArray.Add(theReadSqlLiteData);
            }
            return readSqlLiteDataArray;
        }
        #endregion

        #region 读取SQLLite数据库的遥信数据
        public static List<ReadSqlLiteData> returnTelemeteringArray(string deviceDataID, string str)
        {
            SQLiteHelper theSQLiteHelper = new SQLiteHelper();
            string sql = string.Format("select * from Telemetering where deviceAddress=\"{0}\" and belongLine=\"{1}\"", deviceDataID, str);
            List<ReadSqlLiteData> readSqlLiteDataArray = new List<ReadSqlLiteData>();
            DataTable dt = theSQLiteHelper.ExecuteDataTable(sql, null);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                ReadSqlLiteData theReadSqlLiteData = new ReadSqlLiteData();
                theReadSqlLiteData.theAddress = dt.Rows[i]["deviceAddress"].ToString();
                theReadSqlLiteData.theDataAddress = dt.Rows[i]["telemeteringAddress"].ToString();
                theReadSqlLiteData.theBelong = dt.Rows[i]["belongLine"].ToString();
                theReadSqlLiteData.theNumber = dt.Rows[i]["numberValue"].ToString();
                theReadSqlLiteData.theValue = dt.Rows[i]["theDescribe"].ToString();
                readSqlLiteDataArray.Add(theReadSqlLiteData);
            }
            return readSqlLiteDataArray;
        }
        #endregion

        #region 更新SQLLite数据库的遥测/遥信数据
        /// <summary>
        /// 更新SQLLite数据库的遥测/遥信数据
        /// </summary>
        /// <param name="theRemoteData"></param>
        /// <param name="list">设备地址</param>
        /// <param name="name">遥测/遥信</param>
        /// <returns></returns>
        internal ReadSqlLiteData toReadSqlLiteData(TheRemoteData theRemoteData, List<byte> list, string name)
        {
            ReadSqlLiteData theReadSqlLiteData = new ReadSqlLiteData();
            switch (name)
            {
                case "遥测":
                    theReadSqlLiteData.theAddress = ByteWithString.byteToHexStr(list.ToArray());//设备地址
                    theReadSqlLiteData.theDataAddress = ByteWithString.byteToHexStr(theRemoteData.RemoteAddress).ToLower();
                    theReadSqlLiteData.theNumber = ((theRemoteData.RemoteValue[1] << 8) | theRemoteData.RemoteValue[0]).ToString();
                    theReadSqlLiteData.theValue = theRemoteData.quility.ToString("X2").ToLower();
                    break;
                case "遥信":
                    theReadSqlLiteData.theAddress = ByteWithString.byteToHexStr(list.ToArray());//设备地址
                    theReadSqlLiteData.theDataAddress = ByteWithString.byteToHexStr(theRemoteData.RemoteAddress);
                    theReadSqlLiteData.theNumber = ByteWithString.byteToHexStr(theRemoteData.RemoteValue);//值
                    if (theRemoteData.LastestModify != null && theRemoteData.LastestModify.Length > 0)
                    {
                        theReadSqlLiteData.theValue = ByteWithString.ConvertCP56TIME2aToDateTime(theRemoteData.LastestModify).ToString("yyyy-MM-dd HH:mm:ss");
                    }
                    else
                    {
                        theReadSqlLiteData.theValue = "";
                    }
                    break;
                default: break;
            }
            return theReadSqlLiteData;
        }
        #endregion

        #region 转换星期几
        /// <summary>
        /// 转换日期
        /// </summary>
        /// <param name="dayOfWeek"></param>
        /// <returns></returns>
        public string DayOfWeek(string dayOfWeek)
        {
            string theDayOfWeek = string.Empty;
            switch (dayOfWeek)
            {
                case "Monday":
                    theDayOfWeek = "星期一";
                    break;
                case "Tuesday":
                    theDayOfWeek = "星期二";
                    break;
                case "Wednesday":
                    theDayOfWeek = "星期三";
                    break;
                case "Thursday":
                    theDayOfWeek = "星期四";
                    break;
                case "Friday":
                    theDayOfWeek = "星期五";
                    break;
                case "Saturday":
                    theDayOfWeek = "星期六";
                    break;
                case "Sunday":
                    theDayOfWeek = "星期日";
                    break;
                default: break;

            }
            return DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss") + " " + theDayOfWeek;
        }
        #endregion


    }
}
