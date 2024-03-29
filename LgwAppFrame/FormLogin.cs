﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LgwAppFrame
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        #region 无边框拖动效果
        [DllImport("user32.dll")]//拖动无窗体的控件
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;
        private void FormLogin_MouseDown(object sender, MouseEventArgs e)
        {   
            //拖动窗体
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }
        #endregion
        #region 登陆过程
        private void  login()
        {
            try
            {
                if(string.IsNullOrWhiteSpace(this.tbUsername.Text))
                {
                    MyFuncLib.msg("登录失败  帐号不能空", "w");
                    return;
                }
                if (string.IsNullOrEmpty(this.tbPassword.Text))
                {
                    MyFuncLib.msg("登录失败  密码不能空", "w");
                    return;
                }
            }
            catch
            {

            }
        }
        #endregion 

        private void btnLogin_Click(object sender, EventArgs e)
        {
            login();
        }
    }
}

