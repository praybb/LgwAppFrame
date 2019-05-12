using LgwAppFrame.SocketHelperHelper;
using System;
using System.Text;

namespace LgwAppFrame.SocketHelper.Basics.Package
{
    /// <summary>
    /// 分包数据处理中心
    /// </summary>
    internal class EncDecSeparateDate
    {
        #region 返回分包数据11位的数据包包头（暗号类型1+暗号1+原暗号+数据标签+长度）
        /// <summary>
        /// 返回分包数据11位的数据包包头（暗号类型1+暗号1+原暗号+数据标签+长度）
        /// </summary>
        /// <param name="date">数据</param>
        /// <param name="textCode">原暗号，什么文件</param>
        /// <param name="state">StateBase</param>
        /// <returns>加密之后的包头</returns>
        internal static byte[] SendHeadEncryption(byte[] date, byte textCode, TransmitData state)
        {
            state.SendFile = new TransmitFile(date);
            state.SendFile.FileLabel = RandomPublic.RandomNumber(14562);
            byte[] headDate = new byte[11];
            //写入暗号
            headDate[0] = CipherCode._bigDateCode;
            headDate[1] = CipherCode._fileHeadCode;
            headDate[2] = textCode;
            //写入数据标签
            ByteToDate.IntToByte(state.SendFile.FileLabel, 3, headDate);
            //写入数据长度
            ByteToDate.IntToByte(date.Length, 7, headDate);
            return headDate;
        }
        #endregion
        #region 对文件主体部分进行加密
        /// <summary>
        /// 对文件主体加入暗号类型+暗号+数据标签
        /// </summary>
        /// <param name="date">数据</param>
        /// <param name="sendDateLabel">标签</param>
        /// <returns>加密之后的数据（暗号类型+暗号+数据标签）</returns>
        private static byte[] SendSubjectEncryption(byte[] date, int sendDateLabel)
        {
            byte[] dateOverall = ByteToDate.OffsetEncryption(date, sendDateLabel, 2);
            dateOverall[0] = CipherCode._bigDateCode;
            dateOverall[1] = CipherCode._fileSubjectCode;
            return dateOverall;
        }
        #endregion
        #region 当收到是分组数据代码的到这里来统一处理
        /// <summary>
        /// 文件解密
        /// </summary>
        /// <param name="date">数据</param>
        /// <param name="state">StateBase</param>
        /// <returns>StateCode</returns>
        internal static DataModel FileDecrypt(byte[] date, TransmitData state)
        {
            DataModel stateCode = null;
            if (date.Length < 6)
                return stateCode;
            byte headDate = date[1];
            if (headDate == CipherCode._fileAgreeReceive)
            {//对方同意接收文件;我应该怎么处理
                int FileLabel = ByteToDate.ByteToInt(2, date);

                if (state.SendFile != null && state.SendFile.FileLabel == FileLabel)
                {
                    byte[] SendSubjectDate = FileGetSendDate(state);
                    if (SendSubjectDate == null)
                        stateCode = new DataModel(CipherCode._dateSuccess);
                    else
                        stateCode = new DataModel(SendSubjectDate);//直接发送
                }
            }
            else if (headDate == CipherCode._dateSuccess)
            {//对方已经接收到数据
                int FileLabel = ByteToDate.ByteToInt(2, date);
                if (state.SendFile != null && state.SendFile.FileLabel == FileLabel)
                {
                    byte[] SendSubjectDate = FileGetSendDate(state);
                    if (SendSubjectDate == null)
                        stateCode = new DataModel(CipherCode._dateSuccess);
                    else
                        stateCode = new DataModel(SendSubjectDate);//直接发送
                }
            }
            //上面是发送方接收要做的;下面是接收方发送要做的事情
            else if (headDate == CipherCode._fileHeadCode)
            {//收到的是文件包头部分
                byte whatCode = date[2];//原暗号
                int fileLabel = ByteToDate.ByteToInt(3, date);//数据标签
                int fileLenth = ByteToDate.ByteToInt(7, date);//长度
                state.ReceiveFile = new TransmitFile(whatCode, fileLabel, fileLenth);
                byte[] dateAll = new byte[6];
                dateAll[0] = CipherCode._bigDateCode;
                dateAll[1] = CipherCode._fileAgreeReceive;
                ByteToDate.IntToByte(fileLabel, 2, dateAll);
                stateCode = new DataModel(dateAll);
            }
            else if (headDate == CipherCode._fileSubjectCode)
            {//收到的是文件主体部分
                int SendDateLabel = 0;
                byte[] dateAll = ByteToDate.OffsetDecrypt(date, out SendDateLabel, 2);
                byte[] ReplyDate = ByteToDate.CombinationTwo(CipherCode._bigDateCode, CipherCode._dateSuccess, state.ReceiveFile.FileLabel);
                if (state.ReceiveFile.FileDateAll == null)
                {
                    state.ReceiveFile.FileDateAll = dateAll;//是第一次接收到主体数据
                    stateCode = new DataModel(ReplyDate);
                }
                else
                {
                    byte[] FileDateAll = new byte[state.ReceiveFile.FileDateAll.Length + dateAll.Length];
                    state.ReceiveFile.FileDateAll.CopyTo(FileDateAll, 0);
                    dateAll.CopyTo(FileDateAll, state.ReceiveFile.FileDateAll.Length);
                    state.ReceiveFile.FileDateAll = FileDateAll;
                    if (FileDateAll.Length == state.ReceiveFile.FileLenth)
                    {
                        if (state.ReceiveFile.FileClassification == CipherCode._textCode)
                        {
                            string str = Encoding.UTF8.GetString(FileDateAll);
                            stateCode = new DataModel(CipherCode._textCode, str, ReplyDate);
                        }
                        else
                        {
                            stateCode = new DataModel(CipherCode._photographCode, FileDateAll, ReplyDate);
                        }
                        state.ReceiveFile = null;//文件接收完成；释放接收器
                    }
                    else
                    { stateCode = new DataModel(ReplyDate); }
                }
            }
            return stateCode;
        }
        #endregion
        #region 直接从这里提取一个要发送的主体字节集数据;已经加密完成
        /// <summary>
        /// 直接从这里提取一个要发送的主体字节集数据;已经加密完成
        /// </summary>
        /// <param name="state">StateBase</param>
        /// <returns>字节集</returns>
        private static byte[] FileGetSendDate(TransmitData state)
        {
            int Date_Max = state.BufferSize - 24;
            if (state.SendFile.FileDateAll == null)
                return null;//说明文件已经发完了
            int FileDateAllLenth = state.SendFile.FileDateAll.Length;
            if (FileDateAllLenth <= Date_Max)
            {
                state.SendFile.SendDate = SendSubjectEncryption(state.SendFile.FileDateAll, state.SendFile.FileLabel);
                state.SendFile.FileDateAll = null;
            }
            else
            {
                //现在送出去
                byte[] dateS = new byte[Date_Max];
                //剩下的数据                
                byte[] dateL = new byte[FileDateAllLenth - Date_Max];
                Array.Copy(state.SendFile.FileDateAll, 0, dateS, 0, Date_Max);
                Array.Copy(state.SendFile.FileDateAll, Date_Max, dateL, 0, FileDateAllLenth - Date_Max);
                state.SendFile.FileDateAll = dateL;
                //在这里加入主体暗号_fileSubjectCode;
                state.SendFile.SendDate = SendSubjectEncryption(dateS, state.SendFile.FileLabel);
            }
            return state.SendFile.SendDate;
        }
        #endregion


    }
}
