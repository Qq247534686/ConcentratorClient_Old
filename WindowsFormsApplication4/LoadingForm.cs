using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Configuration;

namespace 集中器控制客户端
{
    public partial class LoadingForm : Form
    {
        public LoadingForm()
        {
            InitializeComponent();
        }
       
        private void LoadingForm_Load(object sender, EventArgs e)
        {
            this.skinEngine2.SkinFile = ConfigurationManager.AppSettings["windowsStyle"].ToString();
            closeTimer.Interval = 8000;
            closeTimer.Start();
        }

        private void closeTimer_Tick(object sender, EventArgs e)
        {
            closeTimer.Stop();
            ConcentratorControlClient concentratorControlClient = new ConcentratorControlClient();
            concentratorControlClient.ShowDialog();
            
           
        }
    }
}
