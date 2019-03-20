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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DBLib.Office
{
    public partial class FormDemo : Form
    {
        public FormDemo()
        {
            InitializeComponent();
        }

        private void btnExcelToHtml_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            if (op.ShowDialog() == DialogResult.OK)
            {
                var excelfile = op.FileName;
                if (excelfile.IsNullOrEmpty())
                {
                    MessageBox.Show("please a file");
                    return;
                }

                var htmlfile = System.IO.Path.ChangeExtension(excelfile, "html");

                DBLib.Office.ExcelHelper. ExcelToHtml(excelfile);

                MessageBox.Show("ok");
            }
        }
    }
}
