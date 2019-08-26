using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management;
using System.Windows;
using LgwAppFrame.Code;
using System.Security.Cryptography;
using System.Text;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]

        public void TestMethod1()
        {
            //string reghive = "hkey_current_user";
            //string winrarpath = @"software\microsoft\windows\currentversion\explorer\user shell folders";
            //string key = @"favorites1";
            //bool okfd = reghelper.createregistrysub(reghive, winrarpath, "test");
            //string value = RegHelper.GetRegistryValue("HKEY_LOCAL_MACHINE",Winrarpath, key);
            //// LogHelper.WriteLine(new Msg(GetCPUSerialNumber(), MsgType.Success));
            //if (okfd)
            // var md5 = MD5.Create();

            //  System.Web.HttpUtility.UrlEncode(strs);
            // Configs.createXml();
            // Console.WriteLine(Configs.GetValue("server"));
            // Configs.SetValue("server","127.0.0.1");
            // Console.WriteLine(SysGuid.GetHardDiskSerialNumber());
            // Console.WriteLine(SysGuid.GetBIOSSerialNumber());
            Console.WriteLine(SysGuid.GetNetCardMACAddress());
            //Console.WriteLine(SysGuid.GetSystemRegProductId());
         
        }


     
    }
}
