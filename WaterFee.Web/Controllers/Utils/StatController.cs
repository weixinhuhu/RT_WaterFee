using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;
using WHC.MVCWebMis.Controllers;

namespace WHC.WaterFeeWeb.Controllers
{
    /// <summary>
    /// 查询统计
    /// </summary>
    public class StatController : BusinessControllerNew<Core.BLL.ArcCustomerInfo, Core.Entity.ArcCustomerInfo>
    {
        // GET: 用水量统计
        public ActionResult StatUsedWater()
        {
            return View();
        }

        public ActionResult StatUsedWaterData()
        {
            CommonResult result = new CommonResult();
            try
            {
                //用水量统计
                //CREATEPROCEDURE[dbo].[up_StatUsedWater]
                //@iParamMode INTEGER,   --< !--参数模式0:全部；1:户号；2:小区；3:集中器地址-- >
                //@sParamValue VARCHAR(MAX),--< !--参数值,调用接口保证参数的正确性-- >
                //@VcBeginDate VARCHAR(10),--< !--水量开始日期，格式"YYYY-MM-DD"-- >
                //@VcEndDate   VARCHAR(10),--< !--水量截止日期，格式"YYYY-MM-DD"-- >
                //@sReturn     VARCHAR(MAX)OUTPUT-- < !--0:成功，其它为错误描述-- >

                var paramMode = RRequest("paramMode");
                var paramValue = RRequest("paramValue");
                var dtStart = RRequest("dtStart");
                var dtEnd = RRequest("dtEnd");
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@iParamMode", SqlDbType.Int) { Value = paramMode });
                param.Add(new SqlParameter("@sParamValue", SqlDbType.VarChar, 256) { Value = paramValue });
                param.Add(new SqlParameter("@VcBeginDate", SqlDbType.VarChar, 10) { Value = dtStart });
                param.Add(new SqlParameter("@VcEndDate", SqlDbType.VarChar, 10) { Value = dtEnd });
                param.Add(new SqlParameter("@sReturn", SqlDbType.VarChar, 1024) { Direction = ParameterDirection.Output });

                var ds = BLLFactory<Core.BLL.AccPayment>.Instance.ExecStoreProcToDataSet("up_StatUsedWater", param);
                var ret = param[param.Count - 1].Value.ToString();

                if (ret == "0")
                {
                    var obj = new { total = ds.Tables[0].Rows.Count, rows = ds.Tables[0] };
                    //result.Data1 = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0]);
                    //result.Obj1 = obj;
                    //result.Success = true;
                    return ToJsonContentDate(obj);
                }
                else
                {
                    result.ErrorMessage = "查询失败!错误如下:" + ret;
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = "查询失败!错误如下:" + ex.Message;
            }
            return ToJsonContentDate(result);
        }


        // GET: 客户档案统计
        public ActionResult StatClientArc()
        {
            return View();
        }

        //客户档案统计
        public ActionResult StatClientArcData()
        {
            CommonResult result = new CommonResult();
            try
            {
                //客户档案统计
                //CREATEPROCEDURE[dbo].[up_StatClientArc]
                //@VcBeginDate VARCHAR(10), --< !--建户开始日期，格式"YYYY-MM-DD"-- >
                //@VcEndDate   VARCHAR(10), --< !--建户截止日期，格式"YYYY-MM-DD"-- >
                //@sReturn     VARCHAR(MAX)OUTPUT-- < !--0:成功，其它为错误描述-- >

                var dtStart = RRequest("dtStart");
                var dtEnd = RRequest("dtEnd");
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@VcBeginDate", SqlDbType.VarChar, 10) { Value = dtStart });
                param.Add(new SqlParameter("@VcEndDate", SqlDbType.VarChar, 10) { Value = dtEnd });
                param.Add(new SqlParameter("@sReturn", SqlDbType.VarChar, 1024) { Direction = ParameterDirection.Output });

                var ds = BLLFactory<Core.BLL.AccPayment>.Instance.ExecStoreProcToDataSet("up_StatClientArc", param);
                var ret = param[param.Count - 1].Value.ToString();

                if (ret == "0")
                {
                    var obj = new { total = ds.Tables[0].Rows.Count, rows = ds.Tables[0] };
                    //result.Data1 = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0]);
                    //result.Obj1 = obj;
                    //result.Success = true;
                    return ToJsonContentDate(obj);
                }
                else
                {
                    result.ErrorMessage = "查询失败!错误如下:" + ret;
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = "查询失败!错误如下:" + ex.Message;
            }
            return ToJsonContentDate(result);
        }


        //预存流水账统计
        public ActionResult StatDeposit()
        {
            return View();
        }

        //预存流水账统计
        public ActionResult StatDepositData()
        {
            CommonResult result = new CommonResult();
            try
            {
                //预存流水账统计
                //CREATEPROCEDURE[dbo].[up_DepositStat]
                //@iParamMode INTEGER,   --< !--参数模式0:全部；1:户号；2:采集器；3:小区-- >
                //@sParamValue VARCHAR(64),--< !--参数值,调用接口保证参数的正确性-- >
                //@iStatMode   INTEGER,--< !--统计模式0:预存款账户；1:预存款流水-- >
                //@DteBegin    DATE,--< !--开始日期-- >
                //@DteEnd DATE,--< !--截至日期-- >
                //@sSumCount  VARCHAR(8)OUTPUT,--< !--符合条件预存总户数-- >
                //@sSumAmount VARCHAR(16)OUTPUT,--< !--符合条件预存总额-- >
                //@sReturn    VARCHAR(MAX)OUTPUT

                var paramMode = RRequest("paramMode");
                var paramValue = RRequest("paramValue");
                var dtStart = RRequest("dtStart");
                var dtEnd = RRequest("dtEnd");
                List<SqlParameter> param = new List<SqlParameter>();
                param.Add(new SqlParameter("@iParamMode", SqlDbType.Int) { Value = paramMode });
                param.Add(new SqlParameter("@sParamValue", SqlDbType.VarChar, 256) { Value = paramValue });
                param.Add(new SqlParameter("@iStatMode", SqlDbType.Int) { Value = 1 });
                param.Add(new SqlParameter("@DteBegin", SqlDbType.Date) { Value = dtStart });
                param.Add(new SqlParameter("@DteEnd", SqlDbType.Date) { Value = dtEnd });
                param.Add(new SqlParameter("@sSumCount", SqlDbType.VarChar, 8) { Direction = ParameterDirection.Output });
                param.Add(new SqlParameter("@sSumAmount", SqlDbType.VarChar, 16) { Direction = ParameterDirection.Output });
                param.Add(new SqlParameter("@sReturn", SqlDbType.VarChar, 1024) { Direction = ParameterDirection.Output });

                var ds = BLLFactory<Core.BLL.AccPayment>.Instance.ExecStoreProcToDataSet("up_DepositStat", param);
                var ret = param[param.Count - 1].Value.ToString();

                if (ret == "0")
                {
                    var obj = new { total = ds.Tables[0].Rows.Count, rows = ds.Tables[0] };
                    //result.Data1 = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0]);
                    //result.Obj1 = obj;
                    //result.Success = true;
                    return ToJsonContentDate(obj);
                }
                else
                {
                    result.ErrorMessage = "查询失败!错误如下:" + ret;
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = "查询失败!错误如下:" + ex.Message;
            }
            return ToJsonContentDate(result);
        }
    }
}