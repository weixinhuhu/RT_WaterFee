using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace DBLib.Net
{
    public class NetHelper
    {
        /// <summary>
        /// 根据url获取页面内容
        /// </summary>
        /// <param name="url">要获取的url</param>
        /// <param name="timeout">超时时间(毫秒),默认为1分钟=60,000毫秒</param>
        /// <returns></returns>
        public static string GetContentByUrl(string url, int timeout = 60000)
        {
            string strResult = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //声明一个HttpWebRequest请求  
                request.Timeout = timeout;
                //设置连接超时时间  
                request.Headers.Set("Pragma", "no-cache");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream streamReceive = response.GetResponseStream();
                Encoding encoding = Encoding.GetEncoding("UTF-8");
                StreamReader streamReader = new StreamReader(streamReceive, encoding);
                strResult = streamReader.ReadToEnd();
                streamReader.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return strResult;
        }


        /// <summary>
        /// 以表单形式,POST请求
        /// </summary>
        /// <param name="url">链接</param>
        /// <param name="data">要POST的数据</param>
        /// <param name="timeout">超时时间(毫秒),默认为1分钟=60,000毫秒</param>
        /// <returns></returns>
        public static string PostContentByUrl(string url, string data, int timeout = 60000)
        {
            string strResult = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                //声明一个HttpWebRequest请求  
                request.Timeout = timeout;
                //设置连接超时时间  
                //request.Headers.Set("Pragma", "no-cache");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                if (!string.IsNullOrWhiteSpace(data))
                {
                    byte[] postBytes = Encoding.GetEncoding("utf-8").GetBytes(data);
                    request.ContentLength = postBytes.Length;
                    Stream newStream = request.GetRequestStream();
                    newStream.Write(postBytes, 0, postBytes.Length);
                    newStream.Close();
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream streamReceive = response.GetResponseStream();
                Encoding encoding = Encoding.GetEncoding("UTF-8");
                StreamReader streamReader = new StreamReader(streamReceive, encoding);
                strResult = streamReader.ReadToEnd();
                streamReader.Close();
                response.Close();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return strResult;
        }
    }
}
