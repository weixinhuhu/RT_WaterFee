using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using WHC.Framework.Commons;
using WHC.Framework.Commons.Collections;
using WHC.Framework.ControlUtil;
using WHC.MVCWebMis.Entity;
using WHC.Security.BLL;
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

        public ActionResult OU_Opr_Server(WaterFeeWeb.ServiceReference1.OU OrgInfo)
        {
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
            else
            {
                result.Success = false;
                result.ErrorMessage = flag;
            }
            return ToJsonContent(result);
        }

        public ActionResult OU_Recover_Server(WaterFeeWeb.ServiceReference1.OU OrgInfo, String Pid)
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
    }
}
