namespace 集中器控制客户端
{
    partial class ShowQueryData
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.TelemeteringView = new System.Windows.Forms.DataGridView();
            this.RemoteView = new System.Windows.Forms.DataGridView();
            this.telemeteringName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.telemeteringNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.telemeteringDescriptive = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remoteName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remoteNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remoteValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TelemeteringView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RemoteView)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.TelemeteringView);
            this.groupBox1.Location = new System.Drawing.Point(12, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(359, 448);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.RemoteView);
            this.groupBox2.Location = new System.Drawing.Point(387, 24);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(359, 448);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            // 
            // TelemeteringView
            // 
            this.TelemeteringView.AllowUserToAddRows = false;
            this.TelemeteringView.AllowUserToResizeRows = false;
            this.TelemeteringView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TelemeteringView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.TelemeteringView.BackgroundColor = System.Drawing.Color.White;
            this.TelemeteringView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.TelemeteringView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.TelemeteringView.ColumnHeadersHeight = 19;
            this.TelemeteringView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.TelemeteringView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.telemeteringName,
            this.telemeteringNumber,
            this.telemeteringDescriptive});
            this.TelemeteringView.Location = new System.Drawing.Point(18, 20);
            this.TelemeteringView.Name = "TelemeteringView";
            this.TelemeteringView.ReadOnly = true;
            this.TelemeteringView.RowHeadersVisible = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.TelemeteringView.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.TelemeteringView.RowTemplate.Height = 23;
            this.TelemeteringView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TelemeteringView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.TelemeteringView.Size = new System.Drawing.Size(326, 409);
            this.TelemeteringView.TabIndex = 4;
            // 
            // RemoteView
            // 
            this.RemoteView.AllowUserToAddRows = false;
            this.RemoteView.AllowUserToResizeRows = false;
            this.RemoteView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RemoteView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.RemoteView.BackgroundColor = System.Drawing.Color.White;
            this.RemoteView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.ActiveCaption;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.RemoteView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.RemoteView.ColumnHeadersHeight = 19;
            this.RemoteView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.RemoteView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.remoteName,
            this.remoteNumber,
            this.remoteValue});
            this.RemoteView.Location = new System.Drawing.Point(16, 20);
            this.RemoteView.Name = "RemoteView";
            this.RemoteView.ReadOnly = true;
            this.RemoteView.RowHeadersVisible = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.RemoteView.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.RemoteView.RowTemplate.Height = 23;
            this.RemoteView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.RemoteView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.RemoteView.Size = new System.Drawing.Size(326, 409);
            this.RemoteView.TabIndex = 5;
            // 
            // telemeteringName
            // 
            this.telemeteringName.FillWeight = 148.7605F;
            this.telemeteringName.HeaderText = "名称";
            this.telemeteringName.Name = "telemeteringName";
            this.telemeteringName.ReadOnly = true;
            this.telemeteringName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // telemeteringNumber
            // 
            this.telemeteringNumber.FillWeight = 84.23449F;
            this.telemeteringNumber.HeaderText = "数值";
            this.telemeteringNumber.Name = "telemeteringNumber";
            this.telemeteringNumber.ReadOnly = true;
            this.telemeteringNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // telemeteringDescriptive
            // 
            this.telemeteringDescriptive.FillWeight = 67.00507F;
            this.telemeteringDescriptive.HeaderText = "品质描述";
            this.telemeteringDescriptive.Name = "telemeteringDescriptive";
            this.telemeteringDescriptive.ReadOnly = true;
            this.telemeteringDescriptive.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // remoteName
            // 
            this.remoteName.FillWeight = 148.7605F;
            this.remoteName.HeaderText = "名称";
            this.remoteName.Name = "remoteName";
            this.remoteName.ReadOnly = true;
            this.remoteName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // remoteNumber
            // 
            this.remoteNumber.FillWeight = 84.23449F;
            this.remoteNumber.HeaderText = "数值";
            this.remoteNumber.Name = "remoteNumber";
            this.remoteNumber.ReadOnly = true;
            this.remoteNumber.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // remoteValue
            // 
            this.remoteValue.FillWeight = 67.00507F;
            this.remoteValue.HeaderText = "时标";
            this.remoteValue.Name = "remoteValue";
            this.remoteValue.ReadOnly = true;
            this.remoteValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ShowQueryData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 490);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "ShowQueryData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ShowQueryData";
            this.Load += new System.EventHandler(this.ShowQueryData_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TelemeteringView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RemoteView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView TelemeteringView;
        private System.Windows.Forms.DataGridView RemoteView;
        private System.Windows.Forms.DataGridViewTextBoxColumn telemeteringName;
        private System.Windows.Forms.DataGridViewTextBoxColumn telemeteringNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn telemeteringDescriptive;
        private System.Windows.Forms.DataGridViewTextBoxColumn remoteName;
        private System.Windows.Forms.DataGridViewTextBoxColumn remoteNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn remoteValue;
    }
}