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
    public partial class MsgGeneratrix : MsgTemplet
    {
        private Generatrix theMsgGeneratrix;
        public int isAddOrUpdate = -1;
        private byte[] thisId = new byte[4] { 0x00, 0x00, 0x00, 0x00 };
        public MsgGeneratrix()
        {
            InitializeComponent();
        }
        public MsgGeneratrix(Generatrix theMsgGeneratrix)
        {
            this.theMsgGeneratrix=theMsgGeneratrix;
            InitializeComponent();
        }
       
        private void MsgGeneratrix_Load(object sender, EventArgs e)
        {
            if (ValidateData.ArrayIsNullOrZero<ConvertingStation>(this.theSettingGrop.TheConvertingStation))
            {
                this.comboBox1.DataSource = this.theSettingGrop.TheConvertingStation;
                this.comboBox1.DisplayMember = "stationName";
                this.comboBox1.ValueMember = "stationID";
            }
            if (theMsgGeneratrix != null)
            {
                SetContent();
            }
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (isAddOrUpdate)
            {
                case 0://add
                    if (HandelControls.Msg("是否确认添加？", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        this.GetContent(0x28, thisId);
                    }
                    break;
                case 1://update
                    if (HandelControls.Msg("是否确认修改？", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        this.GetContent(0x28, thisId);
                    }
                    break;
                default: break;
            }
            this.Close();
        }
        private void SetContent()
        {
            textBox1.Text = theMsgGeneratrix.GeneratrixName;
            textBox2.Text = theMsgGeneratrix.GeneratrixNumber;
            comboBox1.SelectedValue = theMsgGeneratrix.GeneratrixToID;
            thisId = ByteWithString.intTo4Byte(int.Parse(theMsgGeneratrix.GeneratrixID));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        } 
    }
}
