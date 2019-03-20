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
    /// http://msdn.microsoft.com/zh-cn/library/3ewxz6et.aspx
    /// </summary>
    public static class ExtensionMethods
    {
      
        /// <summary>
        /// 转成百分比
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToPercent(this int value)
        {
            return value.ToString() + "%";
        }

        /// <summary>
        /// 转成保留到两位小数的百分比
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToPercent(this double value)
        {
            return value.ToString("0.00") + "%";
        }

        /// <summary>
        /// 转成保留两位小数的百分比
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToPercent(this long value)
        {
            return value.ToString("0.00") + "%";
        }

        /// <summary>
        /// 不等于0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNotZero(this int value)
        {
            return value != 0;
        }

        /// <summary>
        /// 不等于0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNotZero(this double value)
        {
            return value != 0d;
        }

        /// <summary>
        /// 不等于0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNotZero(this long value)
        {
            return value != 0L;
        }

        /// <summary>
        /// 等于0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsZero(this int value)
        {
            return value == 0;
        }

        /// <summary>
        /// 等于0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsZero(this double value)
        {
            return value == 0d;
        }

        /// <summary>
        /// 等于0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsZero(this long value)
        {
            return value == 0L;
        }

        /// <summary>
        /// 大于于0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsBiggerZero(this int value)
        {
            return value > 0;
        }

        /// <summary>
        /// 大于于0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsBiggerZero(this int? value)
        {
            return value > 0;
        }

        /// <summary>
        /// 大于于0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsBiggerZero(this double value)
        {
            return value > 0d;
        }
    }
}
