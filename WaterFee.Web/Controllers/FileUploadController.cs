using Aspose.Cells;
using Aspose.Slides;
using Aspose.Slides.Pptx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WHC.Attachment.BLL;
using WHC.Attachment.Entity;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;
using WHC.MVCWebMis.BLL;
using WHC.MVCWebMis.Entity;

namespace WHC.MVCWebMis.Controllers
{
    public class FileUploadController : BaseController
    {
        /// <summary>
        /// 上传附件到服务器上
        /// </summary>
        /// <param name="fileData">附件信息</param>
        /// <param name="guid">附件组GUID</param>
        /// <param name="folder">指定的上传目录</param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Upload(HttpPostedFileBase fileData, string guid, string folder)
        {
            CommonResult result = new CommonResult();
            if (fileData != null)
            {
                try
                {
                    ControllerContext.HttpContext.Request.ContentEncoding = Encoding.GetEncoding("UTF-8");
                    ControllerContext.HttpContext.Response.ContentEncoding = Encoding.GetEncoding("UTF-8");
                    ControllerContext.HttpContext.Response.Charset = "UTF-8";

                    // 文件上传后的保存路径
                    string filePath = Server.MapPath("~/UploadFiles/");
                    DirectoryUtil.AssertDirExist(filePath);

                    string fileName = Path.GetFileName(fileData.FileName);      //原始文件名称
                    string fileExtension = Path.GetExtension(fileName);         //文件扩展名
                    string saveName = Guid.NewGuid().ToString() + fileExtension; //保存文件名称

                    FileUploadInfo info = new FileUploadInfo();
                    info.FileData = ReadFileBytes(fileData);
                    if (info.FileData != null)
                    {
                        info.FileSize = info.FileData.Length;
                    }
                    info.Category = folder;
                    info.FileName = fileName;
                    info.FileExtend = fileExtension;
                    info.AttachmentGUID = guid;
                    info.AddTime = DateTime.Now;
                    info.Editor = CurrentUser.Name;//登录人
                    //info.Owner_ID = OwerId;//所属主表记录ID

                    result = BLLFactory<FileUpload>.Instance.Upload(info);
                    if (!result.Success)
                    {
                        LogTextHelper.Error("上传文件失败:" + result.ErrorMessage);
                    }
                }
                catch (Exception ex)
                {
                    result.ErrorMessage = ex.Message;
                    LogTextHelper.Error(ex);
                }
            }
            else
            {
                result.ErrorMessage = "fileData对象为空";
            }

            return ToJsonContent(result);
        }

        /// <summary>
        /// 删除单个附件信息
        /// </summary>
        /// <param name="id">附件的ID</param>
        /// <returns></returns>
        public ActionResult Delete(string id)
        {
            CommonResult result = new CommonResult();
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    result.Success = BLLFactory<FileUpload>.Instance.Delete(id);
                }
                catch(Exception ex)
                {
                    LogTextHelper.Error(ex);
                    result.ErrorMessage = ex.Message;
                }
            }
            return ToJsonContent(result);
        }
        
        /// <summary>
        /// 根据路径下载文件，主要用于生成的文件的下载
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public ActionResult DownloadFile(string file)
        {
            string realPath = Server.MapPath(file);
            string saveFileName = FileUtil.GetFileName(realPath);

            Response.WriteFile(realPath);
            Response.Charset = "GB2312";
            Response.ContentEncoding = Encoding.GetEncoding("GB2312");
            Response.ContentType = "application/ms-excel/msword";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(saveFileName));
            Response.Flush();
            Response.End();

            return new FileStreamResult(Response.OutputStream, "application/ms-excel/msword");
        }

        /// <summary>
        /// 获取附件并生成相关展示的html,添加可删除附件链接
        /// </summary>
        /// <param name="strGuid">附件组guid</param>
        /// <returns>string</returns>
        public ActionResult GetAttachmentHtml(string guid)
        {
            string html = @"<li>附件暂无</li>";

            if (string.IsNullOrEmpty(guid))
                return Content(html);

            int seq = 1;
            StringBuilder sb = new StringBuilder();

            List<FileUploadInfo> fileList = BLLFactory<FileUpload>.Instance.GetByAttachGUID(guid);
            if (fileList != null && fileList.Count > 0)
            {
                #region 构建附件展示的HTML代码
                foreach (FileUploadInfo info in fileList)
                {
                    string fileName = info.FileName.Trim();
                    fileName = System.Web.HttpContext.Current.Server.UrlEncode(fileName);

                    sb.Append("<tr>");
                    sb.AppendFormat("<td style='width:20px'> <img border='0' width='16px' height='16px' src='/Content/images/delete.gif' onclick=\"deleteAttach('{0}')\"/> </td> ", info.ID);
                    sb.AppendFormat(@"<td style='width:300px'> <li><span>[ 附件{0} ]</span>", seq++);
                    sb.AppendFormat(@"<img border='0' width='16px' height='16px' src='{0}' />", ConvertExtensionIcon(info.FileExtend.Trim('.')));
                    sb.AppendFormat(@"<a onclick=""ShowAttach('{0}', '{1}')"">&nbsp;{2}</a></li> </td> ", info.ID, info.FileExtend.Trim('.').ToLower(), info.FileName);
                    sb.Append("</tr>");
                }
                sb.Append("<tr><td colspan='2' style='height:20px'>&nbsp;</td></tr>");//增加一个空行
                sb.Append("<tr>");
                sb.AppendFormat(@"<td colspan='2'><a class='easyui-linkbutton' data-options=""iconCls: 'icon-save' "" 
                    onclick=""DownloadAttach('{0}')""><img border='0' width='16px' height='16px' src='/Content/images/attachtb.gif'/>全部打包加载</a></td>", guid);
                sb.Append("</tr>");

                string result = string.Format("<table style='border:0px solid #A8CFEB;'>{0}</table>", sb.ToString()); 
                #endregion

                return Content(result);
            }
            else
            {
                return Content(html);
            }
        }

        /// <summary>
        /// 获取附件的展示HTML代码(不含删除操作）
        /// </summary>
        /// <param name="guid">附件组GUID</param>
        /// <returns></returns>
        public ActionResult GetViewAttachmentHtml(string guid)
        {
            string html = @"<li>附件暂无</li>";

            if (string.IsNullOrEmpty(guid))
                return Content(html);

            StringBuilder sb = new StringBuilder();
            int seq = 1;
                        
            List<FileUploadInfo> fileList = BLLFactory<FileUpload>.Instance.GetByAttachGUID(guid);
            if (fileList != null && fileList.Count > 0)
            {
                foreach (FileUploadInfo info in fileList)
                {
                    string fileName = info.FileName.Trim();
                    fileName = System.Web.HttpContext.Current.Server.UrlEncode(fileName);

                    sb.Append("<tr>");
                    sb.AppendFormat(@"<td style='width:300px'> <li><span>[ 附件{0} ]</span>", seq++);
                    sb.AppendFormat(@"<img border='0' width='16px' height='16px' src='{0}' />", ConvertExtensionIcon(info.FileExtend.Trim('.')));
                    sb.AppendFormat(@"<a onclick=""ShowAttach('{0}', '{1}')"">&nbsp;{2}</a></li> </td> ", info.ID, info.FileExtend.Trim('.').ToLower(), info.FileName);
                    sb.Append("</tr>");
                }
                sb.Append("<tr><td colspan='2' style='height:20px'>&nbsp;</td></tr>");//增加一个空行
                sb.Append("<tr>");
                sb.AppendFormat(@"<td colspan='2'><a class='easyui-linkbutton' data-options=""iconCls: 'icon-save' "" 
                    onclick=""DownloadAttach('{0}')""><img border='0' width='16px' height='16px' src='/Content/images/attachtb.gif'/>全部打包加载</a></td>", guid);
                sb.Append("</tr>");

                string result = string.Format("<table style='border:0px solid #A8CFEB;'>{0}</table>", sb.ToString());
                return Content(result);
            }
            else
            {
                return Content(html);
            }
        }

        /// <summary>
        /// 根据附件ID，获取对应查看的视图URL。
        /// 一般规则如果是图片文件，返回视图URL地址'/FileUpload/ViewAttach'；
        /// 如果是Office文件（word、PPT、Excel）等，可以通过微软的在线查看地址进行查看：'http://view.officeapps.live.com/op/view.aspx?src='，
        /// 也可以进行本地生成HTML文件查看。如果是其他文件，可以直接下载地址。
        /// </summary>
        /// <param name="id">附件的ID</param>
        /// <returns></returns>
        public ActionResult GetAttachViewUrl(string id)
        {
            string viewUrl = "";
            FileUploadInfo info = BLLFactory<FileUpload>.Instance.FindByID(id);
            if (info != null)
            {
                string ext = info.FileExtend.Trim('.').ToLower();
                string filePath = GetFilePath(info);

                bool officeInternetView = false;//是否使用互联网在线预览
                string hostName = HttpUtility.UrlPathEncode("http://www.iqidi.com/");//可以配置一下，如果有必要

                if (ext == "xls" || ext == "xlsx" || ext == "doc" || ext == "docx" || ext == "ppt" || ext == "pptx")
                {
                    if (officeInternetView)
                    {
                        //返回一个微软在线浏览Office的地址，需要加上互联网域名或者公网IP地址
                        viewUrl = string.Format("http://view.officeapps.live.com/op/view.aspx?src={0}{1}", hostName, filePath);
                    }
                    else
                    {
                        #region 动态第一次生成文件
                        //检查本地Office文件是否存在，如不存在，先生成文件，然后返回路径供查看
                        string webPath = string.Format("/GenerateFiles/Office/{0}.htm", info.ID);
                        string generateFilePath = Server.MapPath(webPath);
                        if (!FileUtil.FileIsExist(generateFilePath))
                        {
                            string templateFile = BLLFactory<FileUpload>.Instance.GetFilePath(info);
                            templateFile = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, templateFile.Replace("\\", "/"));

                            if (ext == "doc" || ext == "docx")
                            {
                                Aspose.Words.Document doc = new Aspose.Words.Document(templateFile);
                                doc.Save(generateFilePath, Aspose.Words.SaveFormat.Html);
                            }
                            else if (ext == "xls" || ext == "xlsx")
                            {
                                Workbook workbook = new Workbook(templateFile);
                                workbook.Save(generateFilePath, SaveFormat.Html);
                            }
                            else if (ext == "ppt" || ext == "pptx")
                            {
                                templateFile = templateFile.Replace("/", "\\");
                                PresentationEx pres = new PresentationEx(templateFile);
                                pres.Save(generateFilePath, Aspose.Slides.Export.SaveFormat.Html);
                            }
                        }
                        #endregion
                        viewUrl = webPath;
                    }
                }
                else
                {
                    viewUrl = filePath;
                }
            }
            return Content(viewUrl);
        }

        /// <summary>
        /// 打包下载指定GUID的所有附件文件
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public ActionResult DownloadAttach(string guid)
        {
            List<FileUploadInfo> list = BLLFactory<FileUpload>.Instance.GetByAttachGUID(guid);
            List<string> fileList = new List<string>();
            foreach(FileUploadInfo info in list)
            {
                string realFolderPath = "";
                string filePath = BLLFactory<FileUpload>.Instance.GetFilePath(info);
                if (!Path.IsPathRooted(filePath))
                {
                    realFolderPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, filePath);
                }
                fileList.Add(realFolderPath);
            }

            ZipUtility.ZipFiles(fileList, Response.OutputStream, "");

            string saveFileName = "AttachPackage.zip";
            Response.Charset = "GB2312";
            Response.ContentEncoding = Encoding.GetEncoding("GB2312");
            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(saveFileName));
            Response.Flush();
            Response.End();

            return new FileStreamResult(Response.OutputStream, "application/octet-stream");
        }


        #region Private辅助函数
        /// <summary>
        /// 根据附件对象信息获取文件路径
        /// </summary>
        /// <param name="info">附件对象信息</param>
        /// <returns></returns>
        private string GetFilePath(FileUploadInfo info)
        {
            string filePath = BLLFactory<FileUpload>.Instance.GetFilePath(info);
            filePath = filePath.Replace("\\", "/");
            return HttpUtility.UrlPathEncode(filePath);
        }

        /// <summary>
        /// 读取文件的字节
        /// </summary>
        /// <param name="fileData">附件信息</param>
        /// <returns></returns>
        private byte[] ReadFileBytes(HttpPostedFileBase fileData)
        {
            byte[] data;
            using (Stream inputStream = fileData.InputStream)
            {
                MemoryStream memoryStream = inputStream as MemoryStream;
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                    inputStream.CopyTo(memoryStream);
                }
                data = memoryStream.ToArray();
            }
            return data;
        }

        /// <summary>
        /// 根据后缀名获取对应的图片地址，方便显示图片
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        private string ConvertExtensionIcon(string extension)
        {
            //默认文件图标
            string result = "/Content/Themes/Default/file_extension/Extension.png";

            //构建文件名称的格式
            string format = "/Content/Themes/Default/file_extension/{0}.png";
            string webPath = string.Format(format, extension);
            string file = Server.MapPath(webPath);
            if (FileUtil.FileIsExist(file))
            {
                result = webPath;
            }
            else if (extension.Length > 3)
            {
                webPath = string.Format(format, extension.Substring(0, 3));
                file = Server.MapPath(webPath);
                if (FileUtil.FileIsExist(file))
                {
                    result = webPath;
                }
            }

            return result;
        } 
        #endregion

        public ActionResult ViewAttach()
        {
            return View("ViewAttach");
        }
    }
}
