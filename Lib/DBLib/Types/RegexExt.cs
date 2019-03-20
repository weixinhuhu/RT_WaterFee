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
using System.Text.RegularExpressions;

namespace System
{
    public static class RegexExt
    {
        //匹配中文字符
        const string strChineseRegex = "[\u4e00-\u9fa5]";
        //匹配双字节字符(包括汉字在内)
        const string strDoubleByte = "[^\x00-\xff]";
        //匹配空白行
        const string strEmpty = @"\n\s*\r";
        //匹配Email地址
        const string strEmail = @"[\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?";
        const string strPhone = @"^1[3|4|5|7|8]\d{9}$";

        public static bool IsMatch(this string value,string strRegex)
        {
            if (string.IsNullOrEmpty(value)) return false;
            Regex reg = new Regex(strRegex);
            return reg.IsMatch(value);
        }

        public static string GetRegexMatchValue(this string value, string strRegex)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            Regex reg = new Regex(strRegex);
            if (reg.IsMatch(value))
                return reg.Match(value).Value;
            else
                return string.Empty;
        }

        /// <summary>
        /// 获取正则匹配的 数字部分
        /// </summary>
        /// <param name="value"></param>
        /// <param name="strRegex"></param>
        /// <returns></returns>
        public static string GetRegexMatchInt(this string value, string strRegex = @"\d+")
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            Regex reg = new Regex(strRegex);
            if (reg.IsMatch(value))
                return reg.Match(value).Value;
            else
                return string.Empty;
        }

        /// <summary>
        /// 获取正则匹配的 浮点数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="strRegex"></param>
        /// <returns></returns>
        public static decimal GetRegexMatchDecimalOrDefault(this string value, string strRegex = @"(-?\d+)(\.\d+)?")
        {
            try
            {
                Regex reg = new Regex(strRegex);
                if (reg.IsMatch(value))
                    return Convert.ToDecimal(reg.Match(value).Value);
                else
                    return 0;
            }
            catch
            {
                return 0;
            }
        }

        public static string[] Split(string input, string pattern)
        {
            return System.Text.RegularExpressions.Regex.Split(input, pattern);
        }

        public static string[] Split(string input, string pattern, RegexOptions options)
        {
            return System.Text.RegularExpressions.Regex.Split(input, pattern, options);
        }

    }
}
