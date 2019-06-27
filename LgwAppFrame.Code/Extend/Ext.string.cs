using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LgwAppFrame.Code
{
    public static partial class Ext
    {

        public static string Reverse(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException("参数不合法");
            }

            StringBuilder sb = new StringBuilder(str.Length);
            for (int index = str.Length - 1; index >= 0; index--)
            {
                sb.Append(str[index]);
            }
            return sb.ToString();
        }

    }
}
