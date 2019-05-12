using System;

namespace LgwAppFrame.SocketHelper.Basics.Package
{
    /// <summary>
    /// 接收数据的一些方法
    /// </summary>
    internal class ReceiveData
    {
        #region 粘包处理后阶段 验证数据的正确性
        /// <summary>
        /// 验证数据的正确性
        /// </summary>
        /// <param name="date">数据</param>
        /// <returns>返回一个数据模型，如返回是NULL说明不是本系统的数据；违法数据等等</returns>
        internal static DataModel DataValidation(byte[] date)
        {

            DataModel statecode = null;
            //如果小于2，说明只有暗号类型与暗号，则返回NULL
            if (date.Length < 2)
                return statecode;
            byte headcode = date[0];
            if (headcode == CipherCode._fileCode || headcode == CipherCode._bigDateCode || headcode == CipherCode._commonCode || headcode == CipherCode._verificationCode)
                statecode = new DataModel(headcode, date);
            return statecode;
        }
        #endregion
        #region 原始传输过来的数据 读取数据盒子内的缓冲区数据，并清空
        /// <summary>
        /// 读取缓冲区数据，并清空
        /// </summary>
        /// <param name="stateOne">StateBase</param>
        /// <param name="insert">数据实际长度</param>
        /// <returns>需要的数据</returns>
        internal static byte[] DateOneManage(TransmitData stateOne, int insert)
        {
            byte[] receiveByte = null;
            if (stateOne.Buffer[0] == 0 && stateOne.BufferBackup != null && stateOne.BufferBackup.Length >= insert)
            {
                receiveByte = stateOne.BufferBackup;
                //清空备用缓冲区
                stateOne.BufferBackup = null;
            }//主要用于缓冲区有扩大缩小
            else
            { receiveByte = stateOne.Buffer; }
            byte[] haveDate = ByteToDate.ByteToByte(receiveByte, insert, 0);
            Array.Clear(stateOne.Buffer, 0, stateOne.Buffer.Length);
            return haveDate;
        }
        #endregion
    }
}
