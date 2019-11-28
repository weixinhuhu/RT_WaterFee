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
    public class AccPaymentController : BusinessControllerNew<Core.BLL.AccPayment, Core.Entity.AccPayment>
    {
        //柜台冲正数据
        public ActionResult CounterReverseData()
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);
            var key = Request["key"];

            //string where = GetPagerCondition();
            PagerInfo pagerInfo = GetPagerInfo();

            var where = " 1=1 ";

            if (key.IsNotNullOrEmpty())
            {
                where += " and IntCustNo in (select IntNo from ArcCustomerInfo where IntNo like '%{0}%' or NvcName like '%{0}%' or NvcAddr like '%{0}%' or NvcVillage like '%{0}%' or VcMobile like '%{0}%' or VcTelNo like '%{0}%' )".FormatWith(key);
            }

            var list = baseBLL.FindWithPager(where, pagerInfo);

            if (list.Count > 0)
            {
                var ids = string.Join(",", list.Select(n => n.IntCustNo).ToArray().Distinct().ToArray());
                where = " IntNo in ({0}) ".FormatWith(ids);
                pagerInfo.CurrenetPageIndex = 1;
                var customerList = BLLFactory<Core.DALSQL.ArcCustomerInfo>.Instance.FindWithPager(where, pagerInfo);
                if (customerList.Count > 0)
                {
                    foreach (var item in list)
                    {
                        item.ArcCustomerInfo = customerList.Where(n => n.IntNo == item.IntCustNo).FirstOrDefault();
                    }
                }

                //list = list.OrderBy(n => n.IntFeeID).ToList();
            }

            var result = new { total = pagerInfo.RecordCount, rows = list };

            return ToJsonContentDate(result);
        }

        //柜台冲正数据
        public ActionResult CounterReverseDataByIntCustNo(int IntCustNo)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);
 
            PagerInfo pagerInfo = GetPagerInfo();

            var where = " IntCustNo="+IntCustNo ;
  
            var list = baseBLL.FindWithPager(where, pagerInfo);

            if (list.Count > 0)
            {
                var ids = string.Join(",", list.Select(n => n.IntCustNo).Distinct().ToArray());
                var FeeIDs = string.Join(",", list.Select(n => n.IntFeeID).Distinct().ToArray());
                where = " IntNo in ({0}) ".FormatWith(ids);
                pagerInfo.CurrenetPageIndex = 1;
                var customerList = BLLFactory<Core.DALSQL.ArcCustomerInfo>.Instance.FindWithPager(where, pagerInfo);
                if (customerList.Count > 0)
                {
                    //扣费账单对应的用水量记录
                    var s = "SELECT A.NumUsed,IntFeeID FROM ArcCalcReading A WHERE A.IntFeeID in ({0})".FormatWith(FeeIDs);
                    var dt = BLLFactory<Core.DALSQL.ArcCustomerInfo>.Instance.SqlTable(s);
                    foreach (var item in list)
                    {
                        item.ArcCustomerInfo = customerList.Where(n => n.IntNo == item.IntCustNo).FirstOrDefault();
                        var rows = dt.Select("IntFeeID=" + item.IntFeeID);
                        item.Data1 = rows.Count() > 0 ? rows[0]["NumUsed"].ToString() : "";//用水量
                    }
                }

                //list = list.OrderBy(n => n.IntFeeID).ToList();
            }

            var result = new { total = pagerInfo.RecordCount, rows = list };

            return ToJsonContentDate(result);
        }
        public ActionResult CounterReverseDataByIntCustNo_Server() {
            var custno = Request["WHC_IntCustNo"] ?? "0";
            var endcode = Session["EndCode"] ?? "0";
            DbServiceReference.ServiceDbClient DbServer = new DbServiceReference.ServiceDbClient();
            var dts = DbServer.Account_GetWriteoffByCustNo(endcode.ToString().ToInt32(), custno.ToInt32());
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

        //当日收费终结对账数据
        public ActionResult TodayBalance()
        {
            return base.FindWithPager();
        }
    }
}
