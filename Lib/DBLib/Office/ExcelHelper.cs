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
using System.Runtime.InteropServices;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using System.IO;
using DBLib.Windows;

namespace DBLib.Office
{
    public class ExcelHelper
    {
        Application gExcelApp;
        Workbooks gWorkBooks;
        Workbook gWorkBook;
        Worksheet gWorkSheet;
        public static ExcelHelper Instance = new ExcelHelper();

        /*
         * Once you filtered the range, you can access the cells 
        that pass the filter criteria by making use of the Range.
        SpecialCells method, passing in a valued of 'Excel.XlCellType.xlCellTypeVisible' 
        in order to get the visible cells.

        Based on your example code, above, accessing the visible cells should look something like this:

        Excel.Range visibleCells = sheet.UsedRange.SpecialCells(
                                       Excel.XlCellType.xlCellTypeVisible, 
                                       Type.Missing)
        From there you can either access each cell in the visible range, 
        via the 'Range.Cells' collection, or access each row, 
        by first accessing the areas via the 'Range.Areas' collection 
        and then iterating each row within the 'Rows' collection for each area. For example:

        foreach (Excel.Range area in visibleCells.Areas)
        {
            foreach (Excel.Range row in area.Rows)
            {
                // Process each un-filtered, visible row here.
            }
        }
        */

        /// <summary>
        /// 将Excel的列字母转为列索引
        /// </summary>
        /// <param name="columnLetter"></param>
        /// <returns></returns>
        public static int ColumnLetterToColumnIndex(string columnLetter)
        {
            columnLetter = columnLetter.ToUpper();
            int sum = 0;
            for (int i = 0; i < columnLetter.Length; i++)
            {
                sum *= 26;
                sum += (columnLetter[i] - 'A' + 1);
            }
            if (sum > 0)
                sum -= 1;
            return sum;
        }

        public static string ColumnIndexToColumnLetter(int colIndex)
        {
            int div = colIndex;
            string colLetter = String.Empty;
            int mod = 0;
            while (div > 0)
            {
                mod = (div - 1) % 26;
                colLetter = (char)(65 + mod) + colLetter;
                div = (int)((div - mod) / 26);
            }
            return colLetter;
        }

        public void OpenExcel(string fullPath)
        {
            fullPath = "D:\\DBLib\\RTT_Exception_Rpt20140109102655.xls";
            //建立Application对象
            gExcelApp = gExcelApp ?? new Application();

            //是否显示Excel窗口
            gExcelApp.Visible = false;
            //建立Workbooks对象
            gWorkBooks = gExcelApp.Application.Workbooks;

            //建立一个System.Reflection.Missing的object对象
            object oMissing = System.Reflection.Missing.Value;

            //打开Excel文件，注意里的“ExccelFilePath”为Excel文件在服务器上的物理地址，包括文件名
            gWorkBook = gWorkBooks.Open(fullPath, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing, oMissing);

            //新建Workseet对象,，此处为要操作的工作表 ，当前要操作的工作表的获取方法有两种：使用工作表的索引值或使用工作表的名称，名称默认为：“sheet1”/“Sheet2”等
            gWorkSheet = (Worksheet)gWorkBook.ActiveSheet;
            gWorkSheet.get_Range("C9").Value = "DILL181";
            gWorkSheet.get_Range("C11").Value = "1";
            //workBook.SaveAs("D:\\DBLib\\new_81_1.xls");
            var d30 = gWorkSheet.get_Range("d30").Value;
            var txt = gWorkSheet.get_Range("d30").Text;
            var date = gWorkSheet.get_Range("C4").Value;

            //workSheet.get_Range("C9").Value = "DILL181";
            //workSheet.get_Range("C11").Value = "2";
            //workBook.SaveAs("D:\\DBLib\\new_81_2.xls");
            //d30 = workSheet.get_Range("d30").Value;
            //workSheet.get_Range("C9").Value = "DILL181";
            //workSheet.get_Range("C11").Value = "3";
            //workBook.SaveAs("D:\\DBLib\\new_81_3.xls");
            //d30 = workSheet.get_Range("d30").Value;
            //txtEnglishName.Value = workSheet.get_Range("F4").Text.ToString();
            //txtChineseName.Value = workSheet.get_Range("F5").Text.ToString();

            CloseExcel();
        }

        /// <summary>
        /// 关闭和销毁 Excel 进程
        /// </summary>
        public void CloseExcel()
        {
            gExcelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(gWorkSheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(gWorkBooks);
            //调用window api查找Excel进程,并用关闭
            IntPtr t = new IntPtr(gExcelApp.Hwnd);
            int processId;
            WinHelper.GetWindowThreadProcessId(t, out processId);
            WinHelper.KillProcessByPID(processId);
            gWorkSheet = null;
            gWorkBook = null;
            gWorkBooks = null;
            gExcelApp = null;
        }

        /// <summary>
        /// 转换excel文件到html文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        public static bool SheetsToSingleFile(string fileName, string savePath = null)
        {
            //实例化Excel  
            var flg = false;
            Microsoft.Office.Interop.Excel.Application excelApp = null;
            Microsoft.Office.Interop.Excel.Workbook workbook = null;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = null;
            try
            {
                excelApp = new Microsoft.Office.Interop.Excel.Application();
                excelApp.Visible = false;
                //提示框
                // excelApp.DisplayAlerts = false;

                //打开文件，n.FullPath是文件路径  
                workbook = excelApp.Application.Workbooks.Open(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];

                var count = workbook.Sheets.Count;
                var missing = Type.Missing;

                //for (var i = count; i > 1; i--)
                //{
                //    for (int j = 1; j < i; j++)
                //    {
                //        excelApp.DisplayAlerts = false;
                //        ((Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[i]).Delete();
                //        excelApp.DisplayAlerts = true;
                //    }
                //    worksheet.SaveAs(@"D:\db_temp\" + worksheet.Name);
                //}

                for (int j = 1; j < 25; j++)
                {
                    excelApp.DisplayAlerts = false;
                    ((Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1]).Delete();
                    excelApp.DisplayAlerts = true;
                    var coun = workbook.Sheets.Count;
                }
                worksheet.SaveAs(@"D:\db_temp\" + worksheet.Name);

                //string filesavefilename = fileName.ToString();
                //if (savePath == null)
                //    savePath = Path.ChangeExtension(filesavefilename, "html");
                //object savefilename = (object)savePath;
                //object ofmt = Microsoft.Office.Interop.Excel.XlFileFormat.xlHtml;
                ////如果文件存在则删除

                //if (System.IO.File.Exists(savePath))
                //    System.IO.File.Delete(savePath);
                ////进行另存为操作    
                //workbook.SaveAs(savefilename, ofmt, Type.Missing, Type.Missing, Type.Missing, Type.Missing
                //    , Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing
                //    , Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                //flg = true;

                ////逐步关闭所有使用的对象  
                //IntPtr t = new IntPtr(excelApp.Hwnd);
                //int processId;

                //excelApp.Quit();
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                //worksheet = null;
                ////垃圾回收  
                //GC.Collect();
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                //workbook = null;
                //GC.Collect();
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp.Application.Workbooks);
                //GC.Collect();
                //System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                //excelApp = null;
                //GC.Collect();

                //Common.WinHelper.GetWindowThreadProcessId(t, out processId);
                //Common.WinHelper.KillProcessByPID(processId);

                return true;
            }
            catch (Exception ex)
            {

                //try
                //{
                //    excelApp.Quit();
                //    System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                //    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                //    //调用window api查找Excel进程,并用关闭
                //    IntPtr t = new IntPtr(excelApp.Hwnd);
                //    int processId;
                //    Common.WinHelper.GetWindowThreadProcessId(t, out processId);
                //    Common.WinHelper.KillProcessByPID(processId);
                //    worksheet = null;
                //    workbook = null;
                //    excelApp = null;
                //}
                //catch (Exception ex2) { throw ex2; }
                throw ex;
            }
            return flg;
        }

        /// <summary>
        /// 转换excel文件到html文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        public static bool ExcelToHtml(string fileName, string savePath = null)
        {
            //实例化Excel  
            var flg = false;
            Microsoft.Office.Interop.Excel.Application excelApp = null;
            Microsoft.Office.Interop.Excel.Workbook workbook = null;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = null;
            try
            {
                excelApp = new Microsoft.Office.Interop.Excel.Application();
                excelApp.Visible = false;
                //提示框
                excelApp.DisplayAlerts = false;

                //打开文件，n.FullPath是文件路径  
                workbook = excelApp.Application.Workbooks.Open(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];
                //for (int i = 1; i < workbook.Sheets.Count; )
                //{
                //    repExcel.DisplayAlerts = false;
                //    worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[i];
                //    worksheet.Delete();
                //    repExcel.DisplayAlerts = true;
                //}
                string filesavefilename = fileName.ToString();
                if (savePath == null)
                    savePath = Path.ChangeExtension(filesavefilename, "html");
                object savefilename = (object)savePath;
                object ofmt = Microsoft.Office.Interop.Excel.XlFileFormat.xlHtml;

                //如果文件存在则删除
                if (System.IO.File.Exists(savePath))
                    System.IO.File.Delete(savePath);
                //进行另存为操作    
                workbook.SaveAs(savefilename, ofmt, Type.Missing, Type.Missing, Type.Missing, Type.Missing
                    , Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing
                    , Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                flg = true;

                //逐步关闭所有使用的对象  
                IntPtr t = new IntPtr(excelApp.Hwnd);
                int processId;

                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                worksheet = null;
                //垃圾回收  
                GC.Collect();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                workbook = null;
                GC.Collect();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp.Application.Workbooks);
                GC.Collect();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                excelApp = null;
                GC.Collect();

                WinHelper.GetWindowThreadProcessId(t, out processId);
                WinHelper.KillProcessByPID(processId);

                return true;
            }
            catch (Exception ex)
            {

                try
                {
                    excelApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                    //调用window api查找Excel进程,并用关闭
                    IntPtr t = new IntPtr(excelApp.Hwnd);
                    int processId;
                    WinHelper.GetWindowThreadProcessId(t, out processId);
                    WinHelper.KillProcessByPID(processId);
                    worksheet = null;
                    workbook = null;
                    excelApp = null;
                }
                catch { }
                //throw ex;
                return false;
            }
            return flg;
        }


        /// <summary>
        /// 另存为
        /// </summary>
        /// <param name="oldFileName"></param>
        /// <param name="newFileName"></param>
        /// <returns></returns>
        public static bool SaveAs(string oldFileName, string newFileName = null)
        {
            //实例化Excel  
            var flg = false;
            Microsoft.Office.Interop.Excel.Application excelApp = null;
            Microsoft.Office.Interop.Excel.Workbook workbook = null;
            Microsoft.Office.Interop.Excel.Worksheet worksheet = null;
            try
            {
                excelApp = new Microsoft.Office.Interop.Excel.Application();
                excelApp.Visible = false;
                //提示框
                excelApp.DisplayAlerts = false;

                //打开文件，n.FullPath是文件路径  
                workbook = excelApp.Application.Workbooks.Open(oldFileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];
                //for (int i = 1; i < workbook.Sheets.Count; )
                //{
                //    repExcel.DisplayAlerts = false;
                //    worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[i];
                //    worksheet.Delete();
                //    repExcel.DisplayAlerts = true;
                //}
                
                if (newFileName == null)
                    newFileName = oldFileName;// Path.ChangeExtension(filesavefilename, "html");
                object savefilename = (object)newFileName;
                object ofmt = Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel8;
                //如果文件存在则删除

                if (System.IO.File.Exists(newFileName))
                    System.IO.File.Delete(newFileName);
                //进行另存为操作    
                workbook.SaveAs(savefilename, ofmt, Type.Missing, Type.Missing, Type.Missing, Type.Missing
                    , Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing
                    , Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                flg = true;

                //逐步关闭所有使用的对象  
                IntPtr t = new IntPtr(excelApp.Hwnd);
                int processId;

                workbook.Close(false, Type.Missing, Type.Missing);
                excelApp.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                worksheet = null;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                workbook = null; 
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp.Application.Workbooks); 
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                excelApp = null; 
                //垃圾回收  
                GC.Collect();

                WinHelper.GetWindowThreadProcessId(t, out processId);
                WinHelper.KillProcessByPID(processId);

                return true;
            }
            catch (Exception ex)
            {

                try
                {
                    excelApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                    //调用window api查找Excel进程,并用关闭
                    IntPtr t = new IntPtr(excelApp.Hwnd);
                    int processId;
                    WinHelper.GetWindowThreadProcessId(t, out processId);
                    WinHelper.KillProcessByPID(processId);
                    worksheet = null;
                    workbook = null;
                    excelApp = null;
                }
                catch { }
                throw ex;
                //return false;
            }
            return flg;
        }

        //public static string ExcelToHtmlBak(string excelFileName)
        //{
        //    //实例化Excel  
        //    Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
        //    Microsoft.Office.Interop.Excel.Workbook workbook = null;
        //    Microsoft.Office.Interop.Excel.Worksheet worksheet = null;
        //    //打开文件，n.FullPath是文件路径  
        //    workbook = excelApp.Application.Workbooks.Open(excelFileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        //    // worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1]; 
        //    //for (int i = 1; i < workbook.Sheets.Count; )
        //    //{
        //    //    repExcel.DisplayAlerts = false;
        //    //    worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[i];
        //    //    worksheet.Delete();
        //    //    repExcel.DisplayAlerts = true;
        //    //}
        //    string filesavefilename = excelFileName.ToString();
        //    string strsavefilename = filesavefilename.Substring(0, filesavefilename.Length - 3) + "html";
        //    object savefilename = (object)strsavefilename;
        //    object ofmt = Microsoft.Office.Interop.Excel.XlFileFormat.xlHtml;
        //    //进行另存为操作    
        //    workbook.SaveAs(savefilename, ofmt, Type.Missing, Type.Missing, Type.Missing, Type.Missing
        //        , Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing
        //        , Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        //    object osave = false;
        //    //逐步关闭所有使用的对象  
        //    workbook.Close(osave, Type.Missing, Type.Missing);
        //    excelApp.Quit();
        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
        //    worksheet = null;
        //    //垃圾回收  
        //    GC.Collect();
        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
        //    workbook = null;
        //    GC.Collect();
        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp.Application.Workbooks);
        //    GC.Collect();
        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
        //    excelApp = null;
        //    GC.Collect();
        //    //依据时间杀灭进程  
        //    System.Diagnostics.Process[] process = System.Diagnostics.Process.GetProcessesByName("EXCEL");
        //    foreach (System.Diagnostics.Process p in process)
        //    {
        //        if (DateTime.Now.Second - p.StartTime.Second > 0 && DateTime.Now.Second - p.StartTime.Second < 5)
        //        {
        //            p.Kill();
        //        }
        //    } 
        //    return savefilename.ToString();
        //}
    } 
}
