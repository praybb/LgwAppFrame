using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LgwAppFrame.Code
{
    public class Md5Helper
    {
        #region 取得md5值
        /// <summary>
        /// 取得md5值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetMd5(string value)
        {
            var result = string.Empty;
            if (string.IsNullOrEmpty(value)) return result;
            using (var md5 = MD5.Create())
            {
                byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
                // Convert.ToBase64String(data);
                var sBuilder = new StringBuilder();
                foreach (byte t in data)
                {
                    sBuilder.Append(t.ToString("x2"));
                }
                return sBuilder.ToString();
            }           
        }
        #endregion
       
        #region Sha1加密
        /// <summary>
        /// 基于Sha1的自定义加密字符串方法：输入一个字符串，返回一个由40个字符组成的十六进制的哈希散列（字符串）。
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns>加密后的十六进制的哈希散列（字符串）</returns>
        public static string GetSha1(string str)
        {
            var result = string.Empty;
            if (string.IsNullOrEmpty(str)) return result;
            using (var sha1 = SHA1.Create())
            {
                var buffer = Encoding.UTF8.GetBytes(str);
                var data = sha1.ComputeHash(buffer);

                var sb = new StringBuilder();
                foreach (var t in data)
                {
                    sb.Append(t.ToString("X2"));
                }
                return sb.ToString();
            }

           
        }
        #endregion
        /// <summary>
        /// 验证输入的字符与md5值是匹配
        /// </summary>
        /// <param name="md5Hash"></param>
        /// <param name="input"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        static bool VerifyMd5(string input, string hash)
        {
            var hashOfInput = GetMd5(input);
            var comparer = StringComparer.OrdinalIgnoreCase;
            return 0 == comparer.Compare(hashOfInput, hash);
        }
        /// <summary>
        ///  验证输入的字符与sha1值是匹配
        /// </summary>
        /// <param name="md5Hash"></param>
        /// <param name="input"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        static bool VerifySha1(MD5 md5Hash, string input, string hash)
        {
            var hashOfInput = GetSha1(input);
            var comparer = StringComparer.OrdinalIgnoreCase;
            return 0 == comparer.Compare(hashOfInput, hash);
        }
    }
}
