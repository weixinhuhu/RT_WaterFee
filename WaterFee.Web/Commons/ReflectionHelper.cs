using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WHC
{
    public class ReflectionHelper
    {
        /// <summary>
        /// 批量替换对象指定数据类型的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t">对象</param>
        /// <param name="type">数据类型,如TypeOf(string)</param>
        /// <param name="oldValue">旧值</param>
        /// <param name="newValue">新值</param>
        /// <returns></returns>
        public static T ReplacePropertyValue<T>(T t, Type type, object oldValue, object newValue)
        {
            if (t == null) return default(T);
            PropertyInfo[] pros = t.GetType().GetProperties();
            foreach (PropertyInfo p in pros)
            {
                if (p.PropertyType != type) continue;
                var objValue = p.GetValue(t);
                if (object.Equals(oldValue, objValue)) p.SetValue(t, newValue);
            }
            return t;
        }
    }
}