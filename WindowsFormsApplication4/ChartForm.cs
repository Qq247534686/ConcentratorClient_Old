using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Windows.Forms.DataVisualization.Charting;
using 集中器控制客户端.HandleClass.reportModels;
using 集中器控制客户端.HandleClass;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
using 集中器控制客户端.Properties;
namespace 集中器控制客户端
{
    public partial class ChartForm : Form
    {
        private List<PortChartData> theListPortChartData = new List<PortChartData>();
        private WaveData listPassagewayWave = new WaveData();
        private string fileName;
        private byte[] address;
        List<string> listName;
        private List<Series> listSeriesArray = new List<Series>();
        public ChartForm()
        {
            InitializeComponent();
        }
        public void theSetValueMethod(WaveData listPassagewayWave, string fileName, byte[] address)
        {
            this.fileName = fileName;
            this.address = address;
            this.listPassagewayWave = listPassagewayWave;
        }
        public void theSetDataMethod(List<PortChartData> theListPortChartData,List<string> listName, string fileName, byte[] address)
        {
            this.fileName = fileName;
            this.listName = listName;
            this.address = address;
            this.theListPortChartData = theListPortChartData;
        }
        CheckBox chAll = new CheckBox();
        private void ChartForm_Load(object sender, EventArgs e)
        {
            //this.Icon = Resources.zxt;
            //chartShowDataView.Series.Clear();//清空图表
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            //chartShowDataView.Series.Clear();
            //CreateCheckedBox(listPassagewayWave.CHANNEL_DATA.Count);
            DefauleMethod();


        }

        private void DefauleMethod()
        {
            try
            {
                //float fl = 100 / listName.Count;
                //tableLayoutPanel1.RowCount = listName.Count;
                //tableLayoutPanel1.ColumnCount = 1;

                for (int i = 0; i < listName.Count; i++)
                {
                   
                    Chart cht = new Chart();
                    cht.Width = 800;
                    cht.Height = 200;
                    ChartArea ch1 = new ChartArea("ChartArea" + i + theListPortChartData[i].PointName);
                    ch1.AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                    Series series = new Series(theListPortChartData[i].PointName + i);
                    series.ChartType = SeriesChartType.Line;
                    series.BorderWidth = 1;
                    //series.LegendText = listName[i];
                    ch1.AxisY.Title = listName[i];
                    ch1.AxisY.TextOrientation = TextOrientation.Horizontal;
                    series.Points.DataBindXY(theListPortChartData.Select(u => u.V_X).ToList(), theListPortChartData.Select(u => u.H_Y[i]).ToList());
                    cht.ChartAreas.Add(ch1);
                    cht.Series.Add(series);
                    cht.Series[theListPortChartData[i].PointName + i].ChartArea = "ChartArea" + i + theListPortChartData[i].PointName;
                    flowLayoutPanel2.Controls.Add(cht);
                    //tableLayoutPanel1.Controls.Add(cht, i, 1);
                }
            }
            catch(Exception msg) {
                //throw msg;
            }
        }

        List<string> listArea = new List<string>();
        /// <summary>
        /// 根据曲线个数创建单选框个数
        /// </summary>
        /// <param name="count"></param>
        private void CreateCheckedBox(int count)
        {
            if (count > 0)
            {
                chAll.Text = "全选";
                chAll.CheckedChanged += chAll_CheckedChanged;
                flowLayoutPanel1.Controls.Add(chAll);
                for (int i = 0; i < count; i++)
                {
                    ChartArea ch1 = new ChartArea("ChartArea" + i.ToString());
                    chartShowDataView.ChartAreas.Add(ch1);
                    listArea.Add("ChartArea" + i.ToString());
                    CheckBox ch = new CheckBox();
                    ch.Text = "曲线" + (i + 1).ToString();
                    ch.Tag = i;
                    flowLayoutPanel1.Controls.Add(ch);
                }
                ChartArea ChartAreaZero = new ChartArea("ChartAreaZero");
                ChartArea ChartAreaALL = new ChartArea("ChartAreaALL");
                ChartAreaZero.Visible = false;
                ChartAreaALL.Visible = false;
                chartShowDataView.ChartAreas.Add(ChartAreaZero);
                chartShowDataView.ChartAreas.Add(ChartAreaALL);
            }
        }

        void chAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chAll.Checked)
            {
                isCheckedAll(true);
            }
            else
            {
                isCheckedAll(false);
            }

        }
        private void isCheckedAll(bool falgChecked)
        {
            foreach (var item in flowLayoutPanel1.Controls)
            {
                if (item is CheckBox)
                {
                    CheckBox theItem = (CheckBox)item;
                    theItem.Checked = falgChecked;
                }
            }
        }
        /// <summary>
        /// 电压
        /// </summary>
        /// <param name="list"></param>
        /// <param name="p"></param>
        private void GetSeriesListCurrent(List<int> listIndex, decimal theCount, string name)
        {
            string selectStr = comboBox1.SelectedItem.ToString();
            for (int i = 0; i < listIndex.Count; i++)
            {
                Series series = new Series("曲线" + (listIndex[i] + 1).ToString() + name);
                series.IsValueShownAsLabel = false;
                series.ChartType = selectStr == "折线图" ? SeriesChartType.Line : SeriesChartType.Point;
                series.BorderWidth = 3;
                List<double> X = new List<double>();
                List<int> Y = new List<int>();
                int index = listIndex[i];
                if (theCount == 0)
                {
                    Y = listPassagewayWave.CHANNEL_DATA[index].channelDatas[1].HEX_POINT_DATA.ToList();
                }
                else
                {
                    Y = listPassagewayWave.CHANNEL_DATA[index].channelDatas[1].HEX_POINT_DATA.Skip(0).Take((int)theCount).ToList();
                }
                for (int j = 0; j < Y.Count; j++)
                {
                    X.Add(j * 0.24);
                }
                series.Points.DataBindXY(X, Y);
                chartShowDataView.Series.Add(series);
                //chartShowDataView.ChartAreas[i].AxisX.IsStartedFromZero = true;
                //chartShowDataView.ChartAreas[i].AxisX2.MajorGrid.Enabled = false;
                chartShowDataView.ChartAreas[i].AxisX.Interval = (double)theRange.Value;
                chartShowDataView.ChartAreas[i].AxisY.TextOrientation = TextOrientation.Horizontal;
                chartShowDataView.Series["曲线" + (listIndex[i] + 1).ToString() + name].ChartArea = "ChartArea" + listIndex[i].ToString();
                chartShowDataView.ChartAreas["ChartArea" + listIndex[i].ToString()].Visible = true;
                listSeriesArray.Add(series);
            }
          

        }
        /// <summary>
        /// 电流
        /// </summary>
        /// <param name="listIndex"></param>
        /// <param name="theCount"></param>
        private void GetSeriesList(List<int> listIndex, decimal theCount, string name)
        {
            List<string> listNow = new List<string>();
            string selectStr = comboBox1.SelectedItem.ToString();
            for (int i = 0; i < listIndex.Count; i++)
            {
                Series series = new Series("曲线" + (listIndex[i] + 1).ToString() + name);
                series.IsValueShownAsLabel = false;
                series.ChartType = selectStr == "折线图" ? SeriesChartType.Line : SeriesChartType.Point;
                series.BorderWidth = 3;
                List<double> X = new List<double>();
                List<int> Y = new List<int>();
                int index = listIndex[i];
                if (theCount == 0)
                {
                    Y = listPassagewayWave.CHANNEL_DATA[index].channelDatas[0].HEX_POINT_DATA.ToList();
                }
                else
                {
                    Y = listPassagewayWave.CHANNEL_DATA[index].channelDatas[0].HEX_POINT_DATA.ToList().Skip(0).Take((int)theCount).ToList();
                }
                for (int j = 0; j < Y.Count; j++)
                {
                    X.Add(j * 0.24);
                }
                series.Points.DataBindXY(X, Y);
                chartShowDataView.Series.Add(series);
                chartShowDataView.ChartAreas[i].AxisX.Interval = (double)theRange.Value;
                chartShowDataView.ChartAreas[i].AxisY.TextOrientation = TextOrientation.Horizontal;
                chartShowDataView.Series["曲线" + (listIndex[i] + 1).ToString() + name].ChartArea = "ChartArea" + listIndex[i].ToString();
                chartShowDataView.ChartAreas["ChartArea" + listIndex[i].ToString()].Visible = true;
                listSeriesArray.Add(series);
            }
            

        }
        private void GetZeroSerie(int[] listIndex)
        {
            if (listIndex.Length > 0)
            {
                for (int i = 0; i < listArea.Count; i++)
                {
                    chartShowDataView.ChartAreas[listArea[i]].Visible = false;
                }
                Series theSeries = new Series("零序曲线");
                theSeries.Tag = "ChartAreaZero";
                theSeries.IsValueShownAsLabel = false;
                string selectStr = comboBox1.SelectedItem.ToString();
                theSeries.ChartType = selectStr == "折线图" ? SeriesChartType.Line : SeriesChartType.Point;
                theSeries.BorderWidth = 3;
                List<double> theX = new List<double>();
                List<int> theY = new List<int>();
                int count = Convert.ToInt32(theShowCount.Value);
                if (count == 0 || count > listIndex.Length)
                {
                    count = listIndex.Length;
                }
                theY = listIndex.Take(count).ToList();
                for (int j = 0; j < theY.Count; j++)
                {
                    theX.Add(j * 0.24);
                }
                theSeries.Points.DataBindXY(theX, theY);
                chartShowDataView.Series.Add(theSeries);
                chartShowDataView.Series["零序曲线"].ChartArea = "ChartAreaALL";
                chartShowDataView.Series["零序曲线"].ChartArea = "ChartAreaZero";
                chartShowDataView.ChartAreas["ChartAreaZero"].AxisX.Interval = (double)theRange.Value;
                chartShowDataView.ChartAreas["ChartAreaZero"].Visible = true;
                listSeriesArray.Add(theSeries);
                //SetCheckedState();

            }
        }
        /// <summary>
        /// 获得选中单选框的下标
        /// </summary>
        /// <returns></returns>
        private List<int> GetCheckedBosTag()
        {
            List<int> listInt = new List<int>();
            foreach (var item in flowLayoutPanel1.Controls)
            {
                if (item is CheckBox && ((CheckBox)item).Tag != null)
                {
                    CheckBox theItem = (CheckBox)item;
                    if (theItem.Checked == true)
                    {
                        chartShowDataView.ChartAreas["ChartArea" + theItem.Tag.ToString()].Visible = true;
                        listInt.Add((int)theItem.Tag);
                    }
                    else
                    {
                        chartShowDataView.ChartAreas["ChartArea" + theItem.Tag.ToString()].Visible = false;
                    }
                }
            }
            return listInt;
        }
        private void button1_Click(object sender, EventArgs e)
        {
          
            try
            {
                if (listPassagewayWave.CHANNEL_DATA.Count > 0)
                {
                    chartShowDataView.Series.Clear();
                    string comValue = comboBox2.SelectedItem.ToString();
                    switch (comValue)
                    {
                        case "全部":
                            if (GetCheckedBosTag().Count > 0)
                            {

                                GetZeroSerie(SumCurrent());
                                GetSeriesList(GetCheckedBosTag(), theShowCount.Value, "(电流I)");
                                GetSeriesListCurrent(GetCheckedBosTag(),theShowCount.Value, "(电压U)");
                                for (int i = 0; i < listArea.Count; i++)
                                {
                                    chartShowDataView.ChartAreas[listArea[i]].Visible = false;
                                }
                                for (int i = 0; i < listSeriesArray.Count; i++)
                                {
                                    listSeriesArray[i].ChartArea = "ChartAreaALL";
                                }
                                chartShowDataView.ChartAreas["ChartAreaZero"].Visible = false;
                                chartShowDataView.ChartAreas["ChartAreaALL"].Visible = true;
                                chartShowDataView.ChartAreas["ChartAreaALL"].AxisY.TextOrientation = TextOrientation.Horizontal;
                                chartShowDataView.ChartAreas["ChartAreaALL"].AxisY.Title = "全部";
                            }
                            break;
                        case "电流":
                            chartShowDataView.ChartAreas["ChartAreaZero"].Visible = false;
                            chartShowDataView.ChartAreas["ChartAreaALL"].Visible = false;
                            GetSeriesList(GetCheckedBosTag(), theShowCount.Value, "");
                            //chartShowDataView.ChartAreas[0].AxisY.Title = "电流";
                            break;
                        case "电压":
                            chartShowDataView.ChartAreas["ChartAreaZero"].Visible = false;
                            chartShowDataView.ChartAreas["ChartAreaALL"].Visible = false;
                            GetSeriesListCurrent(GetCheckedBosTag(), theShowCount.Value, "");
                            //chartShowDataView.ChartAreas[0].AxisY.Title = "电压";
                            break;
                        case "电流零序":
                            GetZeroSerie(SumCurrent());
                            //chartShowDataView.ChartAreas[0].AxisY.Title = "电流零序";
                            break;
                        default: break;
                    }
                }
            }
            catch (Exception msg)
            {
                Log.LogWrite(msg);
            }
        }
        /// <summary>
        /// 重新设定零序复选框状态
        /// </summary>
        private void SetCheckedState()
        {
            foreach (var item in flowLayoutPanel1.Controls)
            {
                if (item is CheckBox)
                {
                    CheckBox ch = item as CheckBox;
                    ch.Checked = true;
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private int[] SumCurrent()
        {
            List<int> sumCurrent = new List<int>();
            int[] theSumCurrent = new int[listPassagewayWave.CHANNEL_DATA[0].channelDatas[0].HEX_POINT_DATA.Length];
            for (int i = 0; i < listPassagewayWave.CHANNEL_DATA.Count; i++)
            {
                for (int j = 0; j < listPassagewayWave.CHANNEL_DATA[i].channelDatas[0].HEX_POINT_DATA.Length; j++)
                {
                    theSumCurrent[j] += listPassagewayWave.CHANNEL_DATA[i].channelDatas[0].HEX_POINT_DATA[j];
                }
            }

            return theSumCurrent;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            //for (int i = 0; i < flowLayoutPanel2.Controls.Count; i++)
            //{
            //    if (flowLayoutPanel2.Controls[i] is Chart)
            //    {
            //        (flowLayoutPanel2.Controls[i] as Chart).ChartAreas[0].AxisY.Interval = trackBar1.Value * 2;
            //    }
            //}
            
            //for (int i = 0; i < chartShowDataView.ChartAreas.Count; i++)
            //{
            //    chartShowDataView.ChartAreas[i].AxisY.Interval = trackBar1.Value * 2;
            //}
           
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void chartShowDataView_MouseMove(object sender, MouseEventArgs e)
        {

        }
    }
}
