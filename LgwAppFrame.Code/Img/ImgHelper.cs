using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;


namespace LgwAppFrame.Code
{
    public class ImgHelper
    {
        #region 图片压缩
        /// <summary>
        /// 图片压缩
        /// </summary>
        /// <param name="srcImage">Image类</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <returns></returns>
        public static Image GetThumbnailImage(Image srcImage, int width, int height)
        {
            Image bitmap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bitmap);

            //设置高质量插值法 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;

            //在指定位置并且按指定大小绘制原图片的指定部分 
            g.DrawImage(srcImage, new Rectangle(0, 0, width, width),
                new Rectangle(0, 0, srcImage.Width, srcImage.Height),
                GraphicsUnit.Pixel);

            return bitmap;
        }
        #endregion
        /// <summary>
        /// 保存Base64文件为图片
        /// </summary>
        /// <param name="strBase64">Base64字符串</param>
        /// <param name="imagesPath">文件绝对路径</param>
        /// <returns>文件目录</returns>
        /// <remarks>图片大小超过1680,1050后会压缩</remarks>
        public static string SavePicture(string strBase64, string imagesPath)
        {
            byte[] bytes = Convert.FromBase64String(strBase64);          
            return SavePicture(bytes, imagesPath,1680,1050);
        }
        /// <summary>
        /// 保存byte文件为图片
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="imagesPath"></param>
        /// <returns></returns>
        public static string SavePicture(byte[] bytes, string imagesPath)
        {           
            return SavePicture(bytes, imagesPath, 1680, 1050);
        }
        /// <summary>
        /// 按比例保存bytes为图片
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="imagesPath">目录</param>
        /// <param name="Maxwidth">最大宽</param>
        /// <param name="Maxheight">最大高</param>
        /// <returns>反回相对目录</returns>
        public static string SavePicture(byte[] bytes,string imgPath,int Maxwidth,int Maxheight)
        {
            MemoryStream ms = new MemoryStream(bytes);
            ms.Write(bytes, 0, bytes.Length);
            var img = Image.FromStream(ms, true);           
            FileHelper.CreateDirectory(imgPath,true);
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", System.Globalization.DateTimeFormatInfo.InvariantInfo) + ".png";

            float ratioW = (float)img.Width / Maxwidth;
            float ratioH = (float)img.Height / Maxheight;
            if (ratioW > 1 && ratioH >1)
            {
                Image bitImage;
                if (ratioW> ratioH)                    
                {
                     bitImage = ImgHelper.GetThumbnailImage(img, Maxwidth,(int)(img.Height/ratioW));
                }
                else
                {
                     bitImage = ImgHelper.GetThumbnailImage(img, (int)(img.Width / ratioW), Maxheight);
                }
                bitImage.Save(FileHelper.SwitchPath(imgPath) +@"/"+fileName);
            }
            img.Save(FileHelper.SwitchPath(imgPath) + @"/" + fileName);          
                        
            return @"/"+imgPath + @"/" + fileName;
        }       
    }
}
