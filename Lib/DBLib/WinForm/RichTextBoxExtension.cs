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

namespace System.Windows.Forms
{
    public static class RichTextBoxExtension
    {
        /// <summary>
        /// 滚动到最后
        /// </summary>
        /// <param name="rtb"></param>
        public static void ScrollToLast(this RichTextBox rtb)
        {
            //========richtextbox滚动条自动移至最后一条记录
            //让文本框获取焦点 
            rtb.Focus();
            //设置光标的位置到文本尾 
            rtb.Select(rtb.TextLength, 0);
            //滚动到控件光标处 
            rtb.ScrollToCaret();
        }

        /// <summary>
        /// 滚动到最前
        /// </summary>
        /// <param name="rtb"></param>
        public static void ScrollToFirst(this RichTextBox rtb)
        {
            //========richtextbox滚动条自动移至最后一条记录
            //让文本框获取焦点 
            rtb.Focus();
            //设置光标的位置到文本尾 
            rtb.Select(0,0);
            //滚动到控件光标处 
            rtb.ScrollToCaret();
        }
    }
}
