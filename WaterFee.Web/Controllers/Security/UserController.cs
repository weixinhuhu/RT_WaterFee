using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.Mvc;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;
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

        public virtual ActionResult DeletedList()
        {
            return View("DeletedList");
        }
     
        /// <summary>
        /// 获取组织结构树
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ActionResult GetMyDeptTreeJson_Server(int iDeleted)
        {
            var userid = Session["UserId"].ToString().ToInt();
            var treelist = new WaterFeeWeb.ServiceReference1.AuthorityClient().Sys_OU_GetTree(userid, iDeleted, 1);
            return ToJsonContentDate(treelist);
        }

        /// <summary>
        /// 查询用户
        /// </summary>
        /// <returns></returns>
        public ActionResult Sys_User_Qry_Server()
        {
            var endcode = Session["EndCode"].ToString().ToInt();
            WaterFeeWeb.ServiceReference1.User userinfo = new WaterFeeWeb.ServiceReference1.User
            {
                NvcHandNo = Request["WHC_HandNo"] ?? "",
                NvcName = Request["WHC_Name"] ?? "",
                NvcFullName = Request["WHC_FullName"] ?? "",
                NvcDeptID = Request["WHC_DeptID"] ?? "",
            };
            var IntDeleted = Request["WHC_Deleted"] ?? "10";
            userinfo.IntDeleted = IntDeleted.ToInt();
            userinfo.NvcComID = Session["Company_ID"].ToString();
            
            var dts = new WaterFeeWeb.ServiceReference1.AuthorityClient().Sys_User_Qry(endcode, userinfo);
            int rows = Request["rows"] == null ? 10 : int.Parse(Request["rows"]);
            int page = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            DataTable dat = new DataTable();
            //复制源的架构和约束
            dat = dts.Clone();
            // 清除目标的所有数据
            dat.Clear();
            //对数据进行分页
            for (int i = (page - 1) * rows; i < page * rows && i < dts.Rows.Count; i++)
            {
                dat.ImportRow(dts.Rows[i]);
            }
            //最重要的是在后台取数据放在json中要添加个参数total来存放数据的总行数，如果没有这个参数则不能分页
            int total = dts.Rows.Count;
            var result = new { total, rows = dat };
            return ToJsonContentDate(result);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="userinfo"></param>
        /// <returns></returns>
        public ActionResult Sys_User_Ins_Server(WaterFeeWeb.ServiceReference1.User userinfo)
        {
            CommonResult result = new CommonResult();
            userinfo.NvcPassword = "12345678";
            userinfo.NvcCreator = Session["FullName"].ToString();
            userinfo.NvcCreatorID = Session["UserId"].ToString();
            userinfo.NvcEditor = Session["FullName"].ToString();
            userinfo.NvcCreatorID = Session["UserId"].ToString();
            userinfo.DtCreate = DateTime.Now;
            userinfo.DtEdit = DateTime.Now;
            userinfo.DtBirthday = DateTime.Now;
            var flag = new WaterFeeWeb.ServiceReference1.AuthorityClient().Sys_User_Opr(userinfo);
            if (flag == "0")
            {
                result.Success = true;
            }
            else
            {
                result.ErrorMessage = flag;
            };
            return ToJsonContentDate(result);
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="userinfo"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult Sys_User_Upd_Server(WaterFeeWeb.ServiceReference1.User userinfo, String ID)
        {
            CommonResult result = new CommonResult();
            userinfo.IntID = ID.ToInt();
            userinfo.NvcEditor = Session["FullName"].ToString();
            userinfo.NvcEditorID = Session["UserId"].ToString();
            userinfo.DtEdit = DateTime.Now;
            userinfo.DtBirthday = DateTime.Now;
            var flag = new WaterFeeWeb.ServiceReference1.AuthorityClient().Sys_User_Opr(userinfo);
            if (flag == "0")
            {
                result.Success = true;
            }
            else
            {
                result.ErrorMessage = flag;
            }
            return ToJsonContentDate(result);
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userinfo"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult Sys_User_UpdPwd_Server(WaterFeeWeb.ServiceReference1.User userinfo, String ID)
        {
            CommonResult result = new CommonResult();
            userinfo.IntID = ID.ToInt();
            userinfo.NvcPassword = "12345678";
            userinfo.NvcEditor = Session["FullName"].ToString();
            userinfo.NvcEditorID = Session["UserId"].ToString();
            userinfo.DtEdit = DateTime.Now;
            userinfo.DtBirthday = DateTime.Now;
            var flag = new WaterFeeWeb.ServiceReference1.AuthorityClient().Sys_User_Opr(userinfo);
            if (flag == "0")
            {
                result.Success = true;
            }
            else
            {
                result.ErrorMessage = flag;
            }
            return ToJsonContentDate(result);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <returns></returns>
        public ActionResult Sys_User_Del_Server()
        {
            CommonResult result = new CommonResult();
            var ids = Request["Ids"];
            var iMode = Request["Mode"].ToString().ToInt();
            var NvcEditor = Session["FullName"].ToString();
            var NvcEditorID = Session["UserId"].ToString();
            List<int> list = new List<int>();
            foreach (string id in ids.Split(','))
            {
                list.Add(id.ToInt());
            }
            var flag = new WaterFeeWeb.ServiceReference1.AuthorityClient().Sys_User_Del(iMode, list.ToArray(), NvcEditorID, NvcEditor);

            if (flag == "0")
            {
                result.Success = true;
            }
            else
            {
                result.ErrorMessage = flag;
            }

            return ToJsonContentDate(result);
        }

        /// <summary>
        /// 显示二、三级树
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMyDeptTreeLevel2Json_Server()
        {
            var iType = 2;
            if ((bool)Session["IsSuperAdmin"])
            {
                iType = 1;
            }
            var treelist = new WaterFeeWeb.ServiceReference1.AuthorityClient().Sys_OU_GetTree_Level(iType);
            return ToJsonContentDate(treelist);
        }
            
        public ActionResult RDLCReport()
        {
            return View("RDLCReport");
        }       
    }
}
