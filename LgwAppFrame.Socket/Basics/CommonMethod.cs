using System;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;

namespace LgwAppFrame.SocketHelperHelper
{
    /// <summary>
    /// 不带参数的委托
    /// </summary>
    public delegate void EventDelegate();
    /// <summary>
    /// 带一个参数的委托
    /// </summary>
    /// <typeparam name="T1">T1</typeparam>
    /// <param name="object1"></param>
    public delegate void EventDelegate<T1>(T1 object1);
    /// <summary>
    /// 带二个参数的委托
    /// </summary>
    /// <typeparam name="T1">T1</typeparam>
    /// <typeparam name="T2">T2</typeparam>
    /// <param name="object1">object1</param>
    /// <param name="object2">object2</param>
    public delegate void EventDelegate<T1, T2>(T1 object1, T2 object2);
    /// <summary>
    /// 常用工具箱
    /// </summary>
    internal class CommonMethod
    {
        #region 域名转换为IP地址
        /// <summary>
        /// 域名转换为IP地址
        /// </summary>
        /// <param name="hostname">域名或IP地址</param>
        /// <returns>IP地址</returns>
        internal static string Hostname2ip(string hostname)
        {
            try
            {
                IPAddress ip;
                if (IPAddress.TryParse(hostname, out ip))
                    return ip.ToString();
                else
                    return Dns.GetHostEntry(hostname).AddressList[0].ToString();
            }
            catch
            {
                throw;
            }
        }
        #endregion
        #region 服务器写日志
        /// <summary>
        /// 服务器信息记录
        /// </summary>
        /// <param name="FileLog">记录地址</param>
        /// <param name="str">记录内容</param>
        internal static void FileOperate(string FileLog, string str)
        {
            if (FileLog == "")
                return;
            try
            {
                FileStream fs = new FileStream(FileLog, FileMode.Append, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                sw.WriteLine(str + DateTime.Now.ToString());
                sw.Close();
                fs.Close();
            }
            catch { throw; }
        }
        #endregion
        #region 外部调用是否需要用Invoket
        /// <summary>
        /// 外部调用是否需要用Invoket
        /// </summary>
        /// <param name="func">事件参数</param>
        internal static void eventInvoket(Action func)
        {
            Form form = Application.OpenForms.Cast<Form>().FirstOrDefault();
            if (form != null && form.InvokeRequired)
            {
                form.Invoke(func);
            }
            else
            {
                func();
            }
        }
        #endregion

        /// <summary>
        /// 具有返回值的 非bool 外部调用是否需要用Invoket
        /// </summary>
        /// <param name="func">方法</param>
        /// <returns>返回客户操作之后的数据</returns>
        internal static object eventInvoket(Func<object> func)
        {
            object haveStr;
            Form form = Application.OpenForms.Cast<Form>().FirstOrDefault();
            if (form != null && form.InvokeRequired)
            {
                haveStr = form.Invoke(func);
            }
            else
            {
                haveStr = func();
            }
            return haveStr;
        }
        /// <summary>
        /// 取文本中某个文本的右边文本
        /// </summary>
        /// <param name="AllDate">总文本</param>
        /// <param name="offstr">标志文本</param>
        /// <returns>取出的文本</returns>
        internal static string StringRight(string AllDate, string offstr)
        {
            int lastoff = AllDate.LastIndexOf(offstr) + offstr.Length;
            string haveString = AllDate.Substring(lastoff, AllDate.Length - lastoff);
            return haveString;
        }
        /// <summary>
        /// throw文本过滤;
        /// </summary>
        /// <param name="str">原文本</param>
        /// <returns>过滤之后的文本</returns>
        internal static string BetweenThrow(string str)
        {
            int dd = str.IndexOf(":");
            if (dd == 0)
                return str;
            return Between(str, ":", ".");
        }
        /// <summary>  
        /// 取文本中间内容  
        /// </summary>  
        /// <param name="str">原文本</param>  
        /// <param name="leftstr">左边文本</param>  
        /// <param name="rightstr">右边文本</param>  
        /// <returns>返回中间文本内容</returns>  
        internal static string Between(string str, string leftstr, string rightstr)
        {
            int i = str.IndexOf(leftstr) + leftstr.Length;
            string temp = str.Substring(i, str.IndexOf(rightstr, i) - i);
            return temp;
        }
        /// <summary>
        /// 读文件操作；如果打开正常返回文件流；异常返回null
        /// </summary>
        /// <param name="fileName">文件地址</param>
        /// <returns>文件流</returns>
        internal static FileStream FileStreamRead(string fileName)
        {
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                return fs;
            }
            catch { return null; }
        }
        /// <summary>
        /// 写文件操作；如果打开正常返回文件流；异常返回null
        /// </summary>
        /// <param name="fileName">文件地址</param>
        /// <returns>FileStream</returns>
        internal static FileStream FileStreamWrite(string fileName)
        {
            try
            {
                FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write);
                return fs;
            }
            catch { return null; }

        }
    }
    /// <summary>
    /// 随机类
    /// </summary>
    internal class RandomPublic
    {
        private static Random _randomNumber = new Random(500);
        private static Random _randomTime = new Random();
        /// <summary>
        /// 根据指定种子取一个随机数
        /// </summary>
        /// <param name="number">最大值</param>
        /// <returns>随机数</returns>
        internal static int RandomNumber(int number)
        {
            return _randomNumber.Next(number);
        }
        /// <summary>
        /// 根据时间为种子取一个随机数
        /// </summary>
        /// <param name="number">最大值</param>
        /// <returns>随机数</returns>
        internal static int RandomTime(int number)
        {
            return _randomTime.Next(number);
        }
    }
   
}
