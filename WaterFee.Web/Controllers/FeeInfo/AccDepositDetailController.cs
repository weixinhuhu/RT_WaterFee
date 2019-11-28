using System;
using System.Data;
using System.Web.Mvc;
using WHC.Framework.Commons;
using WHC.Pager.Entity;
using WHC.WaterFeeWeb.DbServiceReference;

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
        public ActionResult GetDetailByCustomerNo_Server()
        {
            var endcode = Session["EndCode"] ?? "0";
            var custno = Request["WHC_IntCustNo"] ?? "0";
            
            ServiceDbClient DbServer = new ServiceDbClient();
            var dts = DbServer.Account_GetDepositDetailByCustNo(endcode.ToString().ToInt32(), custno.ToInt32());
            
            //分页参数
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
            var result = new {total, rows = dat };

            return ToJsonContentDate(result);
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
        public ActionResult CurrentDateList_Server()
        {           
            var date = RRequest("WHC_DteAccount");
            var UserId = CurrentUser.ID;
            ServiceDbClient DbServer = new ServiceDbClient();
            var dts = DbServer.Account_GetDepositDetail(UserId, date.ToDateTime(), date.ToDateTime());
            //分页参数
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
    }
}
