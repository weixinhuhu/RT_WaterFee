using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WHC.Framework.Commons;
using WHC.Framework.Commons.Collections;
using WHC.Framework.ControlUtil;
using WHC.MVCWebMis.Entity;
using WHC.Security.BLL;
using WHC.Security.Common;
using WHC.Security.Entity;

namespace WHC.MVCWebMis.Controllers
{
    public class OUController : BusinessController<OU, OUInfo>
    {
        public OUController() : base()
        {
        }
        public ActionResult UserMenu()
        {        
            return View();
        }

        /// <summary>
        /// 获取组织机构的分类：集团、公司、部门、工作组
        /// </summary>
        /// <returns></returns>
        public ActionResult GetOUCategorys()
        {
            List<EasyTreeData> treeList = new List<EasyTreeData>();
            string[] enumNames = EnumHelper.GetMemberNames<OUCategoryEnum>();

            foreach (string item in enumNames)
            {
                treeList.Add(new EasyTreeData("", item));
            }
            return ToJsonContent(treeList);
        }

        public ActionResult GetListItems()
        {
            List<CListItem> listItem = new List<CListItem>();
            List<OUInfo> list = BLLFactory<OU>.Instance.GetAll();
            foreach (OUInfo info in list)
            {
                listItem.Add(new CListItem(info.Name, info.ID.ToString()));
            }
            return Json(listItem, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTreeList()
        {
            List<OUInfo> comboList = BLLFactory<OU>.Instance.GetAll();
            comboList = CollectionHelper<OUInfo>.Fill(-1, 0, comboList, "PID", "ID", "Name");
            return Json(comboList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditUserRelation(string ouid, string addList, string removeList)
        {
            CommonResult result = new CommonResult();
            if (!string.IsNullOrEmpty(ouid) && ValidateUtil.IsValidInt(ouid))
            {
                if (!string.IsNullOrWhiteSpace(removeList))
                {
                    foreach (string id in removeList.Split(','))
                    {
                        if (!string.IsNullOrEmpty(id) && ValidateUtil.IsValidInt(id))
                        {
                            BLLFactory<OU>.Instance.RemoveUser(Convert.ToInt32(id), Convert.ToInt32(ouid));
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(addList))
                {
                    foreach (string id in addList.Split(','))
                    {
                        if (!string.IsNullOrEmpty(id) && ValidateUtil.IsValidInt(id))
                        {
                            BLLFactory<OU>.Instance.AddUser(Convert.ToInt32(id), Convert.ToInt32(ouid));
                        }
                    }
                }

                result.Success = true;
            }
            else
            {
                result.ErrorMessage = "参数ouid不正确";
            }
            return ToJsonContent(result);
        }

        public ActionResult EditOuUsers(string ouid, string newList)
        {
            CommonResult result = new CommonResult();
            if (!string.IsNullOrEmpty(ouid) && ValidateUtil.IsValidInt(ouid))
            {
                if (!string.IsNullOrWhiteSpace(newList))
                {
                    List<int> list = new List<int>();
                    foreach (string id in newList.Split(','))
                    {
                        list.Add(id.ToInt32());
                    }

                    result.Success = BLLFactory<OU>.Instance.EditOuUsers(ouid.ToInt32(), list);
                }
            }
            else
            {
                result.ErrorMessage = "参数ouid不正确";
            }
            return ToJsonContent(result);
        }

        /// <summary>
        /// 根据角色获取对应的机构
        /// </summary>
        /// <param name="roleid">角色ID</param>
        /// <returns></returns>
        public ActionResult GetOUsByRole(string roleid)
        {
            if (!string.IsNullOrEmpty(roleid) && ValidateUtil.IsValidInt(roleid))
            {
                List<OUInfo> list = BLLFactory<OU>.Instance.GetOUsByRole(Convert.ToInt32(roleid));
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            return Content("");
        }

        /// <summary>
        /// 根据用户ID获取对应的机构关系
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        public ActionResult GetOUsByUser(string userid)
        {
            if (!string.IsNullOrEmpty(userid) && ValidateUtil.IsValidInt(userid))
            {
                List<OUInfo> ouList = BLLFactory<OU>.Instance.GetOUsByUser(Convert.ToInt32(userid));
                return Json(ouList, JsonRequestBehavior.AllowGet);
            }

            return Content("");
        }

        /// <summary>
        /// 新增和编辑同时需要修改的内容
        /// </summary>
        /// <param name="info"></param>
        private void SetCommonInfo(OUInfo info)
        {
            info.Editor = CurrentUser.FullName;
            info.Editor_ID = CurrentUser.ID.ToString();
            info.EditTime = DateTime.Now;

            OUInfo pInfo = BLLFactory<OU>.Instance.FindByID(info.PID);
            if (pInfo != null)
            {
                //pInfo.Category == "集团" ||
                if (pInfo.Category == "公司")
                {
                    info.Company_ID = pInfo.ID.ToString();
                    info.CompanyName = pInfo.Name;
                }
                else if (pInfo.Category == "部门" || pInfo.Category == "工作组")
                {
                    info.Company_ID = pInfo.Company_ID;
                    info.CompanyName = pInfo.CompanyName;
                }
            }
        }

        public ActionResult OU_Opr_Server(WaterFeeWeb.ServiceReference1.OU OrgInfo) {
            CommonResult result = new CommonResult();
            OrgInfo.IntEnabled = 1;
            OrgInfo.IntDeleted = 0;
            OrgInfo.DtEdit = DateTime.Now;
            OrgInfo.NvcCreator = Session["FullName"].ToString();
            OrgInfo.NvcCreatorID = Session["UserId"].ToString();
            var flag = new WaterFeeWeb.ServiceReference1.AuthorityClient().Sys_OU_Opr(OrgInfo);
            if (flag == "0")
            {
                result.Success = true;
            }
            else {
                result.Success = false;
                result.ErrorMessage = flag;
            }
            return ToJsonContent(result);
        }

        public ActionResult OU_Recover_Server(WaterFeeWeb.ServiceReference1.OU OrgInfo,String Pid)
        {
            CommonResult result = new CommonResult();
            OrgInfo.IntEnabled = 1;
            OrgInfo.IntDeleted = 0;
            OrgInfo.IntPID = Pid.ToInt();
            OrgInfo.DtEdit = DateTime.Now;
            OrgInfo.NvcCreator = Session["FullName"].ToString();
            OrgInfo.NvcCreatorID = Session["UserId"].ToString();
            var flag = new WaterFeeWeb.ServiceReference1.AuthorityClient().Sys_OU_Opr(OrgInfo);
            if (flag == "0")
            {
                result.Success = true;
            }
            else
            {
                result.Success = false;
                result.ErrorMessage = flag;
            }
            return ToJsonContent(result);
        }
        public ActionResult OU_Del_Server(String id)
        {
            CommonResult result = new CommonResult();
            WaterFeeWeb.ServiceReference1.OU OrgInfo = new WaterFeeWeb.ServiceReference1.OU();
            OrgInfo.IntDeleted = 1;
            OrgInfo.IntID = id.ToInt();
            OrgInfo.NvcCreator = Session["FullName"].ToString();
            OrgInfo.NvcCreatorID = Session["UserId"].ToString();
            var flag = new WaterFeeWeb.ServiceReference1.AuthorityClient().Sys_OU_Opr(OrgInfo);
            if (flag == "0")
            {
                result.Success = true;
            }
            else
            {
                result.Success = false;
                result.ErrorMessage = flag;
            }
            return ToJsonContent(result);
        }
   
        public ActionResult OU_FindById_Server()
        {
            CommonResult result = new CommonResult();
            var id = Request["ID"].ToInt();
            var dt = new WaterFeeWeb.ServiceReference1.AuthorityClient().Sys_Ou_GetByID(id);        
            return ToJsonContent(dt);
        }

        public override ActionResult Insert(OUInfo info)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.InsertKey);

            CommonResult result = new CommonResult();
            info.Company_ID = Session["Company_ID"].ToString();
            if (info != null)
            {
                try
                {
                    string filter = string.Format("Name='{0}' and Company_ID={1}", info.Name, info.Company_ID);
                    bool isExist = BLLFactory<OU>.Instance.IsExistRecord(filter);
                    if (isExist)
                    {
                        result.ErrorMessage = "指定机构名称重复，请重新输入！";
                    }
                    else
                    {
                        info.CreateTime = DateTime.Now;
                        info.Creator = CurrentUser.FullName;
                        info.Creator_ID = CurrentUser.ID.ToString();
                        SetCommonInfo(info);

                        result.Success = baseBLL.Insert(info);
                    }
                }
                catch (Exception ex)
                {
                    LogTextHelper.Error(ex);//错误记录
                    result.ErrorMessage = ex.Message;
                }
            }
            return ToJsonContent(result);
        }

        public override ActionResult Insert2(OUInfo info)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.InsertKey);

            int result = -1;
            if (info != null)
            {
                string filter = string.Format("Name='{0}' and Company_ID={1}", info.Name, info.Company_ID);
                bool isExist = BLLFactory<OU>.Instance.IsExistRecord(filter);
                if (isExist)
                {
                    throw new ArgumentException("指定机构名称重复，请重新输入！");
                }

                info.CreateTime = DateTime.Now;
                info.Creator = CurrentUser.FullName;
                info.Creator_ID = CurrentUser.ID.ToString();
                SetCommonInfo(info);

                result = baseBLL.Insert2(info);
            }
            return Content(result);
        }

        /// <summary>
        /// 重写方便写入公司、部门、编辑时间的名称等信息
        /// </summary>
        /// <param name="id">对象ID</param>
        /// <param name="info">对象信息</param>
        /// <returns></returns>
        protected override bool Update(string id, OUInfo info)
        {
            info.Company_ID = Session["Company_ID"].ToString();
            string filter = string.Format("Name='{0}' and ID <>{1} and Company_ID={2}", info.Name, info.ID, info.Company_ID);
            bool isExist = BLLFactory<OU>.Instance.IsExistRecord(filter);
            if (isExist)
            {
                throw new ArgumentException("指定机构名称重复，请重新输入！");
            }
            SetCommonInfo(info);
            return base.Update(id, info);
        }

        public ActionResult GetTreeJson()
        {
            string folder = iconBasePath + "organ.png";
            string leaf = iconBasePath + "organ.png";
            string json = GetTreeJson(-1, folder, leaf);
            json = json.Trim(',');
            return Content(string.Format("[{0}]", json));
        }

        /// <summary>
        /// 递归获取树形信息
        /// </summary>
        /// <param name="PID"></param>
        /// <returns></returns>
        private string GetTreeJson(int PID, string folderIcon, string leafIcon)
        {
            string condition = string.Format("PID={0}", PID);
            List<OUInfo> nodeList = BLLFactory<OU>.Instance.Find(condition);
            StringBuilder content = new StringBuilder();
            foreach (OUInfo model in nodeList)
            {
                int ParentID = (model.PID == -1 ? 0 : model.PID);
                string subMenu = this.GetTreeJson(model.ID, folderIcon, leafIcon);
                string parentMenu = string.Format("{{ \"id\":{0}, \"pId\":{1}, \"name\":\"{2}\" ", model.ID, ParentID, model.Name);
                if (string.IsNullOrEmpty(subMenu))
                {
                    if (!string.IsNullOrEmpty(leafIcon))
                    {
                        parentMenu += string.Format(",\"icon\":\"{0}\" }},", leafIcon);
                    }
                    else
                    {
                        parentMenu += "},";
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(folderIcon))
                    {
                        parentMenu += string.Format(",\"icon\":\"{0}\" }},", folderIcon);
                    }
                    else
                    {
                        parentMenu += "},";
                    }
                }

                content.AppendLine(parentMenu.Trim());
                content.AppendLine(subMenu.Trim());
            }

            return content.ToString().Trim();
        }

        /// <summary>
        /// 获取公司部门的数量，返回Json字符串，供图表统计
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCompanyDeptCountJson()
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();

            List<OUInfo> ouList = BLLFactory<OU>.Instance.GetTopGroup();
            foreach (OUInfo info in ouList)
            {
                List<OUInfo> companyList = BLLFactory<OU>.Instance.GetAllCompany(info.ID);
                foreach (OUInfo companyInfo in companyList)
                {
                    string condition = string.Format("Company_ID={0} AND Deleted=0", companyInfo.ID);
                    int count = BLLFactory<OU>.Instance.GetRecordCount(condition);
                    if (!dict.ContainsKey(companyInfo.Name))
                    {
                        dict.Add(companyInfo.Name, count);
                    }
                }
            }

            return ToJsonContent(dict);
        }

    }
}
