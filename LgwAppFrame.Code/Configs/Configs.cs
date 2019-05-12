using System;
using System.Configuration;

namespace LgwAppFrame.Code
{
    public class Configs
    {
        #region 根据Key取配置文件中Value值
        public static string configPath = FileHelper.SwitchPath(@"/Configs/system.config");
        /// <summary>
        /// 根据Key取Value值
        /// </summary>
        /// <param name="key"></param>
        public static string GetValue(string key)
        {

            if (!FileHelper.IsExistFile(configPath))
                throw new Exception(string.Format("配置文件不存在：{0}", configPath));
            ExeConfigurationFileMap ecf = new ExeConfigurationFileMap();
            ecf.ExeConfigFilename = configPath;

            Configuration config = ConfigurationManager.OpenMappedExeConfiguration(ecf, ConfigurationUserLevel.None);
            return config.AppSettings.Settings[key].Value.ToString().Trim();//读取配置文件key对应的值
        }
        #endregion

        #region 根据Key修改配置文件中Value值
        /// <summary>
        /// 根据Key修改Value
        /// </summary>
        /// <param name="key">要修改的Key</param>
        /// <param name="value">要修改为的值</param>
        public static void SetValue(string key, string value)
        {
            System.Xml.XmlDocument xDoc = new System.Xml.XmlDocument();
            xDoc.Load(configPath);
            System.Xml.XmlNode xNode;
            System.Xml.XmlElement xElem1;
            System.Xml.XmlElement xElem2;
            xNode = xDoc.SelectSingleNode("//appSettings");

            xElem1 = (System.Xml.XmlElement)xNode.SelectSingleNode("//add[@key='" + key + "']");
            if (xElem1 != null) xElem1.SetAttribute("value", value);
            else
            {
                xElem2 = xDoc.CreateElement("add");
                xElem2.SetAttribute("key", key);
                xElem2.SetAttribute("value", value);
                xNode.AppendChild(xElem2);
            }
            xDoc.Save(configPath);
        }
        #endregion
    }
}
