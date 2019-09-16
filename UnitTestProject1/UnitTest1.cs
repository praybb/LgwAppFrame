using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management;
using System.Windows;
using LgwAppFrame.Code;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using LgwAppFrame.Domain.Entity.SystemManage;
using LgwAppFrame.Domain.IRepository.SystemManage;
using LgwAppFrame.Repository.SystemManage;
using System.Linq;
using LgwAppFrame.EFDate;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        private IUserRepository service = new UserRepository();

        [TestMethod]

        public void TestMethod1()
        {
         //   FEDbContext dbcontext = new FED bContext();
          //  var f = (from m in dbcontext.)
        //     UserEntity t = new UserEntity();

        ////   Console.WriteLine(service.FindEntity("dfdsfsf").password.ToString()); 


        //    t.Create();
        //   Console.WriteLine(t.CDATE_);
        //   List<UserEntity> e = service.FindList("select * from dbo.Sys_User where UUID_ = 'dfdsfsf'");
        //   foreach (var ew in e) {
        //       Console.WriteLine(e.ToJson());
      //  Console.WriteLine("\a");
        }
    }
}

       //     var str = service.IQueryable().ToList();
       //     // List<string> w = new List<string>();
       //foreach (var e  in str)
       //     {
       //         Console.WriteLine(e.ToString());
       //     }
           
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
         //   Console.WriteLine(SysGuid.GetNetCardMACAddress());
            //Console.WriteLine(SysGuid.GetSystemRegProductId());
   
