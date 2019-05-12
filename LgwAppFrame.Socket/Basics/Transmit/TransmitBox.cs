using System;
using System.Net.Sockets;

namespace LgwAppFrame.SocketHelper
{
    /// <summary>
    /// 传输用的盒子
    /// </summary>
    internal class TransmitBox : TransmitData
    {
        #region 连接是否正常 ConnectOk
        /// <summary>
        /// 连接是否正常
        /// </summary>
        private bool _connectOk = false;//是否真正与对方相连接;主要用与服务器中的对象;
        /// <summary>
        /// 连接是否正常
        /// </summary>
        /// <remarks>二个作用，客户端真正关闭了引擎；服务器:是否真正与对方相连接;主要用与服务器中的对象;</remarks>
        internal bool ConnectOk
        {
            get { return _connectOk; }
            set { _connectOk = value; }
        }
        #endregion     
        #region 心跳时间 HeartTime
        private DateTime _heartTime = DateTime.Now;
        /// <summary>
        /// 心跳时间,接收到信息的时间，用于心跳设置
        /// </summary>
        internal DateTime HeartTime
        {
            get { return _heartTime; }
            set { _heartTime = DateTime.Now; }
        }
        #endregion
        /// <summary>
        /// 处理粘包之用;如果有残留,下面一个包和这个接上
        /// </summary>
        internal byte[] Residualpackage = null;
        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="socket">Socket</param>
        /// <param name="bufferSize">缓冲区大小</param>
        internal TransmitBox(Socket socket, int bufferSize) : base(socket, bufferSize)
        {
        }
    }
}
