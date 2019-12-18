using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;
using WHC.MVCWebMis.Entity;
using WHC.Security.BLL;
using WHC.Security.Entity;

namespace WHC.MVCWebMis.Controllers
{
    /// <summary>
    /// 角色业务操作控制器
    /// </summary>
    public class RoleController : BusinessController<Role, RoleInfo>
    {       
        public RoleController() : base()
        {
        }

        /// <summary>
        /// 获取角色分类：系统角色、业务角色、应用角色...
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRoleCategorys()
        {
            List<CListItem> listItem = new List<CListItem>();
            string[] enumNames = EnumHelper.GetMemberNames<RoleCategoryEnum>();

            foreach (string item in enumNames)
            {
                listItem.Add(new CListItem(item));
            }
            return Json(listItem, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditUsers(string roleId, string newList)
        {
            if (!string.IsNullOrEmpty(roleId) && ValidateUtil.IsValidInt(roleId))
            {
                if (!string.IsNullOrWhiteSpace(newList))
                {
                    List<int> list = new List<int>();
                    foreach (string id in newList.Split(','))
                    {
                        list.Add(id.ToInt32());
                    }

                    BLLFactory<Role>.Instance.EditRoleUsers(roleId.ToInt32(), list);
                    return Content("true");
                }
            }
            return Content("");
        }

        public ActionResult EditOUs(string roleId, string newList)
        {
            if (!string.IsNullOrEmpty(roleId) && ValidateUtil.IsValidInt(roleId))
            {
                if (!string.IsNullOrWhiteSpace(newList))
                {
                    List<int> list = new List<int>();
                    foreach (string id in newList.Split(','))
                    {
                        list.Add(id.ToInt32());
                    }

                    BLLFactory<Role>.Instance.EditRoleOUs(roleId.ToInt32(), list);
                    return Content("true");
                }
            }
            return Content("");
        }
        public ActionResult EditFunctions(string roleId, string newList)
        {
            if (!string.IsNullOrEmpty(roleId) && ValidateUtil.IsValidInt(roleId))
            {
                if (!string.IsNullOrWhiteSpace(newList))
                {
                    List<string> list = new List<string>();
                    foreach (string id in newList.Split(','))
                    {
                        list.Add(id);
                    }

                    BLLFactory<Role>.Instance.EditRoleFunctions(roleId.ToInt32(), list);
                    return Content("true");
                }
            }
            return Content("");
        }

        public ActionResult Sys_OU_Menu_Save(string roleId, string newList)
        {
            CommonResult result = new CommonResult();
            if (!string.IsNullOrEmpty(roleId) && ValidateUtil.IsValidInt(roleId))
            {
                if (!string.IsNullOrWhiteSpace(newList))
                {
                    List<string> list = new List<string>();
                    foreach (string id in newList.Split(','))
                    {
                        list.Add(id);
                    }
                    var flag = new WaterFeeWeb.ServiceReference1.AuthorityClient().Sys_OU_Menu_Save(roleId.ToInt32(),list.ToArray());
                    if (flag == "0")                       
                    {
                        result.Data1 = roleId;
                        result.Success = true;
                    }
                    else {
                        result.ErrorMessage = flag;
                    }        
                }
            }
            return ToJsonContent(result);
        }

        public ActionResult EditUserRelation(string roleId, string addList, string removeList)
        {
            if (!string.IsNullOrEmpty(roleId) && ValidateUtil.IsValidInt(roleId))
            {
                if (!string.IsNullOrWhiteSpace(removeList))
                {
                    foreach (string id in removeList.Split(','))
                    {
                        if (!string.IsNullOrEmpty(id) && ValidateUtil.IsValidInt(id))
                        {
                            BLLFactory<Role>.Instance.RemoveUser(Convert.ToInt32(id), Convert.ToInt32(roleId));
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(addList))
                {
                    foreach (string id in addList.Split(','))
                    {
                        if (!string.IsNullOrEmpty(id) && ValidateUtil.IsValidInt(id))
                        {
                            BLLFactory<Role>.Instance.AddUser(Convert.ToInt32(id), Convert.ToInt32(roleId));
                        }
                    }
                }

                return Content("true");
            }
            return Content("");
        }

        public ActionResult EditOURelation(string roleId, string addList, string removeList)
        {
            if (!string.IsNullOrEmpty(roleId) && ValidateUtil.IsValidInt(roleId))
            {
                if (!string.IsNullOrWhiteSpace(removeList))
                {
                    foreach (string id in removeList.Split(','))
                    {
                        if (!string.IsNullOrEmpty(id) && ValidateUtil.IsValidInt(id))
                        {
                            BLLFactory<Role>.Instance.RemoveOU(Convert.ToInt32(id), Convert.ToInt32(roleId));
                        }
                    }
                }
                if (!string.IsNullOrWhiteSpace(addList))
                {
                    foreach (string id in addList.Split(','))
                    {
                        if (!string.IsNullOrEmpty(id) && ValidateUtil.IsValidInt(id))
                        {
                            BLLFactory<Role>.Instance.AddOU(Convert.ToInt32(id), Convert.ToInt32(roleId));
                        }
                    }
                }
                return Content("true");
            }
            return Content("");
        }

        public ActionResult EditFunctionRelation(string roleId, string addList, string removeList)
        {
            if (!string.IsNullOrEmpty(roleId) && ValidateUtil.IsValidInt(roleId))
            {
                if (!string.IsNullOrWhiteSpace(removeList))
                {
                    foreach (string id in removeList.Split(','))
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            BLLFactory<Role>.Instance.RemoveFunction(id, Convert.ToInt32(roleId));
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(addList))
                {
                    foreach (string id in addList.Split(','))
                    {
                        if (!string.IsNullOrEmpty(id))
                        {
                            BLLFactory<Role>.Instance.AddFunction(id, Convert.ToInt32(roleId));
                        }
                    }
                }
                return Content("true");
            }
            return Content("");
        }

        public ActionResult GetRolesByUser(string userid)
        {
            if (!string.IsNullOrEmpty(userid) && ValidateUtil.IsValidInt(userid))
            {
                List<RoleInfo> roleList = BLLFactory<Role>.Instance.GetRolesByUser(Convert.ToInt32(userid));
                return Json(roleList, JsonRequestBehavior.AllowGet);
            }
            return Content("");
        }

        public ActionResult GetRolesByFunction(string functionId)
        {
            if (!string.IsNullOrEmpty(functionId))
            {
                List<RoleInfo> roleList = BLLFactory<Role>.Instance.GetRolesByFunction(functionId);
                return Json(roleList, JsonRequestBehavior.AllowGet);
            }
            return Content("");
        }

        public ActionResult GetRolesByOU(string ouid)
        {
            if (!string.IsNullOrEmpty(ouid) && ValidateUtil.IsValidInt(ouid))
            {
                List<RoleInfo> roleList = BLLFactory<Role>.Instance.GetRolesByOU(Convert.ToInt32(ouid));
                return Json(roleList, JsonRequestBehavior.AllowGet);
            }
            return Content("");
        }

        /// <summary>
        /// 新增和编辑同时需要修改的内容
        /// </summary>
        /// <param name="info"></param>
        private void SetCommonInfo(RoleInfo info)
        {
            info.Editor = CurrentUser.FullName;
            info.Editor_ID = CurrentUser.ID.ToString();
            info.EditTime = DateTime.Now;

            OUInfo companyInfo = BLLFactory<OU>.Instance.FindByID(info.Company_ID);
            if (companyInfo != null)
            {
                info.CompanyName = companyInfo.Name;
            }
        }
       
        /// <summary>
        /// 重写方便写入公司、部门、编辑时间的名称等信息
        /// </summary>
        /// <param name="id">对象ID</param>
        /// <param name="info">对象信息</param>
        /// <returns></returns>
        protected override bool Update(string id, RoleInfo info)
        {
            string filter = string.Format("Name='{0}' and ID <>'{1}' and Company_ID={2}", info.Name, info.ID, info.Company_ID);
            bool isExist = BLLFactory<Role>.Instance.IsExistRecord(filter);
            if (isExist)
            {
                throw new ArgumentException("指定角色名称重复，请重新输入！");
            }

            SetCommonInfo(info);

            return base.Update(id, info);
        }

        /// <summary>
        /// 获取用户的部门角色树结构(分级需要）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public ActionResult GetMyRoleTreeJson(int userId)
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
                        EasyTreeData topnode = new EasyTreeData("dept" + groupInfo.ID, groupInfo.Name, GetIconcls(groupInfo.Category));
                        AddRole(groupInfo, topnode);

                        if (groupInfo.Category == "集团")
                        {
                            List<OUInfo> sublist = BLLFactory<OU>.Instance.GetAllCompany(groupInfo.ID);
                            foreach (OUInfo info in sublist)
                            {
                                if (!info.Deleted)
                                {
                                    EasyTreeData companyNode = new EasyTreeData("dept" + info.ID, info.Name, GetIconcls(info.Category));
                                    topnode.children.Add(companyNode);

                                    AddRole(info, companyNode);
                                }
                            }
                        }

                        content.Append(base.ToJson(topnode));
                    }
                }
            }

            string json = string.Format("[{0}]", content.ToString().Trim(','));
            return Content(json);
        }
        /// <summary>
        /// 系统-角色-查询树
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMyRoleTreeJson_Server() {
            var iUserId = Session["UserID"].ToString().ToInt();
            var listtree = new WaterFeeWeb.ServiceReference1.AuthorityClient().Sys_Role_GetTree(iUserId, 0);
            return ToJsonContent(listtree);
        }

        /// <summary>
        /// 系统-角色-通过ID查角色、角色菜单及角色用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Sys_Role_GetRoleUserByID_Server(int id)
        {
            CommonResult result = new CommonResult();
            if (id > 9000)
            {
                id -= 9000;
            }
           
            var rs = new WaterFeeWeb.ServiceReference1.AuthorityClient().Sys_Role_GetRoleMenuUserByID(id);
            if (rs.IsSuccess)
            {
                result.Success = true;
                result.Data1 = rs.StrData1;
                result.Data2 = rs.StrData2;
                result.Data3 = rs.StrData3;
            }      
            else {
                result.ErrorMessage = rs.ErrorMsg;
            }
            return ToJsonContent(result);
        }

        /// <summary>
        /// 系统-角色-操作
        /// </summary>
        /// <param name="RoleInfo"></param>
        /// <param name="type">0 添加 1 修改</param>
        /// <returns></returns>
        public ActionResult Sys_Role_Opr_Server(WaterFeeWeb.ServiceReference1.Role RoleInfo,String type,int id) {
            CommonResult result = new CommonResult();
            RoleInfo.NvcEditor = Session["FullName"].ToString();
            RoleInfo.NvcEditorID = Session["UserID"].ToString();
            RoleInfo.DtEdit = DateTime.Now;
            RoleInfo.IntID = id;
            if (type == "0") {
                RoleInfo.NvcCreator = Session["FullName"].ToString();
                RoleInfo.NvcCreatorID = Session["UserID"].ToString();
                RoleInfo.DtCreate = DateTime.Now;
            }
            var flag = new WaterFeeWeb.ServiceReference1.AuthorityClient().Sys_Role_Opr(RoleInfo);
            if (flag=="0") {
                result.Success = true;
            }
            return ToJsonContent(result);
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="RoleInfo"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Sys_Role_Del_Server(WaterFeeWeb.ServiceReference1.Role RoleInfo,int id)
        {
            CommonResult result = new CommonResult();
            RoleInfo.NvcEditor = Session["FullName"].ToString();
            RoleInfo.NvcEditorID = Session["UserID"].ToString();
            RoleInfo.DtEdit = DateTime.Now;
            RoleInfo.IntID = id;
            RoleInfo.IntDeleted = 1;

            var flag = new WaterFeeWeb.ServiceReference1.AuthorityClient().Sys_Role_Opr(RoleInfo);
            if (flag == "0")
            {
                result.Success = true;
            }
            return ToJsonContent(result);
        }
        /// <summary>
        /// 系统-角色-包含菜单保存
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="newList"></param>
        /// <returns></returns>
        public ActionResult Sys_Role_MenuSave_Server(string roleId, string newList)
        {
            CommonResult result = new CommonResult();
            if (!string.IsNullOrEmpty(roleId) && ValidateUtil.IsValidInt(roleId))
            {
                if (!string.IsNullOrWhiteSpace(newList))
                {
                    List<string> list = new List<string>();
                    foreach (string id in newList.Split(','))
                    {
                        list.Add(id);
                    }
                    var flag = new WaterFeeWeb.ServiceReference1.AuthorityClient().Sys_Role_MenuSave(roleId.ToInt32(), list.ToArray());
                    if (flag == "0")
                    {
                        result.Data1 = roleId;
                        result.Success = true;
                    }
                    else
                    {
                        result.ErrorMessage = flag;
                    }
                }
            }
            return ToJsonContent(result);
        }
        public ActionResult Sys_Role_UserSave_Server(string roleId, string newList)
        {
            CommonResult result = new CommonResult();
            List<int> list = new List<int>();
            if (!string.IsNullOrEmpty(roleId) && ValidateUtil.IsValidInt(roleId))
            {
                if (!string.IsNullOrWhiteSpace(newList))
                {
                    foreach (string id in newList.Split(','))
                    {
                        list.Add(id.ToInt());
                    }
                }
                var flag = new WaterFeeWeb.ServiceReference1.AuthorityClient().Sys_Role_UserSave(roleId.ToInt32(), list.ToArray());
                if (flag == "0")
                {
                    result.Success = true;
                }
                else
                {
                    result.ErrorMessage = flag;
                }
            }
            return ToJsonContent(result);
        }

        private void AddRole(OUInfo ouInfo, EasyTreeData treeNode)
        {
            List<RoleInfo> roleList = BLLFactory<Role>.Instance.GetRolesByCompany(ouInfo.ID.ToString());
            foreach (RoleInfo roleInfo in roleList)
            {
                EasyTreeData roleNode = new EasyTreeData("role" + roleInfo.ID, roleInfo.Name, "icon-group-key");
                treeNode.children.Add(roleNode);
            }
        }
    }
}
