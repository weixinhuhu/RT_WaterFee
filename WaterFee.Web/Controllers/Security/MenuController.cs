using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;
using WHC.Security.BLL;
using WHC.Security.Entity;

namespace WHC.MVCWebMis.Controllers
{
    public class MenuController : BusinessController<Menu, MenuInfo>
    {
        public ActionResult GetMenuData_Server()
        {
            var userid = Session["UserID"].ToString().ToInt();
            var SysTypeID = "WareMIS";
            var rs = new WaterFeeWeb.ServiceReference1.AuthorityClient().Sys_Login_GetMenuByUserID(userid, SysTypeID);
            return Content(rs);
        }

        /// <summary>
        /// 系统-菜单-增改
        /// </summary>
        /// <returns></returns>
        public ActionResult Sys_Menu_Opr_Server(WaterFeeWeb.ServiceReference1.Menu menuinfo)
        {
            CommonResult result = new CommonResult();
            menuinfo.NvcEditor = Session["FullName"].ToString();
            menuinfo.NvcEditorID = Session["UserId"].ToString();
            menuinfo.DtEdit = DateTime.Now;
            menuinfo.DtEdit = DateTime.Now;
            menuinfo.NvcSysTypeID = MyConstants.SystemType;
            //判断是添加还是修改
            if (menuinfo.NvcID == "" || menuinfo.NvcID == null)
            {
                menuinfo.NvcCreator = Session["FullName"].ToString();
                menuinfo.NvcCreatorID = Session["UserId"].ToString();
                menuinfo.DtCreate = DateTime.Now;
            }

            var rs = new WaterFeeWeb.ServiceReference1.AuthorityClient().Sys_Menu_Opr(menuinfo);
            if (rs == "0")
            {
                result.Success = true;

            }
            else
            {
                result.Success = false;
                result.ErrorMessage = rs;
            }
            return ToJsonContent(result);
        }

        /// <summary>
        /// 系统-菜单-删除
        /// </summary>
        /// <returns></returns>
        public ActionResult Sys_Menu_Del_Server(string Ids)
        {
            CommonResult result = new CommonResult();
            var arrIds = Ids.Split(',');
            var rs = new WaterFeeWeb.ServiceReference1.AuthorityClient().Sys_Menu_BatchDel(arrIds);
            if (rs == "0")
            {
                result.Success = true;

            }
            else
            {
                result.Success = false;
                result.ErrorMessage = rs;
            }
            return ToJsonContent(result);
        }
        /// <summary>
        /// 获取菜单JSON
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMenuTreeJson_Server()
        {
            var treelist = new WaterFeeWeb.ServiceReference1.AuthorityClient().Sys_Menu_GetTree(0, 10);
            return ToJsonContent(treelist);
        }

        public ActionResult Sys_Menu_GetSon_Server()
        {
            var iQryType = Request["WHC_QryType"];
            var dts = new DataTable();
            if (iQryType == "0")
            {
                var sMenuID = Request["MenuID"];
                dts = new WaterFeeWeb.ServiceReference1.AuthorityClient().Sys_Menu_GetSon(sMenuID, 0, 10);
            }
            else
            {
                WaterFeeWeb.ServiceReference1.Menu menu = new WaterFeeWeb.ServiceReference1.Menu
                {
                    NvcName = Request["WHC_Name"],
                    NvcIcon = Request["WHC_Icon"],
                    NvcSeq = Request["WHC_Seq"],
                    NvcFuncId = Request["WHC_FunctionId"],
                    NvcWinformType = Request["WHC_WinformType"],
                    NvcUrl = Request["WHC_Url"],
                    NvcWebIcon = Request["WHC_WebIcon"]
                };
                dts = new WaterFeeWeb.ServiceReference1.AuthorityClient().Sys_Menu_Qry(menu);
            }

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

        public ActionResult Sys_Menu_FindById()
        {
            WaterFeeWeb.ServiceReference1.Menu menu = new WaterFeeWeb.ServiceReference1.Menu();
            menu.NvcID = Request["WHC_ID"] ?? "";
            var dt = new WaterFeeWeb.ServiceReference1.AuthorityClient().Sys_Menu_Qry(menu);
            return ToJsonContent(dt);
        }

        private void AddChildNode(List<MenuNodeInfo> list, EasyTreeData fnode)
        {
            foreach (MenuNodeInfo info in list)
            {
                EasyTreeData item = new EasyTreeData(info.ID, info.Name, "icon-view");
                fnode.children.Add(item);
                AddChildNode(info.Children, item);
            }
        }
    }
}
