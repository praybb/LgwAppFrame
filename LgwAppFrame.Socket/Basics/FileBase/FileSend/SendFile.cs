﻿using LgwAppFrame.SocketHelper.Basics.Package;
using System;
using System.IO;
using System.Threading;
namespace LgwAppFrame.SocketHelper.Basics
{
    /// <summary>
    /// 文件发送处理中心
    /// </summary>
    public class SendFile : FileToBase, IFileSend
    {
        /// <summary>
        /// 发送必须实现的一些方法
        /// </summary>
        internal FileSendMust SendMust = null;
        /// <summary>
        /// 带参数和构造函数
        /// </summary>
        /// <param name="sendMust">IFileSendMust</param>
        internal SendFile(IFileSendMust sendMust)
        {
            SendMust = new FileSendMust(sendMust);
        }
        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="fileLable">文件标签</param>
        /// <param name="fileName">文件地址</param>
        /// <param name="stateOne">StateBase</param>
        /// <returns>形成之后的数据</returns>
        internal byte[] Send(ref int fileLable, string fileName, TransmitData stateOne)
        {
            int fileLenth = 0;
            FileStream fs;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                fileLenth = (int)fs.Length;
            }
            catch (Exception Ex) { throw new Exception(Ex.Message); }
            fileLable = RandomPublic.RandomNumber(16787);
            FileState fileState = new FileState(fileLable, fileLenth, fileName, fs);
            fileState.StateOne = stateOne;
            FS.Add(fileState);
            byte[] haveByte = EncDecFile.FileHeadEncryption(fileState);
            return haveByte;
        }
        /// <summary>
        /// 发送方的文件进行续传;如果回复是null;说明不存在
        /// </summary>
        /// <param name="fileLable">文件标签</param>
        /// <param name="stateOne">StateBase</param>
        /// <returns>byte[]</returns>
        internal byte[] FileContinue(int fileLable, TransmitData stateOne)
        {
            byte[] haveDate = null;
            FileState state = FileLabelToState(fileLable);
            if (state != null && state.StateFile == 2)
            {
                haveDate = EncDecFile.FileSevenEncryption(CipherCode._sendUser, CipherCode._fileContinue, fileLable);
                state.StateOne = stateOne;//可以续传；用新的stateOne代替旧的
            }
            return haveDate;
        }
        /// <summary>
        /// 接收方发过来的数据；
        /// </summary>
        /// <param name="receiveToDate">收到的数据</param>
        /// <param name="stateOne">StateBase</param>
        /// <returns>回复的数据</returns>
        internal byte[] ReceiveDateTO(byte[] receiveToDate, TransmitData stateOne)
        {
            byte[] haveDate = null;
            int fileLabel = ByteToDate.ByteToInt(3, receiveToDate);
            byte code = receiveToDate[2];
            FileState state = FileLabelToState(fileLabel);
            if (state == null)
            {
                if (code == CipherCode._fileCancel)
                    return null;
                else
                    return EncDecFile.FileSevenEncryption(CipherCode._sendUser, CipherCode._fileCancel, fileLabel);
            }
            else
            {
                switch (code)
                {
                    case CipherCode._fileOk://对方同意接收文件
                        Thread.Sleep(500);
                        state.StateFile = 1;
                        haveDate = EncDecFile.FileSubjectEncryption(state, stateOne.BufferSize);
                        SendMust.FileStartOn(fileLabel);//第一次发送用原来尺寸
                        break;
                    case CipherCode._fileRefuse://对方拒绝接收文件
                        FileRemove(fileLabel);
                        SendMust.FileRefuse(fileLabel);
                        break;
                    case CipherCode._dateSuccess://主体部分数据发送成功；准备发送下一批
                        haveDate = EncDecFile.FileSubjectEncryption(state, BufferSize);
                        SendMust.FileProgress(state);
                        if (haveDate == null)//说明这个文件已经发送成功了
                        { FileRemove(fileLabel); SendMust.SendSuccess(fileLabel); }
                        break;
                    case CipherCode._fileCancel://对方已经取消了这个文件；
                        FileRemove(fileLabel);
                        SendMust.FileCancel(fileLabel);
                        break;
                    case CipherCode._sendStop://对方暂停发送
                        FileStopIn(state, SendMust);
                        break;
                    case CipherCode._fileContinue://对方发过来一个续传的确认信息;你是否同意；
                        state.StateOne = stateOne;//有可能stateOne会有变化
                        FileStopIn(state, SendMust);//对方已经暂停了这个文件；我这边肯定也要先暂停掉
                        bool orStop = SendMust.FileOrNotContingue(fileLabel);//让客户确认；是否续传
                        if (orStop)
                        {
                            state.StateFile = 1;
                            state.FileOkLenth = ByteToDate.ByteToLong(7, receiveToDate);
                            state.Filestream.Position = state.FileOkLenth;//设置流的当前读位置
                            haveDate = EncDecFile.FileSubjectEncryption(state, stateOne.BufferSize);
                            if (haveDate != null)
                            {
                                haveDate[2] = CipherCode._fileContinueOk;
                                SendMust.FileContinue(fileLabel);
                            }
                        }
                        else { haveDate = EncDecFile.FileSevenEncryption(CipherCode._sendUser, CipherCode._fileContinueNo, fileLabel); }
                        break;
                    case CipherCode._fileContinueOk://对方同意续传
                        state.FileOkLenth = ByteToDate.ByteToLong(7, receiveToDate);
                        state.StateFile = 1;
                        state.Filestream.Position = state.FileOkLenth;//设置流的当前读位置
                        haveDate = EncDecFile.FileSubjectEncryption(state, stateOne.BufferSize);
                        SendMust.FileContinue(fileLabel);
                        break;
                    case CipherCode._fileContinueNo://对方拒绝续传
                        SendMust.FileNoContinue(fileLabel);
                        FileStopIn(state, SendMust);
                        break;
                }
            }
            return haveDate;
        }
    }
}
