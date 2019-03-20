using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using System.Web.Http;
using WHC.Framework.ControlUtil;
using WHC.Security.BLL;
using WHC.Security.Entity;

namespace MVCWebApi.Controllers
{
    public class BaseApiController : ApiController
    {

        #region 辅助方法
        /// <summary>
        /// 把字符串转换为Json格式
        /// </summary>
        /// <param name="content">字符串内容</param>
        /// <returns></returns>
        protected HttpResponseMessage Content(string content)
        {
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new StringContent(content); //new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(result)));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return response;
        }

        /// <summary>
        /// 把对象为json字符串
        /// </summary>
        /// <param name="obj">待序列号对象</param>
        /// <returns></returns>
        protected string ToJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        } 
        #endregion
    }
}
