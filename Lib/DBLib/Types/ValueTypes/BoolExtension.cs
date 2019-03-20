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
    /// bool 扩展方法
    /// </summary>
    public static class BoolExtension
    { 
         
        /// <summary>
        /// 转换成int类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(this bool value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch (Exception ex) { throw ex; }
        }
 

        /// <summary>
        /// 转换成int,如异常则返回 0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToIntOrZero(this bool value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch { return 0; }
        } 
        
        /// <summary>
        /// 转换成int,如异常则返回 0
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToIntOrZero(this bool? value)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch { return 0; }
        }
    }
}
