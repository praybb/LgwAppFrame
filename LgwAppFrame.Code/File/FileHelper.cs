using System;
using System.Data;
using System.IO;
using System.Text;

namespace LgwAppFrame.Code
{
    class FileHelper
    {
        /// <summary>
        /// 程序集基目录
        /// </summary>
        /// <remarks>输出D:\bin\Debug</remarks>
        public static string wedPath = AppDomain.CurrentDomain.BaseDirectory;
        #region 转换相对目录为绝对目录
        /// <summary>
        /// 转换相对目录为绝对目录
        /// </summary>
        /// <param name="RelativePath"></param>
        /// <returns></returns>
        public static string SwitchPath(string RelativePath)
        {
            RelativePath = RelativePath.Replace("/", "\\");
            var inte = RelativePath.IndexOf("\\");
            if (RelativePath.IndexOf("\\") == 0)
                RelativePath = RelativePath.Substring(1);
            return wedPath + "\\" + RelativePath;
        }
        #endregion
        #region 检测
        #region 检测路径目录是否存在
        /// <summary>
        /// 检测路径目录是否存在
        /// </summary>
        /// <param name="directoryPath">目录的路径</param>
        /// <returns></returns>
        public static bool IsExistDirectory(string directoryPath)
        {

            return Directory.Exists(directoryPath);
        }
        /// <summary>
        /// 检测路径目录是否存在
        /// </summary>
        /// <param name="directoryPath">目录的路径</param>
        /// <param name="Relative">true相对路径</param>
        /// <returns></returns>
        public static bool IsExistDirectory(string directoryPath, bool Relative)
        {
            if (Relative)
                return IsExistDirectory(SwitchPath(directoryPath));
            return IsExistDirectory(directoryPath);
        }
        #endregion
        #region 检测路径文件是否存在
        /// <summary>
        /// 检测路径文件是否存在
        /// </summary>
        /// <param name="filePath">文件的路径</param>        
        public static bool IsExistFile(string filePath)
        {
            return File.Exists(filePath);
        }
        /// <summary>
        /// 检测路径文件是否存在
        /// </summary>
        /// <param name="filePath">文件的路径</param>
        /// <param name="Relative">true相对路径</param>
        /// <returns></returns>
        public static bool IsExistFile(string filePath, bool Relative)
        {
            if (Relative)
                return File.Exists(SwitchPath(filePath));
            return File.Exists(filePath);
        }
        #endregion

        #region 检测指定目录是否为空
        /// <summary>
        /// 检测指定目录是否为空
        /// </summary>
        /// <param name="directoryPath">指定目录的路径</param>        
        public static bool IsEmptyDirectory(string directoryPath)
        {
            try
            {
                //判断是否存在文件
                string[] fileNames = GetFileNames(directoryPath);
                if (fileNames.Length > 0)
                {
                    return false;
                }

                //判断是否存在文件夹
                string[] directoryNames = GetDirectories(directoryPath);
                if (directoryNames.Length > 0)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                //这里记录日志
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                return true;
            }
        }
        /// <summary>
        /// 检测指定目录是否为空
        /// </summary>
        /// <param name="directoryPath">指定目录的路径</param>
        /// <param name="Relative">true相对路径</param>
        /// <returns></returns>
        public static bool IsEmptyDirectory(string directoryPath, bool Relative)
        {
            if (Relative)
                return IsEmptyDirectory(SwitchPath(directoryPath));
            return IsEmptyDirectory(directoryPath);
        }
        #endregion

        #region 检测绝对路径目录中是否存在指定的文件(不包括子目录)
        /// <summary>
        /// 检测指定目录中是否存在指定的文件(不包括子目录)
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>    
        /// <remarks>若要搜索子目录请使用重载方法.</remarks>
        public static bool Contains(string directoryPath, string searchPattern)
        {
            return Contains(directoryPath, searchPattern, false);
        }
        #endregion
        #region 检测绝对路径目录中是否存在指定的文件
        /// <summary>
        /// 检测绝对路径目录中是否存在指定的文件(包括子目录)
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param> 
        /// <param name="isSearchChild">是否搜索子目录</param>
        public static bool Contains(string directoryPath, string searchPattern, bool isSearchChild)
        {
            try
            {
                //获取指定的文件列表
                string[] fileNames = GetFileNames(directoryPath, searchPattern, isSearchChild);

                //判断指定文件是否存在
                if (fileNames.Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
            }
        }
        #endregion

        #endregion
        #region 操作
        #region 创建路径目录
        /// <summary>
        /// 创建路径目录
        /// </summary>
        /// <param name="directoryPath">目录</param>
        /// <param name="Relative">true相对路径</param>
        public static void CreateDirectory(string directoryPath, bool Relative)
        {
            if (directoryPath.Length == 0) return;
            if (Relative)
                CreateDirectory(SwitchPath(directoryPath));
            CreateDirectory(directoryPath);

        }
        /// <summary>
        /// 创建路径目录
        /// </summary>
        /// <param name="directoryPath">绝对目录</param>
        public static void CreateDirectory(string directoryPath)
        {
            //如果目录不存在则创建该目录
            if (!IsExistDirectory(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);

            }
        }
        #endregion
        #region 输出内容追创建追加到文件
        /// <summary>
        /// 输出内容追创建追加到文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="content">内容</param>
        /// <param name="Relative">true相对路径</param>
        public static void OutputCreateFile(string path, string content, bool Relative)
        {
            if (Relative)
                OutputCreateFile(SwitchPath(path), content);
            OutputCreateFile(path, content);
        }
        /// <summary>
        /// 输出内容并创建绝对路径文件(字符)
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="content">内容</param>
        public static void OutputCreateFile(string path, string content)
        {
            FileInfo fi = new FileInfo(path);
            var di = fi.Directory;
            if (!di.Exists)
            {
                di.Create();
            }
            Writecontent(path, true, content);
        }

        #endregion
        # region 输出字节追创建追加到文件
        /// <summary>
        /// 输出字节追创建追加到文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="buffer">二进制数据字节流</param>
        /// <param name="Relative">true相对路径</param>
        public static void CreateFile(string filePath, byte[] buffer, bool Relative)
        {
            if (Relative)
                CreateFile(SwitchPath(filePath), buffer);
            CreateFile(filePath, buffer);
        }
        /// <summary>
        /// 创建一个文件,并将字节流写入文件。
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="buffer">二进制数据字节流</param>
        public static void CreateFile(string filePath, byte[] buffer)
        {
            try
            {
                //如果文件不存在则创建该文件
                if (!IsExistFile(filePath))
                {
                    //创建一个FileInfo对象
                    FileInfo file = new FileInfo(filePath);

                    //创建文件
                    FileStream fs = file.Create();

                    //写入二进制流
                    fs.Write(buffer, 0, buffer.Length);

                    //关闭文件流
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                throw ex;
            }
        }
        #endregion

        #region 创建绝对路径文件(字节)
        /// <summary>
        /// 创建绝对路径文件(字节)
        /// </summary>
        /// <param name="filePath">文件的路径</param>
        /// <param name="Relative">true相对</param>
        public static void CreateFile(string filePath, bool Relative)
        {
            if (Relative)
                CreateFile(SwitchPath(filePath));
            CreateFile(filePath);
        }
        /// <summary>
        /// 创建绝对路径文件(字节)
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static void CreateFile(string filePath)
        {
            try
            {
                //如果文件不存在则创建该文件
                if (!IsExistFile(filePath))
                {
                    //创建一个FileInfo对象
                    FileInfo file = new FileInfo(filePath);

                    //创建文件
                    FileStream fs = file.Create();

                    //关闭文件流
                    fs.Close();
                }


            }
            catch (Exception ex)
            {

                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                throw ex;
            }

        }
        #endregion


        #region 删除路径目录
        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="dir">要删除的目录路径和名称</param>
        public static void DeleteDir(string dir, bool Relative)
        {
            if (dir.Length == 0) return;
            if (Relative)
                DeleteDir(SwitchPath(dir));


        }
        /// <summary>
        /// 删除绝对路径目录
        /// </summary>
        /// <param name="dir"></param>
        public static void DeleteDir(string dir)
        {
            if (Directory.Exists(dir))
                Directory.Delete(dir, false);
        }
        #endregion
        #region 删除路径文件
        /// <summary>
        /// 删除相对路径文件
        /// </summary>
        /// <param name="file">要删除的文件路径和名称</param>
        public static void DeleteFile(string file, bool Relative)
        {
            if (Relative)
                DeleteFile(SwitchPath(file));
            DeleteFile(file);
        }
        /// <summary>
        /// 删除绝对路径文件
        /// </summary>
        /// <param name="file">要删除的文件路径和名称</param>
        public static void DeleteFile(string file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
            }
        }
        #endregion

        #region 删除指定文件夹对应其他文件夹里的文件
        /// <summary>
        /// 删除指定文件夹对应其他文件夹里的文件
        /// </summary>
        /// <param name="varFromDirectory">指定文件夹路径</param>
        /// <param name="varToDirectory">对应其他文件夹路径</param>
        public static void DeleteFolderFiles(string varFromDirectory, string varToDirectory)
        {
            Directory.CreateDirectory(varToDirectory);

            if (!Directory.Exists(varFromDirectory)) return;

            string[] directories = Directory.GetDirectories(varFromDirectory);

            if (directories.Length > 0)
            {
                foreach (string d in directories)
                {
                    DeleteFolderFiles(d, varToDirectory + d.Substring(d.LastIndexOf("\\")));
                }
            }


            string[] files = Directory.GetFiles(varFromDirectory);

            if (files.Length > 0)
            {
                foreach (string s in files)
                {
                    File.Delete(varToDirectory + s.Substring(s.LastIndexOf("\\")));
                }
            }
        }
        #endregion


        #region 将文件移动到指定目录
        /// <summary>
        /// 将文件移动到指定目录
        /// </summary>
        /// <param name="dir1">需要移动的源文件的路径</param>
        /// <param name="dir2">移动到的目录的路径</param>
        /// <param name="Relative">true相对目录</param>
        public static void MoveFile(string dir1, string dir2, bool Relative)
        {
            if (Relative)
                MoveFile(SwitchPath(dir1), SwitchPath(dir2));
            MoveFile(dir1, dir2);
        }

        /// <summary>
        /// 将文件移动到指定目录
        /// </summary>
        /// <param name="sourceFilePath">需要移动的源文件的绝对路径</param>
        /// <param name="descDirectoryPath">移动到的目录的绝对路径</param>
        public static void MoveFile(string sourceFilePath, string descDirectoryPath)
        {
            //获取源文件的名称
            string sourceFileName = GetFileName(sourceFilePath);

            if (IsExistDirectory(descDirectoryPath))
            {
                //如果目标中存在同名文件,则删除
                if (IsExistFile(descDirectoryPath + "\\" + sourceFileName))
                {
                    DeleteFile(descDirectoryPath + "\\" + sourceFileName);
                }
                //将文件移动到指定目录
                File.Move(sourceFilePath, descDirectoryPath + "\\" + sourceFileName);
            }
        }
        #endregion

        #region 复制路径文件
        /// <summary>
        /// 复制路径文件
        /// </summary>
        /// <param name="sourceFilePath">源文件的路径</param>
        /// <param name="destFilePath">目标文件的路径</param>
        /// <param name="Relative">ture相对目录</param>
        public static void CopyFile(string sourceFilePath, string destFilePath, bool Relative)
        {
            if (Relative)
                Copy(SwitchPath(sourceFilePath), SwitchPath(destFilePath));
            Copy(sourceFilePath, destFilePath);
        }
        /// <summary>
        /// 复制绝对路径文件
        /// </summary>
        /// <param name="sourceFilePath">源文件的绝对路径</param>
        /// <param name="destFilePath">目标文件的绝对路径</param>
        public static void Copy(string sourceFilePath, string destFilePath)
        {
            if (File.Exists(sourceFilePath))
            {
                File.Copy(sourceFilePath, destFilePath, true);
            }
        }
        #endregion
        #region 复制绝对路径文件夹(包括文件)
        /// <summary>
        /// 复制绝对路径文件夹(包括文件)
        /// </summary>
        /// <param name="varFromDirectory">源文件夹绝对路径</param>
        /// <param name="varToDirectory">目标文件夹绝对路径</param>
        public static void CopyFolder(string varFromDirectory, string varToDirectory)
        {
            Directory.CreateDirectory(varToDirectory);

            if (!Directory.Exists(varFromDirectory)) return;

            string[] directories = Directory.GetDirectories(varFromDirectory);

            if (directories.Length > 0)
            {
                foreach (string d in directories)
                {
                    CopyFolder(d, varToDirectory + d.Substring(d.LastIndexOf("\\")));
                }
            }
            string[] files = Directory.GetFiles(varFromDirectory);
            if (files.Length > 0)
            {
                foreach (string s in files)
                {
                    File.Copy(s, varToDirectory + s.Substring(s.LastIndexOf("\\")), true);
                }
            }
        }
        #endregion





        #endregion
        #region 获取绝对路径目录中的文件列表
        /// <summary>
        /// 获取指定目录中所有文件列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>        
        public static string[] GetFileNames(string directoryPath)
        {
            //如果目录不存在，则抛出异常
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }

            //获取文件列表
            return Directory.GetFiles(directoryPath);
        }
        #endregion
        #region 获取绝对路径目录中子目录列表
        /// <summary>
        /// 获取绝对路径目录中子目录列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>    
        /// <remarks>若要搜索嵌套的子目录列表,请使用重载方法.</remarks>
        public static string[] GetDirectories(string directoryPath)
        {
            try
            {
                return Directory.GetDirectories(directoryPath);
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 获取绝对路径目录匹配字符的目录的列表
        /// <summary>
        /// 获取绝对路径目录匹配字符的目录的列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        public static string[] GetDirectories(string directoryPath, string searchPattern, bool isSearchChild)
        {
            try
            {
                if (isSearchChild)
                {
                    return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                else
                {
                    return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
        #endregion
        #region 获取绝对路径目录匹配字符的文件的列表
        /// <summary>
        /// 获取绝对路径目录匹配字符的文件的列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        public static string[] GetFileNames(string directoryPath, string searchPattern, bool isSearchChild)
        {
            //如果目录不存在，则抛出异常
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }

            try
            {
                if (isSearchChild)
                {
                    return Directory.GetFiles(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                else
                {
                    return Directory.GetFiles(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 获取绝对路径中的文件名(不包含扩展名)
        /// <summary>
        /// 获取绝对路径中的文件名(不包含扩展名)
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static string GetFileNameNoExtension(string filePath)
        {
            //获取文件的名称
            FileInfo fi = new FileInfo(filePath);
            return fi.Name.Split('.')[0];
        }
        #endregion
        #region 获取绝对路径中的文件获取扩展名
        /// <summary>
        /// 获取绝对路径中的文件获取扩展名
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static string GetExtension(string filePath)
        {
            //获取文件的名称
            FileInfo fi = new FileInfo(filePath);
            return fi.Extension;
        }
        #endregion
        #region 获取绝对路径中的文件名(包含扩展名)
        /// <summary>
        /// 获取绝对路径中的文件名(包含扩展名)
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static string GetFileName(string filePath)
        {
            //获取文件的名称
            FileInfo fi = new FileInfo(filePath);
            return fi.Name;
        }
        #endregion

        #region 文件操作
        #region 保存到字符串内容到文件(字符流)
        /// <summary>
        /// 保存到字符串内容到文件(字符流)
        /// </summary>
        /// <param name="path">绝对路径</param>
        /// <param name="isCover">是否覆盖,trun为追加,flase为覆盖</param>
        /// <param name="content">内容</param>
        /// <param name="encoding">指定编码</param>
        public static void Writecontent(string path, bool isCover, string content, Encoding encoding)
        {
            StreamWriter sw = new StreamWriter(path, isCover, encoding);
            sw.Write(content);
            sw.Close();
        }
        /// <summary>
        /// 保存到字符串内容到文件(字符流)
        /// </summary>
        /// <param name="path">绝对路径</param>
        /// <param name="isCover">是否覆盖,trun为追加,flase为覆盖</param>
        /// <param name="content">内容</param>
        public static void Writecontent(string path, bool isCover, string content)
        {
            Writecontent(path, isCover, content, System.Text.Encoding.GetEncoding("GB2312"));
        }
        #endregion

        #region 保存到字符串内容到文件(字节流)
        /// <summary>
        /// 保存到字符串内容到文件(字节流)
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="content">写入的内容</param>
        public static void AppendText(string filePath, string content)
        {
            File.AppendAllText(filePath, content);
        }
        #endregion
        #region 获取绝对路径文本文件的行数
        /// <summary>
        /// 获取绝对路径文本文件的行数
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static int GetLineCount(string filePath)
        {
            //将文本文件的各行读到一个字符串数组中
            string[] rows = File.ReadAllLines(filePath);

            //返回行数
            return rows.Length;
        }
        #endregion
        #region 获取绝对路径文件的长度
        /// <summary>
        /// 获取绝对路径文件的长度,单位为Byte
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static long GetFileSize(string filePath)
        {
            //创建一个文件对象
            FileInfo fi = new FileInfo(filePath);

            //获取文件的大小
            return fi.Length;
        }
        #endregion
        #region 获取文件大小并以B，KB，GB，TB
        /// <summary>
        /// 计算文件大小函数(保留两位小数),Size为字节大小
        /// </summary>
        /// <param name="size">初始文件大小</param>
        /// <returns></returns>
        public static string ToFileSize(long size)
        {
            string m_strSize = "";
            long FactSize = 0;
            FactSize = size;
            if (FactSize < 1024.00)
                m_strSize = FactSize.ToString("F2") + " 字节";
            else if (FactSize >= 1024.00 && FactSize < 1048576)
                m_strSize = (FactSize / 1024.00).ToString("F2") + " KB";
            else if (FactSize >= 1048576 && FactSize < 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00).ToString("F2") + " MB";
            else if (FactSize >= 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " GB";
            return m_strSize;
        }
        #endregion
        #region 清空文件内容
        /// <summary>
        /// 清空文件内容
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static void ClearFile(string filePath)
        {
            //删除文件
            File.Delete(filePath);

            //重新创建该文件
            CreateFile(filePath);
        }
        #endregion
        #region 清空指定目录
        /// <summary>
        /// 清空指定目录下所有文件及子目录,但该目录依然保存.
        /// </summary>
        /// <param name = "directoryPath" > 指定目录的绝对路径 </ param >
        public static void ClearDirectory(string directoryPath)
        {

            if (IsExistDirectory(directoryPath))
            {
                //删除目录中所有的文件
                string[] fileNames = GetFileNames(directoryPath);
                for (int i = 0; i < fileNames.Length; i++)
                {
                    DeleteFile(fileNames[i]);
                }
                //删除目录中所有的子目录
                string[] directoryNames = GetDirectories(directoryPath);
                for (int i = 0; i < directoryNames.Length; i++)
                {
                    DeleteDir(directoryNames[i]);
                }
            }
        }
        #endregion
        #endregion


        #region 根据时间得到目录名 / 格式:yyyyMMdd 或者 HHmmssff
        /// <summary>
        /// 根据时间得到目录名yyyyMMdd
        /// </summary>
        /// <returns></returns>
        public static string GetDateDir()
        {
            return DateTime.Now.ToString("yyyyMMdd");
        }
        /// <summary>
        /// 根据时间得到文件名HHmmssff
        /// </summary>
        /// <returns></returns>
        public static string GetDateFile()
        {
            return DateTime.Now.ToString("HHmmssff");
        }
        #endregion
        #region 根据时间获取指定路径的 后缀名的 的所有文件
        /// <summary>
        /// 根据时间获取指定路径的 后缀名的 的所有文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="Extension">后缀名 比如.txt</param>
        /// <returns></returns>
        public static DataRow[] GetFilesByTime(string path, string Extension)
        {
            if (Directory.Exists(path))
            {
                string fielExts = string.Format("*{0}", Extension);
                string[] files = Directory.GetFiles(path, fielExts);
                if (files.Length > 0)
                {
                    DataTable table = new DataTable();
                    table.Columns.Add(new DataColumn("filename", Type.GetType("System.String")));
                    table.Columns.Add(new DataColumn("createtime", Type.GetType("System.DateTime")));
                    for (int i = 0; i < files.Length; i++)
                    {
                        DataRow row = table.NewRow();
                        DateTime creationTime = File.GetCreationTime(files[i]);
                        string fileName = Path.GetFileName(files[i]);
                        row["filename"] = fileName;
                        row["createtime"] = creationTime;
                        table.Rows.Add(row);
                    }
                    return table.Select(string.Empty, "createtime desc");
                }
            }
            return new DataRow[0];
        }
        #endregion

    }
}
