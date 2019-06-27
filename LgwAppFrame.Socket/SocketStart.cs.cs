namespace LgwAppFrame.SocketHelper
{
  public  class SocketStart
    {
        /// <summary>
        /// 注册为服务器,返回一个ITxServer类,再从ITxServer中的startServer一个方法启动服务器
        /// </summary>
        /// <param name="port">端口</param>
        /// <returns>ITxServer</returns>
        public static ISocketServer startServer(int port)
        {
            ISocketServer server = new SocketServer(port);
            return (server);
        }
        /// <summary>
        /// 注册为客户端,返回一个ITxServer类,再从ITxClient中的startClient一个方法启动客户端;
        /// </summary>
        /// <param name="ip">ip地址</param>
        /// <param name="port">端口</param>
        /// <returns>ITxClient</returns>
        public static ISocketClient startClient(string ip, int port)
        {
            ISocketClient client = new SocketClient(ip, port);
            return (client);
        }
        /// <summary>
        /// 注册Udp服务端；端口在Port属性设置，默认为随机；具体到StartEngine启动
        /// </summary>
        /// <returns></returns>
        public static ISocketUdb startUdp()
        {
            ISocketUdb udp = new SocketUdb();
            return (udp);
        }
    }
}
