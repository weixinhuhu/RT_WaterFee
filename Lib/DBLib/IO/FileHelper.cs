using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DBLib.IO
{
    public class FileHelper
    {
        private string path { get; set; }

        public FileHelper() { }

        public FileHelper(string path)
        {
            this.path = path;
        }
        /// <summary>
        /// 取文件的扩展名
        /// </summary>
        /// <param name="FileName">文件名称</param>
        /// <returns>string</returns>
        public static string GetExtFileTypeName(string FileName)
        {
            string sFile = FileName;// myFile.PostedFile.FileName;
            sFile = sFile.Substring(sFile.LastIndexOf("\\") + 1);
            sFile = sFile.Substring(sFile.LastIndexOf(".")).ToLower();
            return sFile;
        }

        public bool SaveConfig<T>(T model)
        {
            try
            {
                using (var fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(T));
                    xs.Serialize(fs, model);
                    return true;
                }
            }
            catch { return false; }
        }

        public T ReadConfig<T>()
        {
            try
            {
                using (var sr = new StreamReader(path))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(T));
                    return (T)xs.Deserialize(sr);
                }
            }
            catch
            {
                return default(T);
            }
        }


        public static bool SaveConfig<T>(T model, string path)
        {
            try
            {
                using (var fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(T));
                    xs.Serialize(fs, model);
                    return true;
                }
            }
            catch { return false; }
        }

        public static T ReadConfig<T>(string path)
        {
            try
            {
                using (var sr = new StreamReader(path))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(T));
                    return (T)xs.Deserialize(sr);
                }
            }
            catch
            {
                return default(T);
            }
        }
    }
}
