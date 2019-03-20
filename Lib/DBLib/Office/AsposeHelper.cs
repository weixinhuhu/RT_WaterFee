using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Data;
using System.IO;

namespace DBLib.Office
{
    /// <summary>
    /// Aspose操作类
    /// </summary>
    public class AsposeHelper
    {
        /// <summary>
        /// Aspose.Cells常用操作封装
        /// </summary>
        public class Excel
        {
            /// <summary>
            /// 将Excel转成DataTable
            /// </summary>
            /// <param name="fileName"></param>
            /// <returns></returns>
            public static System.Data.DataTable ReadExcel(string fileName)
            {
                Workbook book = new Workbook(fileName);
                Worksheet sheet = book.Worksheets[0];
                Aspose.Cells.Cells cells = sheet.Cells;
                return cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, true);
            }

            /// <summary>
            /// 初始化Workbook
            /// </summary>
            /// <param name="fileName"></param>
            /// <returns></returns>
            public static Workbook GetWorkbook(string fileName)
            {
                Workbook book = new Workbook(fileName);
                return book;
            }


            /// <summary>
            /// 将workbook从浏览器端下载
            /// </summary>
            /// <param name="workbook">Workbook</param>
            /// <param name="response">HttpResponse</param>
            /// <param name="filename">保存的文件名</param>
            public static void Download(Workbook workbook, System.Web.HttpResponse response, string filename = null)
            {
                if (string.IsNullOrEmpty(filename)) filename = DateTime.Now.ToString("yyyyMMdd_hhMMssfff") + ".xls";
                response.Clear();
                response.Buffer = true;
                response.Charset = "utf-8";
                response.AppendHeader("Content-Disposition", "attachment;filename=" + filename);
                response.ContentEncoding = System.Text.Encoding.UTF8;
                response.ContentType = "application/ms-excel";
                response.BinaryWrite(workbook.SaveToStream().ToArray());
                response.End();
            }

          

            public static void Download<T>(IEnumerable<T> data, HttpResponse response, string filename = null)
            {
                Workbook workbook = new Workbook();
                Worksheet sheet = (Worksheet)workbook.Worksheets[0];

                PropertyInfo[] ps = typeof(T).GetProperties();
                var colIndex = "A";

                foreach (var p in ps)
                {

                    sheet.Cells[colIndex + 1].PutValue(p.Name);
                    int i = 2;
                    foreach (var d in data)
                    {
                        sheet.Cells[colIndex + i].PutValue(p.GetValue(d, null));
                        i++;
                    }

                    colIndex = ((char)(colIndex[0] + 1)).ToString();
                }
                if (string.IsNullOrEmpty(filename)) filename = DateTime.Now.ToString("yyyyMMdd_hhMMssfff") + ".xls";

                response.Clear();
                response.Buffer = true;
                response.Charset = "utf-8";
                response.AppendHeader("Content-Disposition", "attachment;filename=" + filename);
                response.ContentEncoding = System.Text.Encoding.UTF8;
                response.ContentType = "application/ms-excel";
                response.BinaryWrite(workbook.SaveToStream().ToArray());
                response.End();
            }

            public static bool DataTableToExcel(DataTable datatable, System.Web.HttpResponse response, out string error)
            {
                
                error = "";
                try
                {
                    string fileName = DateTime.Now.ToString("yyyyMMdd_hhMMssfff") + ".xls";
                    if (datatable == null)
                    {
                        error = "DataTableToExcel:datatable 为空";
                        return false;
                    }

                    Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();
                    Aspose.Cells.Worksheet sheet = workbook.Worksheets[0];
                    Aspose.Cells.Cells cells = sheet.Cells;

                    int nRow = 0;
                    foreach (DataRow row in datatable.Rows)
                    {
                        nRow++;
                        try
                        {
                            for (int i = 0; i < datatable.Columns.Count; i++)
                            {
                                if (row[i].GetType().ToString() == "System.Drawing.Bitmap")
                                {
                                    //------插入图片数据-------
                                    System.Drawing.Image image = (System.Drawing.Image)row[i];
                                    MemoryStream mstream = new MemoryStream();
                                    image.Save(mstream, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    sheet.Pictures.Add(nRow, i, mstream);
                                }
                                else
                                {
                                    cells[nRow, i].PutValue(row[i]);
                                }
                            }
                        }
                        catch (System.Exception e)
                        {
                            error = error + " DataTableToExcel: " + e.Message;
                        }
                    }
                    response.Clear();
                    response.Buffer = true;
                    response.Charset = "utf-8";
                    response.AppendHeader("Content-Disposition", "attachment;filename=" + fileName);
                    response.ContentEncoding = System.Text.Encoding.UTF8;
                    response.ContentType = "application/ms-excel";
                    response.BinaryWrite(workbook.SaveToStream().ToArray());
                    response.End();

                    //workbook.Save(filepath);
                    return true;
                }
                catch (System.Exception e)
                {
                    error = error + " DataTableToExcel: " + e.Message;
                    return false;
                }
            }

        }
    }
}
