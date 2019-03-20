using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;
using WHC.MVCWebMis.Common;
using WHC.MVCWebMis.Entity;
using WHC.Security.BLL;
using WHC.Security.Entity;

namespace WHC.MVCWebMis.Controllers
{
    public class UserController : BusinessController<User, UserInfo>
    {
        public UserController() : base()
        {
        }

        #region 写入数据前修改部分属性
        protected override void OnBeforeInsert(UserInfo info)
        {
            //留给子类对参数对象进行修改
            info.CreateTime = DateTime.Now;
            info.Creator = CurrentUser.FullName;
            info.Creator_ID = CurrentUser.ID.ToString();

            info.Company_ID = CurrentUser.Company_ID;
            info.CompanyName = BLLFactory<OU>.Instance.GetName(CurrentUser.Company_ID.ToInt32());
            info.Dept_ID = CurrentUser.Dept_ID;
            info.DeptName = BLLFactory<OU>.Instance.GetName(CurrentUser.Dept_ID.ToInt32());
        }

        protected override void OnBeforeUpdate(UserInfo info)
        {
            //留给子类对参数对象进行修改
            info.Editor = CurrentUser.FullName;
            info.Editor_ID = CurrentUser.ID.ToString();
            info.EditTime = DateTime.Now;
        }
        #endregion

        public virtual ActionResult DeletedList()
        {
            return View("DeletedList");
        }

        /// <summary>
        /// 删除多个ID的记录(彻底删除)
        /// </summary>
        /// <param name="ids">多个id组合，逗号分开（1,2,3,4,5）</param>
        /// <returns></returns>
        public virtual ActionResult ConfirmDeleteByIds(string ids)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.DeleteKey);

            CommonResult result = new CommonResult();
            try
            {
                if (!string.IsNullOrEmpty(ids))
                {
                    string condition = string.Format("ID in ({0}) ", ids);
                    result.Success = baseBLL.DeleteByCondition(condition);
                }
            }
            catch (Exception ex)
            {
                LogTextHelper.Error(ex);//错误记录
                result.ErrorMessage = ex.Message;
            }
            return ToJsonContent(result);
        } 

        /// <summary>
        /// 重置用户密码为12345678
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        public ActionResult ResetPassword(string id)
        {
            CommonResult result = new CommonResult();
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    result.ErrorMessage = "用户id不能为空";
                }
                else
                {
                    UserInfo info = BLLFactory<User>.Instance.FindByID(id);
                    if (info != null)
                    {
                        string defaultPassword = "12345678";
                        bool tempBool = BLLFactory<User>.Instance.ModifyPassword(info.Name, defaultPassword);
                        if (tempBool)
                        {
                            result.Success = true;
                        }
                        else
                        {
                            result.ErrorMessage = "口令初始化失败";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogTextHelper.Error(ex);//错误记录
                result.ErrorMessage = ex.Message;
            }
            return ToJsonContent(result);
        }

        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="name">用户名称</param>
        /// <param name="oldpass">旧密码</param>
        /// <param name="newpass">修改密码</param>
        /// <returns></returns>
        public ActionResult ModifyPass(string name, string oldpass, string newpass)
        {
            CommonResult result = new CommonResult();
            try
            {
                string identity = BLLFactory<User>.Instance.VerifyUser(name, oldpass, "WareMis");
                if (string.IsNullOrEmpty(identity))
                {
                    result.ErrorMessage = "原口令错误";
                }
                else
                {
                    bool tempBool = BLLFactory<User>.Instance.ModifyPassword(name, newpass);
                    if (tempBool)
                    {
                        result.Success = true;
                    }
                    else
                    {
                        result.ErrorMessage = "口令修改失败";
                    }
                }
            }
            catch (Exception ex)
            {
                LogTextHelper.Error(ex);//错误记录
                result.ErrorMessage = ex.Message;
            }
            return ToJsonContent(result);
        }

        /// <summary>
        /// 获取用户的部门树结构(分级需要）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public ActionResult GetMyDeptTreeJson(int userId)
        {
            StringBuilder content = new StringBuilder();
            UserInfo userInfo = BLLFactory<User>.Instance.FindByID(userId);
            if (userInfo != null)
            {
                 List<OUInfo> list = BLLFactory<OU>.Instance.GetMyTopGroup(CurrentUser.ID);
                 foreach (OUInfo groupInfo in list)
                 {
                     if (groupInfo != null && !groupInfo.Deleted)
                     {
                         List<OUNodeInfo> sublist = BLLFactory<OU>.Instance.GetTreeByID(groupInfo.ID);

                         EasyTreeData treeData = new EasyTreeData(groupInfo.ID, groupInfo.Name, GetIconcls(groupInfo.Category));
                         GetTreeDataWithOUNode(sublist, treeData);

                         content.Append(base.ToJson(treeData));
                     }
                 }
            }
            string json = string.Format("[{0}]", content.ToString().Trim(','));
            return Content(json);
        }

        /// <summary>
        /// 获取用户的公司结构(分级需要）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public ActionResult GetMyCompanyTreeJson(int userId)
        {
            List<EasyTreeData> treeList = new List<EasyTreeData>();

            UserInfo userInfo = BLLFactory<User>.Instance.FindByID(userId);
            if (userInfo != null)
            {
                List<OUNodeInfo> list = new List<OUNodeInfo>();
                if (BLLFactory<User>.Instance.UserInRole(userInfo.Name, RoleInfo.SuperAdminName))
                {
                    list = BLLFactory<OU>.Instance.GetGroupCompanyTree();
                }
                else
                {
                    OUInfo myCompanyInfo = BLLFactory<OU>.Instance.FindByID(userInfo.Company_ID);
                    if (myCompanyInfo != null)
                    {
                        list.Add(new OUNodeInfo(myCompanyInfo));
                    }
                }

                if (list.Count > 0)
                {
                    OUNodeInfo info = list[0];//无论是集团还是公司，节点只有一个
                    EasyTreeData node = new EasyTreeData(info.ID, info.Name, GetIconcls(info.Category));
                    GetTreeDataWithOUNode(info.Children, node);
                    treeList.Add(node);
                }
            }

            return ToJsonContent(treeList);
        }

        private void GetTreeDataWithOUNode(List<OUNodeInfo> list, EasyTreeData parent)
        {
            List<EasyTreeData> result = new List<EasyTreeData>();
            foreach (OUNodeInfo ouInfo in list)
            {
                EasyTreeData treeData = new EasyTreeData(ouInfo.ID, ouInfo.Name, GetIconcls(ouInfo.Category));
                GetTreeDataWithOUNode(ouInfo.Children, treeData);

                result.Add(treeData);
            }

            parent.children.AddRange(result);
        }

        /// <summary>
        /// 根据公司ID获取对应部门的树Json
        /// </summary>
        /// <param name="parentId">父部门ID</param>
        /// <returns></returns>
        public ActionResult GetDeptTreeJson(string parentId)
        {
            List<EasyTreeData> treeList = new List<EasyTreeData>();
            treeList.Insert(0, new EasyTreeData(-1, "无"));

            if (!string.IsNullOrEmpty(parentId) && parentId != "null")
            {
                OUInfo groupInfo = BLLFactory<OU>.Instance.FindByID(parentId);
                if (groupInfo != null)
                {
                    List<OUNodeInfo> list = BLLFactory<OU>.Instance.GetTreeByID(groupInfo.ID);

                    EasyTreeData treeData = new EasyTreeData(groupInfo.ID, groupInfo.Name, "icon-group");
                    GetTreeDataWithOUNode(list, treeData);

                    treeList.Add(treeData);
                }
            }

            return ToJsonContent(treeList);
        }
                
        /// <summary>
        /// 根据用户获取对应人员层次的树Json
        /// </summary>
        /// <param name="deptId">用户所在部门</param>
        /// <returns></returns>
        public ActionResult GetUserTreeJson(int deptId)
        {
            List<EasyTreeData> treeList = new List<EasyTreeData>();
            treeList.Insert(0, new EasyTreeData(-1, "无"));

            List<UserInfo> list = BLLFactory<User>.Instance.FindByDept(deptId);
            foreach (UserInfo info in list)
            {
                treeList.Add(new EasyTreeData(info.ID, info.FullName, "icon-user"));
            }

            return ToJsonContent(treeList);
        }

        /// <summary>
        /// 根据角色获取对应的用户
        /// </summary>
        /// <param name="roleid">角色ID</param>
        /// <returns></returns>
        public ActionResult GetUsersByRole(string roleid)
        {
            ActionResult result = Content("");
            if (!string.IsNullOrEmpty(roleid) && ValidateUtil.IsValidInt(roleid))
            {
                List<UserInfo> roleList = BLLFactory<User>.Instance.GetUsersByRole(Convert.ToInt32(roleid));
                result = ToJsonContentDate(roleList);
            }
            return result;
        }

        /// <summary>
        /// 根据机构获取对应的用户
        /// </summary>
        /// <param name="ouid">机构ID</param>
        /// <returns></returns>
        public ActionResult GetUsersByOU(string ouid)
        {
            ActionResult result = Content("");
            if (!string.IsNullOrEmpty(ouid) && ValidateUtil.IsValidInt(ouid))
            {
                List<UserInfo> roleList = BLLFactory<User>.Instance.GetUsersByOU(Convert.ToInt32(ouid));
                result = ToJsonContentDate(roleList);
            }
            return result;
        }

        /// <summary>
        /// 获取分页操作的查询条件
        /// </summary>
        /// <returns></returns>
        protected override string GetPagerCondition()
        {
            string condition = "";
            //增加对角色、部门、公司的判断
            string deptId = Request["Dept_ID"] ?? "";    

            if (!string.IsNullOrEmpty(deptId))
            {
                condition = string.Format("Dept_ID = {0} or Company_ID ={0}", deptId);
            }
            else
            {
                condition = base.GetPagerCondition();
            }

            return condition;
        }

        /// <summary>
        /// 重写分页操作，对特殊条件进行处理
        /// </summary>
        /// <returns></returns>
        public override ActionResult FindWithPager()
        {
            string roleId = Request["Role_ID"] ?? "";
            if (!string.IsNullOrEmpty(roleId))
            {
                //检查用户是否有权限，否则抛出MyDenyAccessException异常
                base.CheckAuthorized(AuthorizeKey.ListKey);
                List<UserInfo> list = BLLFactory<User>.Instance.GetUsersByRole(roleId.ToInt32());

                //Json格式的要求{total:22,rows:{}}
                //构造成Json的格式传递
                var result = new { total = list.Count, rows = list };
                return ToJsonContentDate(result);
            }
            else
            {
                return base.FindWithPager();
            }
        }

        public override ActionResult Insert(UserInfo info)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.InsertKey);

            CommonResult result = new CommonResult();
            try
            {
                string filter = string.Format("Name='{0}' ", info.Name);
                bool isExist = BLLFactory<User>.Instance.IsExistRecord(filter);
                if (isExist)
                {
                    result.ErrorMessage = "指定用户名重复，请重新输入！";
                }
                else
                { 
                    info = ReflectionHelper.ReplacePropertyValue(info, typeof(string), null, string.Empty);
                    result.Success = baseBLL.Insert(info);
                }
            }
            catch (Exception ex)
            {
                LogTextHelper.Error(ex);//错误记录
                result.ErrorMessage = ex.Message;
            }
            return ToJsonContent(result);            
        }

        public override ActionResult Insert2(UserInfo info)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.InsertKey);

            string filter = string.Format("Name='{0}' ", info.Name);
            bool isExist = BLLFactory<User>.Instance.IsExistRecord(filter);
            if (isExist)
            {
                throw new ArgumentException("指定用户名重复，请重新输入！");
            }

            return base.Insert2(info);
        }

        /// <summary>
        /// 重写方便写入公司、部门、编辑时间的名称等信息
        /// </summary>
        /// <param name="id">对象主键ID</param>
        /// <param name="info">对象信息</param>
        /// <returns></returns>
        protected override bool Update(string id, UserInfo info)
        {
            string filter = string.Format("Name='{0}' and ID <>'{1}'", info.Name, info.ID);
            bool isExist = BLLFactory<User>.Instance.IsExistRecord(filter);
            if (isExist)
            {
                throw new ArgumentException("指定用户名重复，请重新输入！");
            }

            return base.Update(id, info);
        }

        public ActionResult ChartIndex()
        {
            return View("ChartIndex");
        }

        /// <summary>
        /// 统计各个分子公司的人数，返回Json字符串，供图表统计
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCompanyUserCountJson()
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();

            List<OUInfo> ouList = BLLFactory<OU>.Instance.GetTopGroup();
            foreach (OUInfo info in ouList)
            {
                List<OUInfo> companyList = BLLFactory<OU>.Instance.GetAllCompany(info.ID);
                foreach (OUInfo companyInfo in companyList)
                {
                    string condition = string.Format("Company_ID={0} AND Deleted=0", companyInfo.ID);
                    int count = BLLFactory<User>.Instance.GetRecordCount(condition);
                    if (!dict.ContainsKey(companyInfo.Name))
                    {
                        dict.Add(companyInfo.Name, count);
                    }
                }
            }

            return ToJsonContent(dict);
        }

        /// <summary>
        /// 根据用户的ID，获取用户的全名，并放到缓存里面
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public ActionResult GetFullNameByID(string userId)
        {
            string result = "";
            if (!string.IsNullOrEmpty(userId))
            {
                System.Reflection.MethodBase method = System.Reflection.MethodBase.GetCurrentMethod();
                //string key = string.Format("{0}-{1}-{2}", method.DeclaringType.FullName, method.Name, userId);
                string key = string.Format("GetFullNameByID-{0}", userId);

                result = MemoryCacheHelper.GetCacheItem<string>(key,
                    delegate() { return BLLFactory<User>.Instance.GetFullNameByID(userId.ToInt32()); },
                    new TimeSpan(0, 30, 0));//30分钟过期
            }
            return ToJsonContent(result); ;
        }

        public ActionResult RDLCReport()
        {
            return View("RDLCReport");
        }

        /// <summary>
        /// 基于RDLC的报表数据操作
        /// </summary>
        /// <param name="format">图片格式</param>
        /// <returns></returns>
        public ActionResult UserRdlcReport(string format)
        {
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Report/WHC.UserReport.rdlc");
            var dt = baseBLL.GetAll();

            ReportDataSource reportDataSource = new ReportDataSource("DataSet1", dt);
            localReport.DataSources.Add(reportDataSource);

            if(string.IsNullOrEmpty(format))
            {
                format = "Image";
            }

            string reportType = format;
            string deviceType = (format.ToLower() == "image") ? "jpeg" : format;
            string mimeType;
            string encoding;
            string fileNameExtension;

            //The DeviceInfo settings should be changed based on the reportType
            //http://msdn2.microsoft.com/en-us/library/ms155397.aspx
            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + deviceType + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            //"  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>1in</MarginLeft>" +
            "  <MarginRight>1in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>";

            if(format.ToLower() == "image")
            {
                //deviceInfo += string.Format("<StartPage>{0}</StartPage>", 0);
                //deviceInfo += string.Format("<EndPage>{0}</EndPage>", int.MaxValue);
                double inchValue = (dt.Count / 37.0) * 11; 
                deviceInfo += string.Format("  <PageHeight>{0}in</PageHeight>", inchValue);
            }
            else
            {
                deviceInfo += "  <PageHeight>11in</PageHeight>";
            }

            deviceInfo += "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = localReport.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            return File(renderedBytes, (format.ToLower() == "image") ? "image/jpeg" : mimeType);
            //return new ReportsResult(renderedBytes, mimeType);

            //Response.AddHeader("content-disposition", "attachment; filename=NorthWindCustomers." + fileNameExtension);

            //return File(renderedBytes, "pdf");
            //return File(renderedBytes, "image/jpeg");
        }
    }
}
