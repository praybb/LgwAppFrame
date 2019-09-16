namespace LgwAppFrame
{
    partial class InputBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InputBox));
            this.panel2 = new System.Windows.Forms.Panel();
            this.dtpTimeInput = new System.Windows.Forms.DateTimePicker();
            this.nudInput = new System.Windows.Forms.NumericUpDown();
            this.dtpInput = new System.Windows.Forms.DateTimePicker();
            this.btnCancel = new System.Windows.Forms.Button();
            this.brnOk = new System.Windows.Forms.Button();
            this.lNoticeLabel = new System.Windows.Forms.Label();
            this.tbInput = new System.Windows.Forms.TextBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudInput)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackgroundImage = global::LgwAppFrame.Properties.Resources.dialogbg;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Controls.Add(this.dtpTimeInput);
            this.panel2.Controls.Add(this.nudInput);
            this.panel2.Controls.Add(this.dtpInput);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.brnOk);
            this.panel2.Controls.Add(this.lNoticeLabel);
            this.panel2.Controls.Add(this.tbInput);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(389, 102);
            this.panel2.TabIndex = 0;
            // 
            // dtpTimeInput
            // 
            this.dtpTimeInput.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpTimeInput.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTimeInput.Location = new System.Drawing.Point(50, 67);
            this.dtpTimeInput.Name = "dtpTimeInput";
            this.dtpTimeInput.Size = new System.Drawing.Size(138, 23);
            this.dtpTimeInput.TabIndex = 6;
            // 
            // nudInput
            // 
            this.nudInput.DecimalPlaces = 2;
            this.nudInput.Location = new System.Drawing.Point(148, 42);
            this.nudInput.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudInput.Minimum = new decimal(new int[] {
            10000,
            0,
            0,
            -2147483648});
            this.nudInput.Name = "nudInput";
            this.nudInput.Size = new System.Drawing.Size(92, 23);
            this.nudInput.TabIndex = 5;
            // 
            // dtpInput
            // 
            this.dtpInput.CustomFormat = "yyyy-MM-dd";
            this.dtpInput.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpInput.Location = new System.Drawing.Point(50, 42);
            this.dtpInput.Name = "dtpInput";
            this.dtpInput.Size = new System.Drawing.Size(92, 23);
            this.dtpInput.TabIndex = 4;
            // 
            // btnCancel
            // 
            this.btnCancel.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.btnCancel.Location = new System.Drawing.Point(333, 71);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(43, 23);
            this.btnCancel.TabIndex = 101;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // brnOk
            // 
            this.brnOk.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.brnOk.Location = new System.Drawing.Point(284, 71);
            this.brnOk.Name = "brnOk";
            this.brnOk.Size = new System.Drawing.Size(43, 23);
            this.brnOk.TabIndex = 100;
            this.brnOk.Text = "确定";
            this.brnOk.Click += new System.EventHandler(this.brnOk_Click);
            // 
            // lNoticeLabel
            // 
            this.lNoticeLabel.Location = new System.Drawing.Point(13, 13);
            this.lNoticeLabel.Name = "lNoticeLabel";
            this.lNoticeLabel.Size = new System.Drawing.Size(363, 23);
            this.lNoticeLabel.TabIndex = 1;
            this.lNoticeLabel.Text = "labelX1";
            // 
            // tbInput
            // 
            this.tbInput.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.tbInput.ForeColor = System.Drawing.Color.Black;
            this.tbInput.Location = new System.Drawing.Point(12, 42);
            this.tbInput.Name = "tbInput";
            this.tbInput.Size = new System.Drawing.Size(364, 23);
            this.tbInput.TabIndex = 0;
            this.tbInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbInput_KeyDown);
            // 
            // InputBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 102);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "InputBox";
            this.Load += new System.EventHandler(this.InputBox_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudInput)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox tbInput;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button brnOk;
        private System.Windows.Forms.Label lNoticeLabel;
        private System.Windows.Forms.DateTimePicker dtpInput;
        private System.Windows.Forms.NumericUpDown nudInput;
        private System.Windows.Forms.DateTimePicker dtpTimeInput;
    }
}