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
    /// double 扩展方法
    /// </summary>
    public static class DoubleExtension
    {
        public static bool IsDouble(this string value)
        {
            try
            {
                double.Parse(value);
                return true;
            }
            catch { return false; }
        }
        /// <summary>
        /// 转换成double类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDouble(this string value)
        {
            try
            {
                return Convert.ToDouble(value);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 转换成double类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static double ToDouble(this int value)
        {
            try
            {
                return Convert.ToDouble(value);
            }
            catch (Exception ex) { throw ex; }
        }

        public static double ToDouble(this long value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch (Exception ex) { throw ex; }
        }

        public static double? ToDoubleOrNull(this string value)
        {
            try
            {
                return double.Parse(value);
            }
            catch (Exception ex) { return null; }
        }
    }
}
