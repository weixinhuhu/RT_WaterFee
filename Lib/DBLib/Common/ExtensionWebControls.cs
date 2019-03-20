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
using System.Web.UI.WebControls;

namespace System
{
    public static class ExtensionDropDownlist
    {
        /// <summary>
        /// 根据值设置DropDownList的选择项
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="value"></param>
        public static void SetSelectedByValue(this DropDownList ddl, string value)
        {
            //if (ddl.Items.Count > 0)
            //    ddl.Items.FindByValue(value).Selected = true;
            try
            {
                ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByValue(value));
            }
            catch
            {
                if (ddl.Items.Count > 0) ddl.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 根据值设置DropDownList的选择项
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="value"></param>
        public static void SetSelectedByValue(this DropDownList ddl, int? value)
        {
            //if (ddl.Items.Count > 0)
            //    ddl.Items.FindByValue(value).Selected = true;
            try
            {
                ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByValue(value.ToString()));
            }
            catch
            {
                if (ddl.Items.Count > 0) ddl.SelectedIndex = 0;
            }
        }
    }
}