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
    /// float扩展方法
    /// </summary>
    public static class FloatExtension
    {
        public static bool IsFloat(this string value)
        {
            try
            {
                float.Parse(value);
                return true;
            }
            catch { return false; }
        }
        /// <summary>
        /// 转换成float类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float ToFloat(this string value)
        {
            try
            {
                return Convert.ToSingle(value);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 转换成float类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float ToFloat(this int value)
        {
            try
            {
                return Convert.ToSingle(value);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 转换成float类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float ToFloat(this int? value)
        {
            try
            {
                return Convert.ToSingle(value);
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 转换成float类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static float ToFloat(this float? value)
        {
            try
            {
                return Convert.ToSingle(value);
            }
            catch (Exception ex) { throw ex; }
        }

        public static double ToFloat(this long value)
        {
            try
            {
                return Convert.ToSingle(value);
            }
            catch (Exception ex) { throw ex; }
        }
        public static double ToFloat(this long? value)
        {
            try
            {
                return Convert.ToSingle(value);
            }
            catch (Exception ex) { throw ex; }
        }

        public static float? ToFloatOrNull(this string value)
        {
            try
            {
                return float.Parse(value);
            }
            catch (Exception ex) { return null; }
        }

        public static string ToPercent(this float value)
        {
            return (value * 100).ToString() + "%";
        }

        public static string ToPercent(this float? value)
        {
            if (value == null) return "";
            return (value * 100).ToFloat().ToString() + "%";
        }

        /// <summary>
        /// 转成百分比,pointLength为小数点后面保留位数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pointLength">小数点后面保留位数</param>
        /// <returns></returns>
        public static string ToPercent(this float value, int pointLength)
        {
            return (value * 100).ToString("f" + pointLength) + "%";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pointLength">小数点后面保留位数</param>
        /// <returns></returns>
        public static string ToPercent(this float? value, int pointLength)
        {
            if (value == null) return "";
            return (value * 100).ToFloat().ToString("f" + pointLength) + "%";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pointLength">小数点后面保留位数</param>
        /// <returns></returns>
        public static string ToPoint(this float? value, int pointLength)
        {
            try
            {
                if (value == null) return "";
                return value.ToFloat().ToString("f" + pointLength);
            }
            catch { return string.Empty; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pointLength">小数点后面保留位数</param>
        /// <returns></returns>
        public static string ToPoint(this float value, int pointLength)
        {
            return value.ToString("f" + pointLength);
        }
    }
}
