using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Aspose.Cells;
using WHC.Framework.Commons;
using System.Text;

namespace TestAsposeWordForWeb
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool hasSqlInjection = SQLInjectionHelper.ValidUrlGetData() || SQLInjectionHelper.ValidUrlPostData();
            if (hasSqlInjection)
            {
                return;
            }
        }

        protected void btnGenWord_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> dictSource = new Dictionary<string, string>();
            dictSource.Add("TIS_HANDLE_NO", "T0001");
            dictSource.Add("ACCUSE_INDUSTRY", "出租车");
            dictSource.Add("ACCUSER_NAME", "张三");

            string templateFile = Server.MapPath("./Templates/Advice.doc");
            Aspose.Words.Document doc = new Aspose.Words.Document(templateFile);

            //使用文本方式替换
            foreach (string name in dictSource.Keys)
            {
                doc.Range.Replace(name, dictSource[name], true, true);
            }

            #region 使用书签替换模式

            Aspose.Words.Bookmark bookmark = doc.Range.Bookmarks["ACCUSER_SEX"];
            if (bookmark != null)
            {
                bookmark.Text = "男";
            }
            bookmark = doc.Range.Bookmarks["ACCUSER_TEL"];
            if (bookmark != null)
            {
                bookmark.Text = "1862029207*";
            }

            #endregion

            doc.Save(Response, "testAdvice.doc", Aspose.Words.ContentDisposition.Attachment,
                Aspose.Words.Saving.SaveOptions.CreateSaveOptions(Aspose.Words.SaveFormat.Doc));
        }

        protected void btnGenExcel_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> dictSource = new Dictionary<string, string>();
            dictSource.Add("TIS_HANDLE_NO", "T0001");
            dictSource.Add("ACCUSE_INDUSTRY", "出租车");
            dictSource.Add("ACCUSER_NAME", "张三");

            string templateFile = Server.MapPath("./Templates/Advice.xls");
            WorkbookDesigner designer = new WorkbookDesigner();
            designer.Workbook = new Workbook(templateFile);

            Aspose.Cells.Worksheet worksheet = designer.Workbook.Worksheets[0];
            //使用文本替换
            foreach (string name in dictSource.Keys)
            {
                worksheet.Replace(name, dictSource[name]);
            }

            //使用绑定数据方式替换
            designer.SetDataSource("ACCUSER_SEX", "男");
            designer.SetDataSource("ACCUSER_TEL", "1862029207*");
            designer.Process();

            string saveFileName = "testAdvice.xls";
            designer.Workbook.Save(Response.OutputStream, SaveFormat.Excel97To2003);

            Response.Charset = "GB2312";
            Response.ContentEncoding = Encoding.GetEncoding("GB2312");
            Response.ContentType = "application/ms-excel/msword";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(saveFileName));
            Response.Flush();
        }

        protected void btnGenWord2_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> dictSource = new Dictionary<string, string>();
            dictSource.Add("TIS_HANDLE_NO", "T0001");
            dictSource.Add("ACCUSE_INDUSTRY", "出租车");
            dictSource.Add("ACCUSER_NAME", "张三");

            string templateFile = "./Templates/Advice.doc";
            WHC.Framework.ControlUtil.AsposeWordTools.WebExportWithReplace(templateFile, "testAdvice.doc", dictSource);
        }

        protected void btnGenExcel2_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> dictSource = new Dictionary<string, string>();
            dictSource.Add("TIS_HANDLE_NO", "T0001");
            dictSource.Add("ACCUSE_INDUSTRY", "出租车");
            dictSource.Add("ACCUSER_NAME", "张三");

            string templateFile = "./Templates/Advice.xls";
            WHC.Framework.ControlUtil.AsposeExcelTools.WebExportWithReplace(templateFile, "testAdvice.xls", dictSource);
        }

        protected void btnConvertWordToHtml_Click(object sender, EventArgs e)
        {
            string templateFile = Server.MapPath("./Templates/Advice.doc");
            string newFile = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "wordExcel/test.html");
            Aspose.Words.Document doc = new Aspose.Words.Document(templateFile);
            doc.Save(newFile, Aspose.Words.SaveFormat.Html);

            templateFile = Server.MapPath("./Templates/支付方式.docx");
            newFile = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "wordExcel/payment.html");
            doc = new Aspose.Words.Document(templateFile);
            doc.Save(newFile, Aspose.Words.SaveFormat.Html);

        }

        protected void btnConvertExcelToHtml_Click(object sender, EventArgs e)
        {
            string templateFile = Server.MapPath("./Templates/Advice.xls");
            string newFile = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "wordExcel/excel.html");
            Workbook workbook = new Workbook(templateFile);
            workbook.Save(newFile, SaveFormat.Html);
           
            templateFile = Server.MapPath("./Templates/调研表.xlsx");
            newFile = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "wordExcel/excel2.html");
            workbook = new Workbook(templateFile);
            workbook.Save(newFile, SaveFormat.Html);

            templateFile = Server.MapPath("./Templates/职位.xls");
            newFile = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "wordExcel/excel3.html");
            workbook = new Workbook(templateFile);
            workbook.Save(newFile, SaveFormat.Html);     
        }
    }
}
