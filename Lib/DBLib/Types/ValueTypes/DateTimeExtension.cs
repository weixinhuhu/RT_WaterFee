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
    /// DateTime 扩展方法
    /// </summary>
    public static class DateTimeExtension
    {
        /// <summary>
        /// 是否是DateTime类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsDateTime(this string value)
        {
            try
            {
                if (value == null) return false;
                Convert.ToDateTime(value);
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// 转换成 System.DateTime
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this DateTime? value)
        {
            return Convert.ToDateTime(value);
        }
        /// <summary>
        /// 转换成yyyy-MM的string类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToYyyyMM(this DateTime value)
        {
            return value.ToString("yyyy-MM");
        }

        /// <summary>
        /// 转换成yyyy-MM-dd的string类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToYyyyMM(this DateTime? value)
        {
            if (value == null) return string.Empty;
            try
            {
                return Convert.ToDateTime(value).ToString("yyyy-MM");
            }
            catch { return string.Empty; }
        }

        /// <summary>
        /// 转换成yyyy-MM-dd的string类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToYyyyMMdd(this DateTime value)
        {
            return value.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 转换成yyyy-MM-dd的string类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToYyyyMMdd(this DateTime? value)
        {
            if (value == null) return string.Empty;
            try
            {
                return Convert.ToDateTime(value).ToString("yyyy-MM-dd");
            }
            catch { return string.Empty; }
        }

        /// <summary>
        /// 转换成yyyy-MM-dd的string类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToYyyyMMdd(this DateTime? value, string format)
        {
            if (value == null) return string.Empty;
            try
            {
                return Convert.ToDateTime(value).ToString(format);
            }
            catch { return string.Empty; }
        }


        /// <summary>
        /// 转换成 yyyy-MM-dd HH:mm 的string类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToYyyyMMddHHmm(this DateTime? value)
        {
            return value.ToYyyyMMdd("yyyy-MM-dd HH:mm");
        }
        /// <summary>
        /// 转换成yyyy-MM-dd的string类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToYyyyMMdd(this string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            try
            {
                return Convert.ToDateTime(value).ToString("yyyy-MM-dd");
            }
            catch { return string.Empty; }
        }

        /// <summary>
        /// 转换成yyyy/MM/dd的string类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToYyyyMMdd2(this DateTime value)
        {
            return value.ToString("yyyy-MM-dd").Replace("-", "/");
        }

        /// <summary>
        /// 转换成yyyy/MM/dd的string类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToYyyyMMdd2(this DateTime? value)
        {
            if (value == null) return string.Empty;
            return value.ToYyyyMMdd().Replace("-", "/");
        }

        /// <summary>
        /// 将字符串转成日期
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string value)
        {
            //if (string.IsNullOrEmpty(value)) return DateTime.MinValue;
            try
            {
                return Convert.ToDateTime(value);
            }
            catch (Exception ex) { throw ex; }
        }
        /// <summary>
        /// 将字符串转成日期
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime? ToDateTimeOrNull(this string value)
        {
            try
            {
                return Convert.ToDateTime(value.Trim());
            }
            catch { return null; }
        }

        /// <summary>  
        /// 得到本周第一天(以星期天为第一天)  
        /// </summary>  
        /// <param name="datetime"></param>  
        /// <returns></returns>  
        public static DateTime GetWeekFirstDaySun(this DateTime datetime)
        {
            //星期天为第一天  
            int weeknow = Convert.ToInt32(datetime.DayOfWeek);
            int daydiff = (-1) * weeknow;

            //本周第一天  
            DateTime FirstDay = datetime.AddDays(daydiff);
            return FirstDay;
        }

        /// <summary>  
        /// 得到本周第一天(以星期一为第一天)  
        /// </summary>  
        /// <param name="datetime"></param>  
        /// <returns></returns>  
        public static DateTime GetWeekFirstDayMon(this DateTime datetime)
        {
            //星期一为第一天  
            int weeknow = Convert.ToInt32(datetime.DayOfWeek);

            //因为是以星期一为第一天，所以要判断weeknow等于0时，要向前推6天。  
            weeknow = (weeknow == 0 ? (7 - 1) : (weeknow - 1));
            int daydiff = (-1) * weeknow;

            //本周第一天  
            DateTime FirstDay = datetime.AddDays(daydiff);
            return FirstDay;
        }

        /// <summary>  
        /// 得到本周最后一天(以星期六为最后一天)  
        /// </summary>  
        /// <param name="datetime"></param>  
        /// <returns></returns>  
        public static DateTime GetWeekLastDaySat(this DateTime datetime)
        {
            //星期六为最后一天  
            int weeknow = Convert.ToInt32(datetime.DayOfWeek);
            int daydiff = (7 - weeknow) - 1;

            //本周最后一天  
            DateTime LastDay = datetime.AddDays(daydiff);
            return LastDay;
        }

        /// <summary>  
        /// 获得当前时间的星期几
        /// <para>输出格式:SAT,SUN</para>
        /// </summary>  
        /// <param name="datetime"></param>  
        /// <returns></returns>  
        public static string GetDayOfWeek0(this DateTime datetime)
        {
            return datetime.DayOfWeek.ToString().Substring(0,3).ToUpper();
        }

        /// <summary>  
        /// 获得当前时间的星期几
        /// <para>输出格式:一,二</para>
        /// </summary>  
        /// <param name="datetime"></param>  
        /// <returns></returns>  
        public static string GetDayOfWeek1(this DateTime datetime)
        {
            string day = null;
            switch (datetime.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    day = "日";
                    break;
                case DayOfWeek.Monday:
                    day = "一";
                    break;
                case DayOfWeek.Tuesday:
                    day = "二";
                    break;
                case DayOfWeek.Wednesday:
                    day = "三";
                    break;
                case DayOfWeek.Thursday:
                    day = "四";
                    break;
                case DayOfWeek.Friday:
                    day = "五";
                    break;
                case DayOfWeek.Saturday:
                    day = "六";
                    break;
                default: break;
            }
            return day;
        }

        /// <summary>
        /// 获得当前时间的星期几
        /// <para>输出格式:一,二 </para>
        /// </summary>
        /// <param name="strDatetime"></param>
        /// <returns></returns>
        public static string GetDayOfWeek1(this string strDatetime)
        {
            return GetDayOfWeek1(strDatetime.ToDateTime());
        }

        /// <summary>  
        /// 获得当前时间的星期几
        /// <para>输出格式:周一,周二</para>
        /// </summary>  
        /// <param name="datetime"></param>  
        /// <returns></returns>  
        public static string GetDayOfWeek2(this DateTime datetime)
        {
            string day = null;
            switch (datetime.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    day = "周日";
                    break;
                case DayOfWeek.Monday:
                    day = "周一";
                    break;
                case DayOfWeek.Tuesday:
                    day = "周二";
                    break;
                case DayOfWeek.Wednesday:
                    day = "周三";
                    break;
                case DayOfWeek.Thursday:
                    day = "周四";
                    break;
                case DayOfWeek.Friday:
                    day = "周五";
                    break;
                case DayOfWeek.Saturday:
                    day = "周六";
                    break;
                default: break;
            }
            return day;
        }

        /// <summary>  
        /// 获得当前时间的星期几
        /// <para>输出格式:周一,周二</para>
        /// </summary>  
        /// <param name="datetime"></param>  
        /// <returns></returns>  
        public static string GetDayOfWeek2(this string datetime)
        {
            return GetDayOfWeek2(datetime.ToDateTime());
        }

        /// <summary>  
        /// 获得当前时间的星期几
        ///<para>输出格式:星期一,星期二,</para> 
        /// </summary>  
        /// <param name="datetime"></param>  
        /// <returns></returns>  
        public static string GetDayOfWeek3(this DateTime datetime)
        {
            string day = null;
            switch (datetime.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    day = "星期日";
                    break;
                case DayOfWeek.Monday:
                    day = "星期一";
                    break;
                case DayOfWeek.Tuesday:
                    day = "星期二";
                    break;
                case DayOfWeek.Wednesday:
                    day = "星期三";
                    break;
                case DayOfWeek.Thursday:
                    day = "星期四";
                    break;
                case DayOfWeek.Friday:
                    day = "星期五";
                    break;
                case DayOfWeek.Saturday:
                    day = "星期六";
                    break;
                default: break;
            }
            return day;
        }

        /// <summary>  
        /// 获得当前时间的星期几
        /// <para>输出格式:星期一,星期二</para>
        /// </summary>  
        /// <param name="datetime"></param>  
        /// <returns></returns>  
        public static string GetDayOfWeek3(this string datetime)
        {
            return GetDayOfWeek3(datetime.ToDateTime());
        }

        /// <summary>  
        /// 得到本周最后一天(以星期天为最后一天)  
        /// </summary>  
        /// <param name="datetime"></param>  
        /// <returns></returns>  
        public static DateTime GetWeekLastDaySun(this DateTime datetime)
        {
            //星期天为最后一天  
            int weeknow = Convert.ToInt32(datetime.DayOfWeek);
            weeknow = (weeknow == 0 ? 7 : weeknow);
            int daydiff = (7 - weeknow);

            //本周最后一天  
            DateTime LastDay = datetime.AddDays(daydiff);
            return LastDay;
        }

        /// <summary>  
        /// 得到月份第一天
        /// </summary>  
        /// <param name="datetime"></param>  
        /// <returns></returns>  
        public static DateTime GetMonthFirstDay(this DateTime datetime)
        {
            return new DateTime(datetime.Year, datetime.Month, 1);
        }

        /// <summary>  
        /// 得到月份最后一天
        /// </summary>  
        /// <param name="datetime"></param>  
        /// <returns></returns>  
        public static DateTime GetMonthLastDay(this DateTime datetime)
        {
            return GetMonthFirstDay(datetime).AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// 根据formatter比较两个时间是否相等
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <param name="formatter"></param>
        /// <returns></returns>
        public static bool IsEqualsByFormatter(this DateTime? dt1, DateTime? dt2, string formatter)
        {
            return Convert.ToDateTime(dt1).ToString(formatter) == Convert.ToDateTime(dt2).ToString(formatter);
        }

        /// <summary>
        /// 根据formatter比较两个时间是否相等
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <param name="formatter"></param>
        /// <returns></returns>
        public static bool IsEqualsByFormatter(this DateTime dt1, DateTime? dt2, string formatter)
        {
            return dt1.ToString(formatter) == Convert.ToDateTime(dt2).ToString(formatter);
        }

        /// <summary>
        /// 转换成时间戳,关于Unix时间戳,大概是这个意思,从1970年0时0分0秒开始到现在的秒数.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>时间戳</returns>
        public static int ToTimeStamp(this DateTime dt)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(dt - startTime).TotalSeconds;
        }

        /// <summary>
        /// 转换成时间戳,关于Unix时间戳,大概是这个意思,从1970年0时0分0秒开始到现在的秒数.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>时间戳</returns>
        public static int ToTimeSpan(this DateTime? dt)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(dt.ToDateTime() - startTime).TotalSeconds;
        }

        /// <summary>  
        /// 时间戳转为标准格式时间  
        /// </summary>  
        /// <param name="timeStamp">Unix时间戳格式</param>  
        /// <returns>标准格式时间</returns>  
        public static DateTime ToDatetime(this int timeSpan)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            //long lTime = long.Parse(timeStamp + "0000000");
            //TimeSpan toNow = new TimeSpan(lTime);
            //return dtStart.Add(toNow); 
            return dtStart.AddSeconds(Convert.ToDouble(timeSpan));
        }
    }
}
