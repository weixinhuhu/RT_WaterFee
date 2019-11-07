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
    public class AccDebtController : BusinessController<Core.BLL.AccDebt, Core.Entity.AccDebt>
    {
        //柜台收费
        public ActionResult CounterPay()
        {
            return View();
        }



        object objLock = new object();
        //柜台收费
        [HttpPost]
        public ActionResult CounterPay(string custNo)
        {
            CommonResult result = new CommonResult();
            try
            {
                var feeCount = Request["FeeCount"];
                var totalMoney = Request["totalMoney"];

                //CREATEPROCEDURE[dbo].[up_BillWriteoff]
                //@iCustNo INTEGER = 1, --< !--客户编号-- >
                //@sUserID VARCHAR(4),--< !--用户编号-- >
                //@DteAccount DATE,--< !--账务日期-- >
                //@sFlowNo VARCHAR(32),--< !--流水号，永不重复-- >
                //@iCount     INTEGER,--< !--缴费笔数，0：全部缴纳，其它数值为多笔欠费的前iCount笔-- >
                //@NumTotal   NUMERIC(9, 2),--< !--欲销账总金额，支持总额取整数到元的模式-- >
                //@sReturn    VARCHAR(MAX)OUTPUT

                lock (objLock)
                {
                    //System.Threading.Thread.Sleep(30000);
                    var sFlowNo = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    List<SqlParameter> param = new List<SqlParameter>();
                    param.Add(new SqlParameter("@iCustNo", SqlDbType.Int) { Value = custNo });
                    param.Add(new SqlParameter("@sUserID", SqlDbType.VarChar, 4) { Value = UserInfo.ID });
                    param.Add(new SqlParameter("@DteAccount", SqlDbType.Date) { Value = DateTime.Now });
                    param.Add(new SqlParameter("@sFlowNo", SqlDbType.VarChar, 32) { Value = sFlowNo });
                    param.Add(new SqlParameter("@iCount", SqlDbType.Int) { Value = feeCount });
                    param.Add(new SqlParameter("@NumTotal", SqlDbType.Decimal) { Value = totalMoney });
                    param.Add(new SqlParameter("@sReturn", SqlDbType.VarChar, 256) { Direction = ParameterDirection.Output });

                    BLLFactory<Core.BLL.AccPayment>.Instance.ExecStoreProc("up_BillWriteoff", param);
                    if (param[param.Count - 1].Value.ToString() == "0")
                    {
                        result.Success = true;
                    }
                    else
                    {
                        result.ErrorMessage = "缴费失败!错误如下:" + param[param.Count - 1].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = "缴费失败!错误如下:" + ex.Message;
            }
            return ToJsonContent(result);
        }

        /// <summary>
        /// 获取未收财务
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAccDebt()
        {
            string where = GetPagerCondition();
            PagerInfo pagerInfo = GetPagerInfo();
            var list = baseBLL.FindWithPager(where, pagerInfo);

            if (list.Count > 0)
            {
                var ids = string.Join(",", list.Select(n => n.IntCustNo).ToArray());
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
                list = list.OrderBy(n => n.IntFeeID).ToList();
            }

            var result = new { total = pagerInfo.RecordCount, rows = list };

            return ToJsonContentDate(result);
        }

        /// <summary>
        /// 获取未收财务(催费通知单)
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPaymentNoticeList()
        {
            string where = GetPagerCondition();
            PagerInfo pagerInfo = GetPagerInfo();
            var list = baseBLL.FindWithPager(where, pagerInfo);

            var listNew = list.GroupBy(o => new { o.IntCustNo }).Select(n => new
            {
                IntCustNo = n.Key.IntCustNo,
                //VcCustName = "",//客户名称
                IntMonthCount = n.Count(x => x.IntCustNo == n.Key.IntCustNo),
                MonFee = n.Where(x => x.IntCustNo == n.Key.IntCustNo).Sum(x => x.MonFee),
                //VcAddr = ""//地址
            });
            if (list.Count > 0)
            {
                list = new List<Core.Entity.AccDebt>();
                var ids = string.Join(",", listNew.Select(n => n.IntCustNo).ToArray());
                where = " IntNo in ({0}) ".FormatWith(ids);
                pagerInfo.CurrenetPageIndex = 1;
                var customerList = BLLFactory<Core.DALSQL.ArcCustomerInfo>.Instance.FindWithPager(where, pagerInfo);
                if (customerList.Count > 0)
                {
                    foreach (var item in listNew)
                    {
                        var info = customerList.Where(n => n.IntNo == item.IntCustNo).FirstOrDefault();
                        list.Add(new Core.Entity.AccDebt()
                        {
                            ArcCustomerInfo = info,
                            IntCustNo = item.IntCustNo,
                            MonFee = item.MonFee,
                            IntYearMon = item.IntMonthCount
                        });
                    }
                }
            }

            var result = new { total = pagerInfo.RecordCount, rows = list };

            return ToJsonContentDate(result);
        }

        //柜台冲正
        public ActionResult CounterReverse()
        {
            return View();
        }

        /// <summary>
        /// 柜台冲正
        /// </summary>
        /// <param name="custNo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CounterReverse(string custNo)
        {
            CommonResult result = new CommonResult();
            try
            {
                var sFlowNo = Request["flowNo"];
                var totalMoney = Request["totalMoney"];

                //CREATEPROCEDURE[dbo].up_BillWithdraw
                //@iCustNo INTEGER,  --< !--客户编号-- >
                //@sFlowNo  VARCHAR(32),--< !--流水号-- >
                //@NumTotal NUMERIC(9, 2),--< !--销账总额-- >
                //@sUserID  VARCHAR(8),--< !--用户编号-- >
                //@sReturn  VARCHAR(MAX)OUTPUT-- < !--0:成功，其它为错误描述-- >

                lock (objLock)
                {
                    //System.Threading.Thread.Sleep(30000); 
                    List<SqlParameter> param = new List<SqlParameter>();
                    param.Add(new SqlParameter("@iCustNo", SqlDbType.Int) { Value = custNo });
                    param.Add(new SqlParameter("@sUserID", SqlDbType.VarChar, 4) { Value = UserInfo.ID });
                    param.Add(new SqlParameter("@sFlowNo", SqlDbType.VarChar, 32) { Value = sFlowNo });
                    param.Add(new SqlParameter("@NumTotal", SqlDbType.Decimal) { Value = totalMoney });
                    param.Add(new SqlParameter("@sReturn", SqlDbType.VarChar, 256) { Direction = ParameterDirection.Output });

                    BLLFactory<Core.BLL.AccPayment>.Instance.ExecStoreProc("up_BillWithdraw", param);
                    if (param[param.Count - 1].Value.ToString() == "0")
                    {
                        result.Success = true;
                    }
                    else
                    {
                        result.ErrorMessage = "冲正失败!错误如下:" + param[param.Count - 1].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = "冲正失败!错误如下:" + ex.Message;
            }
            return ToJsonContent(result);
        }

        //柜台冲正数据
        public ActionResult CounterReverseData()
        {
            return new AccPaymentController().CounterReverseData();
        }

        //存取预存款

        public ActionResult PayGetMoney()
        {
           
            return View();
        }

        /// <summary>
        /// 存款
        /// </summary>
        /// <param name="custNo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PayGetMoney_Pay(string custNo)
        {
            CommonResult result = new CommonResult();
            try
            {
                //var sFlowNo = Request["flowNo"];
                var payMoney = Request["payMoney"];

                //预付费模式预存款操作
                //CREATEPROCEDURE[dbo].[up_DepositAccess]
                //@iCustNo INTEGER,--< !--客户号-- >
                //@sFlowNo    VARCHAR(32),--< !--存取流水号-- >
                //@MonAmount  NUMERIC(9, 2),--< !--存取款金额-- >
                //@sDesc      VARCHAR(32),--< !--原因描述-- >
                //@sUserID    VARCHAR(8),--< !--用户工号-- >
                //@sReceNo    VARCHAR(32),--< !--收据号码-- >
                //@sSumAmount VARCHAR(16)OUTPUT,--< !--当前总额-- >
                //@sLstUpd    VARCHAR(32)OUTPUT,--< !--最后更新时间戳-- >
                //@sReturn    VARCHAR(MAX)OUTPUT-- < !--0:成功，其它为错误描述-- >


                lock (objLock)
                {
                    var sFlowNo = GetFlowNo();
                    List<SqlParameter> param = new List<SqlParameter>();
                    param.Add(new SqlParameter("@iCustNo", SqlDbType.Int) { Value = custNo });
                    param.Add(new SqlParameter("@sFlowNo", SqlDbType.VarChar, 32) { Value = sFlowNo });
                    param.Add(new SqlParameter("@MonAmount", SqlDbType.Decimal) { Value = payMoney });
                    param.Add(new SqlParameter("@sDesc", SqlDbType.VarChar, 32) { Value = "存入" });
                    param.Add(new SqlParameter("@sUserID", SqlDbType.VarChar, 8) { Value = UserInfo.ID });
                    param.Add(new SqlParameter("@sReceNo", SqlDbType.VarChar, 32) { Value = "" });
                    param.Add(new SqlParameter("@sSumAmount", SqlDbType.VarChar, 16) { Direction = ParameterDirection.Output });
                    param.Add(new SqlParameter("@sLstUpd", SqlDbType.VarChar, 32) { Direction = ParameterDirection.Output });
                    param.Add(new SqlParameter("@sReturn", SqlDbType.VarChar, 256) { Direction = ParameterDirection.Output });

                    BLLFactory<Core.BLL.AccPayment>.Instance.ExecStoreProc("up_DepositAccess", param);
                    if (param[param.Count - 1].Value.ToString() == "0")
                    {
                        result.Success = true;
                    }
                    else
                    {
                        result.ErrorMessage = "存款失败!错误如下:" + param[param.Count - 1].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = "存款失败!错误如下:" + ex.Message;
            }
            return ToJsonContent(result);
        }


        /// <summary>
        /// 取款
        /// </summary>
        /// <param name="custNo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PayGetMoney_Get(string custNo)
        {
            CommonResult result = new CommonResult();
            try
            {
                //var sFlowNo = Request["flowNo"];
                var payMoney = Request["payMoney"];

                //预付费模式预存款操作
                //CREATEPROCEDURE[dbo].[up_DepositAccess]
                //@iCustNo INTEGER,--< !--客户号-- >
                //@sFlowNo    VARCHAR(32),--< !--存取流水号-- >
                //@MonAmount  NUMERIC(9, 2),--< !--存取款金额-- >
                //@sDesc      VARCHAR(32),--< !--原因描述-- >
                //@sUserID    VARCHAR(8),--< !--用户工号-- >
                //@sReceNo    VARCHAR(32),--< !--收据号码-- >
                //@sSumAmount VARCHAR(16)OUTPUT,--< !--当前总额-- >
                //@sLstUpd    VARCHAR(32)OUTPUT,--< !--最后更新时间戳-- >
                //@sReturn    VARCHAR(MAX)OUTPUT-- < !--0:成功，其它为错误描述-- >


                lock (objLock)
                {
                    var sFlowNo = GetFlowNo();
                    List<SqlParameter> param = new List<SqlParameter>();
                    param.Add(new SqlParameter("@iCustNo", SqlDbType.Int) { Value = custNo });
                    param.Add(new SqlParameter("@sFlowNo", SqlDbType.VarChar, 32) { Value = sFlowNo });
                    param.Add(new SqlParameter("@MonAmount", SqlDbType.Decimal) { Value = payMoney });
                    param.Add(new SqlParameter("@sDesc", SqlDbType.VarChar, 32) { Value = "取出" });
                    param.Add(new SqlParameter("@sUserID", SqlDbType.VarChar, 8) { Value = UserInfo.ID });
                    param.Add(new SqlParameter("@sReceNo", SqlDbType.VarChar, 32) { Value = "" });
                    param.Add(new SqlParameter("@sSumAmount", SqlDbType.VarChar, 16) { Direction = ParameterDirection.Output });
                    param.Add(new SqlParameter("@sLstUpd", SqlDbType.VarChar, 32) { Direction = ParameterDirection.Output });
                    param.Add(new SqlParameter("@sReturn", SqlDbType.VarChar, 256) { Direction = ParameterDirection.Output });

                    BLLFactory<Core.BLL.AccPayment>.Instance.ExecStoreProc("up_DepositAccess", param);
                    if (param[param.Count - 1].Value.ToString() == "0")
                    {
                        result.Success = true;
                    }
                    else
                    {
                        result.ErrorMessage = "取款失败!错误如下:" + param[param.Count - 1].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = "取款失败!错误如下:" + ex.Message;
            }
            return ToJsonContent(result);
        }

        //结清欠款销户
        public ActionResult CloseAccount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CloseAccount_Query(string custNo)
        {
            CommonResult result = new CommonResult();
            try
            {
                //CREATEPROCEDURE[dbo].[up_BillQry]
                //@iCustNo INTEGER,   --< !--客户编号-- >
                //@sReturn VARCHAR(MAX)OUTPUT-- < !--0:成功，其它为错误描述-- >

                lock (objLock)
                {
                    var sFlowNo = GetFlowNo();
                    List<SqlParameter> param = new List<SqlParameter>();
                    param.Add(new SqlParameter("@iCustNo", SqlDbType.Int) { Value = custNo });
                    param.Add(new SqlParameter("@sReturn", SqlDbType.VarChar, 256) { Direction = ParameterDirection.Output });

                    var ds = BLLFactory<Core.BLL.AccPayment>.Instance.ExecStoreProcToDataSet("up_BillQry", param);
                    if (param[param.Count - 1].Value.ToString() == "0")
                    {
                        result.Data1 = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0]);
                        var dt = new { total = ds.Tables[1].Rows.Count, rows = ds.Tables[1] };
                        result.Obj1 = dt;
                        result.Success = true;
                    }
                    else
                    {
                        result.ErrorMessage = "查询欠费失败!错误如下:" + param[param.Count - 1].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = "查询欠费失败!错误如下:" + ex.Message;
            }
            return ToJsonContent(result);
        }

        public ActionResult TodayBalance()
        {
            return View();
        }

        public ActionResult PaymentNotice()
        {
            return View();
        }
        public ActionResult PaymentNoticeExport()
        {
            var intCustNo = Request["WHC_IntCustNo"];
            string where = GetPagerCondition();
            PagerInfo pagerInfo = GetPagerInfo();
            var list = baseBLL.FindWithPager(where, pagerInfo);

            var listNew = list.GroupBy(o => new { o.IntCustNo }).Select(n => new
            {
                IntCustNo = n.Key.IntCustNo,
                //VcCustName = "",//客户名称
                IntMonthCount = n.Count(x => x.IntCustNo == n.Key.IntCustNo),
                MonFee = n.Where(x => x.IntCustNo == n.Key.IntCustNo).Sum(x => x.MonFee),
                //VcAddr = ""//地址
            });
            if (list.Count > 0)
            {
                var columns = "客户编号,客户名称,欠费月数,欠费总金额,地址".Split(',');
                var dt = new System.Data.DataTable();
                foreach (var item in columns)
                {
                    //if (item == "欠费总金额" || item == "欠费月数")
                    //    dt.Columns.Add(item, typeof(decimal));
                    //else
                    dt.Columns.Add(item, typeof(string));
                }

                var ids = string.Join(",", listNew.Select(n => n.IntCustNo).ToArray());
                where = " IntNo in ({0}) ".FormatWith(ids);
                pagerInfo.CurrenetPageIndex = 1;
                var customerList = BLLFactory<Core.DALSQL.ArcCustomerInfo>.Instance.FindWithPager(where, pagerInfo);
                if (customerList.Count > 0)
                {
                    foreach (var item in listNew)
                    {
                        var row = dt.NewRow();
                        var info = customerList.Where(n => n.IntNo == item.IntCustNo).FirstOrDefault();
                        row["客户编号"] = item.IntCustNo;
                        row["客户名称"] = info.NvcName;
                        row["欠费月数"] = item.IntMonthCount;
                        row["欠费总金额"] = item.MonFee;
                        row["地址"] = info.NvcAddr;
                        dt.Rows.Add(row);
                    }
                }
                //导出目录创建与清空
                var root = Server.MapPath("~\\");
                var dir = new System.IO.DirectoryInfo(root + "temp\\");
                if (dir.Exists == false) dir.Create();
                try
                {
                    foreach (var item in dir.GetFiles())
                    {
                        item.Delete();
                    }
                }
                catch { }

                var filename = dir + Guid.NewGuid().ToString() + ".xls";

                var ds = new System.Data.DataSet();
                ds.Tables.Add(dt);
                ExcelHelper.DataSetToExcel(ds, filename);

                return Redirect(filename.Replace(root, "/").Replace("\\", "/"));
            }
            return View();
        }
    }
}
