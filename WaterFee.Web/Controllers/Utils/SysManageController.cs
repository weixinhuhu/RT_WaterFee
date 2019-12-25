using System;
using System.Web.Mvc;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;

namespace WHC.WaterFeeWeb.Controllers
{
    public class SysManageController : BusinessControllerNew<Core.BLL.AccDebt, Core.Entity.AccDebt>
    {
        //设置
        public ActionResult Setting()
        {
            var dt = BLLFactory<Core.BLL.AccPayment>.Instance.SqlTable("select top 1 * from Settings ");
            if (dt.Rows.Count > 0)
            {
                ViewBag.PayMode = dt.Rows[0]["PayMode"].ToString();
                ViewBag.BalanceDay = dt.Rows[0]["BalanceDay"].ToString();
                ViewBag.AreaCode = dt.Rows[0]["AreaCode"].ToString();
            }
            else
            {
                ViewBag.PayMode = 1;
                ViewBag.BalanceDay = 1;
                ViewBag.AreaCode = string.Empty;
            }
            return View();
        }
        public ActionResult Setting_Save()
        {
            CommonResult result = new CommonResult();
            try
            {
                //0预付费,1后付费
                var payMode = Request["payMode"].ToIntOrDefault(1);
                var balanceDay = Request["balanceDay"].ToIntOrDefault(1);
                var AreaCode = Request["AreaCode"];

                var sql = "update settings set payMode='{0}',BalanceDay='{1}',AreaCode='{2}' ;".FormatWith(payMode, balanceDay, AreaCode);
                //客户信息表的付费模式
                sql += "update ArcCustomerInfo set IntAccMode='{0}' ;".FormatWith(payMode);
                var i = BLLFactory<Core.BLL.AccPayment>.Instance.SqlExecute(sql);
                result.Success = (i > 0);
            }
            catch (Exception ex)
            {
                result.ErrorMessage = "保存失败!错误如下:" + ex.Message;
            }
            return ToJsonContent(result);
        }

        public ActionResult AutoSwitch()
        {
            var dt = BLLFactory<Core.BLL.AccPayment>.Instance.SqlTable("select top 1 * from Settings ");
            if (dt.Rows.Count > 0)
            {
                var auto = dt.Rows[0]["IntAutoSwitch"].ToString();
                ViewBag.AutoSwitch = auto.IsNullOrEmpty() ? "0" : auto;
            }
            else
            {
                ViewBag.AutoSwitch = 0;
            }
            return View();
        }
        public ActionResult AutoSwitch_Save()
        {
            CommonResult result = new CommonResult();
            try
            {
                //是否允许自动开关阀，0：不允许1：允许
                var AutoSwitch = Request["AutoSwitch"].ToIntOrDefault(1);

                var sql = "update settings set IntAutoSwitch='{0}' ;".FormatWith(AutoSwitch);
                //客户信息表的付费模式
                //sql += "update ArcCustomerInfo set IntAccMode='{0}' ;".FormatWith(AutoSwitch);
                var i = BLLFactory<Core.BLL.AccPayment>.Instance.SqlExecute(sql);
                result.Success = (i > 0);
            }
            catch (Exception ex)
            {
                result.ErrorMessage = "保存失败!错误如下:" + ex.Message;
            }
            return ToJsonContent(result);
        }
        object objLock = new object();

    }
}
