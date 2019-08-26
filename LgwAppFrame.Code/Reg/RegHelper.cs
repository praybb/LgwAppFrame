using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LgwAppFrame.Code
{
    public class RegHelper
    {

        #region 新增项
        /// <summary>
        /// 新增项
        /// </summary>
        /// <param name="RegHive">注册表基项如 HKEY_LOCAL_MACHINE</param>
        /// <param name="SubKeyPath">项的注册表键path</param>
        /// <param name="SubKey">新增的项</param>       
        /// <returns></returns>
        public static bool CreateRegistrySub(string RegHive, string keyPath,string SubKey)
        {
            try
            {
                RegistryKey optionKey = OpenRegistryKey(RegHive, keyPath, true);
               // RegistryKey optionKey = regKey.OpenSubKey(SubKeyPath, true);
                //添加子键和值
               // if (string.IsNullOrEmpty(SubKey))
                RegistryKey subkey = optionKey.CreateSubKey(SubKey);
              //  subkey.SetValue(keyName, valueStr, getRegVlaue(VlaueKind));
                return true;
            }
            catch
            {
                return false;
                throw;
            }

        }
        #endregion
        #region 删除项
        /// <summary>
        /// 删除项
        /// </summary>
        /// <param name="RegHive">注册表基项如 HKEY_LOCAL_MACHINE</param>
        /// <param name="keyPath">项的注册表键path</param>
        /// <param name="SubKey">项</param>
        /// <returns></returns>
        public static bool DeleteRegistrySub(string RegHive, string keyPath, string SubKey)
        {

            return DeleteRegistrySub(RegHive, keyPath, SubKey,false);
        }
        /// <summary>
        /// 删除项递归
        /// </summary>
        /// <param name="RegHive">注册表基项如 HKEY_LOCAL_MACHINE</param>
        /// <param name="keyPath">项的注册表键path</param>
        /// <param name="SubKey">项</param>
        /// <param name="recursion">递归</param>
        /// <returns></returns>
        public static bool DeleteRegistrySub(string RegHive, string keyPath, string SubKey, bool recursion)
        {
           try
            {
                RegistryKey optionKey = OpenRegistryKey(RegHive, keyPath, true);
                string[] subKeys = optionKey.GetSubKeyNames();

                foreach (string akey in subKeys)
                {
                    if (akey == SubKey)
                    {
                        if (recursion)
                            optionKey.DeleteSubKeyTree(SubKey);
                        else
                            optionKey.DeleteSubKey(SubKey);
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
                throw;
            }

        }
        #endregion
        #region 新增或修改注册表键
        /// <summary>
        ///  新增或修改某键值
        /// </summary>
        /// <param name="RegHive">注册表基项如 HKEY_LOCAL_MACHINE</param>
        /// <param name="keyPath">键的注册表键path</param>
        /// <param name="valueName">键名</param>
        /// <param name="VlaueKind">键类型 如:"REG_EXPAND_SZ"</param>
        /// <param name="valueStr">键值</param>
        /// <returns> RegHelper.SetRegistryValue("HKEY_CURRENT_USER", @"Software\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders", @"Favorites", "REG_EXPAND_SZ", @"D:\我的文档\收藏夹");</returns>
        public static bool SetRegistryValue(string RegHive, string keyPath, string valueName, string VlaueKind, string valueStr)
        {
            bool sc = false;
            try
            {
                //  RegistryKey regKey = Registry.LocalMachine;
                RegistryKey mainKey = OpenRegistryKey(RegHive, keyPath, true);
               
                mainKey.SetValue(valueName, valueStr, getRegVlaue(VlaueKind));
                mainKey.Close();
                sc = true;
            }
            catch (Exception ex)
            {
                sc = false;
                LogHelper.WriteLine(new Msg(ex.ToString(), MsgType.Warning));
            }
            return sc;
        }
        #endregion
        #region  取得注册表的键值
        /// <summary>
        /// 取得注册表的键值
        /// </summary>
        /// <param name="Hive">注册表基项如 HKEY_LOCAL_MACHINE</param>
        /// <param name="path">键的path</param>
        /// <param name="valueName">键名</param>
        /// <returns></returns>
        public static string GetRegistryValue(string Hive, string path, string valueName)
        {
            RegistryKey regkey = null;
            try
            {
                regkey = OpenRegistryKey(Hive, path, false);
                // regkey = Registry.LocalMachine.OpenSubKey(path);
                if (regkey == null)
                {
                    LogHelper.WriteLine(new Msg("无法找到注册表路径:" + path, MsgType.Warning));
                    return null;
                }

                object val = regkey.GetValue(valueName);
                if (val == null)
                {
                    LogHelper.WriteLine(new Msg("无法找到注册表项:" + valueName, MsgType.Warning));
                    return null;
                }

                return val.ToString();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLine(new Msg(ex.ToString(), MsgType.Error));
                return null;
            }
            finally
            {
                if (regkey != null)
                {
                    regkey.Close();
                }
            }
        }
        #endregion
        #region 删除键
        /// <summary>
        /// 删除键
        /// </summary>
        /// <param name="RegHive">注册表基项如 HKEY_LOCAL_MACHINE</param>
        /// <param name="keyPath">键的注册表键path</param>
        /// <param name="valueName">键名</param>
        /// <returns></returns>
        public static bool DelRegistryValue(string RegHive, string keyPath, string valueName)
        {



            bool sc = false;
            try
            {
                //  RegistryKey regKey = Registry.LocalMachine;
                RegistryKey mainKey = OpenRegistryKey(RegHive, keyPath, true);

                mainKey.DeleteValue(valueName,true);
                mainKey.Close();
                sc = true;
            }
            catch (Exception ex)
            {
                sc = false;
                LogHelper.WriteLine(new Msg(ex.ToString(), MsgType.Warning));
            }
            return sc;
        }
        #endregion

        #region 判断注册表项是否存在
        /// <summary>
        /// 判断注册表项是否存在
        /// </summary>
        /// <param name="RegHive">注册表基项如 HKEY_LOCAL_MACHINE</param>
        /// <param name="keyPath">项的注册表项path</param>
        /// <param name="sKeyName"></param>
        /// <returns></returns>
        public bool IsRegistryKeyExist(string RegHive ,string keyPath ,string sKeyName)
        {
            string[] sKeyNameColl;
            RegistryKey optionKey = OpenRegistryKey(RegHive, keyPath, false);
            sKeyNameColl = optionKey.GetSubKeyNames(); //获取SOFTWARE下所有的子项
            foreach (string sName in sKeyNameColl)
            {
                if (sName == sKeyName)
                {
                    optionKey.Close();
                 //   hkSoftWare.Close();
                    return true;
                }
            }
            optionKey.Close();
            //hkSoftWare.Close();
            return false;
        }

        #endregion
        #region 判断键值是否存在

        /// <summary>
        /// 判断键值是否存在
        /// </summary>
        /// <param name="RegHive">注册表基项如 HKEY_LOCAL_MACHINE</param>
        /// <param name="keyPath">项的注册表键path</param>
        /// <param name="sValueName"></param>
        /// <returns></returns>
        private bool IsRegistryValueNameExist(string RegHive, string keyPath, string sValueName)
        {
            string[] sValueNameColl;
            RegistryKey optionKey = OpenRegistryKey(RegHive, keyPath, false);
            sValueNameColl = optionKey.GetValueNames(); //获取test下所有键值的名称
            foreach (string sName in sValueNameColl)
            {
                if (sName == sValueName)
                {
                    optionKey.Close();                   
                    return true;
                }
            }
            optionKey.Close();
            return false;
        }
        #endregion

        #region 取得注册表的值类型
        /// <summary>
        /// 取得注册表的值类型
        /// </summary>
        /// <param name="VlaueType">注册</param>
        /// <returns></returns>
        private static RegistryValueKind getRegVlaue(string VlaueType)
        {
          try { 
            RegistryValueKind vlauekind;
            switch (VlaueType)
            {
                case "REG_EXPAND_SZ":
                    vlauekind = RegistryValueKind.ExpandString;
                    break;
                case "REG_SZ":
                    vlauekind = RegistryValueKind.String;
                    break;
                case "REG_BINARY":
                    vlauekind = RegistryValueKind.Binary;
                    break;
                case "REG_DWORD":
                    vlauekind = RegistryValueKind.DWord;
                    break;
                case "REG_QWORD":
                    vlauekind = RegistryValueKind.QWord;
                    break;
                case "REG_MULTI_SZ":
                    vlauekind = RegistryValueKind.MultiString;
                    break;
                default:
                    throw new ArgumentException("参数无效,未知的注册表值类型:" + VlaueType);
            }
            return vlauekind;
            }
            catch 
            {
                throw new ArgumentException("参数无效,未知的注册表类型:" + VlaueType);
            }
        }
        #endregion
        #region  打开并取得当前操作系统相对应的视图
        /// <summary>
        /// 打开并取得当前操作系统相对应的视图
        /// </summary>
        /// <param name="RegHive">注册表的基项</param>
        /// <param name="keyPath">注册表路径</param>
        /// <param name="access">读写权限</param>
        /// <returns></returns>
        private static RegistryKey OpenRegistryKey(string RegHive, string keyPath,bool access)
        {
            try
            {
                RegistryHive ret;
                switch (RegHive)
                {
                    case "HKEY_CLASSES_ROOT":
                        ret = RegistryHive.ClassesRoot;
                        break;
                    case "HKEY_CURRENT_USER":
                        ret = RegistryHive.CurrentUser;
                        break;
                    case "HKEY_LOCAL_MACHINE":
                        ret = RegistryHive.LocalMachine;
                        break;
                    case "HKEY_USERS":
                        ret = RegistryHive.Users;
                        break;
                    case "HKEY_PERFORMANCE_DATA":
                        ret = RegistryHive.PerformanceData;
                        break;
                    case "HKEY_CURRENT_CONFIG":
                        ret = RegistryHive.CurrentConfig;
                        break;
                    case "HKEY_DYN_DATA":
                        ret = RegistryHive.DynData;
                        break;
                    default:
                        ret = RegistryHive.LocalMachine;
                        throw new Exception("未知的注册基项");

                }
                RegistryKey localMachineRegistry = RegistryKey.OpenBaseKey(ret,
                                                       Environment.Is64BitOperatingSystem
                                                      ? RegistryView.Registry64
                                                      : RegistryView.Registry32);

                return string.IsNullOrEmpty(keyPath)
                    ? localMachineRegistry
                    : localMachineRegistry.OpenSubKey(keyPath, access);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLine(new Msg("无法找到注册表基项:" + RegHive, MsgType.Warning));
                return null;
            }
        }
        #endregion
      
    }
}
