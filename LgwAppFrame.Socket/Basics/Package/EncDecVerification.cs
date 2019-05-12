using System.Text;

namespace LgwAppFrame.SocketHelper.Basics.Package
{
    /// <summary>
    ///  验证类型暗号处理中心
    /// </summary>
    internal class EncDecVerification
    {
        /// <summary>
        /// 返回2字节验证类型的暗号的数据包
        /// </summary>
        /// <param name="Verification">暗号</param>
        /// <returns>加密之后数据</returns>
        internal static byte[] EncryptionVerification(byte Verification)
        {
            byte[] haveDate = new byte[2];
            haveDate[0] = CipherCode._verificationCode;
            haveDate[1] = Verification;
            return haveDate;
        }
        /// <summary>
        /// 取得暗号
        /// </summary>
        /// <param name="Verification">收到的暗号数据</param>
        /// <returns>返回暗号</returns>
        internal static byte DecryptVerification(byte[] Verification)
        {
            if (Verification.Length != 2)
                return 0;
            return Verification[1];
        }
    }
}
