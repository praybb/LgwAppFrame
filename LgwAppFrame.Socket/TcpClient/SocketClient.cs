using LgwAppFrame.SocketHelper.Basics.Package;
using System;
using System.Net.Sockets;
using System.Threading;

namespace LgwAppFrame.SocketHelper
{
    /// <summary>
    /// 客户端
    /// </summary>
    public class SocketClient : EngineParameter , ISocketClient
    {

        #region 基本属性区块
        private TcpTransmitBox stateOne = null;
        private Thread HeartThread = null;
        private Thread StartThread = null;
        
        #region 重连属性
        /// <summary>
        /// 当连接断开时是否重连,0为不重连,默认重连三次;
        /// </summary>
        private int _reconnectMax = 5;
        /// <summary>
        /// 当连接断开时是否重连,默认重连;
        /// </summary>
        public int ReconnectMax
        {
            get { return _reconnectMax; }
            set { _reconnectMax = value; }
        }
        #endregion
        #region 超时时间属性
        /// <summary>
        /// 登录超时时间
        /// </summary>
        private int _outTime = 10;//
        /// <summary>
        /// 登录超时时间设置，默认10秒
        /// </summary>
        public int OutTime
        {
            get { return _outTime; }
            set { _outTime = value; }
        }
        #endregion 
        /// <summary>
        /// 超时用到的临时变量
        /// </summary>
        private bool outtimebool = false;//
        /// <summary>
        /// 有没有在重连的临时变量
        /// </summary>
        private bool reconnectOn = false;//
        /// <summary>
        /// 已经重连了几次的临时变量
        /// </summary>
        private int reconnectCi = 0;//
        /// <summary>
        /// 自动重连开始的时候,触发此事件
        /// </summary>
        public event EventDelegate ReconnectionStart;
        /// <summary>
        /// 客户端登录成功或失败都会触发此事件,登录失败的话会有失败的原因
        /// </summary>
        public event EventDelegate <bool, string> StartResult;
        #endregion
        #region 服务器ip与端口构造函数
        /// <summary>
        /// 服务器ip与端口构造函数
        /// </summary>
        /// <param name="ip">服务器的ip地址</param>
        /// <param name="port">服务器的端口</param>
        internal SocketClient(string ip, int port)
        { Ip = ip; Port = port; }
        #endregion
        #region 启动客户端
        #region 启动客户端引擎
        /// <summary>
        /// 启动客户端,设置超时间在OutTime里设置,无论失败或成功都会触发StartResult事件;
        /// </summary>
        override public void StartEngine()
        {
            if (_engineStart)
                return;
            StartThread = new Thread(start);
            StartThread.IsBackground = true;
            StartThread.Start();
        }
        #endregion
        #region 启动客户端基础的一个线程
        /// <summary>
        /// 启动客户端基础的一个线程
        /// </summary>
        private void start()
        {
            if (reconnectOn)//如果是重连的延迟10秒
                Thread.Sleep(9000 + RandomPublic.RandomTime(1000));
            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.SendTimeout = 1000;
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.NoDelay, true);
                socket.BeginConnect(IpEndPoint, new AsyncCallback(AcceptCallback), socket);
                loginTimeout(socket);//超时判断方法
            }
            catch (Exception Ex)
            {
                loginFailure(Ex.Message);//登录失败触发
            }
        }
        #endregion
        #endregion
        #region 当连接服务器之后的回调函数
        /// <summary>
        /// 当连接服务器之后的回调函数
        /// </summary>
        /// <param name="ar">TcpClient</param>
        private void AcceptCallback(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;
            stateOne = new TcpTransmitBox(socket, BufferSize);
            try
            {
                socket.EndConnect(ar);
                socket.BeginReceive(stateOne.Buffer, 0, stateOne.Buffer.Length, 0, new AsyncCallback(ReadCallback), stateOne);
            }
            catch (Exception Ex)
            {
                if (outtimebool == true)
                    return; //说明已经超时了，已经触发登录失败了
                loginFailure(Ex.Message);//登录失败触发
            }
        }
        #endregion

        #region  登录篇
        #region 重连模块
        /// <summary>
        /// 重连模块
        /// </summary>
        private void reconnect()
        {
            if (_reconnectMax == 0)
                return;//不重连直接返回
            reconnectCi++;//每重连一次重连的次数加1
            if (stateOne != null)
            {
                stateOne.WorkSocket.Close();
                stateOne = null;
            }
            if (reconnectOn == false)
            {
                reconnectOn = true;
                CommonMethod.eventInvoket(() => { ReconnectionStart(); });
            }
            _engineStart = false;
            StartEngine();//调用启动引擎
        }
        #endregion
        #region 登录之超时判断
        /// <summary>
        /// 登录之超时判断
        /// </summary>
        private void loginTimeout(Socket socket)
        {
            DateTime time1 = DateTime.Now;
            outtimebool = false;
            while (true)
            {
                Thread.Sleep(10);
                if (_engineStart == true || outtimebool == true)
                    break;
                if ((int)(DateTime.Now - time1).TotalSeconds > _outTime)
                {
                    outtimebool = true; socket.Close();
                    loginFailure("连接超时");//登录失败触发
                    break;
                }
            }
        }
        #endregion
        #region 登录失败之后要处理的事情
        /// <summary>
        /// 登录失败之后要处理的事情
        /// </summary>
        private void loginFailure(string str)
        {
            outtimebool = true;//登录有结果了，判断超时的线程跳出
            if (_engineStart == true)//失败的时候引擎都是关闭的
                return;
            if (reconnectOn && reconnectCi < _reconnectMax)
                reconnect();//继续重连
            else
            {
                if (reconnectOn)
                    CommonMethod.eventInvoket(() => { StartResult(false, "重连失败" + str); });
                else
                    CommonMethod.eventInvoket(() => { StartResult(false, str); });
                CloseEngine();//不重连了就关闭客户端，释放资源
            }//登录失败触发此事件
        }
        #endregion
        #region 当客户端完全连接上服务器之后要处理的一些事情
        /// <summary>
        /// 当客户端完全连接上服务器之后要处理的一些事情
        /// </summary>
        private void loginSuccess()
        {
            _engineStart = true;
            if (HeartThread == null)
            {//连接成功之后启动心跳线程
                HeartThread = new Thread(heartThread);
                HeartThread.IsBackground = true;
                HeartThread.Start();
            }
            if (reconnectOn)
            { CommonMethod.eventInvoket(() => { StartResult(true, "重连成功"); }); reconnectOn = false; reconnectCi = 0; }
            else { CommonMethod.eventInvoket(() => { StartResult(true, "启动成功"); }); }
        }
        #endregion
        #region 接收到信息区块 连接上会调用
        /// <summary>
        /// 当接收到数据之后的回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void ReadCallback(IAsyncResult ar)
        {
            if (stateOne == null)
                return;
            Socket handler = stateOne.WorkSocket;
            try
            {
                int bytesRead = handler.EndReceive(ar);
                if (bytesRead > 0)
                {
                    //MessageBox.Show(stateOne.Buffer[0].ToString());
                    byte[] haveDate = ReceiveData.DateOneManage(stateOne, bytesRead);//接收完成之后对数组进行重置

                    handler.BeginReceive(stateOne.Buffer, 0, stateOne.Buffer.Length, 0, new AsyncCallback(ReadCallback), stateOne);
                    UnpackStickyData(stateOne, haveDate);
                }
                else { handler.BeginReceive(stateOne.Buffer, 0, stateOne.Buffer.Length, 0, new AsyncCallback(ReadCallback), stateOne); }
            }
            catch (Exception Ex)
            {
                lostClient(Ex.Message);//当突然断开的时候
            }
        }
        #endregion       
        #region 登录以及心跳代码进行处理；由基类在操作
        /// <summary>
        /// 登录以及心跳代码进行处理；由基类在操作
        /// </summary>
        /// <param name="stateOne">TcpState</param>
        /// <param name="haveDate">byte</param>
        override internal void VerificationCodeManage(TcpTransmitBox stateOne, byte haveDate)
        {
            if (stateOne == null)
                return;
            switch (haveDate)
            {
                case 0://不是需要的数据
                    break;
                case CipherCode._heartbeatCode://是心跳信息
                    //stateOne.HeartTime = DateTime.Now;
                    break;
                case CipherCode._serverToClient://客户端和服务端暗号正确；已登录;
                    Send(stateOne, EncDecVerification.EncryptionVerification(CipherCode._clientToServer));
                    //启用心跳进程
                    loginSuccess();
                    break;
                case CipherCode._clientCloseCode://服务器要求客户端关掉;
                    CloseEngine();
                    break;
            }
        }
        #endregion
        #endregion
        #region 发送信息区块
        #region 客户端向服务器发送图片数据
        /// <summary>
        /// 客户端向服务器发送图片数据
        /// </summary>
        /// <param name="data">字节数据</param>
        public void sendMessage(byte[] data)
        {
            sendMessage(stateOne, data);
        }
        #endregion
        #region 客户端向服务器发送文本数据
        /// <summary>
        /// 客户端向服务器发送文本数据
        /// </summary>
        /// <param name="data">文本数据</param>
        public void sendMessage(string data)
        {
            sendMessage(stateOne, data);
        }
        #endregion
        #region 发送文件；如果地址等不正确会抛出相应的异常；首先要先到FileStart启动文件发送系统;
        /// <summary>
        /// 发送文件；如果地址等不正确会抛出相应的异常；首先要先到FileStart启动文件发送系统;
        /// </summary>
        /// <param name="fileName">文件地址</param>
        /// <returns>返回文件标签；可以控制文件的任何事情</returns>
        public int SendFile(string fileName)
        {
            try
            {
                int haveInt = FileSend(stateOne, fileName);
                return haveInt;
            }
            catch { throw; }
        }
        #endregion
        #region 对文件进行续传；如果有不正确会抛出相应的异常
        /// <summary>
        /// 对文件进行续传；如果有不正确会抛出相应的异常
        /// </summary>
        /// <param name="fileLable">文件标签</param>
        public void ContinueFile(int fileLable)
        {
            try
            {
                FileContinue(stateOne, fileLable);
            }
            catch { throw; }
        }
        #endregion
        #region  实现客户端发送数据的虚方法
        /// <summary>
        /// 向服务器发送数据,最基础和原始的
        /// </summary>
        /// <param name="stateBase">StateBase</param>
        /// <param name="data">发送的数据</param>
        override internal void Send(TransmitData stateBase, byte[] data) //实现客户端虚方法
        {
            if (stateBase == null)
                return;
            Socket handler = stateBase.WorkSocket;
            //对data数据进行粘包加密
            StickPackage.EncryptionPackage(ref data);
            try
            {
                //发送
                handler.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), handler);
            }
            catch
            {
            }
        }
        #endregion
        #region 发送完数据之后的回调函数
        /// <summary>
        /// 发送完数据之后的回调函数
        /// </summary>
        /// <param name="ar">Clicent</param>
        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket handler = (Socket)ar.AsyncState;
                int bytesSent = handler.EndSend(ar);
            }
            catch
            {
            }
        }
        #endregion
        #endregion
        #region 客户端心跳线程
         #region 心跳线程
        /// <summary>
        /// 心跳线程
        /// </summary>
        private void heartThread()
        {
            while (true)
            {
                Thread.Sleep(HeartTime * 1000);
                if (stateOne == null)
                {
                    continue;
                }
                else if ((int)(DateTime.Now - stateOne.HeartTime).TotalSeconds > HeartTime * 4)//4次没有收到失去联系
                {
                    lostClient("客户端长期连接不上服务器根据Reconnection值判断是否重连");//当突然断开的时候
                    continue;
                }
                else
                {
                    Send(stateOne, EncDecVerification.EncryptionVerification(CipherCode._heartbeatCode));
                }
                if (_engineStart == false)
                    break;
            }
            HeartThread = null;
        }
        #endregion
        #endregion
        #region  断开篇
        #region 关闭相连的scoket以及关联的StateObject,释放所有的资源
        /// <summary>
        /// 关闭相连的scoket以及关联的StateObject,释放所有的资源
        /// </summary>
        override public void CloseEngine()
        {
            if (stateOne != null)
            {
                stateOne.ConnectOk = true;
                stateOne.WorkSocket.Close();
            }
            reconnectOn = false;
            reconnectCi = 0;//前面三个初始化
            OnEngineClose();//引擎完全释放资源触发此事件
        }
        #endregion
        #region 当客户端突然与服务器断开的时候
        /// <summary>
        /// 当客户端突然与服务器断开的时候
        /// </summary>
        /// <param name="str"></param>
        private void lostClient(string str)
        {
            if (stateOne.ConnectOk == true)
                return;//说明这个引擎已经触发关闭了;下面的就不用执行了
            if (_engineStart == false)
            { loginFailure(str); return; }//这里说明已经登录了；但由于服务器的原因被拒绝了
            OnEngineLost(str);//当客户端突然断开的时候触发此事件
            if (_reconnectMax > 0)//如果是重连就重连，不重连就关闭客户端释放资源
                reconnect();
            else
                CloseEngine();
        }
        #endregion
        #endregion
    }
}
