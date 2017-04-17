using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 集中器控制客户端.HandleClass.reportModels;

namespace 集中器控制客户端
{
    public partial class ShowData : Form
    {
        cycleDataArray thecycleDataArray = new cycleDataArray();
        DetectionData theDetectionData = new DetectionData();
        public ShowData(cycleDataArray thecycleDataArray, DetectionData theDetectionData)
        {
            this.thecycleDataArray = thecycleDataArray;
            this.theDetectionData = theDetectionData;
            InitializeComponent();
        }

        private void ShowData_Load(object sender, EventArgs e)
        {
            SettingName();
        }
        private void SettingName()
        {
            textBox1.Text = thecycleDataArray.cycleDataListI[0].Select(u => u.overZeroPoint).ToList()[0].ToString();
            comboBox1.DataSource = thecycleDataArray.cycleDataListI[0].Select(u => u.rootValue).ToList();
            comboBox2.DataSource = thecycleDataArray.cycleDataListI[0].Select(u => u.absValue).ToList();

            textBox2.Text = thecycleDataArray.cycleDataListI[1].Select(u => u.overZeroPoint).ToList()[0].ToString();
            comboBox3.DataSource = thecycleDataArray.cycleDataListI[1].Select(u => u.rootValue).ToList();
            comboBox4.DataSource = thecycleDataArray.cycleDataListI[1].Select(u => u.absValue).ToList();

            textBox3.Text = thecycleDataArray.cycleDataListI[2].Select(u => u.overZeroPoint).ToList()[0].ToString();
            comboBox5.DataSource = thecycleDataArray.cycleDataListI[2].Select(u => u.rootValue).ToList();
            comboBox6.DataSource = thecycleDataArray.cycleDataListI[2].Select(u => u.absValue).ToList();

            textBox4.Text = thecycleDataArray.cycleDataListZero[0].Select(u => u.overZeroPoint).ToList()[0].ToString();
            comboBox7.DataSource = thecycleDataArray.cycleDataListZero[0].Select(u => u.rootValue).ToList();
            comboBox8.DataSource = thecycleDataArray.cycleDataListZero[0].Select(u => u.absValue).ToList();


        }


    }
}
