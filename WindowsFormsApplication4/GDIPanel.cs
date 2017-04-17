using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using 集中器控制客户端.HandleClass;
using 集中器控制客户端.HandleClass.reportModels;

namespace 集中器控制客户端
{
    public partial class GDIPanel : Form
    {
        public GDIPanel()
        {
            InitializeComponent();
        }
        string selectItemName { get; set; }
        public GDIPanel(string selectItemName)
        {
            this.selectItemName = selectItemName;
            InitializeComponent();
        }

        private void ReadXmlSetting(string selectItemName)
        {
            textBox2.Text = this.selectItemName;
            string filePath = @"SettingTest/" + selectItemName + ".xml";
            if (File.Exists(filePath))
            {
                using (StreamReader sr = new StreamReader(Path.GetFullPath(filePath)))
                {
                    DetectionData theDetectionData=new DetectionData();
                    theDetectionData.detectionDataArray = new List<DetectionDataArray>();
                    theDetectionData = XmlUtil.Deserialize(typeof(DetectionData), sr.ReadToEnd()) as DetectionData;
                    if (theDetectionData != null && theDetectionData.detectionDataArray.Count > 0)
                    {
                        textBox18.Text = theDetectionData.DeviceLine;
                        textBox6.Text = theDetectionData.detectionDataArray[0].DetectionName;
                        textBox17.Text = theDetectionData.detectionDataArray[0].DetectionNumber;
                        textBox3.Text = theDetectionData.detectionDataArray[0].DetectionRouteA;
                        textBox4.Text = theDetectionData.detectionDataArray[0].DetectionRouteB;
                        textBox5.Text = theDetectionData.detectionDataArray[0].DetectionRouteC;

                        textBox7.Text = theDetectionData.detectionDataArray[1].DetectionName;
                        textBox16.Text = theDetectionData.detectionDataArray[1].DetectionNumber;
                        textBox10.Text = theDetectionData.detectionDataArray[1].DetectionRouteA;
                        textBox9.Text = theDetectionData.detectionDataArray[1].DetectionRouteB;
                        textBox8.Text = theDetectionData.detectionDataArray[1].DetectionRouteC;

                        textBox11.Text = theDetectionData.detectionDataArray[2].DetectionName;
                        textBox15.Text = theDetectionData.detectionDataArray[2].DetectionNumber;
                        textBox14.Text = theDetectionData.detectionDataArray[2].DetectionRouteA;
                        textBox13.Text = theDetectionData.detectionDataArray[2].DetectionRouteB;
                        textBox12.Text = theDetectionData.detectionDataArray[2].DetectionRouteC;
                    }
                }

            }
        }
        public event EventHandler addDevice;
        private void GDIPanel_Load(object sender, EventArgs e)
        {
            //this.Paint += GDIPanel_Paint;
            if (!string.IsNullOrEmpty(selectItemName))
            {
                ReadXmlSetting(selectItemName);
            }

        }
        public delegate void AddDevice(DetectionData theDetectionData, string name);
        public AddDevice theAddDevice;
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox2.Text.Trim().Length <= 0)
                {
                    MessageBox.Show("请填写设备地址");
                    return;
                }
                DetectionData theDetectionData = new DetectionData();
                theDetectionData.DeviceAddress = textBox2.Text;
                theDetectionData.DeviceLine = textBox18.Text;
                theDetectionData.detectionDataArray = new List<DetectionDataArray>();
                DetectionDataArray theDetectionDataArray = new DetectionDataArray();
                theDetectionDataArray.DetectionName = textBox6.Text;
                theDetectionDataArray.DetectionNumber = textBox17.Text;
                theDetectionDataArray.DetectionRouteA = textBox3.Text;
                theDetectionDataArray.DetectionRouteB = textBox4.Text;
                theDetectionDataArray.DetectionRouteC = textBox5.Text;
                theDetectionData.detectionDataArray.Add(theDetectionDataArray);
                DetectionDataArray towDetectionDataArray = new DetectionDataArray();
                towDetectionDataArray.DetectionName = textBox7.Text;
                towDetectionDataArray.DetectionNumber = textBox16.Text;
                towDetectionDataArray.DetectionRouteA = textBox10.Text;
                towDetectionDataArray.DetectionRouteB = textBox9.Text;
                towDetectionDataArray.DetectionRouteC = textBox8.Text;
                theDetectionData.detectionDataArray.Add(towDetectionDataArray);
                DetectionDataArray threeDetectionDataArray = new DetectionDataArray();
                threeDetectionDataArray.DetectionName = textBox11.Text;
                threeDetectionDataArray.DetectionNumber = textBox15.Text;
                threeDetectionDataArray.DetectionRouteA = textBox14.Text;
                threeDetectionDataArray.DetectionRouteB = textBox13.Text;
                threeDetectionDataArray.DetectionRouteC = textBox12.Text;
                theDetectionData.detectionDataArray.Add(threeDetectionDataArray);
                if (theDetectionData != null && theDetectionData.detectionDataArray != null)
                {
                        theAddDevice.Invoke(theDetectionData, textBox2.Text);
                }
            }
            catch (Exception msg)
            {
                HandleTool.LogWrite(msg);
            }


        }


    }
}
