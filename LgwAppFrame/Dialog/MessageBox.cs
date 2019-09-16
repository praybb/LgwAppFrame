using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LgwAppFrame
{
    public partial class MessageBox : Form
    {
        public Boolean result;
        public string labelText = string.Empty;
        public string imageType;
        
        public MessageBox()
        {
            InitializeComponent();
        }

        private void MessageBox_Load(object sender, EventArgs e)
        {
            if (imageType.Equals("w"))
                this.pictureBox1.Image = global::LgwAppFrame.Properties.Resources.warning24;
            else if (imageType.Equals("e"))
                this.pictureBox1.Image = global::LgwAppFrame.Properties.Resources.error;
            else if (pictureBox1.Equals("h"))
                this.pictureBox1.Image = global::LgwAppFrame.Properties.Resources.help24;
            else
                this.pictureBox1.Image = global::LgwAppFrame.Properties.Resources.info24;
            this.label1.Text = this.labelText;
            result = false;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            result = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            result = false;
            this.Close();
        }
    }
}
