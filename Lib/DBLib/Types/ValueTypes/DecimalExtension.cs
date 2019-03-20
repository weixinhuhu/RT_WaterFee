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
    /// decimal 扩展方法
    /// </summary>
    public static class DecimalExtension
    {
        /// <summary>
        /// 是否是Decimal类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDecimal(this string value)
        {
            try
            {
                decimal.Parse(value);
                return true;
            }
            catch { return false; }
        }
        /// <summary>
        /// 转换成decimal类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string value)
        {
            try
            {
                return Convert.ToDecimal(value);
            }
            catch (Exception ex)
            {
                if (value == "5E-05")
                    return 0.00005m;
                throw ex;
            }
        }

        /// <summary>
        /// 转换成decimal类型,默认为0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ToDecimalOrDefault(this string value)
        {
            try
            {
                return Convert.ToDecimal(value.ToDecimalOrNull());
            }
            catch { return 0; }
        }

        /// <summary>
        /// 转换成decimal类型,如转换出错则默认为0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ToDecimalOrZero(this string value)
        {
            try
            {
                return Convert.ToDecimal(value.ToDecimalOrNull());
            }
            catch { return 0; }
        }

        /// <summary>
        /// 转换成decimal类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this int value)
        {
            try
            {
                return Convert.ToDecimal(value);
            }
            catch (Exception ex) { throw ex; }
        }

        public static decimal ToDecimal(this long value)
        {
            try
            {
                return Convert.ToDecimal(value);
            }
            catch (Exception ex) { throw ex; }
        }

        public static decimal ToDecimal(this decimal? value)
        {
            try
            {
                return Convert.ToDecimal(value);
            }
            catch (Exception ex) { throw ex; }
        }

        public static decimal ToDecimalOrZero(this decimal? value)
        {
            try
            {
                return Convert.ToDecimal(value);
            }
            catch (Exception ex) { return 0; }
        }

        /// <summary>
        /// 除以
        /// </summary>
        /// <param name="value">被除数</param>
        /// <param name="num">除数</param>
        /// <returns></returns>
        public static decimal DividedBy(this decimal value, int num)
        {
            try
            {
                return value / num;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 除以
        /// </summary>
        /// <param name="value">被除数</param>
        /// <param name="num">除数</param>
        /// <returns></returns>
        public static decimal DividedBy(this decimal? value, int num)
        {
            try
            {
                return Convert.ToDecimal(value) / num;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        ///除以(如果异常,则返回0)
        /// </summary>
        /// <param name="value">被除数</param>
        /// <param name="num">除数</param>
        /// <returns></returns>
        public static decimal DividedByOrDefault(this decimal? value, int num)
        {
            try
            {
                return Convert.ToDecimal(value) / num;
            }
            catch { return 0; }
        }

        /// <summary>
        /// 除以(如果异常,则返回0)
        /// </summary>
        /// <param name="value">被除数</param>
        /// <param name="num">除数</param>
        /// <returns></returns>
        public static decimal DividedByOrDefault(this decimal? value, decimal? num)
        {
            try
            {
                return Convert.ToDecimal(value) / num.ToDecimal();
            }
            catch { return 0; }
        }
        /// <summary>
        /// 除以(如果异常,则返回0)
        /// </summary>
        /// <param name="value">被除数</param>
        /// <param name="num">除数</param>
        /// <returns></returns>
        public static decimal DividedByOrDefault(this decimal value, decimal? num)
        {
            try
            {
                return value / num.ToDecimal();
            }
            catch { return 0; }
        }

        public static decimal? ToDecimalOrNull(this string value)
        {
            try
            {
                decimal d = 1;
                if (value.IndexOf("%") > -1) { d = 100; }
                value = value.Replace("%", "").Replace(" ", "");
                return decimal.Parse(value) / d;
            }
            catch { return null; }
        }


        /// <summary>
        /// 将含有百分的string转成Decimal,如果没有百分号时默认除以100
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal? ToPercentDecimalOrNullNotBigOne(this string value)
        {
            try
            {
                decimal d = 1;
                bool isNotContainsPercent = false;
                if (value.IndexOf("%") > -1) { d = 100; }
                else
                {
                    isNotContainsPercent = true;
                }
                value = value.Replace("%", "").Replace(" ", "");
                var vd = decimal.Parse(value);
                if (isNotContainsPercent)
                {
                    return vd / 100;
                }
                else
                    return vd / d;
            }
            catch { return null; }
        }

        public static string ToPercent(this decimal value)
        {
            return (value * 100).ToString() + "%";
        }

        public static string ToPercent(this decimal? value)
        {
            if (value == null) return "";
            return (value * 100).ToDecimal().ToString() + "%";
        }

        /// <summary>
        /// 转成百分比,pointLength为小数点后面保留位数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pointLength">小数点后面保留位数</param>
        /// <returns></returns>
        public static string ToPercent(this decimal value, int pointLength)
        {
            return (value * 100).ToString("f" + pointLength) + "%";
        }

        /// <summary>
        /// 输入参数为 小数点后面保留位数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pointLength">小数点后面保留位数</param>
        /// <returns></returns>
        public static string ToPercent(this decimal? value, int pointLength)
        {
            if (value == null) return "";
            return (value * 100).ToDecimal().ToString("f" + pointLength) + "%";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pointLength">小数点后面保留位数</param>
        /// <returns></returns>
        public static string ToPoint(this decimal? value, int pointLength)
        {
            try
            {
                if (value == null) return "";
                return value.ToDecimal().ToString("f" + pointLength);
            }
            catch { return string.Empty; }
        }

        /// <summary>
        /// 转成小数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pointLength">小数点后面保留位数</param>
        /// <returns></returns>
        public static string ToPoint(this decimal value, int pointLength)
        {
            return value.ToString("f" + pointLength);
        }
    }
}
