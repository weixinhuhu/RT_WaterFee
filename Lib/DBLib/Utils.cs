using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBLib
{
    /// <summary>
    /// 通用类
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// 合并路径
        /// </summary>
        /// <param name="separator">分割符</param>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static string Combine(char separator = '\\', params string[] paths)
        {
            var list = new List<string>();
            foreach (var item in paths)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    var arr = item.Split(new char[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var part in arr)
                    {
                        list.Add(part);
                    }
                }
            }
            if (list.Count == 0) return string.Empty;

            return string.Join(separator.ToString(), list);
        }

        /// <summary>
        /// 合并路径
        /// </summary> 
        /// <param name="paths"></param>
        /// <returns></returns>
        public static string CombinePath(params string[] paths)
        {
            return Combine('\\', paths);
        }

        /// <summary>
        /// 合并URL
        /// </summary> 
        /// <param name="paths"></param>
        /// <returns></returns>
        public static string CombineUrl(params string[] paths)
        {
            var list = new List<string>();
            foreach (var item in paths)
            {
                if (!string.IsNullOrWhiteSpace(item))
                {
                    var arr = item.Split(new char[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var part in arr)
                    {
                        list.Add(part);
                    }
                }
            }
            if (list.Count == 0) return string.Empty;

            return string.Join("/", list);
        }
    }
}
