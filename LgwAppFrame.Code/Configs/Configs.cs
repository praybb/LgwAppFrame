using System;
using System.Configuration;
using System.Xml;

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
            //  connectionStrings
           
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
            XmlDocument xDoc = new System.Xml.XmlDocument();
            if (!FileHelper.IsExistFile(configPath))
                createXml();
            xDoc.Load(configPath);  
            XmlNode xNode = xDoc.SelectSingleNode("//appSettings");
            XmlElement xElem1 = (XmlElement)xNode.SelectSingleNode("//add[@key='" + key + "']");
            if (xElem1 != null) xElem1.SetAttribute("value", value);
            else
            {
                XmlElement xElem2 = xDoc.CreateElement("add");
                xElem2.SetAttribute("key", key);
                xElem2.SetAttribute("value", value);
                xNode.AppendChild(xElem2);
            }
            xDoc.Save(configPath);
        }
        #endregion

        public static void createXml()
        {
            //使用XmlDocument创建xml
            XmlDocument xmldoc = new XmlDocument();
            XmlDeclaration xmldec = xmldoc.CreateXmlDeclaration("1.0", "utf-8", "yes");
            xmldoc.AppendChild(xmldec);
            //添加根节点
            XmlElement configurationElement = xmldoc.CreateElement("configuration");
            xmldoc.AppendChild(configurationElement);
            //添加根节点下的子节点元素
            XmlElement appSettingsElement = xmldoc.CreateElement("appSettings");
            configurationElement.AppendChild(appSettingsElement);
            ////添加子节点下的元素
            //XmlElement addElement = xmldoc.CreateElement("add");
            //addElement.SetAttribute("key", "server");
            //addElement.SetAttribute("value", "127.0.0.1");
            //appSettingsElement.AppendChild(addElement);
             //保存文件
            xmldoc.Save(configPath);
         

        }
    }
}
