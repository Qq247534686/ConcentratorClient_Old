using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows.Forms;
using 集中器控制客户端.HandleClass;
using 集中器控制客户端.Properties;
using 集中器控制客户端.HandleClass.portClass;

namespace 集中器控制客户端
{
    public partial class PortSetting : Form
    {
        public PortSetting()
        {
            InitializeComponent();
        }
        /*
         设定参数（设备地址，参数绑定，解析报文格式）
         */
        private static SerialPort THE_SERIAL_PORT = null;
        private PortHandle thePortHandle = null;
        private int THE_DATA_LENGTH = 0;
        private string RECEIVE_DATA = string.Empty;
        private void PortSetting_Load(object sender, EventArgs e)
        {
            this.Icon = Resources.Port;//窗体的Ico
            THE_SERIAL_PORT = new SerialPort();
            THE_SERIAL_PORT.DataReceived += theSerialPort_DataReceived;//注册串口接收事件
            com_PortName.DataSource = SerialPort.GetPortNames().ToList();
            com_PortNumber.DataSource = ConfigurationManager.AppSettings["PortNumber"].Split(',').ToList();
            if (com_PortName.Items.Count > 0)
            {
                com_PortName.SelectedIndex = com_PortNumber.SelectedIndex = 0;
            }
        }
        void theSerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            THE_DATA_LENGTH = THE_SERIAL_PORT.BytesToRead;
            byte[] receviceBuff = new byte[THE_DATA_LENGTH];//声明一个临时数组存储当前来的串口数据  
            THE_SERIAL_PORT.Read(receviceBuff, 0, THE_DATA_LENGTH);//读取缓冲数据
            thePortHandle.HandleData(receviceBuff);
            if (receviceBuff.Length > 0)
            {
                RECEIVE_DATA += ByteWithString.byteToHexStrAppend(receviceBuff, " ");//以16进制进行接收
            }
            HandelData(RECEIVE_DATA);
        }
        //处理接收数据
        private void HandelData(string reviceData)
        {
            try
            {

                if (string.IsNullOrEmpty(txt_ReviceData.Text))
                {
                    txt_ReviceData.Text = reviceData;
                }
                else
                {
                    txt_ReviceData.Text = string.Format("{0}\r\n\r\n{1}", reviceData, txt_ReviceData.Text);
                }
            }
            catch (Exception msg)
            {
                Log.LogWrite(msg);
            }
            finally
            {
                RECEIVE_DATA = string.Empty;
            }
        }
        //打开连接
        private void btn_OpenPort_Click(object sender, EventArgs e)
        {
            if (com_PortName.Items.Count > 0)
            {
                string selectPortName = com_PortName.SelectedItem.ToString();
                try
                {
                    if (SerialPort.GetPortNames().ToList().Contains(selectPortName))
                    {
                        if (!THE_SERIAL_PORT.IsOpen)
                        {
                            THE_SERIAL_PORT.PortName = selectPortName;
                            THE_SERIAL_PORT.BaudRate = Convert.ToInt32(com_PortNumber.SelectedItem.ToString());
                            THE_SERIAL_PORT.Open();
                            thePortHandle = new PortHandle(THE_SERIAL_PORT);
                            btn_OpenPort.Text = "关闭串口";
                            picture_Port.Image = Resources.OpenPort;
                            com_PortName.Enabled = com_PortNumber.Enabled = false;
                        }
                        else
                        {

                            THE_SERIAL_PORT.Close();
                            btn_OpenPort.Text = "打开串口";
                            picture_Port.Image = Resources.ClosePort;
                            com_PortName.Enabled = com_PortNumber.Enabled = true;
                        }
                    }
                    else
                    {
                        MessageBox.Show(string.Format("【{0}】不存在", selectPortName));
                    }
                }
                catch (Exception msg)
                {
                    Log.LogWrite(msg);
                    MessageBox.Show(string.Format("【{0}】已被占用", selectPortName));
                }
            }
            else
            {
                MessageBox.Show("请选择串口名称", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        //关闭窗口
        private void PortSetting_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (THE_SERIAL_PORT.IsOpen)
            {
                THE_SERIAL_PORT.Close();
            }
        }
        //清空接受信息
        private void btn_txtEmpty_Click(object sender, EventArgs e)
        {
            txt_ReviceData.Text = string.Empty;
        }

        private void picture_Port_MouseMove(object sender, MouseEventArgs e)
        {

        }
        //获取汇集单元基本参数
        private void button41_Click(object sender, EventArgs e)
        {

        }
        //设置汇集单元基本参数
        private void button10_Click(object sender, EventArgs e)
        {

        }
        //获取汇集单元运行参数
        private void button2_Click(object sender, EventArgs e)
        {

        }
        //设置汇集单元运行参数
        private void button11_Click(object sender, EventArgs e)
        {

        }
        //获取汇集单元通信参数
        private void button3_Click(object sender, EventArgs e)
        {

        }
        //设置汇集单元通信参数
        private void button12_Click(object sender, EventArgs e)
        {

        }
        //获取采集单元电网参数
        private void button4_Click(object sender, EventArgs e)
        {

        }
        //设置采集单元电网参数
        private void button16_Click(object sender, EventArgs e)
        {

        }
        //获取采集单元通信地址
        private void button5_Click(object sender, EventArgs e)
        {

        }
        //设置采集单元通信地址
        private void button17_Click(object sender, EventArgs e)
        {

        }
        //获取故障指示器上报延时
        private void button6_Click(object sender, EventArgs e)
        {

        }
        //设置故障指示器上报延时
        private void button15_Click(object sender, EventArgs e)
        {

        }
        //获取故障指示器点表配置
        private void button8_Click(object sender, EventArgs e)
        {

        }
        //设置故障指示器点表配置
        private void button14_Click(object sender, EventArgs e)
        {

        }
        //获取遥测点表配置
        private void button7_Click(object sender, EventArgs e)
        {

        }
        //设置遥测点表配置
        private void button13_Click(object sender, EventArgs e)
        {

        }
    }
}
