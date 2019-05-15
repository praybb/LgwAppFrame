using System;
using System.Collections.Generic;
using System.Net;
using LgwAppFrame.SocketHelper.Basics.Package;
using LgwAppFrame.SocketHelperHelper;
using LgwAppFrame.SocketHelper.Basics;

namespace LgwAppFrame.SocketHelper
{
    /// <summary>
    /// 引擎参数与方法
    /// </summary>
    public class EngineParameter : IEngineParameter
    {
        #region 事件
        #region 引擎非正常原因自动断开的时候触发此事件
        /// <summary>
        /// 当引擎非正常原因自动断开的时候触发此事件
        /// </summary>
        public event EventDelegate<string> EngineLost;
        /// <summary>
        /// 当引擎非正常原因自动断开的时候触发此事件
        /// </summary>
        /// <param name="str">断开原因</param>
        internal void OnEngineLost(string str)
        {
            if (this.EngineLost != null)
            {
                CommonMethod.eventInvoket(() => { this.EngineLost(str); });
                FileStart.FileStopAll();//文件处理那里中断所有的文件
            }
        }
        #endregion
        #region 当引擎完全关闭释放资源的时候
        /// <summary>
        /// 当引擎完全关闭释放资源的时候
        /// </summary>
        public event EventDelegate EngineClose;
        /// <summary>
        /// 当引擎完全关闭释放资源的时候触发此事件
        /// </summary>
        internal void OnEngineClose()
        {
            if (this.EngineClose != null && _engineStart == true)
            {
                _engineStart = false;
                CommonMethod.eventInvoket(() => { this.EngineClose(); });
                FileStart.FileStopAll();//文件处理那里中断所有的文件
            }
        }
        #endregion
        #region 当接收到文本数据的时候,触发此事件
        /// <summary>
        /// 接收到文本数据的时候的事件
        /// </summary>
        public event EventDelegate<IPEndPoint, string> AcceptString;
        /// <summary>
        /// 触发当接收到文本数据的事件
        /// </summary>
        /// <param name="iPEndPoint">对方终结点</param>
        /// <param name="str">文本数据</param>
        internal void OnAcceptString(IPEndPoint iPEndPoint, string str)
        {
            if (this.AcceptString != null)
            {
                CommonMethod.eventInvoket(() => { AcceptString(iPEndPoint, str); });
            }
        }
        #endregion
        #region 当接收到图片数据的时候,触发此事件
        /// <summary>
        /// 当接收到图片数据的事件
        /// </summary>
        public event EventDelegate<IPEndPoint, byte[]> AcceptByte;
        /// <summary>
        /// 当接收到图片数据的时候,触发此事件
        /// </summary>
        /// <param name="iPEndPoint">对方终结点</param>
        /// <param name="bytes">图片数据</param>
        internal void OnAcceptByte(IPEndPoint iPEndPoint, byte[] bytes)
        {
            if (this.AcceptByte != null)
            {
                CommonMethod.eventInvoket(() => { this.AcceptByte(iPEndPoint, bytes); });
            }
        }
        #endregion
        #region 当将数据发送成功且对方已经收到的时候,触发此事件
        /// <summary>
        /// 当将数据发送成功且对方已经收到的时候,触发此事件
        /// </summary>
        public event EventDelegate<IPEndPoint> dateSuccess;
        /// <summary>
        /// 当将数据发送成功且对方已经收到的时候,触发此事件
        /// </summary>
        /// <param name="iPEndPoint">对方终结点</param>
        internal void OndateSuccess(IPEndPoint iPEndPoint)
        {
            if (this.dateSuccess != null)
            {
                CommonMethod.eventInvoket(() => { this.dateSuccess(iPEndPoint); });
            }
        }
        #endregion
        #endregion
        #region 缓冲区大小
        private int _bufferSize = 1024;//缓冲区大小
        /// <summary>
        /// 缓冲区大小；默认为1024字节；不影响最大发送量，如果内存够大或经常发送大数据可以适当加大缓冲区
        /// 大小；从而可以提高发送速度；否则会自动分包发送，到达对方自动组包；
        /// </summary>
        public int BufferSize
        {
            get
            {
                return _bufferSize;
            }
            set
            {
                _bufferSize = value;
            }
        }
        #endregion
        #region 端口与IP
        private string _ip = "";//服务器的IP地址
        /// <summary>
        /// ip地址设置和读取
        /// </summary>
        public string Ip
        {
            get { return _ip; }
            set { _ip = value; }
        }
        private int _port = 0;
        /// <summary>
        /// 端口号设置和读取
        /// </summary>
        public int Port
        {
            get { return _port; }
            set { _port = value; }
        }
        #endregion
        #region 本地的终结点地址封装

        private IPEndPoint _ipEndPoint = null;//终结点地址项目里面用
        /// <summary>
        /// 本地的终结点地址封装；
        /// </summary>
        internal IPEndPoint IpEndPoint
        {
            get
            {
                try
                {
                    IPAddress ipAddress = null;
                    if (Ip == "")
                        ipAddress = IPAddress.Any;
                    else
                        ipAddress = IPAddress.Parse(CommonMethod.Hostname2ip(Ip));
                    _ipEndPoint = new IPEndPoint(ipAddress, Port);
                    _port = _ipEndPoint.Port;
                }
                catch { throw; }
                return _ipEndPoint;
            }
        }
        #endregion
        #region 客户端引擎是否已经启动
        /// <summary>
        /// 客户端引擎是否已经启动;
        /// </summary>
        protected bool _engineStart = false;
        /// <summary>
        /// 引擎是否已经启动;
        /// </summary>
        public bool EngineStart
        {
            get { return _engineStart; }
        }
        #endregion
        #region 引擎的关闭与启动
        /// <summary>
        /// 启动引擎
        /// </summary>
        virtual public void StartEngine() //虚方法,子类实现
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 关闭引擎释放资源
        /// </summary>
        virtual public void CloseEngine() //虚方法,子类实现
        {
            throw new NotImplementedException();
        }
        #endregion

        #region 发送数据
        #region 发送字节数据(基础虚方法)
        /// <summary>
        /// 发送字节数据(基础虚方法)
        /// </summary>
        /// <param name="stateOne">StateBase</param>
        /// <param name="data">发送的数据</param>
        virtual internal void Send(TransmitData stateOne, byte[] data)
        { }
        #region 发送文本数据
        /// <summary>
        /// 发送文本数据
        /// </summary>
        /// <param name="stateOne">StateBase</param>
        /// <param name="data">未加密的数据</param>
        internal void sendMessage(TransmitData stateOne, string data)
        {
            if (stateOne == null)
                return;
            //建立一个数据
            DataModel stateCode = new DataModel(CipherCode._textCode, data);
            //对一个数据进行加密-这里应该是把数据装入快速盒子
            byte[] sendDate = EncryptionDecrypt.encryption(stateCode, stateOne);
            //放入传输的快递盒子
            stateOne.SendDate = sendDate;
            //发送出去（）//在这里发送时再进行粘包处理 前面为粘包代码4位+sendDate包长度4位+sendDate
            Send(stateOne, sendDate);
        }
        #endregion
        #region 发送图片数据
        /// <summary>
        /// 服务器向客户端发送图片数据
        /// </summary>
        /// <param name="stateOne">StateBase</param>
        /// <param name="data">未加密的数据</param>
        internal void sendMessage(TransmitData stateOne, byte[] data)
        {
            if (stateOne == null)
                return;
            DataModel stateCode = new DataModel(CipherCode._photographCode, data);
            //取得要发数据
            byte[] sendDate = EncryptionDecrypt.encryption(stateCode, stateOne);
            stateOne.SendDate = sendDate;
            Send(stateOne, sendDate);
        }
        #endregion
        #endregion
        #region 发送文件
        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="stateOne">StateBase</param>
        /// <param name="fileName">文件地址</param>
        /// <returns>文件标签,首先要注册文件发送系统；会返回一个整数型的文件标签；用来控制这个文件以后一系列操作</returns>
        internal int FileSend(TransmitData stateOne, string fileName)
        {
            if (FileStart.fileSend == null)
                throw new Exception("请先注册文件发送系统");
            int haveInt = 0;
            byte[] haveByte = null;
            try
            {
                haveByte = FileStart.fileSend.Send(ref haveInt, fileName, stateOne);
            }
            catch { throw; }
            Send(stateOne, haveByte);
            return haveInt;
        }
        #endregion
        #region 对文件进行续传
        /// <summary>
        /// 对文件进行续传;
        /// </summary>
        /// <param name="stateOne">StateBase</param>
        /// <param name="fileLable">文件标签</param>
        internal void FileContinue(TransmitData stateOne, int fileLable)
        {
            byte[] haveDate = FileStart.FileContinue(fileLable, stateOne);
            if (haveDate == null)
                throw new Exception("文件不存在或状态不在暂停状态");
            Send(stateOne, haveDate);
        }
        #endregion
        #endregion



        #region 解决粘包数据处理(也是数据的第一次处理)
        /// <summary>
        /// 解决粘包数据处理;也是数据的第一次处理
        /// </summary>
        /// <param name="stateOne">当前的连接属性</param>
        /// <param name="reciverByte">收到的数据</param>
        internal void UnpackStickyData(TcpTransmitBox stateOne, byte[] reciverByte)
        {
            //设置心跳包
            stateOne.HeartTime = DateTime.Now;
            //去掉粘包代码，得到数据集
            List<byte[]> listDate = StickPackage.DecryptPackage(reciverByte, ref stateOne.Residualpackage);

            foreach (byte[] date in listDate)
            {
                DataModel statecode = ReceiveData.DataValidation(date);
                CategorizeData(stateOne, statecode);
            }
        }
        #endregion
        #region 数据按分类进行处理(数据第二次处理)
        /// <summary>
        /// 数据按分类进行处理(数据第二次处理)
        /// </summary>
        /// <param name="stateOne">传输用的盒子</param>
        /// <param name="statecode">收到的数据</param>
        internal void CategorizeData(TcpTransmitBox stateOne, DataModel statecode)
        {

            if (statecode == null || stateOne == null)
                return;
            //说明是验证类型；抛给验证暗号处理中心，如果不是抛给普通数据处理
            if (statecode.State == CipherCode._verificationCode)
            {
                byte haveDate = EncDecVerification.DecryptVerification(statecode.DateByte);
                VerificationCodeManage(stateOne, haveDate);
            }
            else
            {
                codeManage(stateOne, statecode);
            }
        }
        /// <summary>
        /// 数据第二层分配中心；把数据归类
        /// </summary>
        /// <param name="stateOne">连接属性</param>
        /// <param name="statecode">收到的数据</param>
        internal void codeManage(TransmitData stateOne, DataModel statecode)
        {
            if (statecode == null || stateOne == null)
                return;
            DataModel stateCode = null;
            switch (statecode.State)
            {
                case CipherCode._commonCode://普通数据信息;抛给普通Code去处理
                    stateCode = EncryptionDecrypt.deciphering(statecode.DateByte, stateOne);
                    CommonCodeManage(stateOne, stateCode);
                    break;
                case CipherCode._bigDateCode://抛给分包Code去处理
                    stateCode = EncDecSeparateDate.FileDecrypt(statecode.DateByte, stateOne);//返回一个带回复数据的模型                    
                    CommonCodeManage(stateOne, stateCode);//发送出去
                    break;
                case CipherCode._fileCode://抛给文件处理器去处理；如果返回null就不用发送了
                    byte[] haveDate = FileStart.ReceiveDateTO(statecode.DateByte, stateOne);
                    if (haveDate != null)
                        Send(stateOne, haveDate);
                    break;
            }
        }
        #endregion
        #region 接收到的普通数据时的处理方法
        /// <summary>
        /// 接收到的普通数据处中心
        /// </summary>
        /// <param name="stateOne">StateBase</param>
        /// <param name="stateCode">StateCode</param>
        internal void CommonCodeManage(TransmitData stateOne, DataModel stateCode)
        {

            if (stateCode == null || stateOne == null)
                return;
            switch (stateCode.State)
            {
                case CipherCode._textCode://文本信息
                    //发送回复数据
                    Send(stateOne, stateCode.ReplyDate);
                    OnAcceptString(stateOne.IpEndPoint, stateCode.Datestring); //触发文本收到的事件
                    break;
                case CipherCode._photographCode://图片信息
                    //发送回复数据
                    Send(stateOne, stateCode.ReplyDate);
                    OnAcceptByte(stateOne.IpEndPoint, stateCode.DateByte);//触发图片收到的事件
                    break;
                case CipherCode._dateSuccess://数据发送成功
                    stateOne.SendDate = null;
                    OndateSuccess(stateOne.IpEndPoint);//对方收到触发事件
                    break;
                case 0://说明这个数据只要直接回复给对方就可以了
                    Send(stateOne, stateCode.ReplyDate);
                    break;
            }
        }
        #endregion
        #region 关于验证暗号怎么处理的类；(客户端与服务端不相同)
        /// <summary>
        /// 要被TCP子类重写的；关于验证暗号怎么处理的类；(客户端与服务端不相同)
        /// </summary>
        /// <param name="stateOne">TcpState</param>
        /// <param name="haveDate">字节</param>
        virtual internal void VerificationCodeManage(TcpTransmitBox stateOne, byte haveDate)
        { }
        #endregion

        #region 心跳间隔时间
        private int _heartTime = 10;//心跳间隔时间
        /// <summary>
        /// 设置发送心跳间隔的时间,以秒为单位,默认为10秒
        /// </summary>
        internal int HeartTime
        {
            get
            {
                return _heartTime;
            }
            set
            {
                _heartTime = value;
            }
        }
        #endregion

        #region 日志记录
        private string _fileLog = "";//记录地址，如果为空表示不记录
                                     /// <summary>
                                     /// 日志文件目录地址；为空表示不记录
                                     /// </summary>
        public string FileLog
        {
            get { return _fileLog; }
            set { _fileLog = value; }
        }
       
        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="str">信息</param>
        internal virtual void EngineLog(string str)
        {
            try
            {
                CommonMethod.FileOperate(FileLog, str);
            }
            catch { FileLog = ""; }
        }
        #endregion
    }
}
