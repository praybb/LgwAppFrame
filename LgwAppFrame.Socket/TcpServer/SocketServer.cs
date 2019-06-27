﻿using LgwAppFrame.SocketHelper.Basics;
using LgwAppFrame.SocketHelper.Basics.Package;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace LgwAppFrame.SocketHelper
{
    /// <summary>
    /// 服务器
    /// </summary>
    public  class SocketServer : EngineParameter, ISocketServer
    {
        #region 基本属性区块
        //服务器上用来与各客户端通信的盒子
        private List<TcpTransmitBox> state = null;
        /// <summary>
        /// 服务器套
        /// </summary>
        private Socket listener = null;
        private Thread HeartThread = null;
        private int _clientMax = 20;//允许最多客户端数
        /// <summary>
        /// 当有客户连接成功的时候,触发此事件
        /// </summary>
        public event EventDelegate <IPEndPoint> Connect;
        /// <summary>
        /// 当有客户突然断开的时候,触发此事件,文本参数是代表断开的原因
        /// </summary>
        public event EventDelegate <IPEndPoint, string> Disconnection;
        /// <summary>
        /// 当前客户端数量
        /// </summary>
        public int ClientNumber
        {
            get { return state.Count; }
        }
        /// <summary>
        /// 允许最多客户端数
        /// </summary>
        public int ClientMax
        {
            get { return _clientMax; }
            set
            {
                if (value > 100)
                    _clientMax = 100;
                else
                    _clientMax = value;
            }
        }
        /// <summary>
        /// 得到所有的客户端
        /// </summary>
        List<IPEndPoint> ISocketServer.ClientAll
        {
            get
            {
                if (state == null || state.Count == 0)
                    return null;
                List<IPEndPoint> IpEndPoint = new List<IPEndPoint>();
                foreach (TcpTransmitBox stateOne in state)
                {
                    IpEndPoint.Add(stateOne.IpEndPoint);
                }
                return IpEndPoint;
            }
        }
        /// <summary>
        /// 带参数的构造函数
        /// </summary>
        /// <param name="port">端口号</param>
        internal SocketServer(int port)
        {
            Port = port;
            if (state == null)
                state = new List<TcpTransmitBox>();
        }
        #endregion
        #region 启动以及接收客户端区块
        /// <summary>
        /// 启动服务器,如果没出现异常,说明启动成功
        /// </summary>
        override public void StartEngine()
        {
            if (EngineStart)
                return;
            try
            {
                listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                listener.Bind(IpEndPoint);
                listener.Listen(200);
                listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
                HeartThread = new Thread(heartThread);
                HeartThread.IsBackground = true;
                HeartThread.Start();//把心跳方法加入到线程里面
                _engineStart = true;//启动成功
                EngineLog("服务器启动成功");//记录
            }
            catch (Exception Ex)
            {
                EngineLog("服务器启动失败");//记录
                if (HeartThread != null)
                    CloseEngine();
                throw new Exception(Ex.Message);
            }
        }
        /// <summary>
        /// 当连接一个客户端之后的回调函数
        /// </summary>
        /// <param name="ar">TcpClient</param>
        private void AcceptCallback(IAsyncResult ar)
        {
            TcpTransmitBox stateOne = null;
            try
            {
                listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
            }
            catch (Exception Ex)
            {
                OnEngineLost(Ex.Message);//当服务器突然断开触发此事件
                CloseEngine();
            }
            try
            {
                Socket Listener = (Socket)ar.AsyncState;
                Socket handler = listener.EndAccept(ar);
                stateOne = new TcpTransmitBox(handler, BufferSize);
                Thread threadLongin = new Thread(loginInitialization);
                threadLongin.IsBackground = true;
                threadLongin.Start(stateOne);//启动客户验证系统
            }
            catch
            {
            }
        }
        #endregion
        #region 当接收到数据之后的回调函数
        /// <summary>
        /// 当接收到数据之后的回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void ReadCallback(IAsyncResult ar)
        {
            TcpTransmitBox stateOne = (TcpTransmitBox)ar.AsyncState;
            Socket handler = stateOne.WorkSocket;
            try
            {
                int bytesRead = handler.EndReceive(ar);
                if (bytesRead > 0)
                {
                    //取缓存区的数据
                    byte[] haveDate = ReceiveData.DateOneManage(stateOne, bytesRead);//接收完成之后对数组进行重置
                    handler.BeginReceive(stateOne.Buffer, 0, stateOne.Buffer.Length, 0, new AsyncCallback(ReadCallback), stateOne);
                    UnpackStickyData(stateOne, haveDate);
                }
                else { handler.BeginReceive(stateOne.Buffer, 0, stateOne.Buffer.Length, 0, new AsyncCallback(ReadCallback), stateOne); }
            }
            catch (Exception Ex)
            {
                int i = Ex.Message.IndexOf("远程主机强迫关闭了一个现有的连接");
                if (stateOne != null && i != -1)
                { socketRemove(stateOne, Ex.Message); }
            }
        }
        /// <summary>
        /// 登录以及心跳代码进行处理；由基类在操作
        /// </summary>
        /// <param name="stateOne">TcpState</param>
        /// <param name="haveDate">代码</param>
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
                case CipherCode._clientToServer://客户端和服务端暗号正确；可以登录;
                    loginSuccess(stateOne);
                    break;
            }
        }
        #endregion
        #region 实现服务端送数据的虚方法
        /// <summary>
        /// 向客户端发送数据,最基础的发送
        /// </summary>
        /// <param name="stateBase">TcpState</param>
        /// <param name="data">发送的数据</param>
        override internal void Send(TransmitData stateBase, byte[] data) //实现服务端虚方法
        {
            if (stateBase == null)
                return;
            StickPackage.EncryptionPackage(ref data);
            //MessageBox.Show(data.Length.ToString()+"你好"+data[9].ToString());
            try
            {
                stateBase.WorkSocket.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), stateBase);
            }
            catch (Exception Ex)
            {
                int i = Ex.Message.IndexOf("远程主机强迫关闭了一个现有的连接");
                if (i != -1)
                {
                    TcpTransmitBox stateOne = IPEndPointToState(stateBase.IpEndPoint);
                    socketRemove(stateOne, Ex.Message);
                }
            }
        }
        /// <summary>
        /// 发送完数据之后的回调函数
        /// </summary>
        /// <param name="ar">Clicent</param>
        private void SendCallback(IAsyncResult ar)
        {
            TransmitData stateBase = (TransmitData)ar.AsyncState;
            if (stateBase == null)
                return;
            Socket handler = stateBase.WorkSocket;
            try
            {
                int bytesSent = handler.EndSend(ar);
            }
            catch
            {
            }
        }
        #endregion
        #region 心跳线程以及释放资源的基础方法private
        /// <summary>
        /// 心跳线程
        /// </summary>
        private void heartThread()
        {
            while (true)
            {
                Thread.Sleep(HeartTime * 1000);
                int i = 0;
                while (i < state.Count)
                {
                    if (state[i] == null)
                    {
                        state.RemoveAt(i);
                        continue;
                    }
                    else if ((int)(DateTime.Now - state[i].HeartTime).TotalSeconds > HeartTime * 4)//4次没有收到失去联系
                    {
                        socketRemove(state[i], "客户端长期连接不上,将断开此客户端");
                        continue;
                    }
                    else
                    {
                        Send(state[i], EncDecVerification.EncryptionVerification(CipherCode._heartbeatCode));
                    }
                    i++;
                }
                if (_engineStart == false || state.Count > 100)//后面的大于100用于限制商业版的
                    break;
            }
        }
        /// <summary>
        /// 关闭相连的scoket以及关联的TcpState,释放所有的资源
        /// </summary>
        /// <param name="stateOne">TcpState</param>
        /// <param name="str">原因</param>
        private void socketRemove(TcpTransmitBox stateOne, string str)
        {
            if (stateOne == null)
                return;
            stateOne.WorkSocket.Close();
            if (state.Remove(stateOne))//当没有登录的时候断掉，不触发下面的事件
            {
                CommonMethod.eventInvoket(() => { Disconnection(stateOne.IpEndPoint, str); }); //当客户端断掉的时候触发此事件
                EngineLog(stateOne.IpEndPoint.ToString() + "已经断开");//记录
                FileStart.FileStopITxBase(stateOne);
            }
            stateOne = null;
        }
        /// <summary>
        /// 当客户端连接之后要处理的一个线程,会验证客户端的身份。成功才允许登陆；
        /// </summary>
        /// <param name="stateOne1">TcpState</param>
        private void loginInitialization(object stateOne1)
        {
            TcpTransmitBox stateOne = (TcpTransmitBox)stateOne1;
            if (ClientNumber >= _clientMax)
            {
                EngineLog("客户端数量已达到上限!");//记录
                socketRemove(stateOne, "客户端数量已达到上限");
                return;
            }
            Send(stateOne, EncDecVerification.EncryptionVerification(CipherCode._serverToClient));//发送登录成功的代码
            try
            {
                stateOne.WorkSocket.BeginReceive(stateOne.Buffer, 0, stateOne.Buffer.Length, 0, new AsyncCallback(ReadCallback), stateOne);
            }
            catch
            {
            }
            DateTime dateOne = DateTime.Now;
            while (true)//如果2秒中之内客户端没有根据登录信息回复，将自动关闭这个客户端
            {
                Thread.Sleep(100);
                if (stateOne == null || stateOne.ConnectOk == true)
                    break;
                if ((int)(DateTime.Now - dateOne).TotalSeconds > 2)
                {
                    clientClose(stateOne); EngineLog(stateOne.IpEndPoint.ToString() + "无法收到客户端登录信息");//记录;
                    break;
                }
            }
        }
        /// <summary>
        /// 客户端完全登录成功之后要处理的事情
        /// </summary>
        /// <param name="stateOne">TcpState</param>
        private void loginSuccess(TcpTransmitBox stateOne)
        {
            stateOne.ConnectOk = true;
            state.Add(stateOne);
            CommonMethod.eventInvoket(() => { Connect(stateOne.IpEndPoint); });
            EngineLog(stateOne.IpEndPoint.ToString() + "登陆成功");//记录
        }
        #endregion
        #region 客户需要操作的一些方法
        /// <summary>
        /// 关闭服务器,释放所有资源
        /// </summary>
        override public void CloseEngine() //关闭服务器,释放所有资源
        {
            try
            {
                if (HeartThread != null)
                    HeartThread.Abort();
                HeartThread = null;
                clientAllClose();
                state = null;
                if (listener != null)
                    listener.Close();
                listener = null;
                OnEngineClose();
                EngineLog("服务器已突然断掉");//记录
            }
            catch { }
        }
        /// <summary>
        /// 关闭所有客户端连接
        /// </summary>
        public void clientAllClose()
        {
            foreach (TcpTransmitBox stateo in state)
            {
                socketRemove(stateo, "服务器关闭所有的客户端");
            }
        }
        /// <summary>
        /// 发送代码的形式服务器强制关闭一个客户端
        /// </summary>
        /// <param name="stateOne">TcpState</param>
        private void clientClose(TcpTransmitBox stateOne)
        {
            if (stateOne == null || ClientNumber == 0)
                return;
            state.Remove(stateOne);//先把这个移除,万一对方没有收到下面的信息;对方一定时候也会自动关闭这个连接
            Send(stateOne, EncDecVerification.EncryptionVerification(CipherCode._clientCloseCode));//发送一个强制关闭的代码过去
        }
        /// <summary>
        /// 服务器强制关闭一个客户端
        /// </summary>
        /// <param name="ipEndPoint">IPEndPoint</param>
        public void clientClose(IPEndPoint ipEndPoint)
        {
            TcpTransmitBox stateOne = IPEndPointToState(ipEndPoint);
            clientClose(stateOne);
        }
        /// <summary>
        /// 检查某个客户端是否在线
        /// </summary>
        /// <param name="ipEndPoint">IPEndPoint</param>
        /// <returns>bool</returns>
        public bool clientCheck(IPEndPoint ipEndPoint)
        {
            TcpTransmitBox stateOne = IPEndPointToState(ipEndPoint);
            if (stateOne == null)
                return false;
            return true;
        }
        /// <summary>
        /// 服务器向客户端发送文本数据
        /// </summary>
        /// <param name="ipEndPoint">IPEndPoint</param>
        /// <param name="data">文本数据</param>
        public void sendMessage(IPEndPoint ipEndPoint, string data)
        {
            TcpTransmitBox stateOne = IPEndPointToState(ipEndPoint);
            sendMessage(stateOne, data);
        }
        /// <summary>
        /// 服务器向客户端发送图片数据
        /// </summary>
        /// <param name="ipEndPoint">IPEndPoint</param>
        /// <param name="data">图片的数据</param>
        public void sendMessage(IPEndPoint ipEndPoint, byte[] data)
        {
            TcpTransmitBox stateOne = IPEndPointToState(ipEndPoint);
            sendMessage(stateOne, data);
        }
        /// <summary>
        /// 发送文件；如果地址等不正确会抛出相应的异常；首先要先到FileStart启动文件发送系统;
        /// </summary>
        /// <param name="ipEndPoint">IPEndPoint</param>
        /// <param name="fileName">文件地址</param>
        /// <returns>返回文件标签；可以控制文件的任何事情</returns>
        public int SendFile(IPEndPoint ipEndPoint, string fileName)
        {
            TcpTransmitBox stateOne;
            try
            {
                stateOne = IPEndPointToState(ipEndPoint);
            }
            catch { throw new Exception("此IPEndPoint不存在"); }
            try
            {
                int haveInt = FileSend(stateOne, fileName);
                return haveInt;
            }
            catch { throw; }
        }
        /// <summary>
        /// 对文件进行续传；如果有不正确会抛出相应的异常
        /// </summary>
        /// <param name="ipEndPoint">IPEndPoint</param>
        /// <param name="fileLable">文件标签</param>
        public void ContinueFile(IPEndPoint ipEndPoint, int fileLable)
        {
            TcpTransmitBox stateOne;
            try
            {
                stateOne = IPEndPointToState(ipEndPoint);
            }
            catch { throw new Exception("此IPEndPoint不存在"); }
            try
            {
                FileContinue(stateOne, fileLable);
            }
            catch { throw; }
        }
        /// <summary>
        /// 把ip地址转化成TcpState
        /// </summary>
        /// <param name="ipEndPoint">IPEndPoint</param>
        /// <returns>TcpState</returns>
        private TcpTransmitBox IPEndPointToState(IPEndPoint ipEndPoint)
        {
            try
            {
                return state.Find(delegate (TcpTransmitBox state1) { return state1.IpEndPoint == ipEndPoint; });
            }
            catch { return null; }
        }

        #endregion
    }
}
