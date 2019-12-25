using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;
using WHC.MVCWebMis.Controllers;

namespace WHC.WaterFeeWeb.Controllers
{
    /// <summary>
    /// 算费菜单
    /// </summary>
    public class CountFeeController : BusinessController<Core.BLL.AccDebt, Core.Entity.AccDebt>
    {
        object objLock = new object();
        //审核抄表数据
        public ActionResult ApproveData()
        {
            return View();
        }

        public ActionResult CountReleaseFee()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CountReleaseFee(int IntCustNo)
        {
            CommonResult result = new CommonResult();
            try
            {
                //CREATEPROCEDURE[dbo].[up_BillGenerate]
                //@iCustNo INTEGER = 1, --< !--客户编号-- >
                //@iUserID VARCHAR(8),--< !--用户编号-- >
                //@sReturn VARCHAR(MAX)OUTPUT

                lock (objLock)
                {
                    //System.Threading.Thread.Sleep(3000); 
                    List<SqlParameter> param = new List<SqlParameter>();
                    param.Add(new SqlParameter("@iCustNo", SqlDbType.Int) { Value = IntCustNo });
                    param.Add(new SqlParameter("@iUserID", SqlDbType.Int) { Value = UserInfo.ID });
                    param.Add(new SqlParameter("@sReturn", SqlDbType.VarChar, 256) { Direction = ParameterDirection.Output });

                    BLLFactory<Core.BLL.AccPayment>.Instance.ExecStoreProc("up_BillGenerate", param);
                    if (param[param.Count - 1].Value.ToString() == "0")
                    {
                        result.Success = true;
                    }
                    else
                    {
                        result.ErrorMessage = "操作失败!错误如下:" + param[param.Count - 1].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = "操作失败!错误如下:" + ex.Message;
            }
            return ToJsonContent(result);
        }


        //预存款销账
        public ActionResult DepositAccount()
        {
            return View();
        }

        //预存款销账
        [HttpPost]
        public ActionResult DepositAccount(int custNo)
        {
            CommonResult result = new CommonResult();
            try
            {
                //up_DepositWriteoff(预付费模式预存款销账)
                //CREATEPROCEDURE[dbo].[up_DepositWriteoff]
                //@iCustNo INTEGER = 1, --< !--客户编号-- >
                //@sUserID VARCHAR(8),--< !--操作员编号-- >
                //@sReturn VARCHAR(MAX)OUTPUT

                lock (objLock)
                {
                    //System.Threading.Thread.Sleep(3000); 
                    List<SqlParameter> param = new List<SqlParameter>();
                    param.Add(new SqlParameter("@iCustNo", SqlDbType.Int) { Value = custNo });
                    param.Add(new SqlParameter("@sUserID", SqlDbType.VarChar, 8) { Value = UserInfo.ID });
                    param.Add(new SqlParameter("@sReturn", SqlDbType.VarChar, 256) { Direction = ParameterDirection.Output });

                    BLLFactory<Core.BLL.AccPayment>.Instance.ExecStoreProc("up_DepositWriteoff", param);
                    if (param[param.Count - 1].Value.ToString() == "0")
                    {
                        result.Success = true;
                    }
                    else
                    {
                        result.ErrorMessage = "操作失败!错误如下:" + param[param.Count - 1].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = "操作失败!错误如下:" + ex.Message;
            }
            return ToJsonContent(result);
        }


        //预付费模式批量销账
        [HttpPost]
        public ActionResult DepositAccountBatch(int yearMonth)
        {
            CommonResult result = new CommonResult();
            try
            {
                //预付费模式批量销账
                //CREATEPROCEDURE[dbo].[up_DepositWriteoffBatch]
                //@iYearMonth INTEGER = 0,  --< !--欲销账费用的费用年月，如果为0，则尝试销账所有欠费-- >
                //@sUserID    VARCHAR(8),--< !--操作员编号-- >
                //@sReturn    VARCHAR(MAX)OUTPUT

                lock (objLock)
                {
                    //System.Threading.Thread.Sleep(3000); 
                    List<SqlParameter> param = new List<SqlParameter>();
                    param.Add(new SqlParameter("@iYearMonth", SqlDbType.Int) { Value = yearMonth });
                    param.Add(new SqlParameter("@sUserID", SqlDbType.VarChar, 8) { Value = UserInfo.ID });
                    param.Add(new SqlParameter("@sReturn", SqlDbType.VarChar, 256) { Direction = ParameterDirection.Output });

                    BLLFactory<Core.BLL.AccPayment>.Instance.ExecStoreProc("up_DepositWriteoffBatch", param);
                    if (param[param.Count - 1].Value.ToString() == "0")
                    {
                        result.Success = true;
                    }
                    else
                    {
                        result.ErrorMessage = "操作失败!错误如下:" + param[param.Count - 1].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = "操作失败!错误如下:" + ex.Message;
            }
            return ToJsonContent(result);
        }
    }
}
