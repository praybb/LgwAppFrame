using LgwAppFrame.SocketHelperHelper;
using System.Text;

namespace LgwAppFrame.SocketHelper.Basics.Package
{
    /// <summary>
    /// 普通数据加密和解密中心
    /// </summary>
    internal class EncryptionDecrypt
    {
        #region 对数据模型处理，返回发送的数据
        /// <summary>
        /// 对文本和图片数据进行加密;如果长度超过限制，直接抛给文件处理中心
        /// </summary>
        /// <param name="stateCode">StateCode</param>
        /// <param name="state">StateBase</param>
        /// <returns>要发送的数据</returns>
        internal static byte[] encryption(DataModel stateCode, TransmitData state)
        {
            byte[] returnByte = null;
            //表示字符暗号
            if (stateCode.State == CipherCode._textCode)
            {
                //取得文本数据
                byte[] date = Encoding.UTF8.GetBytes(stateCode.Datestring);
                //对数据主体部分进行加密,加密之后的数据（1暗号+1暗号+4数据标签+date）
                returnByte = encryptionTemporary(date, stateCode.State, state);
            }
            else if (stateCode.State == CipherCode._photographCode)
            {
                //对数据主体部分进行加密,加密之后的数据（1暗号+1暗号+4数据标签+date）
                returnByte = encryptionTemporary(stateCode.DateByte, stateCode.State, state);
            }
            return returnByte;
        }
        #endregion
        #region 对数据加入暗号与数据标签
        /// <summary>
        /// 对数据加入暗号与数据标签
        /// </summary>
        /// <param name="date">要加密的数据</param>
        /// <param name="textCode">数据模型的暗号</param>
        /// <param name="state">StateBase</param>
        /// <returns>加密之后的数据（1暗号+1暗号+4数据标签+date）</returns>
        private static byte[] encryptionTemporary(byte[] date, byte textCode, TransmitData state)
        {

            if (date.Length > state.BufferSize - 20)
                //超出通过文件大数据包处理发送
                return EncDecSeparateDate.SendHeadEncryption(date, textCode, state);
            //给发送的数据进行编号
            state.SendDateLabel = RandomPublic.RandomNumber(16787);
            //编号并加密 （加密
            byte[] dateOverall = ByteToDate.OffsetEncryption(date, state.SendDateLabel, 2);
            dateOverall[0] = CipherCode._commonCode;
            dateOverall[1] = textCode;
            return dateOverall;
        }
        #endregion
        #region 对文本和图片数据进行解密;
        /// <summary>
        /// 对文本和图片数据进行解密;
        /// </summary>
        /// <param name="date">接收到的数据</param>
        /// <param name="state">StateBase</param>
        /// <returns>返回一个图片</returns>
        internal static DataModel deciphering(byte[] date, TransmitData state)
        {
            DataModel stateCode = null;
            //小于6，是说：暗号类型1位+暗号1位+数据标签4位
            if (date.Length < 6)
                return stateCode;//收到的数据不正确
            byte headDate = date[1];
            //当是图片与文本时
            if (headDate == CipherCode._textCode || headDate == CipherCode._photographCode)
            {
                int SendDateLabel = 0;
                //取得数据，并取得数据标签
                byte[] dateAll = ByteToDate.OffsetDecrypt(date, out SendDateLabel, 2);
                //回复此数据标签的数据是成功收到了
                byte[] ReplyDate = ByteToDate.CombinationTwo(CipherCode._commonCode, CipherCode._dateSuccess, SendDateLabel);
                //判断是否文本
                if (headDate == CipherCode._textCode)
                {
                    //文本
                    string str = Encoding.UTF8.GetString(dateAll);
                    stateCode = new DataModel(CipherCode._textCode, str, ReplyDate);//解析出来是文本数据
                }
                else
                {
                    //图片                    
                    stateCode = new DataModel(CipherCode._photographCode, dateAll, ReplyDate);//解释出来是图片数据
                }

            }
            else if (headDate == CipherCode._dateSuccess)//数据成功或重发
            {
                //找出已发数据的标签
                int SendDateLabel = ByteToDate.ByteToInt(2, date);
                if (headDate == CipherCode._dateSuccess)
                {
                    stateCode = new DataModel(headDate);//生成一个成功信息的数据模型
                    if (SendDateLabel == state.SendDateLabel)
                    { state.SendDate = null; }//已经成功对已发数据进行删除
                }
            }
            return stateCode;
        }
        #endregion
    }
}
