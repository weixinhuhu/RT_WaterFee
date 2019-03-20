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
using System.Web;

namespace DBLib.Web
{
    public class jqGrid
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page">当前页数</param>
        /// <param name="total">总页数</param>
        /// <param name="records">记录数</param>
        /// <param name="rows">Json结果集</param>
        /// <returns></returns>
        public static string Pager(int page, int pageSize, int records, string rows)
        {
            //var str="{\"page\":\"2\",\"total\":2,\"records\":\"13\",\"rows\":[]}
            var total = Math.Ceiling(records.ToDouble() / pageSize);
            return string.Format("{4}\"page\":\"{0}\",\"total\":{1},\"records\":\"{2}\",\"rows\":{3}{5}"
                , page, total, records, rows, "{", "}");
        }
    }
}