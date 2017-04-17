using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
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
    public partial class RouteMap : Form
    {
        public RouteMap()
        {
            InitializeComponent();
        }
        int EXCHANGE_COLOR = 0;
        Queue<Control> query = new Queue<Control>();
        SQLiteHelper theSQLiteHelper = new SQLiteHelper();
        List<Label> theControlArray = new List<Label>();
        private SettingGrop theSettingGrop = SettingGrop.createSettingGrop();
        private void RouteMap_Load(object sender, EventArgs e)
        {
            this.Icon = Resources.lineError;
            //绑定线路
            if (ValidateData.ArrayIsNullOrZero<Circuit>(theSettingGrop.TheCircuit))
            {
                this.comboBox1.DataSource = theSettingGrop.TheCircuit;
                this.comboBox1.DisplayMember = "CircuitName";
                this.comboBox1.ValueMember = "CircuitID";
                //..10
                timer1.Start();
                timer2.Start();
            }
            this.Paint += RouteMap_Paint;


        }


        //
        private List<Detection> GetArray()
        {
            List<Detection> theNewArray = new List<Detection>();
            string theId = comboBox1.SelectedValue.ToString();
            //根据路线查设备
            var queryArray = (from array in theSettingGrop.TheDetection where array.DetectionToID == theId select array).ToList();
            if (queryArray != null && queryArray.Count > 0)
            {
                queryArray = queryArray.OrderBy(u => u.DetectionToSecondID).ToList();
                theNewArray.AddRange((from array in theSettingGrop.TheDetection where array.DetectionToSecondID == "0" select array).ToList());
                //设备所属路线的id进行排序
                queryArray.ForEach((nowElement) =>
                {
                    //当前设备id是否有关联的上一个点
                    var returnArray = queryArray.Exists(u => u.DetectionToSecondID == nowElement.DetectionID);
                    if (returnArray)
                    {
                        //查询所关联的设备和他的前一个检测点
                        var filterArray = (from array in queryArray where array.DetectionToSecondID == nowElement.DetectionID select array).ToList();
                        theNewArray.AddRange(filterArray);//加入集合
                    }
                });
            }
            return theNewArray;
        }
        //画A,B,C相的直线
        void RouteMap_Paint(object sender, PaintEventArgs e)
        {
            Graphics myGDI = e.Graphics;
            Pen myPen = new Pen(Color.Black, 2);//画笔
            myGDI.DrawLine(myPen, 60, 130, 1000, 130);//画直线
            myGDI.DrawLine(myPen, 60, 245, 1000, 245);//画直线
            myGDI.DrawLine(myPen, 60, 360, 1000, 360);//画直线
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Detection> theArray = GetArray();
            if (theArray != null && theArray.Count > 0)
            {
                //清空点和字的Lable控件
                RemoveLable();
                //初始化X,Y轴
                int theX = 130, theY = 122;
                //变量设备
                theArray.ForEach((item) =>
                {
                    Label theA = new Label();
                    Label theB = new Label();
                    Label theC = new Label();
                    theA.Tag = item.DetectionID;
                    theB.Tag = item.DetectionID;
                    theC.Tag = item.DetectionID;
                    Label theA_Name = new Label();
                    Label theB_Name = new Label();
                    Label theC_Name = new Label();
                    theA_Name.Tag = "remove";
                    theB_Name.Tag = "remove";
                    theC_Name.Tag = "remove";
                    theA_Name.Text = item.DetectionName;
                    theB_Name.Text = item.DetectionName;
                    theC_Name.Text = item.DetectionName;
                    theA.Text = theB.Text = theC.Text = "●";
                    theA.Font = theB.Font = theC.Font = new System.Drawing.Font("华文新魏", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));//改变字体的大小
                    theA.Size = theB.Size = theC.Size = new Size(20, 20);//点的大小
                    theA_Name.Height = theB_Name.Height = theC_Name.Height = 15;//字体的高度
                    theA.ForeColor = theB.ForeColor = theC.ForeColor = Color.LimeGreen;//字体的颜色
                    //点的位置
                    theA.Location = new Point(theX, theY);
                    theB.Location = new Point(theX, theY + 115);
                    theC.Location = new Point(theX, theY + 230);
                    //字的位置
                    theA_Name.Location = new Point(theX - 8, theY - 20);
                    theB_Name.Location = new Point(theX - 8, theY + 95);
                    theC_Name.Location = new Point(theX - 8, theY + 210);
                    theX += 100;
                    //加点
                    this.Controls.Add(theA);
                    this.Controls.Add(theB);
                    this.Controls.Add(theC);
                    //加字
                    this.Controls.Add(theA_Name);
                    this.Controls.Add(theB_Name);
                    this.Controls.Add(theC_Name);
                    //入队
                    query.Enqueue(theA);
                    query.Enqueue(theB);
                    query.Enqueue(theC);
                    query.Enqueue(theB_Name);
                    query.Enqueue(theA_Name);
                    query.Enqueue(theC_Name);
                    //注册事件
                    theA.Click += theA_Click;
                    theB.Click += theB_Click;
                    theC.Click += theC_Click;
                }
            );
                ShowColorRedOrGreen();
            }
            else
            {
                RemoveLable();
            }
        }

        private void ShowColorRedOrGreen()
        {
            theControlArray.Clear();
            foreach (var item in this.Controls)
            {
                if (item is Label)
                {
                    Label theLabel = item as Label;
                    if (theLabel.Tag != null && theLabel.Tag.ToString() != "remove")
                    {
                        theControlArray.Add(theLabel);
                    }
                }
            }
        }
        private void QueryDeviceData(string number)
        {
            var theDevice = (from data in theSettingGrop.TheDeviceDataSetting where data.RemoteAToID == number || data.RemoteBToID == number || data.RemoteCToID == number select data).FirstOrDefault();
            if (theDevice != null)
            {
                string str = "";//所属路线
                if (theDevice.RemoteAToID == number)
                {
                    str = "1";
                }
                else if (theDevice.RemoteBToID == number)
                {
                    str = "2";
                }
                else if (theDevice.RemoteCToID == number)
                {
                    str = "3";
                }
                //读取地址和路线的集合
                List<ReadSqlLiteData> remoteArray = HandelControls.returnRemoteArray(theDevice.DeviceDataID, str);
                List<ReadSqlLiteData> telemeteringArray = HandelControls.returnTelemeteringArray(theDevice.DeviceDataID, str);
                ShowQueryData theShowQueryData = new ShowQueryData(remoteArray, telemeteringArray, str);
                theShowQueryData.Text = "第"+str+"路遥信遥测数据";
                theShowQueryData.ShowDialog();

            }
            else
            {
                HandelControls.Msg("暂无数据", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        void theA_Click(object sender, EventArgs e)
        {
            Label lbA = sender as Label;
            if (lbA.Tag != null)
            {
                string number = lbA.Tag.ToString();
                QueryDeviceData(number);
            }
        }

        void theB_Click(object sender, EventArgs e)
        {
            Label lbB = sender as Label;
            if (lbB.Tag != null)
            {
                string number = lbB.Tag.ToString();
                QueryDeviceData(number);
            }
        }

        void theC_Click(object sender, EventArgs e)
        {
            Label lbC = sender as Label;
            if (lbC.Tag != null)
            {
                string number = lbC.Tag.ToString();
                QueryDeviceData(number);
            }
        }

        //清空所在栈内的控件
        private void RemoveLable()
        {
            while (query.Count != 0)
            {
                query.Dequeue().Dispose();
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (theControlArray.Count > 0 && theSettingGrop.TheHitch != null)
            {
                if (EXCHANGE_COLOR == 0)
                {
                    theControlArray.ForEach((u) =>
                    {
                        if (theSettingGrop.TheHitch.NOW_ID.Contains<int>(int.Parse(u.Tag.ToString())))
                        {
                            u.ForeColor = Color.Red;
                        }
                    });
                    EXCHANGE_COLOR = 1;
                }
                else
                {
                    theControlArray.ForEach((u) =>
                    {
                        if (theSettingGrop.TheHitch.NOW_ID.Contains<int>(int.Parse(u.Tag.ToString())))
                        {
                            u.ForeColor = Color.LimeGreen;
                        }
                    });
                    EXCHANGE_COLOR = 0;
                }

            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {

            if (theControlArray.Count > 0 && theSettingGrop.TheHitch != null)
            {
                theControlArray.ForEach((u) =>
                {
                    string number = u.Tag.ToString();
                    var theDevice = (from data in theSettingGrop.TheDeviceDataSetting where data.RemoteAToID == number || data.RemoteBToID == number || data.RemoteCToID == number select data).FirstOrDefault();
                    if (theDevice != null)
                    {
                        string str = "";//所属路线
                        string count = "";//所属路线的所有数值个数   
                        if (theDevice.RemoteAToID == number)
                        {
                            str = "1"; count = "23";
                        }
                        else if (theDevice.RemoteBToID == number)
                        {
                            str = "2"; count = "21";
                        }
                        else if (theDevice.RemoteCToID == number)
                        {
                            str = "3"; count = "21";
                        }
                        string sql = String.Format("Select count(*) From Remote where belongline=\"{0}\" and numbervalue=\"{1}\"", str, "10");
                        if (theSQLiteHelper.ExecuteScalar(sql).ToString() == count)
                        {
                            u.ForeColor = Color.LimeGreen;
                            theControlArray.Remove(u);
                        }
                    }
                });
            }
        }

    }
}
