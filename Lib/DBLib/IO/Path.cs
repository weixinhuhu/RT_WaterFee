using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBLib.IO
{
    public class Path
    {
        public static string Combine(params string[] paths)
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

            return string.Join("\\", list);
        }

    }
}
