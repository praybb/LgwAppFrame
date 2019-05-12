using System;
using System.Collections.Generic;

namespace LgwAppFrame.SocketHelper.Basics.Package
{ /// <summary>
  /// 粘包处理类；
  /// </summary>
  /// <remarks>用于粘包的加密与解密工作</remarks>
    internal class StickPackage
    {
        /// <summary>
        /// 对TCP发送进行粘包加密；
        /// </summary>
        /// <param name="sendDate">要加密的数据</param>
        /// <returns>加密之后的数据为：粘包代码4byte+数据包长度4byte</returns>
        /// <remarks>把数据包处理为：粘包代码4byte+数据包长度4byte</remarks>
        internal static void EncryptionPackage(ref byte[] sendDate)
        {
            //把数据包处理为：粘包代码4byte+数据包长度4byte
            byte[] dateAll = new byte[sendDate.Length + 8];
            ByteToDate.IntToByte(CipherCode._stickPackageCode, 0, dateAll);
            ByteToDate.IntToByte(sendDate.Length, 4, dateAll);
            sendDate.CopyTo(dateAll, 8);
            sendDate = dateAll;
        }
        /// <summary>
        /// 对TCP粘包数据进行解密；把所有完整的包通过集合形式返回给客户
        /// </summary>
        /// <param name="receiveDate">接收到的数据</param>
        /// <param name="residualpackage">上次残留的数据</param>
        /// <returns>返回的数据集合; 注意ref侧重修改，out侧重输出。</returns>
        internal static List<byte[]> DecryptPackage(byte[] receiveDate, ref byte[] residualpackage)
        {
            //设置一个字节数据列表
            List<byte[]> listDate = new List<byte[]>();
            //如果长度小于，则直接返回
            if (receiveDate.Length < 4)
                return listDate;
            while (true)
            {
                //返加一个完整包的数据
                byte[] haveDate = DecryptByte(receiveDate, ref residualpackage);
                //不为空放到数据最后面
                if (haveDate != null)
                    listDate.Add(haveDate);
                //没数据，又没残留,直接退出循环
                if (haveDate == null || residualpackage == null)
                    break;
                //残留数据不够4位退出循环
                if (residualpackage.Length < 4)
                    break;
                receiveDate = residualpackage; residualpackage = null;
            }
            return listDate;
        }
        /// <summary>
        /// 解密数组
        /// </summary>
        /// <param name="receiveDate">接收到的数据</param>
        /// <param name="residualpackage">残留的数据</param>
        /// <returns>返加一个完整包的数据 （这里得到的是已去除粘包代码与数据长度的数据包）</returns>
        private static byte[] DecryptByte(byte[] receiveDate, ref byte[] residualpackage)
        {
            byte[] haveDate = null;
            //   bool ddd = false;
            //取得粘包代码，（取前4字节）            
            int stickPackageCode = ByteToDate.ByteToInt(0, receiveDate);
            if (stickPackageCode == CipherCode._stickPackageCode)
            {
                //为粘包代码时
                if (receiveDate.Length < 9) //这里表示收到空的数据
                { residualpackage = receiveDate; return null; }
                //取数据长度
                int datelenth = ByteToDate.ByteToInt(4, receiveDate);
                //清空残留数据
                residualpackage = null;//对残留数据进行初始化
                if (datelenth == receiveDate.Length - 8)
                { //说明整个收到的数据就是一个完整包
                    haveDate = new byte[datelenth];
                    Array.Copy(receiveDate, 8, haveDate, 0, datelenth);
                }
                else if (datelenth > receiveDate.Length - 8)
                { //说明数据没有收完全,把数据放在残留包里进行下一轮接收
                    residualpackage = receiveDate;
                }
                else
                {
                    //说明有至少二个包粘在一起
                    //取出粘包的前面一个包数据
                    haveDate = new byte[datelenth];
                    //残留的数据长度                    
                    var residualDataLength = receiveDate.Length - 8 - datelenth;
                    Array.Copy(receiveDate, 8, haveDate, 0, datelenth);
                    //包后面的数据放到残留，把剩下的扔然放在残留里
                    residualpackage = new byte[residualDataLength];
                    Array.Copy(receiveDate, 8 + datelenth, residualpackage, 0, residualDataLength);

                }
            }
            else
            {
                //不为粘包代码时；说明数据有可能是前面一个接下去的
                //  ddd = true;
                if (residualpackage != null)
                {
                    // 这里为上次 “说明数据没有收完全,把数据放在残留包里进行下一轮接收”这里下来的
                    //残留数据不为空,合并到接收的数据中（格式：残留数据+收到的数据） ,并清空残留
                    byte[] addDate = new byte[receiveDate.Length + residualpackage.Length];
                    residualpackage.CopyTo(addDate, 0);
                    receiveDate.CopyTo(addDate, residualpackage.Length);
                    receiveDate = addDate; residualpackage = null;
                }
                else//不知道是什么数据，直接扔掉
                { return null; }
            }

            return haveDate;
        }
    }
}
