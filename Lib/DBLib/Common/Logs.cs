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

namespace DBLib.Common
{
    public class Logs
    {
        public Logs()
        {
            logPath = AppDomain.CurrentDomain.BaseDirectory + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            datetime = DateTime.Now;
        }
        private string logPath;
        public string LogPath
        {
            get
            {
                return logPath;
            }
            set
            {
                logPath = value;
            }

        }
        private DateTime datetime;
        public DateTime DateTime
        {
            get
            {
                return datetime;
            }
            set
            {
                datetime = value;
            }
        }
        private string msg;
        public string Msg
        {
            get { return msg; }
            set { msg = value; }
        }

        public static bool Write(Logs log)
        {
            bool flg = true;
            try
            {
                using (var sw = new System.IO.StreamWriter(log.LogPath, true, UTF8Encoding.UTF8))
                {
                    sw.Write(string.Format("{0} => {1}", log.DateTime, log.Msg));
                }
            }
            catch { flg = false; }
            return flg;
        }

        public static bool Write(Logs log, string dateTimeFormat)
        {
            bool flg = true;
            try
            {
                using (var sw = new System.IO.StreamWriter(log.LogPath, true, UTF8Encoding.UTF8))
                {
                    sw.Write(string.Format("{0} => {1}", log.DateTime.ToString(dateTimeFormat), log.Msg));
                }
            }
            catch { flg = false; }
            return flg;
        }

        public static bool WriteLine(Logs log)
        {
            bool flg = true;
            try
            {
                using (var sw = new System.IO.StreamWriter(log.LogPath, true, UTF8Encoding.UTF8))
                {
                    sw.WriteLine(string.Format("{0} => {1}", log.DateTime, log.Msg));
                }
            }
            catch { flg = false; }
            return flg;
        }

        public static bool WriteLine(Logs log, string dateTimeFormat)
        {
            bool flg = true;
            try
            {
                using (var sw = new System.IO.StreamWriter(log.LogPath, true, UTF8Encoding.UTF8))
                {
                    sw.WriteLine(string.Format("{0} => {1}", log.DateTime.ToString(dateTimeFormat), log.Msg));
                }
            }
            catch { flg = false; }
            return flg;
        }
    }
}
