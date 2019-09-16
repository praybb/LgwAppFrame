using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LgwAppFrame
{
    class MyFuncLib
    {
        public static bool Islogin { get; set; }
        #region 弹出信息
        /// <summary>
        /// 自定义MessageBox对话框,支持询问q,程序错误e,警告w,提示i
        /// </summary>
        /// <param name="lable"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool msg(string lable, string type)
        {
            MessageBox frm = new MessageBox();
            frm.labelText = lable;
            frm.imageType = type;
            if ("q".Equals(type))
                frm.Text = "询问";
            else if ("e".Equals(type))
                frm.Text = "程序错误";
            else if ("w".Equals(type))
                frm.Text = "警告";
            else
                frm.Text = "提示";
            frm.ShowDialog();
            return frm.result;
        }
        #endregion
        #region 输入信息
        /// <summary>
        /// 自定义输入对话框
        /// </summary>
        /// <param name="title">对话框标题</param>
        /// <param name="lable">文本提示</param>
        /// <returns></returns>
        public static string inputBox(string title, string lable)
        {
            InputBox frm = new InputBox();
            frm.Text = title;
            frm.labelText = lable;
            frm.inputType = "string";
            frm.ShowDialog();
            return frm.returnStringValue;
        }

        /// <summary>
        /// 自定义输入对话框
        /// </summary>
        /// <param name="title">对话框标题</param>
        /// <param name="lable">文本提示</param>
        /// <returns></returns>
        public static string inputBox(string title, string lable, DateTime date)
        {
            InputBox frm = new InputBox();
            frm.Text = title;
            frm.labelText = lable;
            frm.defaultDateValue = date;
            frm.inputType = "date";
            frm.ShowDialog();
            return frm.returnDateValue;
        }

        /// <summary>
        /// 自定义输入对话框，含字符掩码，适用于密码的输入
        /// </summary>
        /// <param name="title">对话框标题</param>
        /// <param name="lable">文本题是</param>
        /// <param name="passwordChar">字符掩码</param>
        /// <returns></returns>
        public static string inputBox(string title, string lable, char passwordChar)
        {
            InputBox frm = new InputBox();
            frm.Text = title;
            frm.labelText = lable;
            frm.inputType = "string";
            frm.passwordChar = passwordChar;
            frm.ShowDialog();
            return frm.returnStringValue;
        }
        #endregion
    }
}
