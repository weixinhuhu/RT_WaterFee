/*
 * 项目地址:http://git.oschina.net/ggshihai/DBLib
 * Author:DeepBlue
 * QQ群:257018781
 * Email:xshai@163.com
 * 说明:一些常用的操作类库.
 * 额外说明:东拼西凑的东西,没什么技术含量,爱用不用,用了你不吃亏,用了你不上当,不用你也取不了媳妇...
 * -------------------------------------------------- 
 * -----------我是长长的美丽的善良的分割线-----------
 * -------------------------------------------------- 
 * 我曾以为无惧时光荏苒 如今明白谁都逃不过似水流年
 * --------------------------------------------------  
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DBLib.Common.Json
{
    public class JsonHelper
    {
        public static string SerializeObject(object value)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(value);
        }
        public static object DeserializeObject(string value)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject(value);
        }
        public static T DeserializeObject<T>(string value)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
        }
    }

    /// <summary>
    /// Newtonsoft.Json 属性小写类
    /// </summary>
    public class LowercaseJsonSerializer
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ContractResolver = new LowercaseContractResolver()
        };

        /// <summary>
        /// 属性全小写
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string SerializeObject(object o)
        {
            return JsonConvert.SerializeObject(o, Settings);
        }

        public class LowercaseContractResolver : DefaultContractResolver
        {
            protected override string ResolvePropertyName(string propertyName)
            {
                return propertyName.ToLower();
            }
        }

        /// <summary>
        /// 属性首字母小写
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string FirstLetterLowercase(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj, Formatting.Indented,
            new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
        }
    }
}
