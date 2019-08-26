using System.Windows.Forms;

namespace LgwAppFrame
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        private void Form1_Load(object sender, System.EventArgs e)
        {
            try
            {
                MyFuncLib.Islogin = false;
                FormLogin f = new FormLogin();
                f.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                f.ShowDialog();
                this.timer1.Enabled = true;
                if (MyFuncLib.Islogin)
                {

                }
                else
                    this.Close();
                f = null;
            }

            catch
            {

            }
        }
    }
}
