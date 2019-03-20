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

namespace System
{
    /// <summary>
    /// String扩展方法
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// 换掉以separator分组最后的那部分
        /// </summary>
        /// <param name="value"></param>
        /// <param name="separator"></param>
        /// <param name="newPart"></param>
        /// <returns></returns>
        public static string ChangeLastPart(this string value, string separator, string newPart)
        {
            if (value.IsNullOrEmpty()) return separator + newPart;
            var arr = value.Split(new string[] { separator }, StringSplitOptions.None);
            List<string> list = new List<string>();
            for (int i = 0; i < arr.Length; i++)
            {
                var item = arr[i];
                if (i + 1 == arr.Length) item = newPart;
                list.Add(item);
            }
            return string.Join(separator, list);
        }

        /// <summary>
        /// 换掉以url以符号 / 分开的最后的那部分
        /// </summary>
        /// <param name="value"></param>
        /// <param name="separator"></param>
        /// <param name="newPart"></param>
        /// <returns></returns>
        public static string ChangeUrlLastPart(this string value, string newPart)
        {
            return value.ChangeLastPart("/", newPart);
        }

        /// <summary>
        /// 指示指定的 System.String 对象是 null 还是 System.String.Empty 字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string value)
        {
            if (value == null) return true;
            return string.IsNullOrEmpty(value.Trim());
        }

        /// <summary>
        /// 指示指定的 String 对象不为 null 或不为 String.Empty 字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNotNullOrEmpty(this string value)
        {
            if (value == null) return false;
            return !string.IsNullOrEmpty(value.Trim());
        }


        /// <summary>
        /// 转成百分比显示
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToPercent(this string value)
        {
            try
            {
                if (string.IsNullOrEmpty(value)) return "";
                if (value.Contains("%"))
                {
                    return value;
                }
                return (value.Trim().ToFloat() * 100f).ToString() + "%";
            }
            catch { return value; }
        }

        /// <summary>
        /// 转成百分比显示 pointLength为小数点后面保留位数
        /// </summary>
        /// <param name="value"></param> 
        /// <param name="pointLength">小数点后面保留位数</param>
        /// <returns></returns>
        public static string ToPercent(this string value, int pointLength)
        {
            try
            {
                if (string.IsNullOrEmpty(value)) return "";
                if (value.Contains("%"))
                {
                    var temp = value.Replace("%", "").Trim();
                    return temp.ToFloat().ToString("f" + pointLength) + "%";
                }
                return (value.Trim().ToFloat() * 100f).ToString("f" + pointLength) + "%";
            }
            catch { return value; }
        }

        /// <summary>
        /// 去掉该字符中包含输入的字符所有字符
        /// </summary>
        /// <param name="value"></param>
        /// <param name="removeString"></param>
        /// <returns></returns>
        public static string Remove(this string value, string removeString)
        {
            if (value == null) return null;
            return value.Replace(removeString, "");
        }

        public static string ToSqlOrString(this string fields, string key)
        {
            if (string.IsNullOrEmpty(fields)) return fields;
            var arrField = fields.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var sb = new System.Text.StringBuilder();
            sb.Append("(");
            for (var i = 0; i < arrField.Length; i++)
            {
                if (i == 0)
                    sb.AppendFormat(" {0} like '%{1}%'", arrField[i], key);
                else
                    sb.AppendFormat(" or {0} like '%{1}%'", arrField[i], key);
            }
            sb.Append(")");
            return sb.ToString();
        }

        public static string ToSqlAndString(this string fields, string key)
        {
            if (string.IsNullOrEmpty(fields)) return fields;
            var arrField = fields.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var sb = new System.Text.StringBuilder();
            sb.Append("(");
            for (var i = 0; i < arrField.Length; i++)
            {
                if (i == 0)
                    sb.AppendFormat(" {0} like '%{1}%'", arrField[i], key);
                else
                    sb.AppendFormat(" and {0} like '%{1}%'", arrField[i], key);
            }
            sb.Append(")");
            return sb.ToString();
        }

        /// <summary>
        ///  将指定字符串中的格式项替换为指定数组中相应对象的字符串表示形式。指定的参数提供区域性特定的格式设置信息。
        /// </summary>
        /// <param name="formats">复合格式字符串。</param>
        /// <param name="args">一个对象数组，其中包含零个或多个要设置格式的对象。</param>
        /// <returns></returns>
        public static string FormatWith(this string formats, params object[] args)
        {
            if (formats == null || args == null)
            {
                throw new ArgumentNullException((formats == null) ? "formats" : "args");
            }
            else
            {
                return new StringBuilder(formats.Length + args.Length * 8).AppendFormat(formats, args).ToString();
            }
        }

        //public static string[] Split(this string value, string pattern)
        //{
        //    return value.Split(new string[] { pattern }, StringSplitOptions.None);
        //}

        public static string[] Split(this string value, string separator, StringSplitOptions options = StringSplitOptions.None)
        {
            return value.Split(new string[] { separator }, options);
        }
    }

}
