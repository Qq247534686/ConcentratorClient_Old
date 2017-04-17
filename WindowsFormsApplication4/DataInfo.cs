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
    public partial class DataInfo : Form
    {
        cycleDataArray thecycleDataArray = new cycleDataArray();
        DetectionData theDetectionData = new DetectionData();
        public DataInfo(cycleDataArray thecycleDataArray, DetectionData theDetectionData)
        {
            this.thecycleDataArray = thecycleDataArray;
            this.theDetectionData = theDetectionData;
            InitializeComponent();
        }

        private void DataInfo_Load(object sender, EventArgs e)
        {
            this.Paint += DataInfo_Paint;
        }

        void DataInfo_Paint(object sender, PaintEventArgs e)
        {
            List<string> listName = new List<string>() { "1-1", "1-2", "1-3", "1-4" };
            CreatePanel(e.Graphics, 70, 300, 850, 300, listName);
        }

        private void CreatePanel(Graphics myGDI, int sx, int sy, int ex, int ey, List<string> name)
        {
            Pen myPen = new Pen(Color.Black, 2);//画笔
            myGDI.DrawLine(myPen, sx, sy, ex, ey);//画直线
            SolidBrush myBrush = new SolidBrush(Color.Black);//画刷
            Font ft = new System.Drawing.Font("华文新魏", 12);
            int fillEllipseY = sy - 8;
            int textY = sy - 30;
            myGDI.DrawString("变电站", ft, new SolidBrush(Color.Blue), sx - 20, sy - 50);
            myGDI.DrawEllipse(myPen, sx - 30, sy - 30, 50, 50);
            myGDI.FillEllipse(myBrush, sx - 10, sy - 30, 50, 50);//画实心椭圆 
            for (int i = 0; i < name.Count; i++)
            {
                sx += 150;
                int textX = sx - 20;
                Label lb = new Label();
                lb.Name = "Test" + (i).ToString();
                lb.Text = "●";
                lb.Tag = name[i];
                lb.Size = new Size(15, 20);
                lb.ForeColor = Color.Red;
                lb.Location = new Point(sx, fillEllipseY);
                this.Controls.Add(lb);
                lb.Click += lb_Click;
                myGDI.DrawString(name[i], ft, new SolidBrush(Color.Blue), textX, textY);
            }

        }

        void lb_Click(object sender, EventArgs e)
        {
            Label lb = sender as Label;
            for (int i = 0; i < theDetectionData.detectionDataArray.Count; i++)
            {
                string name = theDetectionData.detectionDataArray[i].DetectionNumber;
                if (name == lb.Tag.ToString())
                {
                    ShowData showData = new ShowData(this.thecycleDataArray, theDetectionData);
                    switch (lb.Tag.ToString())
                    {
                        case "1-1":
                           
                            showData.ShowDialog();
                            break;
                        case "1-2":
                           
                            showData.ShowDialog();
                            break;
                        case "1-3":
                           
                            showData.ShowDialog();
                            break;
                        case "1-4":
                            
                            showData.ShowDialog();
                            break;
                        default: break;
                    }

                }
            }
        }
    }
}
