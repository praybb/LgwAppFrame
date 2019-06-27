namespace LgwAppFrame.Server
{
    partial class Server
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.ServerStartbut = new System.Windows.Forms.Button();
            this.checkbox1 = new System.Windows.Forms.CheckBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Pc_ip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Connect_time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBox_port = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ServerCloseBut = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label_zt = new System.Windows.Forms.Label();
            this.label_all = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBox_msg = new System.Windows.Forms.TextBox();
            this.Sendbut = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // ServerStartbut
            // 
            this.ServerStartbut.Location = new System.Drawing.Point(175, 19);
            this.ServerStartbut.Name = "ServerStartbut";
            this.ServerStartbut.Size = new System.Drawing.Size(75, 23);
            this.ServerStartbut.TabIndex = 0;
            this.ServerStartbut.Text = "启动服务器";
            this.ServerStartbut.UseVisualStyleBackColor = true;
            this.ServerStartbut.Click += new System.EventHandler(this.ServerStartbut_Click);
            // 
            // checkbox1
            // 
            this.checkbox1.AutoSize = true;
            this.checkbox1.Location = new System.Drawing.Point(365, 22);
            this.checkbox1.Name = "checkbox1";
            this.checkbox1.Size = new System.Drawing.Size(60, 16);
            this.checkbox1.TabIndex = 1;
            this.checkbox1.Text = "NT服务";
            this.checkbox1.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Pc_ip,
            this.Connect_time});
            this.dataGridView1.Location = new System.Drawing.Point(18, 52);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(458, 204);
            this.dataGridView1.TabIndex = 2;
            // 
            // Pc_ip
            // 
            this.Pc_ip.FillWeight = 200F;
            this.Pc_ip.HeaderText = "计算机IP";
            this.Pc_ip.Name = "Pc_ip";
            this.Pc_ip.Width = 200;
            // 
            // Connect_time
            // 
            this.Connect_time.HeaderText = "连接时间";
            this.Connect_time.Name = "Connect_time";
            this.Connect_time.Width = 200;
            // 
            // textBox_port
            // 
            this.textBox_port.Location = new System.Drawing.Point(69, 20);
            this.textBox_port.Name = "textBox_port";
            this.textBox_port.Size = new System.Drawing.Size(100, 21);
            this.textBox_port.TabIndex = 3;
            this.textBox_port.Text = "10020";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "端口号:";
            // 
            // ServerCloseBut
            // 
            this.ServerCloseBut.Enabled = false;
            this.ServerCloseBut.Location = new System.Drawing.Point(274, 19);
            this.ServerCloseBut.Name = "ServerCloseBut";
            this.ServerCloseBut.Size = new System.Drawing.Size(75, 23);
            this.ServerCloseBut.TabIndex = 5;
            this.ServerCloseBut.Text = "关闭客户端";
            this.ServerCloseBut.UseVisualStyleBackColor = true;
            this.ServerCloseBut.Click += new System.EventHandler(this.ServerCloseBut_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label_zt);
            this.groupBox1.Controls.Add(this.ServerCloseBut);
            this.groupBox1.Controls.Add(this.label_all);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.textBox_port);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Controls.Add(this.checkbox1);
            this.groupBox1.Controls.Add(this.ServerStartbut);
            this.groupBox1.Location = new System.Drawing.Point(19, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(510, 618);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "TCP服务器";
            // 
            // label_zt
            // 
            this.label_zt.AutoSize = true;
            this.label_zt.Location = new System.Drawing.Point(23, 603);
            this.label_zt.Name = "label_zt";
            this.label_zt.Size = new System.Drawing.Size(17, 12);
            this.label_zt.TabIndex = 20;
            this.label_zt.Text = "--";
            // 
            // label_all
            // 
            this.label_all.AutoSize = true;
            this.label_all.Location = new System.Drawing.Point(421, 603);
            this.label_all.Name = "label_all";
            this.label_all.Size = new System.Drawing.Size(89, 12);
            this.label_all.TabIndex = 13;
            this.label_all.Text = "当前在线人数:0";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.listView1);
            this.groupBox2.Location = new System.Drawing.Point(18, 394);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(458, 179);
            this.groupBox2.TabIndex = 19;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "收到的消息";
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.listView1.Location = new System.Drawing.Point(3, 17);
            this.listView1.Name = "listView1";
            this.listView1.ShowItemToolTips = true;
            this.listView1.Size = new System.Drawing.Size(452, 159);
            this.listView1.TabIndex = 10;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "时间";
            this.columnHeader1.Width = 120;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "客户端";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "消息内容";
            this.columnHeader3.Width = 300;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.textBox_msg);
            this.groupBox4.Controls.Add(this.Sendbut);
            this.groupBox4.Controls.Add(this.comboBox1);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Location = new System.Drawing.Point(18, 269);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(455, 103);
            this.groupBox4.TabIndex = 18;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "发送消息给客户端";
            // 
            // textBox_msg
            // 
            this.textBox_msg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_msg.Location = new System.Drawing.Point(100, 32);
            this.textBox_msg.Name = "textBox_msg";
            this.textBox_msg.Size = new System.Drawing.Size(349, 21);
            this.textBox_msg.TabIndex = 5;
            this.textBox_msg.Text = "测试文本";
            // 
            // Sendbut
            // 
            this.Sendbut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Sendbut.Enabled = false;
            this.Sendbut.Location = new System.Drawing.Point(374, 66);
            this.Sendbut.Name = "Sendbut";
            this.Sendbut.Size = new System.Drawing.Size(75, 23);
            this.Sendbut.TabIndex = 4;
            this.Sendbut.Text = "发送";
            this.Sendbut.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(100, 68);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(268, 20);
            this.comboBox1.TabIndex = 3;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(5, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 23);
            this.label3.TabIndex = 0;
            this.label3.Text = "发送的文本：";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(5, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "在线客户端：";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 646);
            this.Controls.Add(this.groupBox1);
            this.Name = "Server";
            this.Text = "server";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ServerStartbut;
        private System.Windows.Forms.CheckBox checkbox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox textBox_port;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ServerCloseBut;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pc_ip;
        private System.Windows.Forms.DataGridViewTextBoxColumn Connect_time;
        private System.Windows.Forms.Label label_all;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox textBox_msg;
        private System.Windows.Forms.Button Sendbut;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label_zt;
    }
}

