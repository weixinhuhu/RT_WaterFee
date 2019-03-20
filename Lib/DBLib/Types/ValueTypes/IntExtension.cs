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
    public static class IntExtension
    {
        public static bool ToBool(this int value)
        {
            return Convert.ToBoolean(value);
        }
        public static bool ToBool(this int? value)
        {
            try
            {
                return Convert.ToBoolean(value);
            }
            catch
            {
                return false;
            }
        }

        public static bool IsInt(this string value)
        {
            try
            {
                int.Parse(value);
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// 转换成int类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(this string value)
        {
            try
            {
                return int.Parse(value.Trim());
            }
            catch (Exception ex) { throw ex; }
        }

        public static int ToInt(this int? value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch (Exception ex) { throw ex; }
        }


        public static int ToInt(this float value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch (Exception ex) { throw ex; }
        }

        public static int ToInt(this float? value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch (Exception ex) { throw ex; }
        }

        public static int ToInt(this double value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch (Exception ex) { throw ex; }
        }
        public static int ToInt(this double? value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch (Exception ex) { throw ex; }
        }

        public static int ToIntOrZero(this int? value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch { return 0; }
        }

        public static int? ToIntOrNull(this string value)
        {
            try
            {
                return int.Parse(value);
            }
            catch (Exception ex) { return null; }
        }

        /// <summary>
        /// 转换成int,如异常则返回 defaultValue
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToIntOrDefault(this string value, int defaultValue = 0)
        {
            try
            {
                return int.Parse(value);
            }
            catch { return defaultValue; }
        }

        /// <summary>
        /// 转换成int,如异常则返回0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToIntOrZero(this string value)
        {
            try
            {
                return int.Parse(value);
            }
            catch { return 0; }
        }
    }
}
