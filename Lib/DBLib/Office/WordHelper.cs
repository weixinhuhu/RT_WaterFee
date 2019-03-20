using Microsoft.Office.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBLib.Office
{
    public class WordHelper
    {

        /// <summary>
        /// Word转成Html
        /// </summary>
        /// <param name="filePath">word文档路径</param>
        /// <param name="htmlPath">保存的html路径</param>
        /// <returns>是否成功</returns>
        public static bool WordToHtml(string filePath, string htmlPath = null)
        {
            var flg = false;
            var word = new Microsoft.Office.Interop.Word.ApplicationClass();
            Type wordType = word.GetType();
            Microsoft.Office.Interop.Word.Documents docs = word.Documents;
            Type docsType = docs.GetType();
            Microsoft.Office.Interop.Word.Document doc = (Microsoft.Office.Interop.Word.Document)docsType.InvokeMember("Open", System.Reflection.BindingFlags.InvokeMethod, null, docs, new Object[] { (object)filePath, true, true });
            Type docType = doc.GetType();

            string strSaveFileName = htmlPath ?? System.IO.Path.ChangeExtension(filePath, "html");

            object saveFileName = (object)strSaveFileName;
            try
            {
                docType.InvokeMember("SaveAs", System.Reflection.BindingFlags.InvokeMethod, null, doc, new object[] { saveFileName, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatFilteredHTML });
                flg = true;
            }
            catch (Exception ex) { throw ex; }
            try
            {
                docType.InvokeMember("Close", System.Reflection.BindingFlags.InvokeMethod, null, doc, null);
                wordType.InvokeMember("Quit", System.Reflection.BindingFlags.InvokeMethod, null, word, null);
            }
            catch { }
            return flg;
        }

        /// <summary>
        /// Word转成Html
        /// </summary>
        /// <param name="path">要转换的文档的路径</param>
        /// <param name="savePath">转换成html的保存路径</param>
        /// <param name="wordFileName">转换成html的文件名字</param>
        public static void Word2Html(string path, string savePath, string wordFileName)
        {
            Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
            Type wordType = word.GetType();
            Microsoft.Office.Interop.Word.Documents docs = word.Documents;
            Type docsType = docs.GetType();
            Microsoft.Office.Interop.Word.Document doc = (Microsoft.Office.Interop.Word.Document)docsType.InvokeMember("Open", System.Reflection.BindingFlags.InvokeMethod, null, docs, new Object[] { (object)path, true, true });
            Type docType = doc.GetType();
            string strSaveFileName = savePath + wordFileName + ".html";
            object saveFileName = (object)strSaveFileName;
            docType.InvokeMember("SaveAs", System.Reflection.BindingFlags.InvokeMethod, null, doc, new object[] { saveFileName, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatFilteredHTML });
            docType.InvokeMember("Close", System.Reflection.BindingFlags.InvokeMethod, null, doc, null);
            wordType.InvokeMember("Quit", System.Reflection.BindingFlags.InvokeMethod, null, word, null);

        }

        /// <summary>
        /// Excel转成Html
        /// </summary>
        /// <param name="filePath">要转换的Excel文档的路径</param>
        /// <param name="htmlPath">转换成Html的保存路径</param>
        /// <returns>是否成功</returns>
        public static bool ExcelToHtml(string filePath, string htmlPath = null)
        {
            string str = string.Empty;
            var flg = false;
            Microsoft.Office.Interop.Excel.Application repExcel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook workbook = null;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = null;
            //是否显示提示框
            repExcel.DisplayAlerts = false;

            workbook = repExcel.Application.Workbooks.Open(filePath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];

            string strSaveFileName = htmlPath ?? System.IO.Path.ChangeExtension(filePath, "html");

            object saveFileName = (object)strSaveFileName;

            object ofmt = Microsoft.Office.Interop.Excel.XlFileFormat.xlHtml;
            try
            {
                workbook.SaveAs(saveFileName, ofmt, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                flg = true;
            }
            catch (Exception ex)
            {
                try
                {
                    workbook.Close(false, Type.Missing, Type.Missing);
                    repExcel.Quit();
                }
                catch
                {
                    DBLib.Windows.WinHelper.KillProcessByName("EXCEL");
                }
                throw ex;
            }
            try
            {
                workbook.Close(false, Type.Missing, Type.Missing);
                repExcel.Quit();
            }
            catch { DBLib.Windows.WinHelper.KillProcessByName("EXCEL"); }
            return flg;
        }

        /// <summary>
        /// Excel转成Html
        /// </summary>
        /// <param name="path">要转换的文档的路径</param>
        /// <param name="savePath">转换成html的保存路径</param>
        /// <param name="wordFileName">转换成html的文件名字</param>
        public static void Excel2Html(string path, string savePath, string wordFileName)
        {
            string str = string.Empty;
            Microsoft.Office.Interop.Excel.Application repExcel = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook workbook = null;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = null;
            workbook = repExcel.Application.Workbooks.Open(path, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];
            object htmlFile = savePath + wordFileName + ".html";
            object ofmt = Microsoft.Office.Interop.Excel.XlFileFormat.xlHtml;
            workbook.SaveAs(htmlFile, ofmt, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            object osave = false;
            workbook.Close(osave, Type.Missing, Type.Missing);
            repExcel.Quit();
        }

        /// <summary>
        /// ppt转成Html
        /// </summary>
        /// <param name="excelPath">要转换的文档的路径</param>
        /// <param name="htmlPath">转换成html的保存路径</param>
        /// <returns>是否成功</returns>
        public static bool PptToHtml(string filePath, string htmlPath = null)
        {
            var flg = false;
            Microsoft.Office.Interop.PowerPoint.Application ppApp = new Microsoft.Office.Interop.PowerPoint.Application();

            string strDestinationFile = htmlPath ?? System.IO.Path.ChangeExtension(filePath, ".html");
            Microsoft.Office.Interop.PowerPoint.Presentation prsPres = ppApp.Presentations.Open(filePath, Microsoft.Office.Core.MsoTriState.msoTrue, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoFalse);
            try
            {
                prsPres.SaveAs(strDestinationFile, Microsoft.Office.Interop.PowerPoint.PpSaveAsFileType.ppSaveAsHTML, MsoTriState.msoTrue);
                flg = true;
            }
            catch (Exception ex) { throw ex; }
            try
            {
                prsPres.Close();
                ppApp.Quit();
            }
            catch { }
            return flg;
        }

        /// <summary>
        /// ppt转成Html
        /// </summary>
        /// <param name="path">要转换的文档的路径</param>
        /// <param name="savePath">转换成html的保存路径</param>
        /// <param name="wordFileName">转换成html的文件名字</param>
        public static void PPT2Html(string path, string savePath, string wordFileName)
        {
            Microsoft.Office.Interop.PowerPoint.Application ppApp = new Microsoft.Office.Interop.PowerPoint.Application();
            string strSourceFile = path;
            string strDestinationFile = savePath + wordFileName + ".html";
            Microsoft.Office.Interop.PowerPoint.Presentation prsPres = ppApp.Presentations.Open(strSourceFile, Microsoft.Office.Core.MsoTriState.msoTrue, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoFalse);

            prsPres.SaveAs(strDestinationFile, Microsoft.Office.Interop.PowerPoint.PpSaveAsFileType.ppSaveAsHTML, MsoTriState.msoTrue);
            prsPres.Close();
            ppApp.Quit();
        }
    }
}
