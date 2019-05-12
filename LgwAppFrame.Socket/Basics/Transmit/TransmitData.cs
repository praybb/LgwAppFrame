using System.Net.Sockets;
using System.Net;

namespace LgwAppFrame.SocketHelper
{ 
    /// <summary>
    /// 传输盒子内的数据
    /// </summary>
    /// <remarks>暗号是放在数据里面的</remarks>
    internal class TransmitData
    {
        #region 工作的socket
        /// <summary>
        /// 工作的socket
        /// </summary>
        private Socket _workSocket = null;
        /// <summary>
        /// 工作的Socket
        /// </summary>
        internal Socket WorkSocket
        {
            get
            {
                return _workSocket;
            }
            set { _workSocket = value; }
        }
        #endregion


        #region 客户端地址,端口号
        /// <summary>
        /// 客户端地址,端口号
        /// </summary>
        private IPEndPoint ipEndPoint = null;
        /// <summary>
        /// IPEndPoint得到客户端地址,端口号；
        /// </summary>
        internal IPEndPoint IpEndPoint
        {
            get { return ipEndPoint; }
            set { ipEndPoint = value; }
        }
        #endregion
        #region 缓冲区大小
        /// <summary>
        /// 缓冲区大小
        /// </summary>
        private int _bufferSize = 1024;
        /// <summary>
        /// 缓冲区大小
        /// </summary>
        internal int BufferSize
        {
            get { return _bufferSize; }
        }
        #endregion
        #region 缓冲区
        /// <summary>
        /// 缓冲区
        /// </summary>
        private byte[] _buffer = null;
        /// <summary>
        /// 缓冲区
        /// </summary>
        internal byte[] Buffer
        {
            get { return _buffer; }
            set { _buffer = value; }
        }
        #endregion
        #region  要发送的数据
        /// <summary>
        ///  要发送的数据
        /// </summary>
        private byte[] _sendDate = null;
        /// <summary>
        /// 要发送的数据
        /// </summary>
        /// <remarks>主要用于对方没有收到信息可以重发用</remarks>
        internal byte[] SendDate
        {
            get { return _sendDate; }
            set { _sendDate = value; }
        }
        #endregion
        #region 发送数据的标签
        /// <summary>
        /// 发送数据的标签
        /// </summary>
        private int _sendDateLabel = 0;
        /// <summary>
        /// 要发送的数据标签
        /// </summary>
        internal int SendDateLabel
        {
            get { return _sendDateLabel; }
            set { _sendDateLabel = value; }
        }
        #endregion
        #region 发送文件类
        /// <summary>
        /// 有文件要发送了在这里进行设置
        /// </summary>
        private TransmitFile _sendFile = null;
        /// <summary>
        /// 发送文件类
        /// </summary>
        internal TransmitFile SendFile
        {
            get { return _sendFile; }
            set { _sendFile = value; }
        }
        #endregion
        #region 接收文件类
        /// <summary>
        /// 接收文件类
        /// </summary>
        /// <remarks>需要接收文件在这里进行设置</remarks>
        private TransmitFile _receiveFile = null;
        /// <summary>
        /// 接收文件类
        /// </summary>
        internal TransmitFile ReceiveFile
        {
            get { return _receiveFile; }
            set { _receiveFile = value; }
        }
        #endregion
        #region 备用缓冲区
        /// <summary>
        /// 备用缓冲区
        /// </summary>
        /// <remarks>主要是缓冲区有时候需要增大或缩小的时候用到；</remarks>
        private byte[] _bufferBackup = null;
        /// <summary>
        /// 备用缓冲区;
        /// </summary>
        /// <remarks>主要是缓冲区有时候需要增大或缩小的时候用到</remarks>
        internal byte[] BufferBackup
        {
            get { return _bufferBackup; }
            set { _bufferBackup = value; }
        }
        #endregion


        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="socket">Socket</param>
        /// <param name="bufferSize">缓冲区大小</param>
        internal TransmitData(Socket socket, int bufferSize)
        {
            _bufferSize = bufferSize;
            _buffer = new byte[bufferSize];
            _workSocket = socket;
            try
            {
                ipEndPoint = (IPEndPoint)socket.RemoteEndPoint;
            }
            catch { }
        }

        /// <summary>
        /// 同时设置发送数据和它的标签的方法
        /// </summary>
        /// <param name="Lable">标签</param>
        /// <param name="sendDate">要发送数据</param>
        internal void SendDateInitialization(int Lable, byte[] sendDate)
        {
            _sendDateLabel = Lable;
            _sendDate = sendDate;
        }

    }
}
