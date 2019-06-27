using LgwAppFrame.Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace LgwAppFrame.Code
{
   public class Js
    {
        private static InvokerHelper ScriptControl = new InvokerHelper("Interop.MSScriptControl.dll", "Interop.MSScriptControl", "MSScriptControl", "ScriptControl");

      
       
        /// <summary>
        /// 执行JS
        /// </summary>
        /// <param name="sExpression">JS的名称方法名</param>
        /// <param name="filepath">JavaScript目录文件文件</param>
        /// <returns>执行后的字符串</returns>
        public static string ExecuteScript(string sExpression, string filepath)
        {




          
        //  var t =  InvokerHelper.SetModelValue("Language", "JScript", InvokerHelper.CreateInstance<object>(ScriptControl.objType));
         //   var v = InvokerHelper.SetModelValue("UseSafeSubset", "JScript", InvokerHelper.CreateInstance<object>(ScriptControl.objType));
            //if (obj == null) return null;
            //object ScriptControl = Activator.CreateInstance(type);
            //obj.InvokeMember("Language", BindingFlags.SetProperty, null, ScriptControl, new object[] { "JavaScript" });
            //string js = File.ReadAllText(filepath);
            //obj.InvokeMember("AddCode", BindingFlags.InvokeMethod, null, ScriptControl, new object[] { js });
            //return obj.InvokeMember("Eval", BindingFlags.InvokeMethod, null, ScriptControl, new object[] { "RL(1, 2)" }).ToString();
            return null;
            //var GetJS = 
            //ScriptControl.CreateClass();
            //UseSafeSubset(true);
            //Language("JScript");
            //AddCode(GetJS);
            //try
            //{
            //    string str = Eval(sExpression).ToString();
            //    return str;
            //}
            //catch (Exception ex)
            //{
            //    string str = ex.Message;
            //}
            //return null;

            //var GetJS = File.ReadAllText(filepath);
            //MSScriptControl.ScriptControl scriptControl = new MSScriptControl.ScriptControl();
            //scriptControl.UseSafeSubset = true;
            //scriptControl.Language = "JScript";
            //scriptControl.AddCode(GetJS);
            //try
            //{
            //    string str = scriptControl.Eval(sExpression).ToString();
            //    return str;
            //}
            //catch (Exception ex)
            //{
            //    string str = ex.Message;
            //}
            //return null;
        }
    }
}
