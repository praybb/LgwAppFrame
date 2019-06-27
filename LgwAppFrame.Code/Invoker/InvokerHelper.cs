using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LgwAppFrame.Code
{
    /// <summary>
    /// 快速反射调用
    /// </summary>
    public class InvokerHelper
    {
        #region 基础属性
        /// <summary>
        /// 类类型
        /// </summary>
        public Type objType { get; set; }
        /// <summary>
        /// 程序集名
        /// </summary> 
        private string objAssemblyName { get; set; }
        /// <summary>
        /// 类的命名空间
        /// </summary>
        private string objSpaceName { get; set; }
        /// <summary>
        /// 类名称
        /// </summary>
        private string objClassName { get; set; }
        #endregion

        #region 构架方法
        /// <summary>
        /// 构架方法
        /// </summary>
        /// <param name="dllfile">文件所在的相对目录</param>
        /// <param name="assemblyName">程序集</param>
        /// <param name="spaceName">命名空间</param>
        /// <param name="className"></param>
        public InvokerHelper(string dllfile, string assemblyName, string spaceName, string className)
        {
            //设置属性
            this.objClassName = className;
            this.objSpaceName = spaceName;
            this.objAssemblyName = assemblyName;

            Assembly asm = Assembly.LoadFrom(FileHelper.SwitchPath(dllfile));//要绝对路径
            objType = asm.GetType(spaceName + "." + className,true);//必须使用名称空间+类名称,利用类型的命名空间和名称获得类型需要实例化类型                   
        }
        #endregion

        #region 创建对象实例
        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <typeparam name="T">类型转换并返回</typeparam>
        /// <param name="objType">类类型</param>
        /// <returns></returns>
        public static T CreateInstance<T>(Type objType)
        {
          //  string path = fullName + "," + assemblyName;//命名空间.类型名,程序集
            //Type o = Type.GetType(path);//加载类型
            object obj = Activator.CreateInstance(objType);//根据类型创建实例
            return (T)obj;//类型转换并返回
        }

        #endregion

        #region 快速调用
        /// <summary>
        /// 快速调用方法
        /// </summary>
        /// <typeparam name="T">返回值的类型</typeparam>
        /// <param name="methodName">方法名</param>
        /// <param name="param">方法参数组 例：new object[] { word, p, 3 };</param>
        /// <param name="paramtypes">方法参数类型组 例：new Type[] { typeof(string),typeof(char),typeof(int) }</param>
        /// <remarks>取得方法，然后快速调用</remarks>
        public T Invokermethod<T>(string methodName, object[] param, Type[] paramtypes)
        {
           // object obj = Activator.CreateInstance(objType);//根据类型创建实例    
            MethodInfo method = objType.GetMethod(methodName, paramtypes);//方法的名称            
            FastMethodInvoker.FastInvokeHandler fastInvoker = FastMethodInvoker.GetMethodInvoker(method);
            return (T)fastInvoker(objType, param);

        }
        #endregion

        #region 实例化与取赋值
        /// <summary>
        /// 取得类中的属性值
        /// </summary>
        /// <param name="FieldName"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetModelValue(string FieldName, object obj)
        {
            try
            {
                Type Ts = obj.GetType();
                object o = Ts.GetProperty(FieldName).GetValue(obj, null);
                string Value = Convert.ToString(o);
                if (string.IsNullOrEmpty(Value)) return null;
                return Value;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 设置类中的属性值
        /// </summary>
        /// <param name="FieldName">成员变量</param>
        /// <param name="Value">值</param>
        /// <param name="obj">实例化</param>
        /// <returns>设置成功返回Trun，不成功fale</returns>
        public static bool SetModelValue(string FieldName, string Value, object obj)
        {
            try
            {
                Type Ts = obj.GetType();
                object v = Convert.ChangeType(Value, Ts.GetProperty(FieldName).PropertyType);
                Ts.GetProperty(FieldName).SetValue(obj, v, null);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
