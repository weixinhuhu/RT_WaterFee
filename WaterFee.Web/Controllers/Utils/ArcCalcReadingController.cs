using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;
using WHC.Pager.Entity;

namespace WHC.WaterFeeWeb.Controllers
{

    public class ArcCalcReadingController : BusinessControllerNew<Core.BLL.ArcCalcReading, Core.Entity.ArcCalcReading>
    {
        //审核数据
        public ActionResult GetApproveDataJson()
        {
            base.CheckAuthorized(AuthorizeKey.ListKey);

            string where = GetPagerCondition();
            var key = RRequest("Key");
            PagerInfo pagerInfo = GetPagerInfo();
            if (key.IsNotNullOrEmpty())
            {
                where += " and VcAddr in (select VcAddr from arcMeterInfo where VcBarCode = '{0}' or NvcName like '%{0}%')".FormatWith(key);
            }
            var list = baseBLL.FindWithPager(where, pagerInfo);

            if (list.Count > 0)
            {
                var vcaddrs = list.Select(n => n.VcAddr).ToArray();
                var insql = new System.Text.StringBuilder();
                foreach (var item in vcaddrs)
                {
                    insql.AppendFormat(insql.Length == 0 ? "'{0}'" : ",'{0}'", item);
                }
                where = " VcAddr in ({0}) ".FormatWith(insql.ToString());
                var page = GetPagerInfo();
                page.CurrenetPageIndex = 1;
                var meterList = BLLFactory<Core.DALSQL.ArcMeterInfo>.Instance.FindWithPager(where, page);
                if (meterList.Count > 0)
                {
                    foreach (var item in list)
                    {
                        item.ArcMeterInfo = meterList.Where(n => n.VcAddr == item.VcAddr).FirstOrDefault();
                    }
                }
            }

            //Json格式的要求{total:22,rows:{}}
            //构造成Json的格式传递
            var result = new { total = pagerInfo.RecordCount, rows = list };

            return ToJsonContentDate(result);
        }

        [HttpPost]
        public ActionResult ApproveData()
        {
            CommonResult result = new CommonResult();
            try
            {
                var arrFeeID = Request["ArrFeeID"].Split(',');
                var list = new List<int>();
                foreach (var item in arrFeeID)
                {
                    list.Add(item.ToIntOrZero());
                }
                var sql = "update ArcCalcReading set IntStatus=1 where IntFeeID in ({0}) and IntStatus=0 "
                        .FormatWith(string.Join(",", list));
                result.Success = BLLFactory<Core.DALSQL.ArcCalcReading>.Instance.SqlExecute(sql) > 0;
                if (result.Success == false)
                {
                    result.ErrorMessage = "审核失败!请检查您的输入是否正确后重试!";
                }
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
