using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace  LgwAppFrame.Code
{
    /// <summary>
    /// NTP服务
    /// </summary>
   public  class NtpHelper
    {
        #region NTP时间取得
        /// <summary>
        /// 取得网络上的时间
        /// </summary>
        /// <returns>
        /// 注意请勿多次调用,不然服务器会阻断其连接！
        /// </returns>
        public static DateTime GetNetworkTime()
        {
            //时间服务器地址 下面是苹果服务器的NTP时间
            //const string ntpServer = "time.asia.apple.com";
            const string ntpServer = "ntp.sjtu.edu.cn";
            var addresses = Dns.GetHostEntry(ntpServer).AddressList;

            // NTP message size - 16 bytes of the digest (RFC 2030)
            var ntpData = new byte[48];

            //Setting the Leap Indicator, Version Number and Mode values
            ntpData[0] = 0x1B; //LI = 0 (no warning), VN = 3 (IPv4 only), Mode = 3 (Client Mode)            

            //The UDP port number assigned to NTP is 123
            var ipEndPoint = new IPEndPoint(addresses[0], 123);
            //NTP uses UDP
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            socket.Connect(ipEndPoint);

            //Stops code hang if NTP is blocked
            socket.ReceiveTimeout = 1000;

            socket.Send(ntpData);
            socket.Receive(ntpData);
            socket.Close();

            //Offset to get to the "Transmit Timestamp" field (time at which the reply
            //departed the server for the client, in 64-bit timestamp format."
            const byte serverReplyTime = 40;

            //Get the seconds part
            ulong intPart = BitConverter.ToUInt32(ntpData, serverReplyTime);

            //Get the seconds fraction
            ulong fractPart = BitConverter.ToUInt32(ntpData, serverReplyTime + 4);

            //Convert From big-endian to little-endian
            intPart = SwapEndianness(intPart);
            fractPart = SwapEndianness(fractPart);

            var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);

            //**UTC** time
            var networkDateTime = (new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc)).AddMilliseconds((long)milliseconds);

            return networkDateTime.ToLocalTime();
        }
        static uint SwapEndianness(ulong x)
        {
            return (uint)(((x & 0x000000ff) << 24) +
            ((x & 0x0000ff00) << 8) +
            ((x & 0x00ff0000) >> 8) +
            ((x & 0xff000000) >> 24));
        }
        #endregion
        #region 返回本地与远程的时间间隔差
        /// <summary>
        /// 返回本地与远程的时间间隔差
        /// </summary>
        /// <returns>
        /// 如果本地时间慢了为正数，如果本地时间快了为负数
        /// 使用方法DateTime.Now.Add(NTPtimeSpan); 关于时间间隔的初始方法DateTime.Now.Add(new TimeSpan(00,00, 00,-51, -597))
        /// </returns>
        public static TimeSpan GetNetworkTimeSpan()
        {
            TimeSpan NTPtimeSpan;
            //快了多少时间
             NTPtimeSpan = GetNetworkTime().Subtract(System.DateTime.Now); 
            return NTPtimeSpan;
        }
        // stackoverflow.com/a/3294698/162671

        #endregion
    }
}
