namespace 集中器控制客户端
{
    partial class MsgCircuit
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(79, 32);
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.Text = "线路名称:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(79, 70);
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.Text = "线路编号:";
            // 
            // button1
            // 
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(79, 111);
            this.label3.Text = "所属母线:";
            // 
            // MsgCircuit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 225);
            this.Name = "MsgCircuit";
            this.Text = "MsgCircuit";
            this.Load += new System.EventHandler(this.MsgCircuit_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

    }
}