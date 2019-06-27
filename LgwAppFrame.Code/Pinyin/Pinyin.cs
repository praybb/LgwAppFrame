using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace LgwAppFrame.Code
{
    /// <summary>
    /// 拼音
    /// </summary>
   public class Pinyin
    {
        private static InvokerHelper pinyinclass = new InvokerHelper("NPinyin.dll", "NPinyin", "NPinyin", "Pinyin");
        private static InvokerHelper ChineseChar = new InvokerHelper("ChnCharInfo.dll", "ChnCharInfo", "Microsoft.International.Converters.PinYinConverter", "ChineseChar");

        private static Encoding gb2312 = Encoding.GetEncoding("GB2312");

        /// <summary>
        /// 汉字转全拼
        /// </summary>
        /// <param name="strChinese"></param>
        /// <returns></returns>
        public static string ConvertToAllSpell(string strChinese)
        {
            try
            {
                if (strChinese.Length != 0)
                {
                    StringBuilder fullSpell = new StringBuilder();
                    for (int i = 0; i < strChinese.Length; i++)
                    {
                        var chr = strChinese[i];
                        fullSpell.Append(GetSpell(chr)+" ");
                    }

                    return fullSpell.ToString();
                }
            }
            catch (Exception e)
            {
               Console.WriteLine("全拼转化出错！" + e.Message);
            }

            return string.Empty;
        }

        /// <summary>
        /// 汉字转首字母
        /// </summary>
        /// <param name="strChinese"></param>
        /// <returns></returns>
        public static string GetFirstSpell(string strChinese)
        {
            //NPinyin.Pinyin.GetInitials(strChinese)  有Bug  洺无法识别
            //return NPinyin.Pinyin.GetInitials(strChinese);

            try
            {
                if (strChinese.Length != 0)
                {
                    StringBuilder fullSpell = new StringBuilder();
                    for (int i = 0; i < strChinese.Length; i++)
                    {
                        var chr = strChinese[i];
                        fullSpell.Append(GetSpell(chr)[0]);
                    }

                    return fullSpell.ToString().ToUpper();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("首字母转化出错！" + e.Message);
            }

            return string.Empty;
        }


        private static string GetSpell(char chr)
        {
            var coverchr = pinyinclass.Invokermethod<string>("GetPinyin", new object[] { chr }, new Type[] { typeof(char) }); ;

            bool isChineses = ChineseChar.Invokermethod<bool>("IsValidChar", new object[] { coverchr[0] },new Type[] { typeof(char)});

            //判断是否为汉字
          //  ChineseChar.IsValidChar();
            if (isChineses)
            {
                var tempstr = ChineseChar.Invokermethod<string[]>("Pinyins", new object[] { coverchr[0] }, new Type[] { typeof(char) });
                foreach (string value in tempstr)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        return value.Remove(value.Length - 1, 1);
                    }
                }
            }

            return coverchr;

        }
    }
}
