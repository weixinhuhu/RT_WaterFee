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

    public class ArcMeterReadingController : BusinessController<Core.BLL.ArcMeterReading, Core.Entity.ArcMeterReading>
    {

        public ActionResult GetApproveDataJson()
        {
            base.CheckAuthorized(AuthorizeKey.ListKey);

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
            }

            //Json格式的要求{total:22,rows:{}}
            //构造成Json的格式传递
            var result = new { total = pagerInfo.RecordCount, rows = list };

            return ToJsonContentDate(result);
        }

        // GET: ArcMeterReading
        public ActionResult List()
        {
            return View();
        }

        public ActionResult ListJson()
        {
            string where = "";
            string sql = "";
            string pageSizeNum = Request["pageNum"];
            string pageSize = Request["pageSize"];

            string WHC_VcAddr = Request["WHC_VcAddr"];
            string WHC_IntCustNo = Request["WHC_IntCustNo"];
            string WHC_StartDteFreeze = Request["WHC_StartDteFreeze"];
            string WHC_EndDteFreeze = Request["WHC_EndDteFreeze"];
            string NvcVillage = Request["NvcVillage"];
            string VcBuilding = Request["VcBuilding"];

            if (NvcVillage != "")
            {
                if (NvcVillage == "所有小区")
                {
                    where += @"  AND NvcVillage =  " + "'" + VcBuilding + "'";
                }
                else
                {
                    where += @"  AND NvcVillage =  " + "'" + NvcVillage + "'";

                    if (VcBuilding != "")
                    {
                        where += @"  AND VcBuilding =  " + "'" + VcBuilding + "'";

                    }
                }
            }

            if (WHC_VcAddr != "")
            {
                where += "  and NvcName like" + "'%" + WHC_VcAddr + "%'";
                where += " OR VcMobile like" + "'%" + WHC_VcAddr + "%'";
                where += " OR NvcVillage like" + "'%" + WHC_VcAddr + "%'";
                where += " OR VcBuilding like" + "'%" + WHC_VcAddr + "%'";
                where += " OR IntUnitNum like'" + WHC_VcAddr + "'";
                where += " OR IntRoomNum like" + "'%" + WHC_VcAddr + "%'";
                where += " OR IntCustNo like" + "'%" + WHC_VcAddr + "%'";
                where += " OR VcAddr like" + "'%" + WHC_VcAddr + "%'";
            }

            if (WHC_StartDteFreeze != "")
            {
                where += "  and DtLastUpd between  '" + WHC_StartDteFreeze + "' and '" + WHC_EndDteFreeze + "'";
            }

            //20190312
            sql = @" SELECT a.IntID ,
		                NvcName,
		                VcMobile,
		                NvcVillage,
		                VcBuilding,
		                IntUnitNum, 
		                IntRoomNum,
                        VcAddr ,
                        IntCustNo ,
                        DteReading ,
                        a.DteFreeze ,
                        NumReading ,
                        VcStatus ,
                        a.IntStatus ,
                        DtLastUpd ,
                        a.DtCreate ,
                        dbo.uf_TransStatusWord(VcStatus) Word ,
                        b.VcDesc
                 FROM   ArcMeterReading a
                        LEFT JOIN DictMeterReadingFlag b ON a.IntFlag = b.IntCode
                        LEFT JOIN dbo.ArcCustomerInfo c ON a.IntCustNo = c.IntNo
                 WHERE  ( 1 = 1 ) ";

            sql += where + "order by DteFreeze desc";


            var dts = BLLFactory<Core.BLL.ArcMeterInfo>.Instance.SqlTable(sql);
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
            var result = new { total = total, rows = dat };
            return ToJsonContentDate(result);
        }

        public ActionResult CollectStat()
        {
            return View();
        }

        public ActionResult CollectStatJson()
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);

            var start = Request["WHC_DteFreezeStart"];

            var end = Request["WHC_DteFreezeEnd"];

            var sb = new System.Text.StringBuilder();
            sb.Append("select max(NumReading)-min(NumReading) Reading,IntCustNo from ArcMeterReading where 1=1 ");
            if (start.IsDateTime())
                sb.AppendFormat(" and DteFreeze>='{0}' ", start);
            if (end.IsDateTime())
                sb.AppendFormat(" and DteFreeze<='{0}' ", end);
            sb.Append(" group by IntCustNo");

            var dt = BLLFactory<Core.DALSQL.ArcMeterReading>.Instance.SqlTable(sb.ToString());
            var list = new List<CollectStatModel>();
            var m = new CollectStatModel();
            foreach (DataRow item in dt.Rows)
            {
                m.Used += item["Reading"].ToString().ToDecimalOrZero();
            }
            m.Count = dt.Rows.Count;
            list.Add(m);

            //Json格式的要求{total:22,rows:{}}
            //构造成Json的格式传递
            var result = new { total = list.Count, rows = list };
            return ToJsonContentDate(result);
        }

        public class CollectStatModel
        {
            public int Count { get; set; }
            public decimal Used { get; set; }
        }



        public ActionResult CollectChart()
        {
            return View();
        }

        public ActionResult CollectChartJson()
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);

            //string where = GetPagerCondition();
            //PagerInfo pagerInfo = GetPagerInfo();
            //var list = baseBLL.FindWithPager(where, pagerInfo);

            var start = Convert.ToDateTime(Request["dtStart"]);
            var end = Convert.ToDateTime(Request["dtEnd"]);

            var sb = new System.Text.StringBuilder();
            sb.Append("select sum(IntReading) IntReading,CONVERT(DteFreeze,char(10)) Date from ArcMeterReading where 1=1 ");
            //if (start.IsDateTime())
            sb.AppendFormat(" and DteFreeze>='{0}' ", start.ToYyyyMMdd());
            //if (end.IsDateTime())
            sb.AppendFormat(" and DteFreeze<='{0}' ", end.ToYyyyMMdd());
            sb.Append(" group by CONVERT(DteFreeze,char(10))");
            sb.Append(" order by Date  ");

            var dt = BLLFactory<Core.DALSQL.ArcMeterReading>.Instance.SqlTable(sb.ToString());
            var chart = new ChartDataSimple()
            {
                categories = new List<string>(),
                series = new List<ChartDataSimple.SeriesItem>()
            };
            var si = new ChartDataSimple.SeriesItem() { data = new List<decimal?>(), name = "用量" };
            for (var now = start; now <= end; now = now.AddDays(1))
            {
                chart.categories.Add(now.ToYyyyMMdd());
                var rows = dt.Select("date='{0}'".FormatWith(now.ToYyyyMMdd()));
                if (rows.Count() > 0) si.data.Add(rows[0]["IntReading"].ToString().ToDecimalOrZero());
                else si.data.Add(0);
            }
            chart.series.Add(si);

            return ToJsonContentDate(chart);
        }


        public ActionResult ShowListByConcentratorIdJson(string id)
        {

            return base.FindWithPager();
        }

        public ActionResult Insert()
        {
            return View();
        }

        [HttpPost]
        public override ActionResult Insert(Core.Entity.ArcMeterReading info)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.InsertKey);

            CommonResult result = new CommonResult();
            try
            {
                info.IntStatus = 0;
                info.DtCreate = DateTime.Now;
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
        public ActionResult Update(Core.Entity.ArcMeterReading info)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.UpdateKey);

            CommonResult result = new CommonResult();
            try
            {
                info.VcStatus = info.VcStatus ?? string.Empty;
                result.Success = baseBLL.Update(info, info.IntID);
            }
            catch (Exception ex)
            {
                LogTextHelper.Error(ex);//错误记录
                result.ErrorMessage = ex.Message;
            }
            return ToJsonContent(result);
        }
        //冼工定义
        //public class ZhaoCeModel
        //{
        //    public int IntCustNo { get; set; }
        //    public decimal Amount { get; set; }
        //    public string VcAddr { get; set; }
        //    public int IntMP { get; set; }
        //    public DateTime Date { get; set; }

        //}

        public class ZhaoCeModel
        {
            public int IntCustNo { get; set; }
            public decimal Amount { get; set; }
            public string VcAddr { get; set; }
            public int IntMP { get; set; }
            public DateTime Date { get; set; }
            public string SW { get; set; }
            public string Status { get; set; }
            public string StatusWord { get; set; }
            public int IntFlag { get; set; }

        }

        /// <summary>
        /// 保存召测的抄表数据
        /// </summary>
        /// <returns></returns>
        /// 
        #region 冼工定义
        //[HttpPost]
        //public ActionResult SaveZhaoCeData()
        //{
        //    CommonResult result = new CommonResult();
        //    var okCount = 0;
        //    var errCount = 0;
        //    //var dbTran = BLLFactory<Core.DALSQL.ArcMeterReading>.Instance.CreateTransaction();
        //    try
        //    {
        //        var json = Request["postData"];
        //        var list = DBLib.Common.Json.JsonHelper.DeserializeObject<List<ZhaoCeModel>>(json);
        //        foreach (var item in list)
        //        {

        //            var reading = BLLFactory<Core.DALSQL.ArcMeterReading>.Instance
        //                .FindSingle("VcAddr='{0}' and IntCustNo={1} and DteReading='{2}' and NumReading={3}  "
        //                .FormatWith(item.VcAddr, item.IntCustNo, item.Date, item.Amount));

        //            try
        //            {
        //                if (reading == null)
        //                {
        //                    reading = new Core.Entity.ArcMeterReading();
        //                    reading.DtCreate = DateTime.Now;
        //                    reading.DteFreeze = item.Date;
        //                    reading.DteReading = reading.DteFreeze;
        //                    reading.DtLastUpd = DateTime.Now;
        //                    reading.IntCustNo = item.IntCustNo;
        //                    reading.NumReading = item.Amount;
        //                    reading.IntStatus = 0;
        //                    reading.VcAddr = item.VcAddr;
        //                    reading.VcStatus = string.Empty;
        //                   // reading.VcStatus = item.VcStatus;
        //                    var flg = BLLFactory<Core.DALSQL.ArcMeterReading>.Instance.Insert(reading);
        //                    //if (flg)
        //                    //{
        //                    //    var condition = "VcAddr='{0}' ".FormatWith(item.VcAddr);
        //                    //    var calcReading = BLLFactory<Core.DALSQL.ArcCalcReading>.Instance.FindSingle(condition);
        //                    //    if (calcReading == null)
        //                    //    {
        //                    //        calcReading = new Core.Entity.ArcCalcReading();
        //                    //        calcReading.NumPrior = calcReading.NumLast = 0;
        //                    //    }

        //                    //    var calc = new Core.Entity.ArcCalcReading();
        //                    //    calc.NumLast = item.Amount;
        //                    //    calc.NumPrior = calcReading.NumLast;
        //                    //    calc.IntStatus = 0;
        //                    //    calc.NumUsed = calc.NumLast - calc.NumPrior;
        //                    //    calc.IntYearMon = item.Date.ToString("yyyyMM").ToInt();
        //                    //    calc.VcAddr = item.VcAddr;
        //                    //    calc.DtCreate = DateTime.Now;
        //                    //    calc.DteFee = item.Date;

        //                    //    flg = BLLFactory<Core.DALSQL.ArcCalcReading>.Instance.Insert(calc);

        //                    //    //if (flg) dbTran.Commit();
        //                    //    //else dbTran.Rollback();
        //                    //}
        //                    if (flg) okCount++;
        //                    else errCount++;
        //                }
        //                else okCount++;
        //            }
        //            catch (Exception ex)
        //            {
        //                errCount++;
        //                //dbTran.Rollback();
        //                //throw ex;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //dbTran.Rollback();
        //        LogTextHelper.Error(ex);//错误记录
        //        result.ErrorMessage = ex.Message;
        //    }
        //    if (okCount == 0)
        //    {
        //        result.ErrorMessage = "操作失败!";
        //    }
        //    else if (errCount == 0)
        //    {
        //        result.Success = true;
        //        result.ErrorMessage = "操作成功!";
        //    }
        //    else
        //    {
        //        result.Success = true;
        //        result.ErrorMessage = "操作部分成功!成功{0}条,失败{1}条!".FormatWith(okCount, errCount);
        //    }
        //    return ToJsonContent(result);
        //}
        #endregion
        [HttpPost]
        public ActionResult SaveZhaoCeData()
        {
            CommonResult result = new CommonResult();
            var okCount = 0;
            var errCount = 0;
            //var dbTran = BLLFactory<Core.DALSQL.ArcMeterReading>.Instance.CreateTransaction();
            try
            {
                var json = Request["postData"];
                var list = DBLib.Common.Json.JsonHelper.DeserializeObject<List<ZhaoCeModel>>(json);
                foreach (var item in list)
                {

                    var reading = BLLFactory<Core.DALSQL.ArcMeterReading>.Instance
                        .FindSingle("VcAddr='{0}' and IntCustNo={1} and DteReading='{2}' and NumReading={3}and Vcstatus ='{4}'and IntFlag={5} "
                        .FormatWith(item.VcAddr, item.IntCustNo, item.Date, item.Amount, item.SW, item.IntFlag));
                    if (item.IntFlag == 0)
                    {
                        var sql = "Update ArcMeterInfo set IntValveState=uf_TransValveStatus('" + item.SW + "') where VcAddr='" + item.VcAddr + "'";
                        var meterInfo = BLLFactory<Core.DALSQL.ArcMeterInfo>.Instance.SqlExecute(sql);
                    }
                    try
                    {
                        if (reading == null)
                        {

                            reading = new Core.Entity.ArcMeterReading();
                            reading.DtCreate = DateTime.Now;
                            reading.DteFreeze = item.Date;


                            reading.IntFlag = item.IntFlag;

                            reading.DteReading = reading.DteFreeze;
                            reading.DtLastUpd = DateTime.Now;
                            reading.IntCustNo = item.IntCustNo;
                            reading.NumReading = item.Amount;
                            reading.IntStatus = 0;
                            reading.VcAddr = item.VcAddr;
                            reading.VcStatus = item.SW;
                            // reading.VcStatus = string.Empty;

                            //if (item.VcStatus == null)
                            //    item.VcStatus = "正常";
                            //reading.VcStatus = item.VcStatus;
                            var flg = BLLFactory<Core.DALSQL.ArcMeterReading>.Instance.Insert(reading);
                            //if (flg)
                            //{
                            //    var condition = "VcAddr='{0}' ".FormatWith(item.VcAddr);
                            //    var calcReading = BLLFactory<Core.DALSQL.ArcCalcReading>.Instance.FindSingle(condition);
                            //    if (calcReading == null)
                            //    {
                            //        calcReading = new Core.Entity.ArcCalcReading();
                            //        calcReading.NumPrior = calcReading.NumLast = 0;
                            //    }

                            //    var calc = new Core.Entity.ArcCalcReading();
                            //    calc.NumLast = item.Amount;
                            //    calc.NumPrior = calcReading.NumLast;
                            //    calc.IntStatus = 0;
                            //    calc.NumUsed = calc.NumLast - calc.NumPrior;
                            //    calc.IntYearMon = item.Date.ToString("yyyyMM").ToInt();
                            //    calc.VcAddr = item.VcAddr;
                            //    calc.DtCreate = DateTime.Now;
                            //    calc.DteFee = item.Date;

                            //    flg = BLLFactory<Core.DALSQL.ArcCalcReading>.Instance.Insert(calc);

                            //    //if (flg) dbTran.Commit();
                            //    //else dbTran.Rollback();
                            //}
                            if (flg) okCount++;
                            else errCount++;
                        }
                        else okCount++;
                    }
                    catch (Exception ex)
                    {
                        errCount++;
                        //dbTran.Rollback();
                        //throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                //dbTran.Rollback();
                LogTextHelper.Error(ex);//错误记录
                result.ErrorMessage = ex.Message;
            }
            if (okCount == 0)
            {
                result.ErrorMessage = "操作失败!";
            }
            else if (errCount == 0)
            {
                result.Success = true;
                result.ErrorMessage = "操作成功!";
            }
            else
            {
                result.Success = true;
                result.ErrorMessage = "操作部分成功!成功{0}条,失败{1}条!".FormatWith(okCount, errCount);
            }
            return ToJsonContent(result);
        }


        /// <summary>
        /// 换表查询
        /// </summary>
        /// <returns></returns>
        /// 
        public ActionResult ChangeTable()
        {

            return View();
        }

        public ActionResult ChangeTables()
        {

            string where = "";
            string sql = "";


            string IntCustNO = Request["WHC_IntCustNO"];
            string NvcName = Request["WHC_NvcName"];
            string VcAddr = Request["WHC_VcAddr"];
            string NvcAddr = Request["WHC_NvcAddr"];

            string NvcVillage = Request["NvcVillage"];
            string VcBuilding = Request["VcBuilding"];


            if (NvcVillage != "")
            {
                if (NvcVillage == "所有小区")
                {
                    where += @"  AND NvcVillage =  " + "'" + VcBuilding + "'";
                }
                else
                {
                    where += @"  AND NvcVillage =  " + "'" + NvcVillage + "'";

                    if (VcBuilding != "")
                    {
                        where += @"  AND VcBuilding =  " + "'" + VcBuilding + "'";

                    }
                }
            }

            if (IntCustNO != null)
            {
                if (IntCustNO != "")
                {
                    where += " and IntCustNo='" + IntCustNO + "' ";
                }
                if (NvcName != "")
                {
                    where += " and NvcName='" + NvcName + "' ";
                }
                if (VcAddr != "")
                {
                    where += " and VcAddr='" + VcAddr + "'";
                }
                if (NvcAddr != "")
                {
                    where += " and NvcAddr  like '%" + NvcAddr + "%'";
                }
            }
            where += " order by DtLastUpd ";

            sql = @"SELECT a.IntID ,
                            VcAddr ,
                            a.NvcName ,
                            a.NvcAddr ,
                            NvcVillage,
                            VcBuilding,
                            IntUnitNum,
                            IntRoomNum,
                            c.VcDesc AS IntValveState ,
                            IntCustNO ,
                            b.VcDesc AS IntStatus ,
                            DtLastUpd ,
                            a.DtCreate
                     FROM   ArcMeterInfo a
                            LEFT JOIN DictConcentStatus b ON a.IntStatus = b.IntCode
                            LEFT JOIN DictValveStatus c ON a.IntValveState = c.IntCode
                            LEFT JOIN dbo.ArcCustomerInfo d ON a.IntCustNO = d.IntNo
                     WHERE  1=1";
                                
            sql += where;


            var dts = BLLFactory<Core.DALSQL.ArcMeterInfo>.Instance.SqlTable(sql);
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
            var result = new { total = total, rows = dat };
            return ToJsonContentDate(result);
            //return View();
        }

        public ActionResult ArcMeterChange()
        {
            var IntCustNo = Request["IntCustNo"];
            var VcAddrOld = Request["VcAddrOld"];
            var VcAddrNew = Request["VcAddrNew"];
            var NumReading = Request["NumReading"];
            var IntType = Request["IntType"];
            var NVcDesc = Request["NVcDesc"];
            var VcUserID = Request["VcUserID"];
            var DteChange = Request["DteChange"];
            var DtCreate = Request["DtCreate"];

            string sql = "insert into ArcMeterChange (IntCustNo,VcAddrOld,VcAddrNew,NumReading,IntType,NVcDesc,VcUserID,DteChange,DtCreate) values (" + IntCustNo + ",'" + VcAddrOld + "','" + VcAddrNew + "','" + NumReading + "'," + IntType + ",'" + NVcDesc + "','" + VcUserID + "','" + DteChange + "','" + DtCreate + "')";

            var num = PROJECT.SqlHelper.ExecteNonQueryText(sql);



            return ToJsonContentDate(num);


        }


        public ActionResult ManualMeterReading()
        {

            return View();
        }
        [HttpPost]
        public ActionResult WriteNumReading()
        {

            CommonResult result = new CommonResult();
            result.Success = false;
            List<SqlParameter> lichange = new List<SqlParameter>();

            var IntCustNo = Request["IntCustNo"];
            var VcAddr = Request["VcAddr"];
            var DteFreeze = Request["DteFreeze"];
            var NumReading = Request["NumReading"];
            //需改参数
            lichange.Add(new SqlParameter("@iCustNo", SqlDbType.Int) { Value = IntCustNo });
            lichange.Add(new SqlParameter("@sMeterAddr", SqlDbType.VarChar, 16) { Value = VcAddr });
            lichange.Add(new SqlParameter("@DteFreeze", SqlDbType.VarChar, 64) { Value = DteFreeze });
            lichange.Add(new SqlParameter("@fReading", SqlDbType.NVarChar, 16) { Value = NumReading });
            lichange.Add(new SqlParameter("@sReturn", SqlDbType.VarChar, 64) { Direction = ParameterDirection.Output });
            //需改存储过程名
            BLLFactory<Core.BLL.ArcMeterReading>.Instance.ExecStoreProc("up_SaveReading_Track", lichange);


            if (lichange[4].Value.ToString() == "0")
            {
                result.Success = true;
            }
            else
            {
                result.ErrorMessage = "执行up_ImportArchive存储过程出错!错误如下:" + lichange[4].Value.ToString();
            }
            return ToJsonContent(result);

        }

    }
}
