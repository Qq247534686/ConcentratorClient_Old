using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using 集中器控制客户端.HandleClass.reportModels;

namespace 集中器控制客户端.HandleClass
{
    /// <summary>
    /// 对Socket的发送和接收的数据进行拼接或处理
    /// </summary>
    public class BytesSerialize
    {
        Thread theThreadMethod;
        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] clientReportUnSerialize(ClientReport data)
        {
            List<byte> reportList = new List<byte>();
            try
            {
                reportList.Add(0x03);
                reportList.AddRange(new byte[] { 0x00, 0x00, 0x00, 0x00 });
                reportList.Add(data.CONTROL_FIELD);
                switch (data.CONTROL_FIELD)
                {
                    case 0x01://Login
                        LoginReport lr = (LoginReport)data.REPORT_BODY;
                        reportList.Add(lr.USERNAME_LENGTH);
                        reportList = reportList.Concat(lr.USERNAME).ToList();
                        reportList.Add(lr.PASSWORD_LENGTH);
                        reportList = reportList.Concat(lr.PASSWORD).ToList();
                        break;
                    case 0x04://DataGridView
                        TheCatalog theCatalog = (TheCatalog)data.REPORT_BODY;
                        reportList = reportList.Concat(theCatalog.DEVICE_ADDRESS).ToList();
                        reportList = reportList.Concat(theCatalog.CATALOG_NAME).ToList();
                        break;
                    case 0x06://UpdataFileInfo
                        CatalogFiles theCatalogFiles = (CatalogFiles)data.REPORT_BODY;
                        reportList = reportList.Concat(theCatalogFiles.DeviceAddress).ToList();
                        reportList.Add(theCatalogFiles.FILE_LENGTH);
                        reportList = reportList.Concat(theCatalogFiles.FILE_NAME).ToList();
                        break;
                    case 0x09://RequestRemoteData(请求遥信)
                        reportList = reportList.Concat((byte[])data.REPORT_BODY).ToList();
                        break;
                    case 0x13://设备状态
                        string[] str = (data.REPORT_BODY as string).Split('|');
                        reportList = reportList.Concat(ByteWithString.strToToHexByte(str[0])).ToList();
                        reportList = reportList.Concat(ByteWithString.strToToHexByte(str[1])).ToList();
                        break;
                    case 0x15://
                        ResponseValueData theResponseValueData = (ResponseValueData)data.REPORT_BODY;
                        reportList = reportList.Concat(theResponseValueData.DEVICE_ADDRESS).ToList();
                        reportList.Add(theResponseValueData.VALUE_TYPE);
                        reportList.Add(theResponseValueData.VALUE_COUNT);
                        for (int i = 0; i < theResponseValueData.theResponseValueArray.Count; i++)
                        {
                            string ss = ByteWithString.byteToHexStr(theResponseValueData.theResponseValueArray[i].VALUE_ADDRESS);
                            reportList = reportList.Concat(theResponseValueData.theResponseValueArray[i].VALUE_ADDRESS).ToList();
                            reportList.Add(theResponseValueData.theResponseValueArray[i].VALUE_LENGTH);
                            reportList = reportList.Concat(theResponseValueData.theResponseValueArray[i].VALUE).ToList();
                        }
                        break;
                    case 0x17:
                        reportList = reportList.Concat((data.REPORT_BODY as TheCatalog).CATALOG_NAME).ToList();
                        break;
                    case 0x18:
                        reportList = reportList.Concat((data.REPORT_BODY as TheCatalog).CATALOG_NAME).ToList();
                        break;
                    case 0x19:
                        reportList = reportList.Concat((byte[])data.REPORT_BODY).ToList();
                        break;
                    case 0x20:
                        reportList = reportList.Concat((byte[])data.REPORT_BODY).ToList();
                        break;
                    case 0x22:
                        reportList = reportList.Concat((byte[])data.REPORT_BODY).ToList();
                        break;
                    case 0x27://维护变电站
                        reportList = reportList.Concat((byte[])data.REPORT_BODY).ToList();
                        break;
                    case 0x28://维护变电站
                        reportList = reportList.Concat((byte[])data.REPORT_BODY).ToList();
                        break;
                    case 0x29://维护变电站
                        reportList = reportList.Concat((byte[])data.REPORT_BODY).ToList();
                        break;
                    case 0x30://维护变电站
                        reportList = reportList.Concat((byte[])data.REPORT_BODY).ToList();
                        break;
                    case 0x31://维护变电站
                        reportList = reportList.Concat((byte[])data.REPORT_BODY).ToList();
                        break;
                    case 0x32:
                        reportList = reportList.Concat((byte[])data.REPORT_BODY).ToList();
                        break;
                    case 0x34://获取线路参数
                        reportList = reportList.Concat((byte[])data.REPORT_BODY).ToList();
                        break;
                    case 0x36://设定定时执行参数
                        reportList = reportList.Concat((byte[])data.REPORT_BODY).ToList();
                        break;
                    case 0x37://请求返回设定参数
                        reportList = reportList.Concat((byte[])data.REPORT_BODY).ToList();
                        break;
                    case 0x39://上传文件
                        reportList = reportList.Concat((byte[])data.REPORT_BODY).ToList();
                        break;
                    default: break;
                }
                reportList.Add(0x00);
                reportList.Add(0x16);
                reportList[reportList.Count - 2] = makeValidateField(reportList);
                reportList[1] = Convert.ToByte(reportList.Count >> 24 & 0xff);
                reportList[2] = Convert.ToByte(reportList.Count >> 16 & 0xff);
                reportList[3] = Convert.ToByte(reportList.Count >> 8 & 0xff);
                reportList[4] = Convert.ToByte(reportList.Count & 0xff);

            }
            catch (Exception msg)
            {
                Log.LogWrite(msg);
            }
            return reportList.ToArray<byte>();
        }
        /// <summary>
        /// 接收
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ClientReport clientReportSerialize(byte[] data)
        {
            HandelControls theHandelControls = HandelControls.createHandelControls();
            ClientReport theClientReport = new ClientReport();
            try
            {
                theClientReport.HEAD_FIELD = data[0];
                theClientReport.REPORT_LENGTH = data.Skip(1).Take(4).ToArray();

                byte[] dataLength = data.Skip(1).Take(4).ToArray();
                int dataLengthInt = returnByteLengthRight(dataLength);
                data = data.Skip(0).Take(dataLengthInt).ToArray();
                theClientReport.CONTROL_FIELD = data[5];
                // theClientReport.REPORT_BODY = data.Skip(6).Take(dataLengthInt-8);
                theClientReport.END_FIELD = data[data.Length - 1];
                data = data.Skip(6).Take(dataLengthInt - 8).ToArray();
                int fileCount = 0;//默认文件长度；
                int nowIndex = 0;//当前位置；
                int dataCount = 0;
                TheCatalog theCatalog = new TheCatalog();//文件目录
                CatalogFiles theCatalogFiles = new CatalogFiles();//文件
                TheRemoteDataArray theRemoteDataArray = new TheRemoteDataArray();//遥信
                ResponseValueData theResponseValue = new ResponseValueData();//设定值
                switch (theClientReport.CONTROL_FIELD)
                {
                    case 0x02:
                        returnLoginInfo theReturnLoginInfo = new returnLoginInfo();
                        byte[] theByte = data;
                        theReturnLoginInfo.isOk = theByte[0];
                        theClientReport.REPORT_BODY = theReturnLoginInfo;
                        break;
                    case 0x03:
                        byte[] checkData = data;
                        DeviceData theTreeArrayData = new DeviceData();
                        theTreeArrayData.theDeviceData = new List<DeviceDataInfo>();
                        int count = checkData[0];
                        checkData = checkData.Skip(1).ToArray();
                        while (count > 0)
                        {
                            DeviceDataInfo theDeviceDataInfo = new DeviceDataInfo();
                            int ct = checkData.Skip(nowIndex).Take(1).ToArray()[0];//设备名称长度
                            theDeviceDataInfo.DeviceName = checkData.Skip(nowIndex + 1).Take(ct).ToArray<byte>();
                            theDeviceDataInfo.DeviceAddress = checkData.Skip(nowIndex + ct + 1).Take(2).ToArray<byte>();
                            theDeviceDataInfo.DeviceStatue = checkData.Skip(nowIndex + ct + 3).Take(1).ToArray()[0];
                            theDeviceDataInfo.DeviceLastOnline = checkData.Skip(nowIndex + ct + 4).Take(7).ToArray();
                            nowIndex += ct + 11;
                            theTreeArrayData.theDeviceData.Add(theDeviceDataInfo);
                            count--;
                        }
                        theClientReport.REPORT_BODY = theTreeArrayData;

                        break;
                    case 0x05:
                        byte[] checkCatalogData = data;
                        theCatalog.DEVICE_ADDRESS = checkCatalogData.Take(2).ToArray<byte>();//设备地址
                        theCatalog.FILES_COUNT = checkCatalogData[2];//文件数量
                        fileCount = theCatalog.FILES_COUNT;
                        theCatalog.THE_CATALOG_FILES = new List<CatalogFiles>();
                        byte[] nowCatalogData = checkCatalogData.Skip(3).Take(checkCatalogData.Length - 2).ToArray<byte>();
                        while (fileCount > 0)
                        {
                            CatalogFiles catalogFiles = new CatalogFiles();
                            catalogFiles.FILE_LENGTH = nowCatalogData[nowIndex];
                            catalogFiles.FILE_NAME = nowCatalogData.Skip(nowIndex + 1).Take(catalogFiles.FILE_LENGTH).ToArray<byte>();
                            theCatalog.THE_CATALOG_FILES.Add(catalogFiles);
                            nowIndex += catalogFiles.FILE_LENGTH + 1;
                            fileCount--;
                        }
                        theClientReport.REPORT_BODY = theCatalog;
                        break;
                    case 0x07:
                        byte[] updateCatalogData = data;
                        theCatalogFiles.DeviceAddress = updateCatalogData.Take(2).ToArray<byte>();//文件地址
                        theCatalogFiles.FILE_LENGTH = updateCatalogData[2];//文件名长度
                        theCatalogFiles.FILE_NAME = updateCatalogData.Skip(3).Take(theCatalogFiles.FILE_LENGTH).ToArray<byte>();//文件名
                        int fileContent = theCatalogFiles.FILE_NAME.Length + 3;
                        theCatalogFiles.FILE_CONTENT = updateCatalogData.Skip(fileContent).ToArray<byte>();//文件长度
                        theClientReport.REPORT_BODY = theCatalogFiles;
                        break;
                    case 0x10://遥信返回（不带时标）
                        byte[] notHaveTimeMark = data;
                        theRemoteDataArray.DEVICE_ADDRESS = notHaveTimeMark.Take(2).ToArray<byte>();
                        theRemoteDataArray.THE_COUNT = notHaveTimeMark[2];
                        theRemoteDataArray.THE_TYPE_NAME = "遥信";
                        fileCount = theRemoteDataArray.THE_COUNT;
                        theRemoteDataArray.THE_REMOTE_DATA = new List<TheRemoteData>();
                        notHaveTimeMark = notHaveTimeMark.Skip(3).ToArray<byte>();
                        while (fileCount > 0)
                        {
                            TheRemoteData theRemoteData = new TheRemoteData();
                            theRemoteData.RemoteAddress = notHaveTimeMark.Skip(nowIndex).Take(2).ToArray<byte>();
                            theRemoteData.RemoteValue = notHaveTimeMark.Skip(nowIndex + 2).Take(1).ToArray<byte>();
                            nowIndex = nowIndex + 3;
                            fileCount--;
                            theRemoteDataArray.THE_REMOTE_DATA.Add(theRemoteData);
                        }
                        theClientReport.REPORT_BODY = theRemoteDataArray;
                        theThreadMethod = new Thread(forEachList);
                        theThreadMethod.IsBackground = true;
                        theThreadMethod.Start(theRemoteDataArray);
                        break;
                    case 0x11://遥信返回（带时标）
                        byte[] haveTimeMark = data;
                        theRemoteDataArray.DEVICE_ADDRESS = haveTimeMark.Take(2).ToArray<byte>();
                        theRemoteDataArray.THE_COUNT = haveTimeMark[2];
                        theRemoteDataArray.THE_COUNT = haveTimeMark[2];
                        theRemoteDataArray.THE_TYPE_NAME = "遥信";
                        fileCount = theRemoteDataArray.THE_COUNT;
                        theRemoteDataArray.THE_REMOTE_DATA = new List<TheRemoteData>();
                        haveTimeMark = haveTimeMark.Skip(3).ToArray<byte>();
                        while (fileCount > 0)
                        {
                            TheRemoteData theRemoteData = new TheRemoteData();
                            theRemoteData.RemoteAddress = haveTimeMark.Skip(nowIndex).Take(2).ToArray<byte>();
                            theRemoteData.RemoteValue = haveTimeMark.Skip(nowIndex + 2).Take(1).ToArray<byte>();
                            theRemoteData.LastestModify = haveTimeMark.Skip(nowIndex + 3).Take(7).ToArray<byte>();
                            nowIndex = nowIndex + 10;
                            fileCount--;
                            theRemoteDataArray.THE_REMOTE_DATA.Add(theRemoteData);
                        }
                        theClientReport.REPORT_BODY = theRemoteDataArray;
                        theThreadMethod = new Thread(forEachList);
                        theThreadMethod.IsBackground = true;
                        theThreadMethod.Start(theRemoteDataArray);
                        break;
                    case 0x12://遥测
                        byte[] telemeteringTimeMark = data;
                        theRemoteDataArray.DEVICE_ADDRESS = telemeteringTimeMark.Take(2).ToArray<byte>();
                        theRemoteDataArray.THE_COUNT = telemeteringTimeMark[2];
                        theRemoteDataArray.THE_TYPE_NAME = "遥测";
                        fileCount = theRemoteDataArray.THE_COUNT;
                        theRemoteDataArray.THE_REMOTE_DATA = new List<TheRemoteData>();
                        telemeteringTimeMark = telemeteringTimeMark.Skip(3).ToArray<byte>();
                        while (fileCount > 0)
                        {
                            TheRemoteData theRemoteData = new TheRemoteData();
                            theRemoteData.LastestModify = new byte[] { 0x00 };
                            theRemoteData.RemoteAddress = telemeteringTimeMark.Skip(nowIndex).Take(2).ToArray<byte>();
                            theRemoteData.RemoteValue = telemeteringTimeMark.Skip(nowIndex + 2).Take(2).ToArray<byte>();
                            theRemoteData.quility = telemeteringTimeMark.Skip(nowIndex + 4).Take(1).ToArray<byte>()[0];
                            nowIndex = nowIndex + 5;
                            fileCount--;
                            theRemoteDataArray.THE_REMOTE_DATA.Add(theRemoteData);
                        }
                        theClientReport.REPORT_BODY = theRemoteDataArray;
                        theThreadMethod = new Thread(forEachList);
                        theThreadMethod.IsBackground = true;
                        theThreadMethod.Start(theRemoteDataArray);
                        break;
                    case 0x14:
                        byte[] responseValueTimeMark = data;
                        theResponseValue.DEVICE_ADDRESS = responseValueTimeMark.Take(2).ToArray<byte>();
                        theResponseValue.VALUE_TYPE = responseValueTimeMark[2];
                        theResponseValue.VALUE_COUNT = responseValueTimeMark[3];
                        fileCount = theResponseValue.VALUE_COUNT;
                        theResponseValue.theResponseValueArray = new List<ResponseValue>();
                        responseValueTimeMark = responseValueTimeMark.Skip(4).ToArray<byte>();
                        while (fileCount > 0)
                        {
                            ResponseValue rsponseValue = new ResponseValue();
                            rsponseValue.VALUE_ADDRESS = responseValueTimeMark.Skip(nowIndex).Take(2).ToArray<byte>();
                            int length = responseValueTimeMark.Skip(nowIndex + 2).Take(1).ToArray<byte>()[0];
                            rsponseValue.VALUE = responseValueTimeMark.Skip(nowIndex + 3).Take(length).ToArray<byte>();
                            nowIndex = nowIndex + 3 + length;
                            theResponseValue.theResponseValueArray.Add(rsponseValue);
                            fileCount--;
                        }
                        theClientReport.REPORT_BODY = theResponseValue;
                        break;
                    case 0x0a:
                        byte[] contentText = data;
                        ReportContentText theReportContent = new ReportContentText();
                        theReportContent.ReportType = contentText[0];
                        theReportContent.ReportDateTime = contentText.Skip(1).Take(7).ToArray<byte>();
                        theReportContent.ReportContent = contentText.Skip(8).ToArray<byte>();
                        theClientReport.REPORT_BODY = theReportContent;
                        break;
                    case 0x21:
                        byte[] writeBytes = data;
                        WriteFile theWriteFile = new WriteFile();
                        theWriteFile.DEVICE_ADDRESS = writeBytes.Take(2).ToArray();
                        theWriteFile.FILE_LENGTH = writeBytes.Skip(2).Take(1).ToArray()[0];
                        theWriteFile.FILE_NAME = writeBytes.Skip(3).Take(theWriteFile.FILE_LENGTH).ToArray();
                        theWriteFile.FILE_COUNT = writeBytes.Skip(3 + theWriteFile.FILE_LENGTH).ToArray();
                        break;
                    case 0x23://接收变电站
                        List<ConvertingStation> theConvertingStationArray = new List<ConvertingStation>();
                        dataCount = returnByteLengthRight(data.Take(4).ToArray());
                        data = data.Skip(4).ToArray();
                        while (dataCount > 0)
                        {
                            ConvertingStation theConvertingStation = new ConvertingStation();
                            int convertingNameLength = returnByteLengthRight(data.Skip(nowIndex).Take(4).ToArray());
                            nowIndex += 4;
                            theConvertingStation.stationName = ByteWithString.bytetoCodeString(data.Skip(nowIndex).Take(convertingNameLength).ToArray(), "utf-8");
                            nowIndex += convertingNameLength;
                            int remarksLength = returnByteLengthRight(data.Skip(nowIndex).Take(4).ToArray());
                            nowIndex += 4;
                            theConvertingStation.stationRemarks = ByteWithString.bytetoCodeString(data.Skip(nowIndex).Take(remarksLength).ToArray(), "utf-8");
                            nowIndex += remarksLength;
                            theConvertingStation.stationID = returnByteLengthRight(data.Skip(nowIndex).Take(4).ToArray()).ToString();
                            nowIndex += 4;
                            dataCount--;
                            theConvertingStationArray.Add(theConvertingStation);
                        }
                        theClientReport.REPORT_BODY = theConvertingStationArray;
                        break;
                    case 0x24://接收母线
                        List<Generatrix> theGeneratrixArray = new List<Generatrix>();
                        dataCount = returnByteLengthRight(data.Take(4).ToArray());
                        data = data.Skip(4).ToArray();
                        while (dataCount > 0)
                        {
                            Generatrix theGeneratrix = new Generatrix();
                            int convertingNameLength = returnByteLengthRight(data.Skip(nowIndex).Take(4).ToArray());
                            nowIndex += 4;
                            theGeneratrix.GeneratrixName = ByteWithString.bytetoCodeString(data.Skip(nowIndex).Take(convertingNameLength).ToArray(), "utf-8");
                            nowIndex += convertingNameLength;
                            int remarksLength = returnByteLengthRight(data.Skip(nowIndex).Take(4).ToArray());
                            nowIndex += 4;
                            theGeneratrix.GeneratrixNumber = ByteWithString.bytetoCodeString(data.Skip(nowIndex).Take(remarksLength).ToArray(), "utf-8");
                            nowIndex += remarksLength;
                            theGeneratrix.GeneratrixID = returnByteLengthRight(data.Skip(nowIndex).Take(4).ToArray()).ToString();
                            nowIndex += 4;
                            int convertingLength = returnByteLengthRight(data.Skip(nowIndex).Take(4).ToArray());
                            nowIndex += 4;
                            theGeneratrix.GeneratrixToName = ByteWithString.bytetoCodeString(data.Skip(nowIndex).Take(convertingLength).ToArray(), "utf-8");
                            nowIndex += convertingLength;
                            theGeneratrix.GeneratrixToID = returnByteLengthRight(data.Skip(nowIndex).Take(4).ToArray()).ToString();
                            nowIndex += 4;
                            dataCount--;
                            theGeneratrixArray.Add(theGeneratrix);

                        }
                        theClientReport.REPORT_BODY = theGeneratrixArray;
                        break;
                    case 0x25://接收线路
                        List<Circuit> theCircuitArray = new List<Circuit>();
                        dataCount = returnByteLengthRight(data.Take(4).ToArray());
                        data = data.Skip(4).ToArray();
                        while (dataCount > 0)
                        {
                            Circuit theCircuit = new Circuit();
                            int convertingNameLength = returnByteLengthRight(data.Skip(nowIndex).Take(4).ToArray());
                            nowIndex += 4;
                            theCircuit.CircuitName = ByteWithString.bytetoCodeString(data.Skip(nowIndex).Take(convertingNameLength).ToArray(), "utf-8");
                            nowIndex += convertingNameLength;
                            int remarksLength = returnByteLengthRight(data.Skip(nowIndex).Take(4).ToArray());
                            nowIndex += 4;
                            theCircuit.CircuitNumber = ByteWithString.bytetoCodeString(data.Skip(nowIndex).Take(remarksLength).ToArray(), "utf-8");
                            nowIndex += remarksLength;
                            theCircuit.CircuitID = returnByteLengthRight(data.Skip(nowIndex).Take(4).ToArray()).ToString();
                            nowIndex += 4;
                            int convertingLength = returnByteLengthRight(data.Skip(nowIndex).Take(4).ToArray());
                            nowIndex += 4;
                            theCircuit.CircuitToName = ByteWithString.bytetoCodeString(data.Skip(nowIndex).Take(convertingLength).ToArray(), "utf-8");
                            nowIndex += convertingLength;
                            theCircuit.CircuitToID = returnByteLengthRight(data.Skip(nowIndex).Take(4).ToArray()).ToString();
                            nowIndex += 4;
                            dataCount--;
                            theCircuitArray.Add(theCircuit);

                        }
                        theClientReport.REPORT_BODY = theCircuitArray;
                        break;
                    case 0x26://接收检测点
                        List<Detection> theDetectionArray = new List<Detection>();
                        dataCount = returnByteLengthRight(data.Take(4).ToArray());
                        data = data.Skip(4).ToArray();
                        while (dataCount > 0)
                        {
                            Detection theDetection = new Detection();
                            int convertingNameLength = returnByteLengthRight(data.Skip(nowIndex).Take(4).ToArray());
                            nowIndex += 4;
                            theDetection.DetectionName = ByteWithString.bytetoCodeString(data.Skip(nowIndex).Take(convertingNameLength).ToArray(), "utf-8");
                            nowIndex += convertingNameLength;
                            int theNumber = returnByteLengthRight(data.Skip(nowIndex).Take(4).ToArray());
                            nowIndex += 4;
                            theDetection.DetectionNumber = ByteWithString.bytetoCodeString(data.Skip(nowIndex).Take(theNumber).ToArray(), "utf-8");
                            nowIndex += theNumber;
                            theDetection.DetectionID = returnByteLengthRight(data.Skip(nowIndex).Take(4).ToArray()).ToString();
                            nowIndex += 4;
                            int detectionToSecondName = returnByteLengthRight(data.Skip(nowIndex).Take(4).ToArray());
                            nowIndex += 4;
                            theDetection.DetectionToSecondName = ByteWithString.bytetoCodeString(data.Skip(nowIndex).Take(detectionToSecondName).ToArray(), "utf-8");
                            nowIndex += detectionToSecondName;
                            theDetection.DetectionToSecondID = returnByteLengthRight(data.Skip(nowIndex).Take(4).ToArray()).ToString();
                            nowIndex += 4;
                            theDetection.RemoteA = ByteWithString.byteToHexStr(data.Skip(nowIndex).Take(2).ToArray());
                            nowIndex += 2;
                            theDetection.RemoteB = ByteWithString.byteToHexStr(data.Skip(nowIndex).Take(2).ToArray());
                            nowIndex += 2;
                            theDetection.RemoteC = ByteWithString.byteToHexStr(data.Skip(nowIndex).Take(2).ToArray());
                            nowIndex += 2;
                            int circuitLength = returnByteLengthRight(data.Skip(nowIndex).Take(4).ToArray());
                            nowIndex += 4;
                            theDetection.DetectionToName = ByteWithString.bytetoCodeString(data.Skip(nowIndex).Take(circuitLength).ToArray(), "utf-8");
                            nowIndex += circuitLength;
                            theDetection.DetectionToID = returnByteLengthRight(data.Skip(nowIndex).Take(4).ToArray()).ToString();
                            nowIndex += 4;
                            dataCount--;
                            theDetectionArray.Add(theDetection);
                        }
                        theClientReport.REPORT_BODY = theDetectionArray;
                        break;
                    case 0x33:
                        List<DeviceDataSetting> theDeviceDataArray = new List<DeviceDataSetting>();
                        dataCount = returnByteLengthRight(data.Take(4).ToArray());
                        data = data.Skip(4).ToArray();
                        while (dataCount > 0)
                        {
                            DeviceDataSetting theDetection = new DeviceDataSetting();
                            int convertingNameLength = returnByteLengthRight(data.Skip(nowIndex).Take(4).ToArray());
                            nowIndex += 4;
                            theDetection.DeviceDataName = ByteWithString.bytetoCodeString(data.Skip(nowIndex).Take(convertingNameLength).ToArray(), "utf-8");//ok
                            nowIndex += convertingNameLength;
                            theDetection.DeviceDataID = ByteWithString.byteToHexStr(data.Skip(nowIndex).Take(2).ToArray()).ToString();
                            nowIndex += 2;
                            theDetection.LastDateTime = ByteWithString.ConvertCP56TIME2aToDateTime(data.Skip(nowIndex).Take(7).ToArray()).ToString("yyyy-MM-dd HH:mm:ss");
                            nowIndex += 7;
                            int ALength = returnByteLengthRight(data.Skip(nowIndex).Take(4).ToArray());
                            nowIndex += 4;
                            theDetection.TheRemoteA = ByteWithString.bytetoCodeString(data.Skip(nowIndex).Take(ALength).ToArray(), "utf-8");
                            nowIndex += ALength;
                            theDetection.RemoteAToID = returnByteLengthRight(data.Skip(nowIndex).Take(4).ToArray()).ToString();
                            nowIndex += 4;
                            int BLength = returnByteLengthRight(data.Skip(nowIndex).Take(4).ToArray());
                            nowIndex += 4;
                            theDetection.TheRemoteB = ByteWithString.bytetoCodeString(data.Skip(nowIndex).Take(BLength).ToArray(), "utf-8");
                            nowIndex += BLength;
                            theDetection.RemoteBToID = returnByteLengthRight(data.Skip(nowIndex).Take(4).ToArray()).ToString();
                            nowIndex += 4;
                            int CLength = returnByteLengthRight(data.Skip(nowIndex).Take(4).ToArray());
                            nowIndex += 4;
                            theDetection.TheRemoteC = ByteWithString.bytetoCodeString(data.Skip(nowIndex).Take(CLength).ToArray(), "utf-8");
                            nowIndex += CLength;
                            theDetection.RemoteCToID = returnByteLengthRight(data.Skip(nowIndex).Take(4).ToArray()).ToString();
                            nowIndex += 4;
                            theDetection.DeviceDataToID = returnByteLengthRight(data.Skip(nowIndex).Take(4).ToArray()).ToString();
                            nowIndex += 4;
                            dataCount--;
                            theDeviceDataArray.Add(theDetection);
                        }
                        theClientReport.REPORT_BODY = theDeviceDataArray;
                        break;
                    case 0x35:
                        List<int> valueBuilder=new List<int>();
                        valueBuilder.Add(returnByteLengthRight(data.Skip(0).Take(4).ToArray()));
                        valueBuilder.Add(returnByteLengthRight(data.Skip(4).Take(4).ToArray()));
                        valueBuilder.Add(returnByteLengthRight(data.Skip(8).Take(4).ToArray()));
                        theClientReport.REPORT_BODY = valueBuilder;
                        break;
                    case 0x38:
                        Hitch theHitch = new Hitch();
                        theHitch.COUNT=dataCount = returnByteLengthRight(data.Take(4).ToArray());
                        theHitch.NOW_ID = new List<int>();
                        data = data.Skip(4).ToArray();
                        while (dataCount > 0)
                        {
                            theHitch.NOW_ID.Add(returnByteLengthRight(data.Skip(nowIndex).Take(4).ToArray()));
                            nowIndex += 4;
                            dataCount--;
                        }
                        theClientReport.REPORT_BODY = theHitch;
                        break;
                    default :break;
                }
                theClientReport.VALIDATE_FIELD = data.Skip(data.Length - 2).Take(1).ToArray()[0];
                theClientReport.END_FIELD = 0x16;
            }
            catch (Exception msg)
            {
                Log.LogWrite(msg);
            }
            return theClientReport;
        }
        /// <summary>
        /// 生成校验码(可重写)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected virtual byte makeValidateField(List<byte> data)
        {

            byte vali = data[1];
            for (int i = 0; i < data.Count - 2; i++)
            {
                vali += data[i];
            }
            return vali;
        }
        public static int returnByteLengthRight(byte[] bytes)
        {
            int count = 0;
            if (bytes.Length == 4)
            {
                count = bytes[0] << 24 | bytes[1] << 16 | bytes[2] << 8 | bytes[3];
            }
            return count;
        }
        public static int returnByteLengthRightLeft(byte[] bytes)
        {
            int count = 0;
            if (bytes.Length == 4)
            {
                count = bytes[3] << 24 | bytes[2] << 16 | bytes[1] << 8 | bytes[0];
            }
            return count;
        }
        private void forEachList(object obj)
        {
            HandelControls theHandelControls = HandelControls.createHandelControls();
            TheRemoteDataArray theRemoteDataArray = obj as TheRemoteDataArray;
            for (int i = 0; i < theRemoteDataArray.THE_REMOTE_DATA.Count; i++)
            {
                TheRemoteData theRemoteData = new TheRemoteData();
                theRemoteData.RemoteAddress = theRemoteDataArray.THE_REMOTE_DATA[i].RemoteAddress;
                theRemoteData.RemoteValue = theRemoteDataArray.THE_REMOTE_DATA[i].RemoteValue;
                theRemoteData.quility = theRemoteDataArray.THE_REMOTE_DATA[i].quility;
                theRemoteData.LastestModify = theRemoteDataArray.THE_REMOTE_DATA[i].LastestModify;
                theHandelControls.UpdataOrAddNowData(theHandelControls.toReadSqlLiteData(theRemoteData, theRemoteDataArray.DEVICE_ADDRESS.ToList(), theRemoteDataArray.THE_TYPE_NAME), theRemoteDataArray.THE_TYPE_NAME);//遥信存入数据库
            }
           
        }
    }
}
