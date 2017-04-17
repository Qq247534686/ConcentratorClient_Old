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
    public partial class MsgCircuit : MsgTemplet
    {
        private Circuit theCircuit;
        public int isAddOrUpdate = -1;
        private byte[] thisId = new byte[4] { 0x00, 0x00, 0x00, 0x00 };
        public MsgCircuit()
        {
            InitializeComponent();
        }
        public MsgCircuit(Circuit theCircuit)
        {
            this.theCircuit=theCircuit;
            InitializeComponent();
        }
        private void MsgCircuit_Load(object sender, EventArgs e)
        {
            if (ValidateData.ArrayIsNullOrZero<Generatrix>(this.theSettingGrop.TheGeneratrix))
            {
                this.comboBox1.DataSource = theSettingGrop.TheGeneratrix;
                this.comboBox1.DisplayMember = "GeneratrixName";
                this.comboBox1.ValueMember = "GeneratrixID"; 
            }
            if (theCircuit != null)
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
                        this.GetContent(0x29, thisId);
                    }
                    break;
                case 1://update
                    if (HandelControls.Msg("是否确认修改？", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        this.GetContent(0x29, thisId);
                    }
                    break;
                default: break;
            }
            this.Close();
        }
        public void SetContent()
        {
            textBox1.Text = theCircuit.CircuitName;
            textBox2.Text = theCircuit.CircuitNumber;
            comboBox1.SelectedValue = theCircuit.CircuitToID;
            thisId = ByteWithString.intTo4Byte(int.Parse(theCircuit.CircuitID));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        } 
    }
}
