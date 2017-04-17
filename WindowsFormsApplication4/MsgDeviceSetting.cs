using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 集中器控制客户端.HandleClass;
using 集中器控制客户端.HandleClass.reportModels;

namespace 集中器控制客户端
{
    public partial class MsgDeviceSetting : Form
    {
        private DeviceDataSetting theDeviceDataSetting;
        private HandleTool theScoket = HandleTool.createHandleTool();
        private SettingGrop theSettingGrop = SettingGrop.createSettingGrop();
        public int isAddOrUpdate = -1;
        private byte[] thisId = new byte[4] { 0x00, 0x00,0x00,0x00 };
        public MsgDeviceSetting()
        {
            InitializeComponent();
        }
        public MsgDeviceSetting(DeviceDataSetting theDeviceDataSetting)
        {
            this.theDeviceDataSetting = theDeviceDataSetting;
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            switch (isAddOrUpdate)
            {
                case 0: //add
                    if (HandelControls.Msg("是否确认添加？", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        isAddOrUpdata();
                    }
                    break;
                case 1: //update
                    if (HandelControls.Msg("是否确认修改？", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        isAddOrUpdata();
                    }
                    break;
                default: break;
            }
            this.Close();
        }
        private void isAddOrUpdata()
        {
            string name = textBoxName.Text;
            List<byte> ListArray = new List<byte>();
            byte[] byteArray = ByteWithString.encodeingToByte(name, "utf-8");//名称
            ListArray.AddRange(ByteWithString.intTo4Byte(byteArray.Length));//名称长度
            ListArray.AddRange(byteArray.ToList());//名称
            ListArray.AddRange(ByteWithString.strToToHexByte(textBoxAddress.Text));//地址
            ListArray.AddRange(ByteWithString.intTo4Byte(int.Parse(comboBox1.SelectedValue.ToString())));//A
            ListArray.AddRange(ByteWithString.intTo4Byte(int.Parse(comboBox2.SelectedValue.ToString())));//B
            ListArray.AddRange(ByteWithString.intTo4Byte(int.Parse(comboBox3.SelectedValue.ToString())));//C
            ListArray.AddRange(thisId);//C
            this.theScoket.theSocketSend<byte[]>(0x31, ListArray.ToArray());
        }

        private void SettingValue()
        {
            textBoxAddress.Text = theDeviceDataSetting.DeviceDataID;
            textBoxName.Text = theDeviceDataSetting.DeviceDataName;
            this.comboBox1.SelectedValue = theDeviceDataSetting.RemoteAToID;
            this.comboBox2.SelectedValue = theDeviceDataSetting.RemoteBToID;
            this.comboBox3.SelectedValue = theDeviceDataSetting.RemoteCToID;
            thisId = ByteWithString.intTo4Byte(int.Parse(theDeviceDataSetting.DeviceDataToID));
        }

        private void MsgDeviceSetting_Load(object sender, EventArgs e)
        {
            if (theSettingGrop.TheDetection != null && theSettingGrop.TheDetection.Count > 0)
            {
                this.comboBox1.DataSource = theSettingGrop.TheDetection.ToArray().Clone();
                this.comboBox1.DisplayMember = "DetectionName";
                this.comboBox1.ValueMember = "DetectionID";

                this.comboBox2.DataSource = theSettingGrop.TheDetection.ToArray().Clone();
                this.comboBox2.DisplayMember = "DetectionName";
                this.comboBox2.ValueMember = "DetectionID";

                this.comboBox3.DataSource = theSettingGrop.TheDetection.ToArray().Clone();
                this.comboBox3.DisplayMember = "DetectionName";
                this.comboBox3.ValueMember = "DetectionID";
            }
            //SetComboxItems();
            if (theDeviceDataSetting != null)
            {

                SettingValue();
            }
        }
        private void SetComboxItems()
        {
            this.comboBox1.Items.Add("请选择");
            this.comboBox2.Items.Add("请选择");
            this.comboBox3.Items.Add("请选择");
            this.comboBox1.SelectedItem = this.comboBox2.SelectedItem = this.comboBox3.SelectedItem = "请选择";
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
    }
}
