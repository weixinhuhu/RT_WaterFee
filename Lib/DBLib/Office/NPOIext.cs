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
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DBLib.Office
{
    public static class NPOIext
    {
        public static string GetCellsValue(this IRow row, int startIndex, int endIndex = -1)
        {
            var sb = new System.Text.StringBuilder();
            if (endIndex == -1)
                endIndex = startIndex;
            for (int i = startIndex; i < endIndex + 1; i++)
            {
                try
                {
                    row.Cells[i].SetCellType(CellType.String);
                    sb.Append(row.Cells[i].StringCellValue.Trim());
                }
                catch { }
            }
            return sb.ToString();
        }

        public static string GetCellsValue(this IRow row, string startLetter, string endLetter = null)
        {
            var sb = new System.Text.StringBuilder();
            var startIndex = ColumnLetterToColumnIndex(startLetter);
            var endIndex = ColumnLetterToColumnIndex(endLetter);
            if (endIndex == -1)
                endIndex = startIndex;
            for (int i = startIndex; i < endIndex + 1; i++)
            {
                try
                {
                    row.Cells[i].SetCellType(CellType.String);
                    sb.Append(row.Cells[i].StringCellValue.Trim());
                }
                catch { }
            }
            return sb.ToString();
        }

        private static string GetCellsValueBak(IRow row, int startIndex, int endIndex)
        {
            var sb = new System.Text.StringBuilder();
            for (int i = startIndex; i < endIndex + 1; i++)
            {
                try
                {
                    row.Cells[i].SetCellType(CellType.String);
                    sb.Append(row.Cells[i].StringCellValue.Trim());
                }
                catch { }
            }
            return sb.ToString();
        }

        public static string GetRangeValue(this ISheet sheet, string cell)
        {
            var sb = new System.Text.StringBuilder();
            try
            {
                Regex reg = new Regex("[a-zA-Z]+");
                var r = reg.Match(cell);
                var columnLetter = r.Groups[0].Value;
                var columnIndex = ColumnLetterToColumnIndex(columnLetter);
                var rowIndex = cell.Replace(columnLetter, "").ToInt();
            }
            catch { }
            return sb.ToString();
        }

        /// <summary>
        /// 将Excel的列字母转为列索引,没有则返回-1
        /// </summary>
        /// <param name="columnLetter"></param>
        /// <returns></returns>
        static int ColumnLetterToColumnIndex(string columnLetter)
        {
            try
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
            catch
            {
                return -1;
            }
        }

        static string ColumnIndexToColumnLetter(int colIndex)
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
    }
}
