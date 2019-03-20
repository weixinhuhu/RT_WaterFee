using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WHC.Framework.Commons;

namespace TestAsposeWordForWeb
{
    public partial class GenerateHtml : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGenerateString_Click(object sender, EventArgs e)
        {
            string tempalte = "Templates/template.htm";//相对目录

            TestInfo info = new TestInfo();
            info.Title = "测试标题";
            info.Content = "测试内容，这是测试内容";
            info.Datetime = DateTime.Now;

            NVelocityHelper adapter = new NVelocityHelper(tempalte);
            adapter.AddKeyValue("title", "This is a title")
                .AddKeyValue("content", "This is a Content")
                .AddKeyValue("datetime", System.DateTime.Now)
                .AddKeyValue("TestInfo", info);

            this.txtCode.InnerHtml = adapter.ExecuteString();
        }

        protected void btnGenerateFile_Click(object sender, EventArgs e)
        {
            string tempalte = "Templates/template.htm";//相对目录

            TestInfo info = new TestInfo();
            info.Title = "测试标题";
            info.Content = "测试内容，这是测试内容";
            info.Datetime = DateTime.Now;

            NVelocityHelper adapter = new NVelocityHelper(tempalte);
            adapter.AddKeyValue("title", "This is a title")
                .AddKeyValue("content", "This is a Content")
                .AddKeyValue("datetime", System.DateTime.Now)
                .AddKeyValue("TestInfo", info);

            adapter.FileNameOfOutput = "testTemplate";
            string filePath = adapter.ExecuteFile();
            if (!string.IsNullOrEmpty(filePath))
            {
                this.txtCode.InnerHtml = FileUtil.FileToString(filePath, System.Text.Encoding.UTF8);
            }
        }
    }
}