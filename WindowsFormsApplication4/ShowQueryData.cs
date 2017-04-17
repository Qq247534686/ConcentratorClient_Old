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
    public partial class ShowQueryData : Form
    {
        public ShowQueryData()
        {
            InitializeComponent();
        }
        string numberLine = "";
        List<ReadSqlLiteData> remoteArray;
        List<ReadSqlLiteData> telemeteringArray;
        KeyValueRemote theKeyValueRemote = new KeyValueRemote();
        KeyValueTelemetering theKeyValueTelemetering = new KeyValueTelemetering();

        public ShowQueryData(List<ReadSqlLiteData> remoteArray, List<ReadSqlLiteData> telemeteringArray, string number)
        {
            this.remoteArray = remoteArray;
            this.telemeteringArray = telemeteringArray;
            this.numberLine = number;
            InitializeComponent();
        }
        private void ShowQueryData_Load(object sender, EventArgs e)
        {
            this.groupBox1.Text = "第" + numberLine + "路遥测信息";
            this.groupBox2.Text = "第" + numberLine + "路遥信信息";
            if (remoteArray.Count>0 || telemeteringArray.Count>0)
            {
                LodingtelemeteringInfo();
                LodingremoteArrayInfo();
            }
        }
        //加载遥信数据
        private void LodingremoteArrayInfo()
        {
            for (int i = 0; i < remoteArray.Count; i++)
            {
                RemoteView.Rows.Add();
                RemoteView.Rows[i].Cells["remoteName"].Value = theKeyValueRemote.SearchKey(remoteArray[i].theDataAddress);
                RemoteView.Rows[i].Cells["remoteNumber"].Value = remoteArray[i].theNumber;
                RemoteView.Rows[i].Cells["remoteValue"].Value = remoteArray[i].theValue;
            }
        }
        //加载遥测数据
        private void LodingtelemeteringInfo()
        {
            for (int i = 0; i < telemeteringArray.Count; i++)
            {
                TelemeteringView.Rows.Add();
                TelemeteringView.Rows[i].Cells["telemeteringName"].Value = theKeyValueTelemetering.SearchKey(telemeteringArray[i].theDataAddress);
                TelemeteringView.Rows[i].Cells["telemeteringNumber"].Value = telemeteringArray[i].theNumber;
                TelemeteringView.Rows[i].Cells["telemeteringDescriptive"].Value = telemeteringArray[i].theValue;
            }
        }
    }
}
