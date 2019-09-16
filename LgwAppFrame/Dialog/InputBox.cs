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
    public partial class InputBox : Form
    {
        /// <summary>
        /// 对话框输入类型, string表示文本框输入, password表示文本框输入, numeric表示数值框输入, date表示日期框输入, datetime表示日期框输入
        /// </summary>
        public string inputType = string.Empty;
        /// <summary>
        /// 返回值是字符串
        /// </summary>
        public string returnStringValue = string.Empty;
        /// <summary>
        /// 返回值是数值
        /// </summary>
        public string returnDecimalValue = string.Empty;
        /// <summary>
        /// 返回值是日期
        /// </summary>
        public string returnDateValue = string.Empty;
        /// <summary>
        /// 字符串默认值
        /// </summary>
        public string defaultStringValue;
        /// <summary>
        /// 数值默认值
        /// </summary>
        public decimal defaultDecimalValue;
        /// <summary>
        /// 日期默认值
        /// </summary>
        public DateTime defaultDateValue;
        /// <summary>
        /// 字符串格式化
        /// </summary>
        public string formatString;
        /// <summary>
        /// 提示文本
        /// </summary>
        public string labelText = string.Empty;
        /// <summary>
        /// 窗体标题
        /// </summary>
        public string formTitle = string.Empty;
        /// <summary>
        /// 设置隐藏输入字符时的符号
        /// </summary>
        public char passwordChar;
        /// <summary>
        /// 是否输入了值
        /// </summary>
        public bool isInput = false;
        /// <summary>
        /// 小数位数
        /// </summary>
        public int DecimalPlaces = 0;
        /// <summary>
        /// 字符串输入最大长度
        /// </summary>
        public int MaxLength = 300;

        public InputBox()
        {
            InitializeComponent();
        }

        private void brnOk_Click(object sender, EventArgs e)
        {
            if (inputType.Equals("string") || inputType.Equals("password"))
                this.returnStringValue = this.tbInput.Text;
            else if (inputType.Equals("numeric"))
                this.returnDecimalValue = this.nudInput.Value.ToString();
            else if (inputType.Equals("date"))
                this.returnDateValue = this.dtpInput.Value.ToString();
            else if (inputType.Equals("datetime"))
                this.returnDateValue = this.dtpInput.Value.ToString();
            isInput = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            isInput = false;
            this.Close();
        }

        private void InputBox_Load(object sender, EventArgs e)
        {
            this.Text = "输入框";
            if (!string.IsNullOrEmpty(this.formTitle))
                this.Text = this.formTitle;
            this.lNoticeLabel.Text = "请输入：";
            if (!string.IsNullOrEmpty(this.labelText))
                this.lNoticeLabel.Text = this.labelText;
            if (inputType.Equals("string") || inputType.Equals("password"))
            {
                this.nudInput.Visible = false;
                this.dtpInput.Visible = false;
                this.dtpTimeInput.Visible = false;
                this.tbInput.Location = new Point(12, 42);
                this.tbInput.Text = this.defaultStringValue;
                this.tbInput.MaxLength = MaxLength;
                if (!string.IsNullOrEmpty(passwordChar.ToString()))
                    this.tbInput.PasswordChar = passwordChar;
                if (inputType.Equals("password"))
                    this.tbInput.PasswordChar = '*';
            }
            else if (inputType.Equals("numeric"))
            {
                this.dtpInput.Visible = false;
                this.tbInput.Visible = false;
                this.dtpTimeInput.Visible = false;
                this.nudInput.Value = defaultDecimalValue;
                this.nudInput.DecimalPlaces = DecimalPlaces;
                this.nudInput.Location = new Point(42, 42);
            }
            else if (inputType.Equals("date"))
            {
                this.nudInput.Visible = false;
                this.tbInput.Visible = false;
                this.dtpTimeInput.Visible = false;
                this.dtpInput.Location = new Point(42, 42);
                this.dtpInput.Format = DateTimePickerFormat.Custom;
                formatString = "yyyy-MM-dd";
                if (!string.IsNullOrEmpty(formatString))
                    this.dtpInput.CustomFormat = formatString;
                if (defaultDateValue != null && defaultDateValue.Year > 1753)
                    this.dtpInput.Value = defaultDateValue;
            }
            else if (inputType.Equals("datetime"))
            {
                this.nudInput.Visible = false;
                this.tbInput.Visible = false;
                this.dtpInput.Visible = false;
                this.dtpTimeInput.Location = new Point(42, 42);
                this.dtpTimeInput.Format = DateTimePickerFormat.Custom;
                formatString = "yyyy-MM-dd HH:mm";
                if (!string.IsNullOrEmpty(formatString))
                    this.dtpTimeInput.CustomFormat = formatString;
                if (defaultDateValue != null && defaultDateValue.Year > 1753)
                    this.dtpTimeInput.Value = defaultDateValue;
            }
            this.tbInput.Text = this.defaultStringValue;
          
        }

        private void tbInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                this.returnStringValue = this.tbInput.Text;
                this.isInput = true;
                this.Close();
            }
        }
    }
}
