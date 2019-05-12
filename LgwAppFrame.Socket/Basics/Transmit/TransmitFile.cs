﻿namespace LgwAppFrame.SocketHelper
{
    /// <summary>
    /// 传输用文件盒子
    /// </summary>
    internal class TransmitFile
    {
        private int _fileLabel = 0;//文件的标签
        private int _fileLenth = 0;//文件总长度
        private byte _fileClassification = CipherCode._photographCode;//图片，文本或其他
        private byte[] _fileDateAll = null;//要发送的数据
        private byte[] _sendDate = null;//已发送的数据
                                        /// <summary>
                                        /// 图片，文本或其他
                                        /// </summary>
        internal byte FileClassification
        {
            get { return _fileClassification; }
        }
        /// <summary>
        /// 文件长度
        /// </summary>
        internal int FileLenth
        {
            get { return _fileLenth; }
        }
        /// <summary>
        /// 发送的数据；主要用于是否重发之用
        /// </summary>
        internal byte[] SendDate
        {
            get { return _sendDate; }
            set { _sendDate = value; }
        }
        /// <summary>
        /// 待发送的数据
        /// </summary>
        internal byte[] FileDateAll
        {
            get { return _fileDateAll; }
            set { _fileDateAll = value; }
        }
        /// <summary>
        /// 文件的标签
        /// </summary>
        internal int FileLabel
        {
            get { return _fileLabel; }
            set { _fileLabel = value; }
        }
        /// <summary>
        /// 带参数的构造函数,发送方用
        /// </summary>
        /// <param name="fileDateAll"></param>
        internal TransmitFile(byte[] fileDateAll)
        {
            _fileDateAll = fileDateAll;
        }
        /// <summary>
        /// 带参数的构造函数，接收方用
        /// </summary>
        /// <param name="fileClassification">图片，文本或其他</param>
        /// <param name="fileLabel">文件的标签</param>
        /// <param name="fileLenth">文件总长度</param>
        internal TransmitFile(byte fileClassification, int fileLabel, int fileLenth)
        {
            _fileClassification = fileClassification;
            _fileLabel = fileLabel;
            _fileLenth = fileLenth;
        }
    }
}
