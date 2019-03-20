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
    /// int 扩展方法
    /// </summary>
    public static class ExtensionInt64
    {
        public static bool IsInt64(this string value)
        {
            try
            {
                Int64.Parse(value);
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// 转换成int类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int64 ToInt64(this string value)
        {
            try
            {
                return Int64.Parse(value.Trim());
            }
            catch (Exception ex) { throw ex; }
        }

        public static Int64 ToInt64(this double value)
        {
            try
            {
                return Convert.ToInt64(value);
            }
            catch (Exception ex) { throw ex; }
        }

        public static Int64 ToInt64(this int? value)
        {
            try
            {
                return Convert.ToInt64(value);
            }
            catch (Exception ex) { throw ex; }
        }

        public static Int64? ToInt64OrNull(this string value)
        {
            try
            {
                return Int64.Parse(value.Trim());
            }
            catch (Exception ex) { return null; }
        }

        /// <summary>
        /// 转换成int,如异常则返回 -1
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Int64 ToInt64OrDefault(this string value)
        {
            try
            {
                return Int64.Parse(value.Trim());
            }
            catch { return -1; }
        }
    }
}