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
    public class FeeController : BusinessController<Core.BLL.AccDebt, Core.Entity.AccDebt>
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

                        try
                        {
                            CounterPay_CheckMeter(custNo);
                        }
                        catch (Exception ex)
                        {
                            result.Success = false;
                            result.ErrorMessage = "缴费成功!但生成[开阀任务]失败,请在[终端管理]->[远程功能]中手动尝试下发开阀命令!";
                        }
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
        /// 交费后检查表是否是关阀状态,是的话如果没有欠费,则生成 开阀任务 
        /// </summary>
        private void CounterPay_CheckMeter(string custNo)
        {
            try
            {
                //查询是否有欠费
                var isExist = BLLFactory<Core.BLL.AccDebt>.Instance.IsExistRecord(" IntCustNo=" + custNo);
                if (isExist) return;//有欠费则不用开阀

                //查询表状态是否是关的状态
                //IntValueState字段1代表关阀,0为开阀状态.
                var meterInfo = BLLFactory<Core.BLL.ArcMeterInfo>.Instance.FindSingle(" IntValueState=1 and IntCustNo=" + custNo);
                if (meterInfo == null) return;

                //生产开阀任务
                //
                //8.1 柜台收费
                //根据欠费金额收取客户费用，支持舍余数和超额转预存，也支持无欠费情况下直接预存；
                //1、缴费后如果有欠费，调用销账接口进行销账；
                //2、销账成功后要判断水表是否为关阀状态；
                //3、如果是关阀状态则调用后台接口生成开发任务；
                //CREATEPROCEDURE[dbo].[up_TaskValveCreate]
                // @sFlowNo VARCHAR(32), --< !--操作流水号, 不可重复，用于查询命令执行状态-- >
                // @sMeterAddr VARCHAR(32), --< !--表地址-- >
                // @iCMD       INTEGER, --< !--阀控命令0:开阀1: 关阀-- >
                // @iUserNo    INTEGER,--< !--操作员-- >
                // @sReturn    VARCHAR(MAX)OUTPUT

                var sFlowNo = UserInfo.ID + DateTime.Now.ToString("-yyyyMMddHHmmssfff");
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@sFlowNo", SqlDbType.VarChar, 32) { Value = sFlowNo });
                param.Add(new SqlParameter("@sMeterAddr", SqlDbType.VarChar, 32) { Value = meterInfo.VcAddr });
                param.Add(new SqlParameter("@iCMD", SqlDbType.Date) { Value = 0 });
                param.Add(new SqlParameter("@iUserNo", SqlDbType.VarChar, 32) { Value = UserInfo.ID });
                param.Add(new SqlParameter("@sReturn", SqlDbType.VarChar, 256) { Direction = ParameterDirection.Output });

                BLLFactory<Core.BLL.ArcMeterInfo>.Instance.ExecStoreProc("up_TaskValveCreate", param);
                if (param[param.Count - 1].Value.ToString() == "0")
                {
                }
                else
                {
                    throw new Exception(param[param.Count - 1].Value.ToString());
                }
            }
            catch (Exception ex) { }
        }

        /// <summary>
        /// 获取未收财务
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAccDebt()
        {
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
                foreach (var item in list)
                {
                    string ret = null;
                    var ds = QueryBill(item.IntCustNo, out ret);
                    if (ret == "0")
                    {
                        var row = ds.Tables[1].Select("费用编号=" + item.IntFeeID);
                        if (row.Count() > 0)
                        {
                            item.MonPenalty = row[0]["违约金"].ToString().ToDecimalOrZero();
                        }
                    }
                }
                list = list.OrderBy(n => n.IntFeeID).ToList();
            }

            var result = new { total = pagerInfo.RecordCount, rows = list };

            return ToJsonContentDate(result);
        }

        /// <summary>
        /// 获取未收财务
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAccDebtByIntCustNo(int intCustNo)
        {
            PagerInfo pagerInfo = GetPagerInfo();

            var where = " IntCustNo=" + intCustNo;

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
                foreach (var item in list)
                {
                    string ret = null;
                    var ds = QueryBill(item.IntCustNo, out ret);
                    if (ret == "0")
                    {
                        var row = ds.Tables[1].Select("费用编号=" + item.IntFeeID);
                        if (row.Count() > 0)
                        {
                            item.MonPenalty = row[0]["违约金"].ToString().ToDecimalOrZero();
                        }
                    }
                }
                list = list.OrderBy(n => n.IntFeeID).ToList();
            }

            var result = new { total = pagerInfo.RecordCount, rows = list };

            return ToJsonContentDate(result);
        }

        /// <summary>
        /// 获取未收财务
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
            InitSignalrScript();
            return View();
        }
        private void InitSignalrScript()
        {
            var signalrUrl = DBLib.Common.ConfigHelper.GetConfigString("SignalrUrl");
            var commandTimeout = DBLib.Common.ConfigHelper.GetConfigString("CommandTimeout");
            ViewBag.SignalrScript = string.Format(@"<script src=""{0}/signalr/hubs""></script><script>var signalrUrl = ""{0}"";var CommandTimeout={1}*1000;</script>", signalrUrl, commandTimeout);

        }
        //存取预存款
        public ActionResult PayGetMoneyPrint(string flowNo)
        {
            var FeeInfo = BLLFactory<Core.BLL.AccDepositDetail>.Instance.Find("VcFlowNo='" + flowNo + "'");
            if (FeeInfo.Count > 0)
            {
                var model = FeeInfo.First();
                var custInfo = BLLFactory<Core.DALSQL.ArcCustomerInfo>.Instance.Find(" IntNo=" + model.IntCustNo);

                ViewBag.FeeInfo = model;
                ViewBag.DteAccount = model.DteAccount.ToYyyyMMdd();
                //var s = "";
                //switch (model.IntType)
                //{
                //    case 0: s = "存入"; break;
                //    case 1: s = "提取"; break;
                //    case 2: s = "销账"; break;
                //    default: s = "未知业务代码:" + model.IntType; break;
                //}
                //ViewBag.IntType = s;
                ViewBag.CustInfo = custInfo.Count > 0 ? custInfo.First() : new Core.Entity.ArcCustomerInfo();
            }
            else
            {
                ViewBag.DteAccount = "";
                //ViewBag.IntType = "";
                ViewBag.FeeInfo = new Core.Entity.AccDepositDetail();
                ViewBag.CustInfo = new Core.Entity.ArcCustomerInfo();
            }
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
                        result.Data1 = sFlowNo;
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
                        result.Data1 = sFlowNo;
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

        public DataSet QueryBill(object custNo, out string result)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@iCustNo", SqlDbType.Int) { Value = custNo });
            param.Add(new SqlParameter("@sReturn", SqlDbType.VarChar, 256) { Direction = ParameterDirection.Output });

            var ds = BLLFactory<Core.BLL.AccPayment>.Instance.ExecStoreProcToDataSet("up_BillQry", param);
            result = param[param.Count - 1].Value.ToString();
            return ds;
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
                    string ret = null;
                    var ds = QueryBill(custNo, out ret);
                    if (ret == "0")
                    {
                        result.Data1 = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0]);
                        var dt = new { total = ds.Tables[1].Rows.Count, rows = ds.Tables[1] };
                        result.Obj1 = dt;
                        result.Success = true;
                    }
                    else
                    {
                        result.ErrorMessage = "查询欠费失败!错误如下:" + ret;
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = "查询欠费失败!错误如下:" + ex.Message;
            }
            return ToJsonContent(result);
        }

        /// <summary>
        /// 销户
        /// </summary>
        /// <param name="custNo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CloseAccount_Close(string custNo)
        {
            CommonResult result = new CommonResult();
            try
            {
                var no = Convert.ToInt32(custNo);
                var sql = "update ArcCustomerInfo set IntStatus=4 where IntNo=" + no;
                var isOk = BLLFactory<Core.BLL.AccPayment>.Instance.SqlExecute(sql);
                if (isOk > 0)
                {
                    result.Success = true;
                }
                else
                {
                    result.ErrorMessage = "操作失败!";
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = "操作失败!错误如下:" + ex.Message;
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

        public ActionResult PrintTicket()
        {
            return View();
        }
        public ActionResult PrintTicketDetail(int IntFeeID)
        {
            var model = BLLFactory<Core.DALSQL.AccPayment>.Instance.FindByID(IntFeeID);
            if (model != null)
            {
                var custInfo = BLLFactory<Core.DALSQL.ArcCustomerInfo>.Instance.Find(" IntNo=" + model.IntCustNo);
                ViewBag.FeeInfo = model;
                ViewBag.DteFee = model.DteFee.ToYyyyMMdd();
                ViewBag.DtePay = model.DtePay.ToYyyyMMdd();
                ViewBag.CustInfo = custInfo.Count > 0 ? custInfo.First() : new Core.Entity.ArcCustomerInfo();
            }
            else
            {
                ViewBag.DteFee = "";
                ViewBag.DtePay = "";
                ViewBag.FeeInfo = new Core.Entity.AccPayment();
                ViewBag.CustInfo = new Core.Entity.ArcCustomerInfo();
            }
            return View();
        }
    }
}
