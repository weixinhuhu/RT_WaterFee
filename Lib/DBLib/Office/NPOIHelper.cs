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
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace DBLib.Office
{
    public class NPOIHelper
    {
        public NPOIHelper() { }

        /// <summary>
        /// 仅支持office2003及之前的版本
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static HSSFWorkbook GetWorkbook(string path)
        {
            HSSFWorkbook workbook;
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                workbook = new HSSFWorkbook(file);
            }
            return workbook;
        }

        public static System.Data.DataTable ToDataTable(string path)
        {
            var hssfworkbook = GetWorkbook(path);
            return ToDataTable(hssfworkbook);
        }

        public static System.Data.DataTable ToDataTable(HSSFWorkbook hssfworkbook)
        {
            ISheet sheet = hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            DataTable dt = new DataTable();
            for (int j = 0; j < 5; j++)
            {
                dt.Columns.Add(Convert.ToChar(((int)'A') + j).ToString());
            }

            while (rows.MoveNext())
            {
                IRow row = (HSSFRow)rows.Current;
                DataRow dr = dt.NewRow();

                for (int i = 0; i < row.LastCellNum; i++)
                {
                    ICell cell = row.GetCell(i);


                    if (cell == null)
                    {
                        dr[i] = null;
                    }
                    else
                    {
                        dr[i] = cell.ToString();
                    }
                }
                dt.Rows.Add(dr);
            }
            //dataSet1.Tables.Add(dt);
            return dt;
        }

        /// <summary>
        /// Office 2007 及后续版本
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static XSSFWorkbook GetWorkbook2007(string path)
        {
            XSSFWorkbook workbook;
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                workbook = new XSSFWorkbook(file);
            }
            return workbook;
        }

        public static List<string> GetSheetNames(string fileName)
        {
            HSSFWorkbook workbook = null;
            if (fileName.ToLower().EndsWith(".xls")) workbook = GetWorkbook(fileName);
            //else workbook = GetWorkbook2007(fileName);

            var list = new List<string>();
            if (workbook == null) return list;
            for (int i = 0; i < workbook.NumberOfSheets; i++)
            {
                list.Add(workbook.GetSheetName(i));
            }
            return list;
        }


        public static List<string> GetSheetNames(IWorkbook workbook)
        {
            var list = new List<string>();
            if (workbook == null) return list;
            for (int i = 0; i < workbook.NumberOfSheets; i++)
            {
                list.Add(workbook.GetSheetName(i));
            }
            return list;
        }

        public static void SaveAs(string fileName, string newSavePath)
        {
            var workbook = GetWorkbook(fileName);
            if (workbook == null) return;

            using (FileStream fs = new FileStream(newSavePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(fs);
            }
        }

        public static void SaveSheetsToSingleSheet(string fileName, string savePath)
        {
            var workbook = GetWorkbook(fileName);
            if (workbook == null) return;

            var count = workbook.NumberOfSheets;
            for (int i = 0; i < count; i++)
            {
                var workbook2 = GetWorkbook(fileName);
                var ss = workbook2.GetSheetAt(i);
                var sheet = RemoveOthers(ss, ss.SheetName);
                var outFile = savePath + ss.SheetName + ".xls";
                using (FileStream fs = new FileStream(outFile, FileMode.Create, FileAccess.Write))
                {
                    sheet.Workbook.Write(fs);
                }
            }
        }

        private static ISheet RemoveOthers(ISheet sheet, string nameLeave)
        {
            var names = GetSheetNames(sheet.Workbook);
            foreach (var item in names)
            {
                if (item != nameLeave)
                    sheet.Workbook.RemoveSheetAt(sheet.Workbook.GetSheetIndex(item));
            }
            return sheet;
        }

        #region Demo
        HSSFWorkbook hssfworkbook;

        void InitializeWorkbook(string path)
        {
            //read the template via FileStream, it is suggested to use FileAccess.Read to prevent file lock.
            //book1.xls is an Excel-2007-generated file, so some new unknown BIFF records are added. 
            using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }
        }

        void ReadExcel()
        {
            //var filePath = Server.MapPath("~\\App_Data\\msds.xls");
            //InitializeWorkbook(filePath);

            //var sheetsCount = hssfworkbook.NumberOfSheets;

            //var sheet = hssfworkbook.GetSheet("17-乙炔");

            //var rows = sheet.GetRowEnumerator();

            //var a1 = sheet.GetRow(0).Cells[0].StringCellValue;
        }
        void ConvertToDataTable()
        {
            ISheet sheet = hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            DataTable dt = new DataTable();
            for (int j = 0; j < 5; j++)
            {
                dt.Columns.Add(Convert.ToChar(((int)'A') + j).ToString());
            }

            while (rows.MoveNext())
            {
                IRow row = (HSSFRow)rows.Current;
                DataRow dr = dt.NewRow();

                for (int i = 0; i < row.LastCellNum; i++)
                {
                    ICell cell = row.GetCell(i);


                    if (cell == null)
                    {
                        dr[i] = null;
                    }
                    else
                    {
                        dr[i] = cell.ToString();
                    }
                }
                dt.Rows.Add(dr);
            }
            //dataSet1.Tables.Add(dt);
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            InitializeWorkbook(@"xls\Book1.xls");
            ConvertToDataTable();

            //dataGridView1.DataSource = dataSet1.Tables[0];
        }

        /// <summary>
        /// 将DataTable导出成Excel
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="strFileName"></param>
        /// <remarks>NPOI认为Excel的第一个单元格是：(0，0)</remarks> 
        public static void ConvertDataTableToExcel(DataTable dtSource, string strFileName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();

            //填充表头
            var dataRow = sheet.CreateRow(0);
            foreach (DataColumn column in dtSource.Columns)
            {
                dataRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
            }


            //填充内容
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                dataRow = sheet.CreateRow(i + 1);
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    dataRow.CreateCell(j).SetCellValue(dtSource.Rows[i][j].ToString());
                }
            }


            //保存
            using (MemoryStream ms = new MemoryStream())
            {
                using (FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
                {
                    workbook.Write(fs);
                }
            }

            workbook = null;
        }

        /// <summary>
        /// 创建工作簿
        /// </summary>
        /// <param name="fileName">下载文件名</param>
        /// <param name="dt">数据源</param>
        /// http://www.cnblogs.com/likeli/p/3896667.html
        public static void ConvertDataTableToExcelAutoColumnWidth(string fileName, DataTable dt)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();

            //创建一个工作表
            ISheet paymentSheet = workbook.CreateSheet(fileName);

            //数据源
            DataTable tbPayment = dt;

            //头部标题
            IRow paymentHeaderRow = paymentSheet.CreateRow(0);

            //循环添加标题
            foreach (DataColumn column in tbPayment.Columns)
                paymentHeaderRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);

            // 内容
            int paymentRowIndex = 1;

            foreach (DataRow row in tbPayment.Rows)
            {
                IRow newRow = paymentSheet.CreateRow(paymentRowIndex);

                //循环添加列的对应内容
                foreach (DataColumn column in tbPayment.Columns)
                {
                    newRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                }

                paymentRowIndex++;
            }

            //列宽自适应，只对英文和数字有效
            for (int i = 0; i <= dt.Rows.Count; i++)
            {
                paymentSheet.AutoSizeColumn(i);
            }
            //获取当前列的宽度，然后对比本列的长度，取最大值
            for (int columnNum = 0; columnNum <= dt.Columns.Count; columnNum++)
            {
                int columnWidth = paymentSheet.GetColumnWidth(columnNum) / 256;
                for (int rowNum = 1; rowNum <= paymentSheet.LastRowNum; rowNum++)
                {
                    IRow currentRow;
                    //当前行未被使用过
                    if (paymentSheet.GetRow(rowNum) == null)
                    {
                        currentRow = paymentSheet.CreateRow(rowNum);
                    }
                    else
                    {
                        currentRow = paymentSheet.GetRow(rowNum);
                    }

                    if (currentRow.GetCell(columnNum) != null)
                    {
                        ICell currentCell = currentRow.GetCell(columnNum);
                        int length = Encoding.Default.GetBytes(currentCell.ToString()).Length;
                        if (columnWidth < length)
                        {
                            columnWidth = length;
                        }
                    }
                }
                //The maximum column width for an individual cell is 255 characters.
                columnWidth = columnWidth > 100 ? 100 : columnWidth;
                paymentSheet.SetColumnWidth(columnNum, columnWidth * 256);
            }

            //将表内容写入流 通知浏览器下载
            workbook.Write(ms);
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition",
                string.Format("attachment; filename={0}.xls", System.Web.HttpUtility.UrlEncode(fileName)));
            System.Web.HttpContext.Current.Response.BinaryWrite(ms.ToArray()); //进行二进制流下在

            workbook = null;
            ms.Close();
            ms.Dispose();
        }

        /// <summary>
        /// 创建工作簿
        /// </summary>
        /// <param name="fileName">下载文件名</param>
        /// <param name="dtSource">数据源</param>
        public static void ConvertDataTableToExcelAutoColumnWidth(System.Web.HttpContext httpContext, string fileName, DataTable dtSource)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();

            //创建一个工作表
            ISheet sheet = workbook.CreateSheet(fileName);

            //头部标题
            IRow sheetHeaderRow = sheet.CreateRow(0);

            //循环添加标题
            foreach (DataColumn column in dtSource.Columns)
                sheetHeaderRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);

            // 内容
            int paymentRowIndex = 1;

            foreach (DataRow row in dtSource.Rows)
            {
                IRow newRow = sheet.CreateRow(paymentRowIndex);

                //循环添加列的对应内容
                foreach (DataColumn column in dtSource.Columns)
                {
                    newRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                }

                paymentRowIndex++;
            }
            //sheet.DisplayGridlines
            //列宽自适应，只对英文和数字有效
            for (int i = 0; i <= dtSource.Rows.Count; i++)
            {
                sheet.AutoSizeColumn(i);
            }
            //获取当前列的宽度，然后对比本列的长度，取最大值
            for (int columnNum = 0; columnNum <= dtSource.Columns.Count; columnNum++)
            {
                int columnWidth = sheet.GetColumnWidth(columnNum) / 256;
                for (int rowNum = 1; rowNum <= sheet.LastRowNum; rowNum++)
                {
                    IRow currentRow;
                    //当前行未被使用过
                    if (sheet.GetRow(rowNum) == null)
                    {
                        currentRow = sheet.CreateRow(rowNum);
                    }
                    else
                    {
                        currentRow = sheet.GetRow(rowNum);
                    }

                    if (currentRow.GetCell(columnNum) != null)
                    {
                        ICell currentCell = currentRow.GetCell(columnNum);
                        int length = Encoding.Default.GetBytes(currentCell.ToString()).Length;
                        if (columnWidth < length)
                        {
                            columnWidth = length;
                        }
                    }
                }
                //The maximum column width for an individual cell is 255 characters.
                columnWidth = columnWidth > 100 ? 100 : columnWidth;
                sheet.SetColumnWidth(columnNum, columnWidth * 256);
            }

            //将表内容写入流 通知浏览器下载
            workbook.Write(ms);
            httpContext.Response.AddHeader("Content-Disposition",
                string.Format("attachment; filename={0}.xls", System.Web.HttpUtility.UrlEncode(fileName)));
            //进行二进制流下载
            httpContext.Response.BinaryWrite(ms.ToArray());

            workbook = null;
            ms.Close();
            ms.Dispose();
        }
        #endregion
    }
}
