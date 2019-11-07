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

namespace WHC.WaterFeeWeb.Controllers
{
    public class AccDepositDetailController : BusinessControllerNew<Core.BLL.AccDepositDetail, Core.Entity.AccDepositDetail>
    {
        //[HttpPost]
        public ActionResult GetDetailByCustomerNo()
        {
            base.CheckAuthorized(AuthorizeKey.ListKey);

            return base.FindWithPager();
        }
        public ActionResult CurrentDateList()
        {
            string where = "1=1";
            var date = RRequest("WHC_DteAccount");
            where += " and convert(DteAccount,char(10))='{0}'".FormatWith(date);
            where += " and VcUserID='{0}'".FormatWith(CurrentUser.ID);
            PagerInfo pagerInfo = GetPagerInfo();
            var list = baseBLL.FindWithPager(where, pagerInfo);
 
            var result = new { total = pagerInfo.RecordCount, rows = list };

            return ToJsonContentDate(result);
        }
    }
}
