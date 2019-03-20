using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;
using WHC.Security.BLL;
using WHC.Security.Entity;

namespace MVCWebApi.Controllers
{
    public class UserController : BaseApiController
    {
        /// <summary>
        /// Token超时的分钟数
        /// </summary>
        private const int ExpireMinutes = 60;

        /// <summary>
        /// 获取口令
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">用户密码</param>
        /// <param name="systemType">系统类型</param>
        /// <returns></returns>
        public HttpResponseMessage GetToken(string username, string password, string systemType)
        {
            //返回的Token有几个属性的键值组成：由用户名，密码Hash，系统类型，过期时间等组成
            string token = "";

            string ip2 = Request.GetClientIpAddress();

            string identity = BLLFactory<User>.Instance.VerifyUser(username, password ?? "", systemType);
            if (!string.IsNullOrEmpty(identity))
            {                
                UserInfo userInfo = BLLFactory<User>.Instance.GetUserByName(username);
                if (userInfo != null)
                {
                    List<string> list = new List<string>();
                    list.Add(userInfo.Name);
                    list.Add(userInfo.Password);
                    list.Add(systemType);
                    list.Add(DateTime.Now.AddMinutes(ExpireMinutes).ToString());

                    token = JsonConvert.SerializeObject(list, Formatting.None);//序列号为Json
                    token = WHC.Framework.Commons.EncodeHelper.EncryptString(token);//对字符串进行加密
                }
            }

            return Content(token);
        }

        /// <summary>
        /// 检查用户的访问令牌
        /// </summary>
        /// <param name="token">用户的访问令牌</param>
        /// <returns></returns>
        [HttpGet]
        public bool CheckToken(string token)
        {
            bool result = false;
            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    string newtoken = WHC.Framework.Commons.EncodeHelper.DecryptString(token, false);//对字符串进行解密
                    if (!string.IsNullOrEmpty(newtoken))
                    {
                        List<string> list = JsonConvert.DeserializeObject<List<string>>(newtoken);
                        if (list.Count == 4)
                        {
                            string username = list[0];
                            string password = list[1];
                            string systemType = list[2];
                            string expiredTime = list[3];

                            if (!string.IsNullOrEmpty(expiredTime) && expiredTime.ToDateTime().Subtract(DateTime.Now).TotalMinutes > 0)
                            {
                                string condition = string.Format("Name = '{0}' AND Password ='{1}' ", username, password);
                                result = BLLFactory<User>.Instance.IsExistRecord(condition);
                            }
                        }
                    }
                }
                catch //(Exception ex)
                {
                    //LogTextHelper.Error(ex);
                }
            }

            return result;
        }

        /// <summary>
        /// 对Token进行解密查看的操作
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public HttpResponseMessage GetDecodeToken(string token)
        {
            string result = "";

            string newtoken = WHC.Framework.Commons.EncodeHelper.DecryptString(token, false);//对字符串进行解密
            if (!string.IsNullOrEmpty(newtoken))
            {
                List<string> list = JsonConvert.DeserializeObject<List<string>>(newtoken);
                result = ToJson(list);
            }

            return Content(result);
        }

        /// <summary>
        /// 根据角色获取对应的用户
        /// </summary>
        /// <param name="roleid">角色ID</param>
        /// <returns></returns>
        public List<UserInfo> GetUsersByRole(string roleid)
        {
            List<UserInfo> result = new List<UserInfo>();
            if (!string.IsNullOrEmpty(roleid) && ValidateUtil.IsValidInt(roleid))
            {
                result = BLLFactory<User>.Instance.GetUsersByRole(Convert.ToInt32(roleid));
            }
            return result;
        }
    }
}
