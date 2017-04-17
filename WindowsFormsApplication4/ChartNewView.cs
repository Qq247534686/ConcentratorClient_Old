using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using 集中器控制客户端.HandleClass.reportModels;

namespace 集中器控制客户端
{
    public partial class ChartNewView : Form
    {
        private List<PortChartData> theListPortChartData = new List<PortChartData>();
        private string fileName;
        private byte[] address;
        List<string> listName;
        private Dictionary<string, List<int>> chartValue = new Dictionary<string, List<int>>();
        public ChartNewView()
        {
            InitializeComponent();
        }

        private void ChartNewView_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            DefauleMethod();
        }
        public void theSetDataMethod(List<PortChartData> theListPortChartData, List<string> listName, string fileName, byte[] address)
        {
            this.fileName = fileName;
            this.listName = listName;
            this.address = address;
            this.theListPortChartData = theListPortChartData;
        }
        private void DefauleMethod()
        {
            try
            {
                float fl = 100 / listName.Count;
                tableLayoutPanel1.RowCount = listName.Count;
                tableLayoutPanel1.ColumnCount = 1;
                for (int i = 0; i < listName.Count; i++)
                {
                    Chart cht = new Chart();
                    cht.Dock = DockStyle.Fill;
                    cht.Width = 800;
                    cht.Height = 200;
                    ChartArea ch1 = new ChartArea("ChartArea" + i + listName[i]);
                    ch1.AlignmentOrientation = AreaAlignmentOrientations.Vertical;
                    Series series = new Series(listName[i] + i);
                    series.ChartType = SeriesChartType.Line;
                    series.BorderWidth = 1;
                    ch1.AxisY.Title = listName[i];
                    ch1.AxisY.TextOrientation = TextOrientation.Horizontal;
                    series.Points.DataBindXY(theListPortChartData.Select(u => u.V_X).ToList(), theListPortChartData.Select(u => u.H_Y[i]).ToList());
                    chartValue.Add(listName[i] + i, theListPortChartData.Select(u => u.H_Y[i]).ToList());
                    cht.ChartAreas.Add(ch1);
                    cht.Series.Add(series);
                    cht.Series[listName[i] + i].ChartArea = "ChartArea" + i + listName[i];
                    tableLayoutPanel1.Controls.Add(cht, i, 0);
                }
            }
            catch (Exception msg)
            {
                //throw msg;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tableLayoutPanel1.Controls.Count > 0)
            {
                int count = int.Parse(theShowCount.Value.ToString());
                for (int i = 0; i < tableLayoutPanel1.Controls.Count; i++)
                {
                    if (tableLayoutPanel1.Controls[i] is Chart)
                    {
                        Chart theChart = tableLayoutPanel1.Controls[i] as Chart;
                        theChart.Series[0].ChartType = comboBox1.SelectedItem.ToString() == "折线图" ? SeriesChartType.Line : SeriesChartType.Point;
                        foreach (var item in chartValue.Keys)
                        {
                            if (theChart.Series[0].Name == item)
                            {
                                if (count == 0)
                                {
                                    theChart.Series[0].Points.DataBindXY(theListPortChartData.Select(u => u.V_X).ToList(), chartValue[item]);
                                }
                                else
                                {
                                    theChart.Series[0].Points.DataBindXY(theListPortChartData.Select(u => u.V_X).Take(count).ToList(), chartValue[item].Take(count).ToList());
                                }

                            }
                        }

                    }
                }
            }
        }
    }
}
