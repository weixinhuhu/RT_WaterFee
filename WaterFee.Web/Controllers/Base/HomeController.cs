using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

using WHC.Framework.ControlUtil;
using WHC.Security.BLL;
using WHC.Security.Entity;

namespace WHC.MVCWebMis.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            if (Session["FullName"] != null)
            {
                ViewBag.FullName = Session["FullName"].ToString();
                ViewBag.Name = Session["UserID"].ToString();
                ViewBag.HeaderScript = "收费系统";//一级菜单代码
                //公司名称
                ViewBag.AppCompanyName = DBLib.Common.ConfigHelper.GetConfigString("AppCompanyName");
            }
            return View();
        }

        public ActionResult Another()
        {
            if (CurrentUser != null)
            {
                ViewBag.FullName = CurrentUser.FullName;
                ViewBag.Name = CurrentUser.Name;
            }
            return View();
        }

        public ActionResult Welcome()
        {
            return View();
        }

        public ActionResult Online()
        {
            return View();
        }

    }
}
