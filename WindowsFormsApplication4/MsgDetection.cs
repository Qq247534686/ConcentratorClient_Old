using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using 集中器控制客户端.HandleClass;
using 集中器控制客户端.HandleClass.reportModels;

namespace 集中器控制客户端
{
    public partial class MsgDetection : MsgTemplet
    {
        private Detection theDetection;
        public int isAddOrUpdate = -1;
        private byte[] thisId = new byte[4] { 0x00, 0x00, 0x00, 0x00 };
        public MsgDetection()
        {
            InitializeComponent();
        }
        public MsgDetection(Detection theDetection)
        {
            this.theDetection = theDetection;
            InitializeComponent();
        }
        private void MsgDetection_Load(object sender, EventArgs e)
        {
            HidelLable();
            if (ValidateData.ArrayIsNullOrZero<Circuit>(this.theSettingGrop.TheCircuit))
            {
                this.comboBox1.DataSource = this.theSettingGrop.TheCircuit;
                this.comboBox1.DisplayMember = "CircuitName";
                this.comboBox1.ValueMember = "CircuitID";
                List<Detection> addDetection=new List<Detection>();
                addDetection.Add(new Detection() { DetectionID = "0", DetectionName = "无" });
                addDetection.AddRange(this.theSettingGrop.TheDetection);
                this.comboBox2.DataSource=addDetection;
                this.comboBox2.DisplayMember = "DetectionName";
                this.comboBox2.ValueMember = "DetectionID";
            }
            if (theDetection != null)
            {
                SetControlsValue();
            }
        }

        private void HidelLable()
        {
            label8.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            label11.Visible = false;
            label12.Visible = false;
        }
        private bool RegexString()
        { 
            bool isTrue=true;
            //Regex.IsMatch()textBox1.


            return isTrue;
        
        }
        private void button1_Click(object sender, EventArgs e)
        {
            switch (isAddOrUpdate)
            {
                case 0://add
                    if (HandelControls.Msg("是否确认添加？", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (RegexString())
                        {
                            isAddOrUpdateMethod();
                        }
                    }
                    break;
                case 1://update
                    if (HandelControls.Msg("是否确认修改？", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (RegexString())
                        {
                            isAddOrUpdateMethod();
                        }
                    }
                    break;
                default: break;
            }
            this.Close();
        }
        private void SetControlsValue()
        {
            textBox1.Text = theDetection.DetectionName;
            textBox2.Text = theDetection.DetectionNumber;
            textBoxA.Text = theDetection.RemoteA;
            textBoxB.Text = theDetection.RemoteB;
            textBoxC.Text = theDetection.RemoteC;
            textID.Text = theDetection.DetectionID;
            comboBox1.SelectedValue = theDetection.DetectionToID;
            comboBox2.SelectedValue = theDetection.DetectionToSecondID;
            thisId = ByteWithString.intTo4Byte(int.Parse(theDetection.DetectionID));
        }
        private void isAddOrUpdateMethod()
        {
            List<byte> ListArray = new List<byte>();
            byte[] byteArray = ByteWithString.encodeingToByte(textBox2.Text, "utf-8");//名称
            byte[] byteArrayRemake = ByteWithString.encodeingToByte(textBox1.Text, "utf-8");//编号
            ListArray.AddRange(ByteWithString.intTo4Byte(int.Parse(byteArray.Length.ToString())));//名称长度
            ListArray.AddRange(byteArray.ToList());//名称
            ListArray.AddRange(ByteWithString.intTo4Byte(int.Parse(byteArrayRemake.Length.ToString())));//编号长度
            ListArray.AddRange(byteArrayRemake);
            ListArray.AddRange(ByteWithString.intTo4Byte(int.Parse(comboBox2.SelectedValue.ToString())));//上一个检测点id
            ListArray.AddRange(ByteWithString.strToToHexByte(textBoxA.Text));//A
            ListArray.AddRange(ByteWithString.strToToHexByte(textBoxB.Text));//B
            ListArray.AddRange(ByteWithString.strToToHexByte(textBoxC.Text));//C
            ListArray.AddRange(ByteWithString.intTo4Byte(int.Parse(comboBox1.SelectedValue.ToString())));//所属的id
            ListArray.AddRange(thisId);//id
            this.theScoket.theSocketSend<byte[]>(0x30, ListArray.ToArray());//
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       

    }
}
