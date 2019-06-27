using System;
using System.Runtime.InteropServices;


namespace LgwAppFrame.Code
{
    /// <summary>
    /// 识别文件中的数字与字母
    /// </summary>
    /// <remarks>对于汉字专业的可以使用Tesseract3来做，但如果简单使用汉字可以使用Onenote</remarks>
    public class OCRHelper
    {
        /// <summary>
        /// 识别文件，返回字符串的平台特定的整数类型
        /// </summary>
        /// <param name="file"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [DllImport("AspriseOCR.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "OCR")]
        public static extern IntPtr OCR(string file, int type);
        /// <summary>
        /// 识别区域内图片文件中的字母与数字
        /// </summary>
        /// <param name="file"></param>
        /// <param name="type"></param>
        /// <param name="startX">识别区域坐标X</param>
        /// <param name="startY">识别区域坐标Y</param>
        /// <param name="width">识别区域宽</param>
        /// <param name="height">识别区域高</param>
        /// <returns></returns>
        [DllImport("AspriseOCR.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "OCRpart")]
        static extern IntPtr OCRpart(string file, int type, int startX, int startY, int width, int height);

        [DllImport("AspriseOCR.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "OCRBarCodes")]
        static extern IntPtr OCRBarCodes(string file, int type);

        [DllImport("AspriseOCR.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "OCRpartBarCodes")]
        static extern IntPtr OCRpartBarCodes(string file, int type, int startX, int startY, int width, int height);
        /// <summary>
        /// 识别图片文件中的字母与数字
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
         public static string OCR(string file)
        {
            if (FileHelper.IsExistFile(file))
            return Marshal.PtrToStringAnsi(OCR(file, -1));
            return "";
        }
    }
}
