using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LgwAppFrame.Code
{
    public static partial class Ext
    {
        /// <summary>
        /// 取得枚举类型的说明
        /// </summary>
        /// <typeparam name="T">enum type</typeparam>
        /// <param name="enumItemName">the enum name</param>
        /// <returns></returns>
        public static string GetDescriptionByName<T>(this T enumItemName)
        {
            FieldInfo fi = enumItemName.GetType().GetField(enumItemName.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return enumItemName.ToString();
            }
        }
    }
}
