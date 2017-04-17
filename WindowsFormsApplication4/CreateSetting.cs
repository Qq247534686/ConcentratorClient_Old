using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using 集中器控制客户端.HandleClass;
using 集中器控制客户端.HandleClass.reportModels;
using 集中器控制客户端.Properties;

namespace 集中器控制客户端
{
    public partial class CreateSetting : Form
    {
        public CreateSetting()
        {
            InitializeComponent();
        }
        private SettingGrop theSettingGrop = SettingGrop.createSettingGrop();
        private HandleTool theScoket = HandleTool.createHandleTool();
        private HandelControls theHandelControls = HandelControls.createHandelControls();
        private void CreateSetting_Load(object sender, EventArgs e)
        {
            this.Icon = Resources.line;
        }
       
        

        #region 将数据绑定到指定表格
        //绑定变电站
        public void bindConvertingStation(List<ConvertingStation> theConvertingStationArray)
        {
            if (ValidateData.ArrayIsNullOrZero<ConvertingStation>(theConvertingStationArray))
            {
                ConvertingStationView.DataSource = theConvertingStationArray;
            }

        }
        //绑定母线
        public void bindGeneratrix(List<Generatrix> theGeneratrixArray)
        {
            if (ValidateData.ArrayIsNullOrZero<Generatrix>(theGeneratrixArray))
            {
                GeneratrixView.DataSource = theGeneratrixArray;
            }

        }
        //绑定变线路
        public void bindCircuit(List<Circuit> theCircuitArray)
        {
            if (ValidateData.ArrayIsNullOrZero<Circuit>(theCircuitArray))
            {
                CircuitView.DataSource = theCircuitArray;
            }

        }
        //绑定变检测点
        public void bindDetection(List<Detection> theDetectionArray)
        {
            if (ValidateData.ArrayIsNullOrZero<Detection>(theDetectionArray))
            {
                DetectionView.DataSource = theDetectionArray;
            }

        }
        //绑定设备
        public void bindDeviceDataSetting(List<DeviceDataSetting> theDetectionArray)
        {
            if (ValidateData.ArrayIsNullOrZero<DeviceDataSetting>(theDetectionArray))
            {
                DeviceDataView.DataSource = theDetectionArray;
            }

        }
        #endregion

        #region 添加
        //添加变电站
        private void button3_Click(object sender, EventArgs e)
        {
            MsgConvertingStation theConvertingStationHandel = new MsgConvertingStation();
            theConvertingStationHandel.Text = "添加变电站";
            theConvertingStationHandel.isAddOrUpdate = 0;
            theConvertingStationHandel.ShowDialog();
        }
       
        //添加母线
        private void button8_Click(object sender, EventArgs e)
        {
            MsgGeneratrix theGeneratrixHandel = new MsgGeneratrix();
            theGeneratrixHandel.Text = "添加母线";
            theGeneratrixHandel.isAddOrUpdate = 0;
            theGeneratrixHandel.ShowDialog();
        }
      
        //添加线路
        private void button11_Click(object sender, EventArgs e)
        {
            MsgCircuit theMsgCircuit = new MsgCircuit();
            theMsgCircuit.Text = "添加路线";
            theMsgCircuit.isAddOrUpdate = 0;
            theMsgCircuit.ShowDialog();
        }
      
        //添加检测点
        private void button14_Click(object sender, EventArgs e)
        {
            MsgDetection theMsgDetection = new MsgDetection();
            theMsgDetection.Text = "添加检测点";
            theMsgDetection.isAddOrUpdate = 0;
            theMsgDetection.ShowDialog();
        }

        //添加设备
        private void button16_Click(object sender, EventArgs e)
        {
            MsgDeviceSetting theMsgDeviceSetting = new MsgDeviceSetting();
            theMsgDeviceSetting.Text = "添加设备";
            theMsgDeviceSetting.isAddOrUpdate = 0;
            theMsgDeviceSetting.ShowDialog();
        }
        #endregion

        #region 修改

        //修改变电站
        private void button4_Click(object sender, EventArgs e)
        {
            if (ConvertingStationView.SelectedRows.Count > 0 && ValidateData.CellIsNullOrZero(ConvertingStationView.SelectedRows[0].Cells["stationID"]))
            {
                ConvertingStation content = new ConvertingStation()
                {
                    stationName = ConvertingStationView.SelectedRows[0].Cells["stationName"].Value.ToString(),
                    stationID = ConvertingStationView.SelectedRows[0].Cells["stationID"].Value.ToString(),
                    stationRemarks = ConvertingStationView.SelectedRows[0].Cells["stationRemarks"].Value.ToString()
                };
                MsgConvertingStation theConvertingStationHandel = new MsgConvertingStation(content);
                theConvertingStationHandel.Text = "修改变电站";
                theConvertingStationHandel.isAddOrUpdate = 1;
                theConvertingStationHandel.ShowDialog();
            }
            else
            {
                HandelControls.Msg("请选择要修改的数据", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //修改母线
        private void button7_Click(object sender, EventArgs e)
        {
            if (GeneratrixView.SelectedRows.Count > 0 && ValidateData.CellIsNullOrZero(GeneratrixView.SelectedRows[0].Cells["GeneratrixID"]))
            {
                Generatrix theGeneratrix = new Generatrix();
                theGeneratrix.GeneratrixName = GeneratrixView.SelectedRows[0].Cells["GeneratrixName"].Value.ToString();
                theGeneratrix.GeneratrixNumber = GeneratrixView.SelectedRows[0].Cells["GeneratrixNumber"].Value.ToString();
                theGeneratrix.GeneratrixID = GeneratrixView.SelectedRows[0].Cells["GeneratrixID"].Value.ToString();
                theGeneratrix.GeneratrixToID = GeneratrixView.SelectedRows[0].Cells["GeneratrixToID"].Value.ToString();
                MsgGeneratrix theGeneratrixHandel = new MsgGeneratrix(theGeneratrix);
                theGeneratrixHandel.Text = "修改变母线";
                theGeneratrixHandel.isAddOrUpdate = 1;
                theGeneratrixHandel.ShowDialog();
            }
            else
            {
                HandelControls.Msg("请选择要修改的数据", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //修改线路
        private void button10_Click(object sender, EventArgs e)
        {
            if (CircuitView.SelectedRows.Count > 0 && ValidateData.CellIsNullOrZero(CircuitView.SelectedRows[0].Cells["CircuitID"]))
            {
                Circuit theCircuit = new Circuit();
                theCircuit.CircuitName = CircuitView.SelectedRows[0].Cells["CircuitName"].Value.ToString();
                theCircuit.CircuitID = CircuitView.SelectedRows[0].Cells["CircuitID"].Value.ToString();
                theCircuit.CircuitNumber = CircuitView.SelectedRows[0].Cells["CircuitNumber"].Value.ToString();
                theCircuit.CircuitToID = CircuitView.SelectedRows[0].Cells["CircuitToID"].Value.ToString();
                MsgCircuit theMsgCircuit = new MsgCircuit(theCircuit);
                theMsgCircuit.Text = "修改线路";
                theMsgCircuit.isAddOrUpdate = 1;
                theMsgCircuit.ShowDialog();
            }
            else
            {
                HandelControls.Msg("请选择要修改的数据", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //修改检测点
        private void button13_Click(object sender, EventArgs e)
        {
            if (DetectionView.SelectedRows.Count > 0 && ValidateData.CellIsNullOrZero(DetectionView.SelectedRows[0].Cells["DetectionID"]))
            {
                Detection theDetection = new Detection();
                theDetection.DetectionID = DetectionView.SelectedRows[0].Cells["DetectionID"].Value.ToString();
                theDetection.DetectionName = DetectionView.SelectedRows[0].Cells["DetectionName"].Value.ToString();
                theDetection.DetectionNumber = DetectionView.SelectedRows[0].Cells["DetectionNumber"].Value.ToString();
                theDetection.DetectionToSecondID = DetectionView.SelectedRows[0].Cells["DetectionToSecondID"].Value.ToString();
                theDetection.RemoteA = DetectionView.SelectedRows[0].Cells["RemoteA"].Value.ToString();
                theDetection.RemoteB = DetectionView.SelectedRows[0].Cells["RemoteB"].Value.ToString();
                theDetection.RemoteC = DetectionView.SelectedRows[0].Cells["RemoteC"].Value.ToString();
                theDetection.DetectionToID = DetectionView.SelectedRows[0].Cells["DetectionToID"].Value.ToString();
                theDetection.DetectionToName = DetectionView.SelectedRows[0].Cells["DetectionToName"].Value.ToString();
                MsgDetection theMsgDetection = new MsgDetection(theDetection);
                theMsgDetection.Text = "修改检测点";
                theMsgDetection.isAddOrUpdate = 1;
                theMsgDetection.ShowDialog();
            }
            else
            {
                HandelControls.Msg("请选择要修改的数据", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //修改设备
        private void button15_Click(object sender, EventArgs e)
        {
            if (DeviceDataView.SelectedRows.Count > 0 && ValidateData.CellIsNullOrZero(DeviceDataView.SelectedRows[0].Cells["DeviceDataID"]))
            {
                DeviceDataSetting theDeviceDataSetting = new DeviceDataSetting();
                theDeviceDataSetting.DeviceDataName = DeviceDataView.SelectedRows[0].Cells["DeviceDataName"].Value.ToString();
                theDeviceDataSetting.DeviceDataID = DeviceDataView.SelectedRows[0].Cells["DeviceDataID"].Value.ToString();
                theDeviceDataSetting.RemoteAToID = DeviceDataView.SelectedRows[0].Cells["RemoteAToID"].Value.ToString();
                theDeviceDataSetting.RemoteBToID = DeviceDataView.SelectedRows[0].Cells["RemoteBToID"].Value.ToString();
                theDeviceDataSetting.RemoteCToID = DeviceDataView.SelectedRows[0].Cells["RemoteCToID"].Value.ToString();
                theDeviceDataSetting.DeviceDataToID = DeviceDataView.SelectedRows[0].Cells["DeviceDataToID"].Value.ToString();
                MsgDeviceSetting theMsgDeviceSetting = new MsgDeviceSetting(theDeviceDataSetting);
                theMsgDeviceSetting.Text = "修改设备";
                theMsgDeviceSetting.isAddOrUpdate = 1;
                theMsgDeviceSetting.ShowDialog();
            }
            else
            {
                HandelControls.Msg("请选择要修改的数据", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region 删除
        //删除变电站
        private void delete_ConvertingStationView_Click(object sender, EventArgs e)
        {
            theHandelControls.deleteSelectRow(ConvertingStationView, "stationID", 0x23, "是否确认删除此变电站");
        }
        //删除母线
        private void button5_Click(object sender, EventArgs e)
        {
            theHandelControls.deleteSelectRow(GeneratrixView, "GeneratrixID", 0x24, "是否确认删除此母线");
        }
        //删除线路
        private void button9_Click(object sender, EventArgs e)
        {
            theHandelControls.deleteSelectRow(CircuitView, "CircuitID", 0x25, "是否确认删除此线路");
        }
        //删除检测点
        private void button12_Click(object sender, EventArgs e)
        {
            theHandelControls.deleteSelectRow(DetectionView, "DetectionID", 0x26, "是否确认删除此检测点");
        }
        //删除设备
        private void button6_Click(object sender, EventArgs e)
        {
            theHandelControls.deleteSelectRow(DeviceDataView, "DeviceDataToID", 0x33, "是否确认删除此设备");
        }
        #endregion

        #region 刷新：0x23,0x24,0x25,0x26,0x33,0x00
        //刷新变电站
        private void button17_Click(object sender, EventArgs e)
        {
            byte[] bytes = new byte[] { 0x23 };
            theScoket.theSocketSend<byte[]>(0x34, bytes.ToArray());
        }
        //刷新母线
        private void button18_Click(object sender, EventArgs e)
        {
            byte[] bytes = new byte[] { 0x24 };
            theScoket.theSocketSend<byte[]>(0x34, bytes.ToArray());
        }
        //刷新线路
        private void button19_Click(object sender, EventArgs e)
        {
            byte[] bytes = new byte[] { 0x25 };
            theScoket.theSocketSend<byte[]>(0x34, bytes.ToArray());
        }
        //刷新检测点
        private void button20_Click(object sender, EventArgs e)
        {
            byte[] bytes = new byte[] { 0x26 };
            theScoket.theSocketSend<byte[]>(0x34, bytes.ToArray());
        }
        //刷新设备
        private void button21_Click(object sender, EventArgs e)
        {
            byte[] bytes = new byte[] { 0x33 };
            theScoket.theSocketSend<byte[]>(0x34, bytes.ToArray());
        }
        #endregion

        private void button25_Click(object sender, EventArgs e)
        {
            theScoket.theSocketSend<byte[]>(0x37, null);
        }

        private void button26_Click(object sender, EventArgs e)
        {
            theScoket.theSocketSend<byte[]>(0x36, GetParameterCount().ToArray());
        }
        private List<byte> GetParameterCount()
        {
            List<byte> listArray = new List<byte>();
            listArray = listArray.Concat(ByteWithString.intTo4Byte(int.Parse(timerSummonInterval.Value.ToString()))).ToList();
            listArray = listArray.Concat(ByteWithString.intTo4Byte(int.Parse(timerTelemetryInterval.Value.ToString()))).ToList();
            listArray = listArray.Concat(ByteWithString.intTo4Byte(int.Parse(timerHeartbeatInterval.Value.ToString()))).ToList();
            return listArray;
        }
        public void bindTimeSetting(List<int> obj)
        {
            timerSummonInterval.Value = (obj[0] <=0 ? 0 : obj[0]);
            timerTelemetryInterval.Value = (obj[1] <= 0 ? 0 : obj[1]);
            timerHeartbeatInterval.Value = (obj[2] <= 0 ? 0 : obj[2]);
        }
    }
}
