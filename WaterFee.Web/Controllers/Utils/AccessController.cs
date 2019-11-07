using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WHC.Attachment.BLL;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;
using WHC.MVCWebMis.Controllers;
using WHC.Pager.Entity;
using WHC.Security.BLL;
using WHC.Security.Entity;

namespace WHC.WaterFeeWeb.Controllers
{
    public class AccessController : BusinessController<User, UserInfo>
    {
        object objLock = new object();

        //Department
        public ActionResult Department()
        {
            return View();
        }

        //Department
        public ActionResult DepartmentJson()
        {
            string where = GetPagerCondition();
            PagerInfo pagerInfo = GetPagerInfo();

            var list = BLLFactory<Core.DALSQL.T_ACL_Department>.Instance.FindWithPager(where, pagerInfo);

            var result = new { total = pagerInfo.RecordCount, rows = list };

            return ToJsonContentDate(result);
        }


        //User
        public ActionResult User()
        {
            return View();
        }

        public ActionResult GetDepartmentJson(bool isAddRoot = true)
        {
            var list = BLLFactory<Core.DALSQL.T_ACL_Department>.Instance.GetAll();

            var treeList = new List<EasyTreeData>();

            var root = new EasyTreeData();
            root.iconCls = "icon-organ";
            root.id = "";
            root.text = "所有部门";
            root.state = "open";
            root.children = new List<EasyTreeData>();

            foreach (var item in list)
            {
                var d = new EasyTreeData();
                d.iconCls = "icon-organ";
                d.id = item.DepartmentID.ToString();
                d.text = item.Name;
                d.state = "open";

                root.children.Add(d);
            }
            if (isAddRoot)
                treeList.Add(root);
            else
                treeList = root.children;
            return ToJsonContentDate(treeList);
        }

        //AccessManage
        public ActionResult AccessManage()
        {
            return View();
        }
    }
}
