﻿using System.Net;
using System.Net.Sockets;

namespace LgwAppFrame.SocketHelper
{
    internal class UdpTransmitBox : TransmitData
    {
        internal EndPoint RemoteEP = new IPEndPoint(IPAddress.Any, 0);
        private SocketAsyncEventArgs _sendSocketArgs = new SocketAsyncEventArgs();
        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="socket">Socket</param>
        /// <param name="bufferSize">缓冲区</param>
        internal UdpTransmitBox(Socket socket, int bufferSize) : base(socket, bufferSize) { }
        /// <summary>
        /// 异步SocketAsyncEventArgs，发送用到的；
        /// </summary>
        internal SocketAsyncEventArgs SendSocketArgs
        {
            get { return _sendSocketArgs; }
            set { _sendSocketArgs = value; }
        }
        /// <summary>
        /// 接收到数据之后提取对方的终结点
        /// </summary>
        internal IPEndPoint ServerIpEndPoint
        {
            get
            {
                return (IPEndPoint)RemoteEP;
            }
        }
    }
}
