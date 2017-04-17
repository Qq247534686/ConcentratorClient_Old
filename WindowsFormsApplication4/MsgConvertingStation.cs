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
    public partial class MsgConvertingStation : Form
    {
        public int isAddOrUpdate = -1;//增加还是修改
        private byte[] thisId = new byte[4] { 0x00, 0x00, 0x00, 0x00 };
        ConvertingStation content;
        private HandleTool theScoket = HandleTool.createHandleTool();
        public MsgConvertingStation()
        {
            InitializeComponent();
        }
        public MsgConvertingStation(ConvertingStation content)
        {
            this.content = content;
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            switch (isAddOrUpdate)
            {
                case 0:
                    if (ValidateData.ValidateString(textBox1.Text))
                    {
                        if (HandelControls.Msg("是否确认添加？", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {

                            isAddOrUpdateMethod();
                            this.Close();

                        }
                    }
                    else
                    {
                        label3.Visible = true;
                    }
                    break;
                case 1:
                    if (ValidateData.ValidateString(textBox1.Text))
                    {
                        if (HandelControls.Msg("是否确认修改？", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            isAddOrUpdateMethod();
                            this.Close();
                        }
                    }
                    else
                    {
                        label3.Visible = true;
                    }
                    break;
                default: break;
            }

        }
       
        private void isAddOrUpdateMethod()
        {
            List<byte> ListArray = new List<byte>();
            byte[] byteArray = ByteWithString.encodeingToByte(textBox1.Text, "utf-8");
            byte[] byteArrayRemake = ByteWithString.encodeingToByte(textBox2.Text, "utf-8");
            ListArray.AddRange(ByteWithString.intTo4Byte(byteArray.Length));
            ListArray.AddRange(byteArray.ToList());
            ListArray.AddRange(ByteWithString.intTo4Byte(byteArrayRemake.Length));
            ListArray.AddRange(byteArrayRemake.ToList());
            ListArray.AddRange(thisId);
            theScoket.theSocketSend<byte[]>(0x27, ListArray.ToArray());
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MsgConvertingStation_Load(object sender, EventArgs e)
        {
            label3.Visible = false;
            if (content != null)
            {
                this.thisId = ByteWithString.intTo4Byte(int.Parse(content.stationID));
                textBox1.Text = content.stationName;
                textBox2.Text = content.stationRemarks;
            }
        }
    }
}
