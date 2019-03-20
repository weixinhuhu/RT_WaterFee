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
    /// 各种编码转换
    /// </summary>
    public static class ExtensionEncoding
    {
        /// <summary>
        /// 转unicode编码,不带\,如u0057
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToUnicodeString(this string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            var sb = new System.Text.StringBuilder();
            foreach (char c in str.ToArray())
            {
                var temp = "0000" + string.Format("{0:x}", (int)c);
                sb.Append("u" + temp.Substring(temp.Length - 4, 4));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 转标准unicode编码,如\u0057
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToUnicodeString2(this string str)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            var sb = new System.Text.StringBuilder();
            foreach (char c in str.ToArray())
            {
                var temp = "0000" + string.Format("{0:x}", (int)c);
                sb.Append("\\u" + temp.Substring(temp.Length - 4, 4));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 解码unicode(不带\的,如u0057u0058)
        /// </summary>
        /// <param name="strUnicode">unicode编码字符串</param>
        /// <returns></returns>
        public static string ToStringFromUnicode(this string strUnicode)
        {
            if (string.IsNullOrEmpty(strUnicode)) return string.Empty;
            var sb = new System.Text.StringBuilder();
            var arr = strUnicode.Split('u');
            foreach (string s in arr)
            {
                if (string.IsNullOrEmpty(s)) continue;
                sb.Append((char)Convert.ToInt32(s, 16));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 解码标准的unicode,如\u0057\u0058
        /// </summary>
        /// <param name="strUnicode">unicode编码字符串</param>
        /// <returns></returns>
        public static string ToStringFromUnicode2(this string strUnicode)
        {
            if (string.IsNullOrEmpty(strUnicode)) return string.Empty;
            var sb = new System.Text.StringBuilder();
            var arr = strUnicode.Replace("\\", "").Split('u');
            foreach (string s in arr)
            {
                if (string.IsNullOrEmpty(s)) continue;
                sb.Append((char)Convert.ToInt32(s, 16));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 解码unicode
        /// </summary>
        /// <param name="strUnicode">标准的unicode编码字符串</param>
        /// <returns></returns>
        public static string ToStringFromUnicodeByRegex(this string strUnicode)
        {
            if (string.IsNullOrEmpty(strUnicode)) return string.Empty;
            return System.Text.RegularExpressions.Regex.Unescape(strUnicode);
        }

        public static string ToBase64(string str)
        {
            return ToBase64("UTF-8", str);
        }

        /// <summary>
        /// 转成base64字符
        /// </summary>
        /// <param name="code_type">编码类型,如UTF-8</param>
        /// <param name="encodeString">待转的字符</param>
        /// <returns></returns>
        public static string ToBase64(string code_type, string encodeString)
        {
            string base64 = "";
            byte[] bytes = Encoding.GetEncoding(code_type).GetBytes(encodeString);
            try
            {
                base64 = Convert.ToBase64String(bytes);
            }
            catch
            {
                base64 = encodeString;
            }
            return base64;
        }

        /// <summary>
        /// 将base64字符转换成普通字符
        /// </summary>
        /// <param name="code_type">编码类型</param>
        /// <param name="base64String">base64字符</param>
        /// <returns></returns>
        public static string ToStringFromBase64(string code_type, string base64String)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(base64String);
            try
            {
                decode = Encoding.GetEncoding(code_type).GetString(bytes);
            }
            catch
            {
                decode = base64String;
            }
            return decode;
        }
    }

}