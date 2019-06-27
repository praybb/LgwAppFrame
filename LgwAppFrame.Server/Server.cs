using System;
using System.Windows.Forms;
using LgwAppFrame.SocketHelper;
using System.Net;
using System.Collections.Generic;

namespace LgwAppFrame.Server
{
    public partial class Server : Form
    {
        private ISocketServer server = null;
       
        public Server()
        {
            InitializeComponent();
        }

        private void ServerStartbut_Click(object sender, EventArgs e)
        {
            try
            {
                server = SocketStart.startServer(int.Parse(textBox_port.Text));
                server.AcceptString += new EventDelegate<IPEndPoint, string>(acceptString);
                //server.AcceptByte += new EventDelegate<IPEndPoint, byte[]>(acceptBytes);
                server.Connect += new EventDelegate<IPEndPoint>(connect);
                server.dateSuccess += new EventDelegate<IPEndPoint>(dateSuccess);
                server.Disconnection += new EventDelegate<IPEndPoint, string>(disconnection);
                server.EngineClose += new EventDelegate(engineClose);
                server.EngineLost += new EventDelegate<string>(engineLost);
                //server.BufferSize=12048;
                //server.FileLog = "C:\\test.txt";
                server.StartEngine();
                this.ServerStartbut.Enabled = false;
                this.ServerCloseBut.Enabled = true;
                this.Sendbut.Enabled = true;
                //  this.button8.Enabled = true;
            }
            catch (Exception Ex) { MessageBox.Show(Ex.Message); }
        }
        /// <summary>
        /// 当接收到来之客户端的文本信息的时候
        /// </summary>
        /// <param name="state"></param>
        /// <param name="str"></param>
        private void acceptString(IPEndPoint ipEndPoint, string str)
        {
            ListViewItem item = new ListViewItem(new string[] { DateTime.Now.ToString(), ipEndPoint.ToString(), str });
            this.listView1.Items.Insert(0, item);
        }
        #region 当有客户端连接上来的时候
        /// <summary>
        /// 当有客户端连接上来的时候
        /// </summary>
        /// <param name="state"></param>
        private void connect(IPEndPoint ipEndPoint)
        {
            show(ipEndPoint, "上线");
        }
        #endregion
        #region 发送按钮
        /// <summary>
        /// 发送按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                IPEndPoint client = (IPEndPoint)this.comboBox1.SelectedItem;
                if (client == null)
                {
                    MessageBox.Show("没有选中任何在线客户端！");
                    return;
                }

                if (!this.server.clientCheck(client))
                {
                    MessageBox.Show("目标客户端不在线！");
                    return;
                }
                server.sendMessage(client, textBox_msg.Text);
            }
            catch (Exception Ex) { MessageBox.Show(Ex.Message); }

        }
        #endregion
        #region 下面显示的
        /// <summary>
        /// 下面显示的
        /// </summary>
        /// <param name="ipEndPoint"></param>
        /// <param name="str"></param>
        private void show(IPEndPoint ipEndPoint, string str)
        {
            label_zt.Text = ipEndPoint.ToString() + ":" + str;
            label_all.Text = "当前在线人数:" + this.server.ClientNumber.ToString();
        }
        #endregion
        #region 当有客户端掉线的时候
        /// <summary>
        /// 当有客户端掉线的时候
        /// </summary>
        /// <param name="state"></param>
        /// <param name="str"></param>
        private void disconnection(IPEndPoint ipEndPoint, string str)
        {
            show(ipEndPoint, "下线");
        }
        #endregion
        #region 当对方已收到我方发送数据的时候
        /// <summary>
        /// 当对方已收到我方发送数据的时候
        /// </summary>
        /// <param name="state"></param>
        private void dateSuccess(IPEndPoint ipEndPoint)
        {
            textBox_msg.Text = "已向" + ipEndPoint.ToString() + "发送成功";
        }
        #endregion
        #region 当服务器完全关闭的时候
        /// <summary>
        /// 当服务器完全关闭的时候
        /// </summary>
        private void engineClose()
        {
            MessageBox.Show("服务器已关闭");
        }
        #endregion
        #region 当服务器非正常原因断开的时候
        /// <summary>
        /// 当服务器非正常原因断开的时候
        /// </summary>
        /// <param name="str"></param>
        private void engineLost(string str)
        {
            MessageBox.Show(str);
        }
        #endregion
        /// <summary>
        /// 关闭客户端的按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServerCloseBut_Click(object sender, EventArgs e)
        {
            IPEndPoint client = (IPEndPoint)this.comboBox1.SelectedItem;
            if (client == null)
            {
                MessageBox.Show("没有选中任何在线客户端！");
                return;
            }

            if (!this.server.clientCheck(client))
            {
                MessageBox.Show("目标客户端不在线！");
                return;
            }
            server.clientClose(client);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                List<IPEndPoint> list = this.server.ClientAll;
                this.comboBox1.DataSource = list;
            }
            catch { }
        }
    }
}
