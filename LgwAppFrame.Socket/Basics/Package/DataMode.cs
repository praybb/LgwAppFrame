namespace LgwAppFrame.SocketHelper.Basics.Package
{
    /// <summary>
    /// 数据模型
    /// </summary>
    /// <remarks>用于对解码和外部类起到一个桥梁的作用</remarks>
    internal class DataModel
    {
        #region 暗号

        /// <summary>
        /// //暗号
        /// </summary>
        private byte _state = 0;
        /// <summary>
        /// 暗号
        /// </summary>
        internal byte State
        {
            get { return _state; }
        }
        #endregion
        #region 文件类数据
        /// <summary>
        /// 文本类数据
        /// </summary>
        private string _datestring = null;
        /// <summary>
        /// 文本类数据
        /// </summary>
        internal string Datestring
        {
            get { return _datestring; }
        }
        #endregion
        #region 数据的标签
        /// <summary>
        /// 数据的标签
        /// </summary>
        private int _sendDateLabel = 0;
        /// <summary>
        /// 数据的标签
        /// </summary>
        internal int SendDateLabel
        {
            get { return _sendDateLabel; }
        }
        #endregion
        #region 回复的数据
        /// <summary>
        /// 回复的数据
        /// </summary>
        private byte[] _replyDate = null;
        /// <summary>
        /// 回复的数据
        /// </summary>
        internal byte[] ReplyDate
        {
            get { return _replyDate; }
        }
        #endregion
        #region 字节类数据
        private byte[] _dateByte = null;//字节类数据
        /// <summary>
        /// 字节类数据
        /// </summary>
        internal byte[] DateByte
        {
            get { return _dateByte; }
        }
        #endregion

        #region 建立文本数据
        /// <summary>
        /// 建立文本数据
        /// </summary>
        /// <param name="i">j</param>
        /// <param name="str">文本内容</param>
        internal DataModel(byte i, string str)
        {
            _state = i;
            _datestring = str;
        }
        /// <summary>
        /// 建立接收正确的文本数据与回复数据
        /// </summary>
        /// <param name="i">暗号</param>
        /// <param name="str">文本内容</param>
        /// <param name="replyDate">回复的数据</param>
        internal DataModel(byte i, string str, byte[] replyDate)
        {
            _state = i;
            _datestring = str;
            _replyDate = replyDate;
        }
        #endregion
        #region 建立字节数据
        /// <summary>
        /// 建立暗号和字节集
        /// </summary>
        /// <param name="i">暗号</param>
        /// <param name="b">字节数组b</param>
        internal DataModel(byte i, byte[] b)
        {
            _state = i;
            _dateByte = b;
        }
        /// <summary>
        /// 建立接收正确的字节集数据与回复数据
        /// </summary>
        /// <param name="i">暗号</param>
        /// <param name="dateByte">字节集数据</param>
        /// <param name="replyDate">回复数据</param>
        internal DataModel(byte i, byte[] dateByte, byte[] replyDate)
        {
            _state = i;
            _dateByte = dateByte;
            _replyDate = replyDate;
        }
        #endregion
        #region 建立数据回复
        /// <summary>
        /// 建立数据回复
        /// </summary>
        /// <param name="replyDate">字节集</param>
        internal DataModel(byte[] replyDate)
        {
            _replyDate = replyDate;
        }
        #endregion
        #region  建立数据的标签与数据
        /// <summary>
        /// 建立数据的标签与数据
        /// </summary>
        /// <param name="Label">数据标签</param>
        /// <param name="dateByte">数据</param>
        internal DataModel(int Label, byte[] dateByte)
        {
            _sendDateLabel = Label;
            _dateByte = dateByte;
        }
        #endregion
        #region 暗号和数据的标签
        /// <summary>
        /// 建立暗号和数据的标签
        /// </summary>
        /// <param name="state">暗号</param>
        /// <param name="Label">数据的标签</param>
        internal DataModel(byte state, int Label)
        {
            _state = state;
            _sendDateLabel = Label;
        }
        #endregion
        /// <summary>
        /// 建立状态
        /// </summary>
        /// <param name="i"></param>
        /// <remarks>归类3,4</remarks>
        internal DataModel(byte i)
        {
            _state = i;
        }

    }
}
