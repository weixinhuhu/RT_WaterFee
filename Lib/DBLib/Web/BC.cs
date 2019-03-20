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
using System.Web;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DBLib
{
    /// <summary>
    /// BaseClass:基础类
    /// QQ群:257018781
    /// </summary>
    public class BC : System.Web.UI.Page
    {
        private static BC instance;
        public static BC Instance
        {
            get
            {
                if (instance == null) instance = new BC();
                return instance;
            }
        }

 

        /// <summary>
        /// 获取当前页面url
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public static string GetCurrentUrl(HttpRequest request)
        {
            string url = null;
            if (request.Url.Port == 80)
                url = request.Url.Scheme + "://" + request.Url.Host + "/";
            else
                url = request.Url.Scheme + "://" + request.Url.Host + ":" + request.Url.Port + "/";
            return url;
        }

        /// <summary>
        /// 获取当前页面url
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public string GetCurrentUrl2(HttpRequest request)
        {
            string url = null;
            if (request.Url.Port == 80)
                url = request.Url.Scheme + "://" + request.Url.Host + "/";
            else
                url = request.Url.Scheme + "://" + request.Url.Host + ":" + request.Url.Port + "/";
            return url;
        }

        /// <summary>
        /// 取得HTML中所有图片的 URL。
        /// </summary>
        /// <param name="sHtmlText">HTML代码</param>
        /// <returns>图片的URL列表</returns>
        public static string[] GetHtmlImageUrlList(string sHtmlText)
        {
            // 定义正则表达式用来匹配 img 标签
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            // 搜索匹配的字符串
            MatchCollection matches = regImg.Matches(sHtmlText);

            int i = 0;
            string[] sUrlList = new string[matches.Count];

            // 取得匹配项列表
            foreach (Match match in matches)
                sUrlList[i++] = match.Groups["imgUrl"].Value;

            return sUrlList;
        }

        /// <summary>
        /// 上传文件并返回真实路径
        /// </summary> 
        public string UploadReturnTrueFilePath2(HttpPostedFile file, string saveDir)
        {
            try
            {
                if (file == null || string.IsNullOrEmpty(saveDir))
                    return "";
                if (!System.IO.Directory.Exists(saveDir))
                    System.IO.Directory.CreateDirectory(saveDir);
                var filename = saveDir + GetRadomName + GetFileExt(file.FileName);
                file.SaveAs(filename);
                return filename;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 上传文件并返回真实路径
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string UploadReturnTrueFilePath(HttpPostedFile file)
        {
            try
            {
                if (file == null)
                    return "";
                var root = Server.MapPath("~/");
                var dataPath = string.Format("~/uploads/{0}/{1}/{2}/", DateTime.Now.Year, DateTime.Now.ToString("MM"), DateTime.Now.ToString("dd"));
                var savePath = Server.MapPath(dataPath);
                if (!System.IO.Directory.Exists(savePath))
                    System.IO.Directory.CreateDirectory(savePath);
                var filename = savePath + GetRadomName + GetFileExt(file.FileName);
                file.SaveAs(filename);
                return filename;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 上传文件并返回真实路径
        /// </summary> 
        public string UploadReturnTrueFilePath(HttpPostedFile file, string _savePath)
        {
            try
            {
                if (file == null)
                    return "";
                var root = Server.MapPath("~/");
                var dataPath = string.Format("~/{0}/", _savePath);
                var savePath = Server.MapPath(dataPath);
                if (!System.IO.Directory.Exists(savePath))
                    System.IO.Directory.CreateDirectory(savePath);
                var filename = savePath + GetRadomName + GetFileExt(file.FileName);
                file.SaveAs(filename);
                return filename;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 上传文件并返回相对路径
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string UploadReturnFilePath(HttpPostedFile file)
        {
            try
            {
                if (file == null)
                    return "";
                var root = Server.MapPath("~/");
                var dataPath = string.Format("~/uploads/{0}/{1}/{2}/", DateTime.Now.Year, DateTime.Now.ToString("MM"), DateTime.Now.ToString("dd"));
                var savePath = Server.MapPath(dataPath);
                if (!System.IO.Directory.Exists(savePath))
                    System.IO.Directory.CreateDirectory(savePath);
                var filename = savePath + GetRadomName + GetFileExt(file.FileName);
                file.SaveAs(filename);
                var relativePath = filename.Replace(root, "/").Replace("\\", "/");
                return relativePath;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 上传文件并返回相对路径
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string UploadReturnFilePath(HttpPostedFile file, string path)
        {
            try
            {
                if (file == null)
                    return "";
                var root = Server.MapPath("~/");
                var dataPath = string.Format("~/{0}/{1}/{2}/{3}/", path, DateTime.Now.Year, DateTime.Now.ToString("MM"), DateTime.Now.ToString("dd"));
                var savePath = Server.MapPath(dataPath);
                if (!System.IO.Directory.Exists(savePath))
                    System.IO.Directory.CreateDirectory(savePath);
                var filename = savePath + GetRadomName + GetFileExt(file.FileName);
                file.SaveAs(filename);
                var relativePath = filename.Replace(root, "").Replace("\\", "/");
                return relativePath;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 上传文件并返回相对路径及物理路径
        /// </summary>
        /// <param name="file">file对象</param>
        /// <param name="path">保存目录,基于根目录下.</param>
        /// <param name="fileFullPath">文件的真实路径</param>
        /// <returns>相对路径</returns>
        public string UploadReturnFilePath(HttpPostedFile file, string path, ref string fileFullPath)
        {
            try
            {
                if (file == null)
                    return "";
                var root = Server.MapPath("~/");
                var dataPath = string.Format("~/{0}/{1}/{2}/{3}/", path, DateTime.Now.Year, DateTime.Now.ToString("MM"), DateTime.Now.ToString("dd"));
                var savePath = Server.MapPath(dataPath);
                if (!System.IO.Directory.Exists(savePath))
                    System.IO.Directory.CreateDirectory(savePath);
                fileFullPath = savePath + GetRadomName + GetFileExt(file.FileName);
                file.SaveAs(fileFullPath);
                var relativePath = fileFullPath.Replace(root, "").Replace("\\", "/");
                return relativePath;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 上传文件并返回相对路径
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string UploadReturnFilePath(HttpPostedFile file, string path, string filename)
        {
            try
            {
                if (file == null)
                    return "";
                var root = Server.MapPath("~/");
                var dataPath = string.Format("~/{0}/{1}/{2}/{3}/", path, DateTime.Now.Year, DateTime.Now.ToString("MM"), DateTime.Now.ToString("dd"));
                var savePath = Server.MapPath(dataPath);
                if (!System.IO.Directory.Exists(savePath))
                    System.IO.Directory.CreateDirectory(savePath);
                var savefilename = savePath + filename;
                file.SaveAs(savefilename);
                var relativePath = savefilename.Replace(root, "").Replace("\\", "/");
                return relativePath;
            }
            catch (Exception ex) { throw ex; }
        }

        /// <summary>
        /// 获取随机文件名
        /// </summary>
        public string GetRadomName
        {
            get
            {
                return DateTime.Now.ToString("yyyyMMddHHmmss") + new Random(Guid.NewGuid().GetHashCode()).Next(10000, 99999);
            }
        }

        /// <summary>
        /// 获取文件扩展名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetFileExt(string fileName)
        {
            string name = "";
            if (string.IsNullOrEmpty(fileName))
                return name;
            var arr = fileName.Split('.');
            if (arr.Length > 0)
            {
                var last = arr.Last();
                if (last.Length > 10)
                    name = ".unknown";
                else
                    name = "." + last;
            }
            return name;
        }

        /// <summary>
        /// 当前站点的虚拟路径,形式:/lghse/
        /// </summary>
        public string GetVirtualPath(HttpRequest req)
        {
            return req.ApplicationPath == "/" ? req.ApplicationPath : req.ApplicationPath + "/";
        }

        /// <summary>
        /// 获取文件扩展名,不带.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GetFileExtNoPoint(string fileName)
        {
            string ext = "";
            if (string.IsNullOrEmpty(fileName))
                return ext;
            var arr = fileName.Split('.');
            if (arr.Length > 0)
            {
                var last = arr.Last();
                if (last.Length > 10)
                    ext = "unknown";
                else
                    ext = last;
            }
            return ext;
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="page"></param>
        /// <param name="fileName">默认文件名</param>
        /// <param name="filePath"></param>
        public void DownFile(System.Web.UI.Page page, string fileName, string filePath)
        {
            // 微软为Response对象提供了一个新的方法TransmitFile来解决使用Response.BinaryWrite 
            // 下载超过400mb的文件时导致Aspnet_wp.exe进程回收而无法成功下载的问题。 
            var ext = GetFileExtNoPoint(filePath);

            //page.Response.ContentType = "application/octet-stream";
            page.Response.ContentType = GetContentType(ext);
            page.Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName));
            //string filename = Server.MapPath(filePath);
            string filename = page.Server.MapPath("~\\" + filePath);
            page.Response.TransmitFile(filename);
            page.Response.Flush();
            page.Response.End();
        }


        /// <summary>
        /// 根据扩展名获取ContentType,默认为application/octet-stream
        /// </summary>
        /// <param name="ext">扩展名</param>
        /// <returns>返回相对应的ContentType,默认为application/octet-stream</returns>
        public string GetContentType(string ext)
        {
            string type = null;
            switch (ext)
            {
                //office 2007 
                case "xlsx": type = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; break;
                case "xltx": type = "application/vnd.openxmlformats-officedocument.spreadsheetml.template"; break;
                case "potx": type = "application/vnd.openxmlformats-officedocument.presentationml.template"; break;
                case "ppsx": type = "application/vnd.openxmlformats-officedocument.presentationml.slideshow"; break;
                case "pptx": type = "application/vnd.openxmlformats-officedocument.presentationml.presentation"; break;
                case "sldx": type = "application/vnd.openxmlformats-officedocument.presentationml.slide"; break;
                case "docx": type = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"; break;
                case "dotx": type = "application/vnd.openxmlformats-officedocument.wordprocessingml.template"; break;
                case "xlam": type = "application/vnd.ms-excel.addin.macroEnabled.12"; break;
                case "xlsb": type = "application/vnd.ms-excel.sheet.binary.macroEnabled.12"; break;
                //
                case "ez": type = "application/andrew-inset"; break;
                case "hqx": type = "application/mac-binhex40"; break;
                case "cpt": type = "application/mac-compactpro"; break;
                case "doc": type = "application/msword"; break;
                case "bin": type = "application/octet-stream"; break;
                case "dms": type = "application/octet-stream"; break;
                case "lha": type = "application/octet-stream"; break;
                case "lzh": type = "application/octet-stream"; break;
                case "exe": type = "application/octet-stream"; break;
                case "class": type = "application/octet-stream"; break;
                case "so": type = "application/octet-stream"; break;
                case "dll": type = "application/octet-stream"; break;
                case "oda": type = "application/oda"; break;
                case "pdf": type = "application/pdf"; break;
                case "ai": type = "application/postscript"; break;
                case "eps": type = "application/postscript"; break;
                case "ps": type = "application/postscript"; break;
                case "smi": type = "application/smil"; break;
                case "smil": type = "application/smil"; break;
                case "mif": type = "application/vnd.mif"; break;
                case "xls": type = "application/vnd.ms-excel"; break;
                case "ppt": type = "application/vnd.ms-powerpoint"; break;
                case "wbxml": type = "application/vnd.wap.wbxml"; break;
                case "wmlc": type = "application/vnd.wap.wmlc"; break;
                case "wmlsc": type = "application/vnd.wap.wmlscriptc"; break;
                case "bcpio": type = "application/x-bcpio"; break;
                case "vcd": type = "application/x-cdlink"; break;
                case "pgn": type = "application/x-chess-pgn"; break;
                case "cpio": type = "application/x-cpio"; break;
                case "csh": type = "application/x-csh"; break;
                case "dcr": type = "application/x-director"; break;
                case "dir": type = "application/x-director"; break;
                case "dxr": type = "application/x-director"; break;
                case "dvi": type = "application/x-dvi"; break;
                case "spl": type = "application/x-futuresplash"; break;
                case "gtar": type = "application/x-gtar"; break;
                case "hdf": type = "application/x-hdf"; break;
                case "js": type = "application/x-javascript"; break;
                case "skp": type = "application/x-koan"; break;
                case "skd": type = "application/x-koan"; break;
                case "skt": type = "application/x-koan"; break;
                case "skm": type = "application/x-koan"; break;
                case "latex": type = "application/x-latex"; break;
                case "nc": type = "application/x-netcdf"; break;
                case "cdf": type = "application/x-netcdf"; break;
                case "sh": type = "application/x-sh"; break;
                case "shar": type = "application/x-shar"; break;
                case "swf": type = "application/x-shockwave-flash"; break;
                case "sit": type = "application/x-stuffit"; break;
                case "sv4cpio": type = "application/x-sv4cpio"; break;
                case "sv4crc": type = "application/x-sv4crc"; break;
                case "tar": type = "application/x-tar"; break;
                case "tcl": type = "application/x-tcl"; break;
                case "tex": type = "application/x-tex"; break;
                case "texinfo": type = "application/x-texinfo"; break;
                case "texi": type = "application/x-texinfo"; break;
                case "t": type = "application/x-troff"; break;
                case "tr": type = "application/x-troff"; break;
                case "roff": type = "application/x-troff"; break;
                case "man": type = "application/x-troff-man"; break;
                case "me": type = "application/x-troff-me"; break;
                case "ms": type = "application/x-troff-ms"; break;
                case "ustar": type = "application/x-ustar"; break;
                case "src": type = "application/x-wais-source"; break;
                case "xhtml": type = "application/xhtml+xml"; break;
                case "xht": type = "application/xhtml+xml"; break;
                case "zip": type = "application/zip"; break;
                case "au": type = "audio/basic"; break;
                case "snd": type = "audio/basic"; break;
                case "mid": type = "audio/midi"; break;
                case "midi": type = "audio/midi"; break;
                case "kar": type = "audio/midi"; break;
                case "mpga": type = "audio/mpeg"; break;
                case "mp2": type = "audio/mpeg"; break;
                case "mp3": type = "audio/mpeg"; break;
                case "aif": type = "audio/x-aiff"; break;
                case "aiff": type = "audio/x-aiff"; break;
                case "aifc": type = "audio/x-aiff"; break;
                case "m3u": type = "audio/x-mpegurl"; break;
                case "ram": type = "audio/x-pn-realaudio"; break;
                case "rm": type = "audio/x-pn-realaudio"; break;
                case "rpm": type = "audio/x-pn-realaudio-plugin"; break;
                case "ra": type = "audio/x-realaudio"; break;
                case "wav": type = "audio/x-wav"; break;
                case "pdb": type = "chemical/x-pdb"; break;
                case "xyz": type = "chemical/x-xyz"; break;
                case "bmp": type = "image/bmp"; break;
                case "gif": type = "image/gif"; break;
                case "ief": type = "image/ief"; break;
                case "jpeg": type = "image/jpeg"; break;
                case "jpg": type = "image/jpeg"; break;
                case "jpe": type = "image/jpeg"; break;
                case "png": type = "image/png"; break;
                case "tiff": type = "image/tiff"; break;
                case "tif": type = "image/tiff"; break;
                case "djvu": type = "image/vnd.djvu"; break;
                case "djv": type = "image/vnd.djvu"; break;
                case "wbmp": type = "image/vnd.wap.wbmp"; break;
                case "ras": type = "image/x-cmu-raster"; break;
                case "pnm": type = "image/x-portable-anymap"; break;
                case "pbm": type = "image/x-portable-bitmap"; break;
                case "pgm": type = "image/x-portable-graymap"; break;
                case "ppm": type = "image/x-portable-pixmap"; break;
                case "rgb": type = "image/x-rgb"; break;
                case "xbm": type = "image/x-xbitmap"; break;
                case "xpm": type = "image/x-xpixmap"; break;
                case "xwd": type = "image/x-xwindowdump"; break;
                case "igs": type = "model/iges"; break;
                case "iges": type = "model/iges"; break;
                case "msh": type = "model/mesh"; break;
                case "mesh": type = "model/mesh"; break;
                case "silo": type = "model/mesh"; break;
                case "wrl": type = "model/vrml"; break;
                case "vrml": type = "model/vrml"; break;
                case "css": type = "text/css"; break;
                case "html": type = "text/html"; break;
                case "htm": type = "text/html"; break;
                case "asc": type = "text/plain"; break;
                case "txt": type = "text/plain"; break;
                case "rtx": type = "text/richtext"; break;
                case "rtf": type = "text/rtf"; break;
                case "sgml": type = "text/sgml"; break;
                case "sgm": type = "text/sgml"; break;
                case "tsv": type = "text/tab-separated-values"; break;
                case "wml": type = "text/vnd.wap.wml"; break;
                case "wmls": type = "text/vnd.wap.wmlscript"; break;
                case "etx": type = "text/x-setext"; break;
                case "xsl": type = "text/xml"; break;
                case "xml": type = "text/xml"; break;
                case "mpeg": type = "video/mpeg"; break;
                case "mpg": type = "video/mpeg"; break;
                case "mpe": type = "video/mpeg"; break;
                case "qt": type = "video/quicktime"; break;
                case "mov": type = "video/quicktime"; break;
                case "mxu": type = "video/vnd.mpegurl"; break;
                case "avi": type = "video/x-msvideo"; break;
                case "movie": type = "video/x-sgi-movie"; break;
                case "ice": type = "x-conference/x-cooltalkcase"; break;
                default: type = "application/octet-stream"; break;
            }
            return type;
        }


        public string UrlEncode(string url)
        {
            return HttpUtility.UrlEncode(url);
        }

        public string UrlDecode(string urlEncrypt)
        {
            return HttpUtility.UrlDecode(urlEncrypt);
        }
    }
}