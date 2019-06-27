using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace LgwAppFrame.Code
{
    public class Md5Helper
    {
        /// <summary>
        /// 取得md5值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Md5(string value)
        {
            var result = string.Empty;
            if (string.IsNullOrEmpty(value)) return result;
            using (var md5 = MD5.Create())
            {
                result = GetMd5Hash(md5, value);
            }
            return result;
        }

        /// <summary>
        /// 取得哈希值值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        static string GetMd5Hash(MD5 md5Hash, string input)
        {
            //计算输入数据的哈希值
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sBuilder = new StringBuilder();
            foreach (byte t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }
            return sBuilder.ToString();
        }
        /// <summary>
        /// 验证输入的字符与md5值是匹配
        /// </summary>
        /// <param name="md5Hash"></param>
        /// <param name="input"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            var hashOfInput = GetMd5Hash(md5Hash, input);
            var comparer = StringComparer.OrdinalIgnoreCase;
            return 0 == comparer.Compare(hashOfInput, hash);
        }
    }
}
