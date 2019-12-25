using System;
using System.Web.Mvc;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;
using WHC.MVCWebMis.Controllers;
using WHC.Pager.Entity;

namespace WHC.WaterFeeWeb.Controllers
{
    public class DepartmentController : BusinessController<Core.BLL.T_ACL_Department, Core.Entity.T_ACL_Department>
    {
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


        [HttpPost]
        public override ActionResult Insert(Core.Entity.T_ACL_Department info)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.InsertKey);

            CommonResult result = new CommonResult();
            try
            {
                //info.Remark  = null ;
                result.Success = baseBLL.Insert(info);
            }
            catch (Exception ex)
            {
                LogTextHelper.Error(ex);//错误记录
                result.ErrorMessage = ex.Message;
            }
            return ToJsonContent(result);
        }

        [HttpPost]
        public ActionResult Update(Core.Entity.T_ACL_Department info)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.UpdateKey);

            CommonResult result = new CommonResult();
            try
            {
                result.Success = baseBLL.Update(info, info.DepartmentID);
            }
            catch (Exception ex)
            {
                LogTextHelper.Error(ex);//错误记录
                result.ErrorMessage = ex.Message;
            }
            return ToJsonContent(result);
        }

    }
}
