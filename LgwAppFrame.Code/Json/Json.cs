using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace LgwAppFrame.Code
{
    public static class Json
    {
        #region 反序列化JSON . net对象.
        /// <summary>
        /// 反序列化JSON . net对象.
        /// </summary>
        /// <param name="Json"></param>
        /// <returns></returns>
        public static object ToJson(this string Json)
        {
            return Json == null ? null : JsonConvert.DeserializeObject(Json);
        }
        #endregion

        #region 序列化指定的object对象
        /// <summary>
        /// 序列化指定的对象一个JSON字符串使用Newtonsoft.Json.JsonConverter的集合。
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>返回个一个JSON对象的字符串</returns>
        public static string ToJson(this object obj)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            return JsonConvert.SerializeObject(obj, timeConverter);
        }
        #endregion

        #region 序列化指定的object对象,使用指定的时间格式
        /// <summary>
        /// 序列化指定的对象一个JSON字符串使用Newtonsoft.Json.JsonConverter的集合。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="datetimeformats">时间格式如"yyyy-MM-dd HH:mm:ss"</param>
        /// <returns>返回个一个JSON对象的字符串</returns>
        public static string ToJson(this object obj, string datetimeformats)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = datetimeformats };
            return JsonConvert.SerializeObject(obj, timeConverter);
        }
        #endregion

        #region 反序列化泛型
        /// <summary>
        /// 反序列化泛型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Json"></param>
        /// <returns>泛型</returns>
        public static T ToObject<T>(this string Json)
        {
            return Json == null ? default(T) : JsonConvert.DeserializeObject<T>(Json);
        }
        #endregion

        #region 反序列化泛型集合
        /// <summary>
        /// 反序列化泛型集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Json"></param>
        /// <returns>泛型集合</returns>
        public static List<T> ToList<T>(this string Json)
        {
            return Json == null ? null : JsonConvert.DeserializeObject<List<T>>(Json);
        }
        #endregion

        #region 反序列化DataTable
        /// <summary>
        /// 反序列化DataTable
        /// </summary>
        /// <param name="Json"></param>
        /// <returns>DataTable</returns>
        public static DataTable ToTable(this string Json)
        {
            return Json == null ? null : JsonConvert.DeserializeObject<DataTable>(Json);
        }
        #endregion

        #region 反序列化,返回JObjec对象
        /// <summary>
        /// 反序列化,返回JObjec对象
        /// </summary>
        /// <param name="Json"></param>
        /// <returns>JObjec对象</returns>
        public static JObject ToJObject(this string Json)
        {
            return Json == null ? JObject.Parse("{}") : JObject.Parse(Json.Replace("&nbsp;", ""));
        }
        #endregion

    }
}
