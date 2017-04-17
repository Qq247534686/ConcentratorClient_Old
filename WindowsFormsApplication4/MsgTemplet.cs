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
    public partial class MsgTemplet : Form
    {
        public MsgTemplet()
        {
            InitializeComponent();
        }
        public SettingGrop theSettingGrop = SettingGrop.createSettingGrop();
        public HandleTool theScoket = HandleTool.createHandleTool();
        private void MsgTemplet_Load(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }
        /// <summary>
        /// 显示提示消息
        /// </summary>
        /// <param name="msg"></param>
        public void Msg(string msg)
        {
            MessageBox.Show(msg);
        }
        /// <summary>
        /// 显示提示消息的类型
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="msgButtons">按钮类型</param>
        /// <param name="msgIco">图标的类型</param>
        /// <returns>消息返回的模式</returns>
        public DialogResult Msg(string msg, MessageBoxButtons msgButtons, MessageBoxIcon msgIco)
        {
            return MessageBox.Show(msg, "提示", msgButtons, msgIco);
        }
        public virtual void SetContent(string name,string number,string id)
        {
            textBox1.Text = name;
            textBox2.Text = number;
            textID.Text = id;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command">标志</param>
        /// <param name="addOrUpdate">增加或修改</param>
        /// <param name="bytesArray">id</param>
        /// <param name="bytesArray">所属于</param>
        public void GetContent(byte command,byte[] bytesId)
        {
            List<byte> ListArray = new List<byte>();
            byte[] byteArray = ByteWithString.encodeingToByte(textBox2.Text, "utf-8");//名称
            byte[] byteArrayRemake = ByteWithString.encodeingToByte(textBox1.Text, "utf-8");//编号
            ListArray.AddRange(ByteWithString.intTo4Byte(byteArray.Length).ToList());//名称长度
            ListArray.AddRange(byteArray.ToList());//名称
            ListArray.AddRange(ByteWithString.intTo4Byte(byteArrayRemake.Length).ToList());//编号长度
            ListArray.AddRange(byteArrayRemake.ToList());//编号
            ListArray.AddRange(ByteWithString.intTo4Byte(int.Parse(comboBox1.SelectedValue.ToString())).ToList());//所属的id
            ListArray.AddRange(bytesId);//id
            theScoket.theSocketSend<byte[]>(command, ListArray.ToArray());//
        }
    }
}
