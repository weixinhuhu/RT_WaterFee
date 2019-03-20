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
namespace DBLib.Office
{
    partial class FormDemo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnExcelToHtml = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnExcelToHtml
            // 
            this.btnExcelToHtml.Location = new System.Drawing.Point(43, 21);
            this.btnExcelToHtml.Name = "btnExcelToHtml";
            this.btnExcelToHtml.Size = new System.Drawing.Size(88, 23);
            this.btnExcelToHtml.TabIndex = 0;
            this.btnExcelToHtml.Text = "ExcelToHtml";
            this.btnExcelToHtml.UseVisualStyleBackColor = true;
            this.btnExcelToHtml.Click += new System.EventHandler(this.btnExcelToHtml_Click);
            // 
            // FormDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 147);
            this.Controls.Add(this.btnExcelToHtml);
            this.Name = "FormDemo";
            this.Text = "FormDemo";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnExcelToHtml;
    }
}