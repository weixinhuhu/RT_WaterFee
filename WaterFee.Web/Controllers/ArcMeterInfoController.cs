using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WHC.Attachment.BLL;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;
using WHC.MVCWebMis.Controllers;
using WHC.Pager.Entity;


namespace WHC.WaterFeeWeb.Controllers
{
    public class ArcMeterInfoController : BusinessController<Core.BLL.ArcMeterInfo, Core.Entity.ArcMeterInfo>
    {
        // GET: CustomerInfo
        public ActionResult List()
        {
            ViewBag.Title = "电表列表";
            return View();
        }

        public ActionResult GetTreeJson()
        {
            var list = BLLFactory<Core.BLL.ArcMeterInfo>.Instance.GetAll();
            return ToJsonContent(list);
        }

        public ActionResult ListJson()
        {
            string where = "";
            string sql = "";
            string pageSize = Request["pageSize"];
            //增加一个CustomedCondition条件，根据客户这个条件进行查询
            string CustomedCondition = Request["CustomedCondition"] ?? "";
            if (!string.IsNullOrWhiteSpace(CustomedCondition))
            {
                where = CustomedCondition;//直接使用条件
            }
            else
            {
                #region 根据数据库字段列，对所有可能的参数进行获值，然后构建查询条件
                SearchCondition condition = new SearchCondition();
                DataTable dt = baseBLL.GetFieldTypeList();
                foreach (DataRow dr in dt.Rows)
                {
                    string columnName = dr["ColumnName"].ToString();
                    string dataType = dr["DataType"].ToString();

                    //字段增加WHC_前缀字符，避免传递如URL这样的Request关键字冲突
                    string columnValue = Request["WHC_" + columnName] ?? "";
                    columnName = "a." + columnName;
                    //对于数值型，如果是显示声明相等的，一般是外键引用，需要特殊处理
                    bool hasEqualValue = columnValue.StartsWith("=");

                    if (IsDateTime(dataType))
                    {
                        condition.AddDateCondition(columnName, columnValue);
                    }
                    else if (IsNumericType(dataType))
                    {
                        //如果数据库是数值类型，而传入的值是true或者false,那么代表数据库的参考值为1,0，需要进行转换
                        bool boolValue = false;
                        bool isBoolenValue = bool.TryParse(columnValue, out boolValue);
                        if (isBoolenValue)
                        {
                            condition.AddCondition(columnName, boolValue ? 1 : 0, SqlOperator.Equal);
                        }
                        else if (hasEqualValue)
                        {
                            columnValue = columnValue.Substring(columnValue.IndexOf("=") + 1);
                            condition.AddCondition(columnName, columnValue, SqlOperator.Equal);
                        }
                        else
                        {
                            condition.AddNumberCondition(columnName, columnValue);
                        }
                    }
                    else
                    {
                        if (ValidateUtil.IsNumeric(columnValue))
                        {
                            condition.AddCondition(columnName, columnValue, SqlOperator.Equal);
                        }
                        else
                        {
                            condition.AddCondition(columnName, columnValue, SqlOperator.Like);
                        }
                    }
                }
                #endregion

                #region MyRegion
                //string SystemType_ID = Request["SystemType_ID"] ?? "";
                //string LoginName = Request["LoginName"] ?? "";
                //string FullName = Request["FullName"] ?? "";
                //string Note = Request["Note"] ?? "";
                //string IPAddress = Request["IPAddress"] ?? "";
                //string MacAddress = Request["MacAddress"] ?? "";
                //string LastUpdated = Request["LastUpdated"] ?? "";

                //SearchCondition condition = new SearchCondition();
                //condition.AddCondition("SystemType_ID", SystemType_ID, SqlOperator.Like);
                //condition.AddCondition("LoginName", LoginName, SqlOperator.Like);
                //condition.AddCondition("FullName", FullName, SqlOperator.Like);
                //condition.AddCondition("Note", Note, SqlOperator.Like);
                //condition.AddCondition("IPAddress", IPAddress, SqlOperator.Like);
                //condition.AddCondition("MacAddress", MacAddress, SqlOperator.Like);

                //condition.AddDateCondition("LastUpdated", LastUpdated); 
                #endregion

                sql = " SELECT ROW_NUMBER() OVER ( order by a.IntID DESC) as RowNumber,  a.IntID,a.VcAddr,a.NvcName,a.NvcAddr,a.VcBarCode,a.VcAssetNo,a.IntProtocol,a.IntCycle,a.IntOrig,a.IntReadFlag,a.IntValveState,a.IntConID ,b.NvcName as NvcNames,c.NvcDesc,a.IntMP,a.IntCustNO,a.IntStatus,a.DtLastUpd,a.DtCreate,a.IntAutoSwitch,a.IntOnline,a.DtOnline,a.IntPriceNo,a.IntTopConID  FROM ArcMeterInfo a,ArcConcentratorInfo b ,PriceProperty c Where ";
                where = condition.BuildConditionSql().Replace("Where", "");
                sql = sql + where + " and a.IntTopConID = b.IntID and c.IntNo=a.IntPriceNo ";


            }
            var dts = BLLFactory<Core.BLL.ArcMeterInfo>.Instance.SqlTable(sql);
            return ToJsonContentDate(dts);
            // return base.FindWithPager();
        }
  

        public ActionResult ListByIntConID(string WHC_IntConID)
        {
         
            var sb = new System.Text.StringBuilder();
            sb.Append(" SELECT B.IntID 编号, B.Repeater1,B.Repeater2,B.Repeater3,A.* ");
            sb.Append(" ,C.NvcAddr VcCustAddr,C.NvcName VcCustName ");
            sb.AppendFormat(" FROM uf_GetDeviceID({0}) B, ArcMeterInfo A ", WHC_IntConID);
            sb.AppendFormat(" left join ArcCustomerInfo C on C.IntNo=A.IntCustNo ");
            sb.Append(" WHERE A.IntConID = B.IntID ORDER BY A.IntMP ");
            var dt = BLLFactory<Core.BLL.ArcCustomerInfo>.Instance.SqlTable(sb.ToString());
            return ToJsonContentDate(dt);
        }

        public ActionResult ListByIntConID_Submit()
        {
            string IntCustNO = Request["IntCustNO"];
            string NvcName = Request["NvcName"];
            string NvcAddr = Request["NvcAddr"];
            string VcAddr = Request["VcAddr"];
            string vcFlag = Request["flag"];
            string NvcVillage = Request["NvcVillage"];
            string VcBuilding = Request["VcBuilding"];

            string where = " (1=1) ";

            if (NvcVillage != "")
            {
                where += @"  AND NvcVillage =  " + "'" + NvcVillage + "'";
            }

            if (VcBuilding != "")
            {
                where += @"  AND VcBuilding =  " + "'" + VcBuilding + "'";
            }

            if (IntCustNO != "")
            {
                where += " and IntCustNO=" + Convert.ToInt32(IntCustNO);
            }
            if (NvcName != "")
            {
                where += " and c.NvcName='" + NvcName + "'";
            }
            if (NvcAddr != "")
            {
                where += " and a.NvcAddr like'%" + NvcAddr + "%'";
            }
            if (VcAddr != "")
            {
                where += " and a.VcAddr='" + VcAddr + "'";
            }
            if (vcFlag == "1")
            {
                where += " and MonSum<=0";
            }
            else
            {
                where += " and MonSum>0";
            }

            var sb = new System.Text.StringBuilder();
            sb.Append(" SELECT A.* ");
            sb.Append(" ,NvcVillage,VcBuilding,IntUnitNum,IntRoomNum,C.NvcName VcCustName,MonSum ");
            sb.AppendFormat(" FROM ArcMeterInfo A " );
            sb.AppendFormat(" left join ArcCustomerInfo C on C.IntNo=A.IntCustNo ");
            sb.AppendFormat(" left join AccDeposit D on D.IntCustNO=A.IntCustNo ");
            sb.Append(" WHERE  "+where);
            var dt = BLLFactory<Core.BLL.ArcCustomerInfo>.Instance.SqlTable(sb.ToString());
            return ToJsonContentDate(dt);
        }
        //设置参数
        public ActionResult SettingMangger()
        {
            string where = "";
            string sql = "";
            string pageSizeNum = Request["pageNum"];
            string pageSize = Request["pageSize"];        
            string WHC_IntCustNo = Request["IntCustNO"];
            string NvcVillage = Request["NvcVillage"];
            string VcBuilding = Request["VcBuilding"];

            if (NvcVillage != "") {
                where += @"  AND A.NvcVillage =  "+  "'" + NvcVillage + "'" ;
            }

            if (VcBuilding != "")
            {
                where += @"  AND A.VcBuilding =  " + "'" + VcBuilding + "'";
            }

            if (WHC_IntCustNo != "")
            {
                where += @"  AND A.NvcName LIKE  " + "'%" + WHC_IntCustNo + "%'";
                where += "  OR NvcVillage LIKE  " + "'%" + WHC_IntCustNo + "%'";
                where += "  OR IntUnitNum LIKE  " + "'%" + WHC_IntCustNo + "%'";
                where += "  OR IntRoomNum LIKE  " + "'%" + WHC_IntCustNo + "%'";
                where += "  OR C.VcAddr LIKE  " + "'%" + WHC_IntCustNo + "%'";
                where += "  OR VcMobile LIKE " + "'%" + WHC_IntCustNo + "%'";
                where += "  OR A.IntNo LIKE  " + "'%" + WHC_IntCustNo + "%'";
            }

            //20190312
            sql = @"  SELECT 
                        A.intId ,
                        A.IntNo ,
		                C.VcAddr,
                        A.NvcName,
                        NvcVillage ,
                        VcMobile ,
                        VcBuilding ,
                        IntUnitNum ,
                        IntRoomNum ,
                        B.ChrFreezeDay ,
                        B.ChrValveMaint ,
                        ChrUpTiming ,
                        CASE B.ChrUpTimingUnit
                          WHEN 1 THEN '分'
                          WHEN 2 THEN '小时'
                          WHEN 3 THEN '天'
                        END ChrUpTimingUnit,
                        B.ChrUpAmount ,
                        B.ChrAllowUsed
                 FROM   dbo.ArcCustomerInfo A
		                LEFT JOIN dbo.ArcMeterInfo C ON A.IntNo=C.IntCustNO
                        LEFT JOIN dbo.ArcMeterConfig B ON C.VcAddr = B.VcAddr
                 WHERE  1 = 1";

            sql += where;

           


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

        public ActionResult ListByIntConID_Data(string WHC_IntConID)
        {
           
            var sb = new System.Text.StringBuilder();
            sb.Append(" SELECT B.IntID 编号, B.Repeater1,B.Repeater2,B.Repeater3,A.* ");
            sb.Append(" ,C.NvcAddr VcCustAddr,C.NvcName VcCustName ");
            sb.AppendFormat(" FROM uf_GetDeviceID({0}) B, ArcMeterInfo A ", WHC_IntConID);
            sb.AppendFormat(" left join ArcCustomerInfo C on C.IntNo=A.IntCustNo ");
            sb.Append(" WHERE A.IntConID = B.IntID and a.INtStatus=0  ORDER BY A.IntMP ");
             

            var dt = BLLFactory<Core.BLL.ArcCustomerInfo>.Instance.SqlTable(sb.ToString());
            var listCustNo = new List<string>();
            foreach (DataRow item in dt.Rows)
            {
                var custNo = item["IntCustNo"].ToString();
                if (!listCustNo.Contains(custNo)) listCustNo.Add(custNo);
            }
            //上次抄表用量
            sb = new System.Text.StringBuilder();
            sb.Append("select * from ( ");
            sb.Append("select IntID, VcAddr, IntCustNo, DteReading,DtCreate,uf_TransStatusWord(VcStatus)VcStatus, NumReading,");
            sb.Append("ROW_NUMBER() over(partition by  intcustNo order by DtCreate desc) RowNum ");
            sb.Append("from ArcMeterReading ) T where RowNum=1 and IntCustNo in (0");
            foreach (var no in listCustNo)
            {
                sb.AppendFormat(",{0}", no);
            }
            sb.Append(" ) order by IntID desc ");
            var dtReading = BLLFactory<Core.BLL.ArcCustomerInfo>.Instance.SqlTable(sb.ToString());
            dt.Columns.Add("LastAmount", typeof(decimal));
            dt.Columns.Add("LastCopyTime", typeof(string));
            dt.Columns.Add("StatusWord", typeof(string));
            foreach (DataRow item in dt.Rows)
            {
                var custNo = item["IntCustNo"].ToString();
                var rows = dtReading.Select("IntCustNo=" + custNo);
                if (rows.Count() > 0)
                {
                    item["LastAmount"] = rows[0]["NumReading"];
                    item["LastCopyTime"] = rows[0]["DtCreate"];
                    item["StatusWord"] = rows[0]["VcStatus"];
                }
                else
                {
                    item["LastAmount"] = 0;
                    item["LastCopyTime"] = "";
                }
            }
            //用户余额  
            dt.Columns.Add("MonSum", typeof(decimal));
            if (listCustNo.Count > 0)
            {
                var strWhere = " and AA.IntNo in ({0})".FormatWith(string.Join(",", listCustNo));
                var dtBalance = new AccDepositController().GetMoneyInfo(strWhere);
                foreach (DataRow item in dt.Rows)
                {
                    var custNo = item["IntCustNo"].ToString();
                    var rows = dtBalance.Select("IntCustNo=" + custNo);
                    if (rows.Count() > 0)
                    {
                        item["MonSum"] = rows[0]["MonSum"];
                    }
                    else
                    {
                        item["MonSum"] = 0;
                    }
                }
            }
            return ToJsonContentDate(dt);
        }

        [HttpPost]
        public override ActionResult Insert(Core.Entity.ArcMeterInfo info)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.InsertKey);

            CommonResult result = new CommonResult();
            try
            {

                //判断客户编号和采集器编号是否存在
                if (info.IntCustNO.ToString() != "0")
                {
                    var cust = BLLFactory<Core.BLL.ArcCustomerInfo>.Instance.Find(" IntNO=" + info.IntCustNO);

                    if (cust.Count == 0)
                    {
                        result.ErrorMessage = ERR.ArcMeterInfo.CustomerNoNotExist;
                        return ToJsonContent(result);
                    }
                }
                var concentor = BLLFactory<Core.BLL.ArcConcentratorInfo>.Instance.Find("IntID=" + info.IntConID);
                if (concentor.Count == 0)
                {
                    result.ErrorMessage = ERR.ArcMeterInfo.ConcentratorNoNotExist;
                    return ToJsonContent(result);
                }

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
     

     
        public ActionResult Update(Core.Entity.ArcMeterInfo info)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.UpdateKey);

            CommonResult result = new CommonResult();
            try
            {
                //判断客户编号和采集器编号是否存在
                var cust = BLLFactory<Core.BLL.ArcCustomerInfo>.Instance.Find(" IntNO=" + info.IntCustNO);
                if (cust.Count == 0)
                {
                    result.ErrorMessage = ERR.ArcMeterInfo.CustomerNoNotExist;
                    return ToJsonContent(result);
                }
                //string Sql = "";
                var concentor = BLLFactory<Core.BLL.ArcConcentratorInfo>.Instance.Find("IntID=" + info.IntConID);
                if (concentor.Count == 0)
                {
                    result.ErrorMessage = ERR.ArcMeterInfo.ConcentratorNoNotExist;
                    return ToJsonContent(result);
                }

                //判断计量点是否重复
                var existMP = BLLFactory<Core.BLL.ArcMeterInfo>.Instance
                        .IsExistRecord("IntConID={0} and IntMP={1} and IntID!={2}".FormatWith(info.IntConID, info.IntMP, info.IntID));
                if (existMP)
                {
                    result.ErrorMessage = "同一采集器下的计量点不能重复!";
                    return ToJsonContent(result);
                }

                result.Success = baseBLL.Update(info, info.IntID);
            }
            catch (Exception ex)
            {
                LogTextHelper.Error(ex);//错误记录
                result.ErrorMessage = ex.Message;
            }
            return ToJsonContent(result);
        }


        public ActionResult FindByIntCustNo(string IntCustNo)
        {
            CommonResult result = new CommonResult();
            try
            {
                var listMeterInfo = BLLFactory<Core.BLL.ArcMeterInfo>.Instance.Find(" IntCustNO=" + IntCustNo);

                //var VcAddr = listMeterInfos[1].VcAddr;
                //var sql = "select a.[IntID],a.[VcAddr],a.[NvcName],a.[NvcAddr],a.[VcBarCode],a.[VcAssetNo],a.[IntProtocol],a.[IntCycle],a.[IntOrig],a.[IntReadFlag],b.VcDesc,a.[IntConID],a.[IntChannal],a.[IntMP],a.[IntRate],a.[IntCustNO],a.[IntStatus],a.[DtLastUpd],a.[DtCreate],a.[IntAutoSwitch],a.[IntNetFlag],a.[IntOnline],a.[DtOnline],a.[IntPriceNo],a.[IntTopConID] from[ArcMeterInfo] a,[DictValveStatus] b where b.IntCode=a.IntValveState and a.VcAddr=" + VcAddr;
                //var listMeterInfo = BLLFactory<Core.BLL.ArcMeterInfo>.Instance.SqlTable(sql);

                if (listMeterInfo.Count == 0)
                {
                    result.ErrorMessage = ERR.ArcMeterInfo.ArcMeterInfoNotExist;
                    return ToJsonContent(result);
                }
                result.Obj1 = listMeterInfo.FirstOrDefault(); ;
                result.Success = true;

            }
            catch (Exception ex)
            {
                LogTextHelper.Error(ex);//错误记录
                result.ErrorMessage = ex.Message;
            }
            return ToJsonContent(result);
        }
        [HttpPost]
        public ActionResult ML() {
            var ML = Request["ML"];
            var VcAddr = Request["VcAddr"];
            var Fn = Request["Fn"];
            if (Fn != null && VcAddr != null) { 
            var sql =" update ArcMeterInfo set IntValveState=" + Fn + " where VcAddr= '" + VcAddr + "'";
                var num = BLLFactory<Core.BLL.ArcConcentratorInfo>.Instance.SqlExecute(sql);
            }
           

            return View();
        }


        [HttpPost]
        public ActionResult Import(string guid)
        {
            CommonResult result = new CommonResult();
            result.Success = false;
            try
            {
                var attach = BLLFactory<FileUpload>.Instance.GetByAttachGUID(guid).FirstOrDefault();
                if (attach != null)
                {
                    var filename = Server.MapPath("~\\" + attach.BasePath + "\\" + attach.SavePath);
                    var dt = new System.Data.DataTable();
                    string err = null;

                    if (AsposeExcelTools.ExcelFileToDataTable(filename, out dt, out err))
                    {
                        //清空中间表
                        BLLFactory<Core.BLL.MidConcentrator>.Instance.DeleteByCondition("1=1");
                        BLLFactory<Core.BLL.MidCustomerMeter>.Instance.DeleteByCondition("1=1");

                        //采集器信息
                        var entity = new Core.Entity.MidConcentrator();
                        var row = dt.Rows[2];
                        var nvcName = row[0].ToString();
                        var nvcAddr = row[1].ToString();
                        var vcAddr = row[2].ToString();
                        var intProtocol = row[3].ToString().ToInt();
                        var intCount = row[4].ToString().ToInt();
                        var intCommMode = row[5].ToString().ToInt();
                        var intCOM = row[6].ToString().ToInt();
                        var vcParam = row[7].ToString();
                        var vcSimNo = row[8].ToString();
                        var intStatus = row[9].ToString().ToInt();
                        var clientIP = "127.0.0.1";

                        entity.VcClientIP = clientIP;
                        entity.IntCOM = intCOM;
                        entity.IntCommMode = intCommMode;
                        entity.IntCount = intCount;
                        entity.IntProtocol = intProtocol;
                        entity.IntStatus = intStatus;
                        entity.NvcAddr = nvcAddr;
                        entity.NvcName = nvcName;
                        entity.VcAddr = vcAddr;
                        entity.VcParam = vcParam;
                        entity.VcSimNo = vcSimNo;

                        //DbTransaction dbTransaction = BLLFactory<Core.BLL.MidConcentrator>.Instance.CreateTransaction();
                        BLLFactory<Core.BLL.MidConcentrator>.Instance.Insert(entity);
                        //dbTransaction.Commit();

                        //客户及表信息
                        for (int i = 5; i < dt.Rows.Count; i++)
                        {
                            var item = dt.Rows[i];
                            var IntCustCode = item[0].ToString().ToInt();
                            var NvcName = item[1].ToString();
                            var NvcAddr = item[2].ToString();
                            var VcTelNo = item[3].ToString();
                            var VcMobile = item[4].ToString();
                            var VcIDNo = item[5].ToString();
                            var IntPriceNo = item[6].ToString().ToInt();
                            var VcCustType = item[7].ToString();
                            var IntStatusUser = item[8].ToString().ToInt();
                            var DteOpen = Convert.ToDateTime(item[9].ToString());
                            var NvcInvName = item[10].ToString();
                            var NvcInvAddr = item[11].ToString();
                            var NvcVillage = item[12].ToString();
                            var VcBuilding = item[13].ToString();
                            var IntUnitNum = item[14].ToString().ToInt();
                            var IntRoomNum = item[15].ToString().ToInt();
                            var IntNumber = item[16].ToString().ToInt();
                            var VcAddr = item[17].ToString();
                            var NvcAddrIns = item[18].ToString();
                            var VcAssetNo = item[19].ToString();
                            var VcBarCode = item[20].ToString();
                            var IntOrig = item[21].ToString().ToInt();
                            var IntChannal = item[22].ToString().ToInt();
                            var IntProtocol = item[23].ToString().ToInt();
                            var DtCreate = Convert.ToDateTime(item[24].ToString());
                            var IntCycle = item[25].ToString().ToInt();
                            var IntState = item[26].ToString().ToInt();

                            var mMeter = new Core.Entity.MidCustomerMeter();
                            mMeter.DtCreate = DateTime.Now;
                            mMeter.DteOpen = DateTime.Now;
                            mMeter.IntChannal = IntChannal;
                            mMeter.IntCustCode = IntCustCode;
                            mMeter.IntCycle = IntCycle;
                            mMeter.IntNumber = IntNumber;
                            mMeter.IntOrig = IntOrig;
                            mMeter.IntPriceNo = IntPriceNo;
                            mMeter.IntProtocol = IntProtocol;
                            mMeter.IntRoomNum = IntRoomNum;
                            mMeter.IntState = IntState;
                            mMeter.IntStatus = IntStatusUser;
                            mMeter.IntUnitNum = IntUnitNum;
                            //mMeter.IntUserID=
                            mMeter.NvcAddr = NvcAddr;
                            mMeter.NvcAddrIns = NvcAddrIns;
                            mMeter.NvcCustType = VcCustType;
                            mMeter.NvcInvAddr = NvcInvAddr;
                            mMeter.NvcInvName = NvcInvName;
                            mMeter.NvcName = NvcName;
                            mMeter.NvcVillage = NvcVillage;
                            mMeter.VcAddr = VcAddr;
                            mMeter.VcAssetNo = VcAssetNo;
                            mMeter.VcBarCode = VcBarCode;
                            mMeter.VcBuilding = VcBuilding;
                            mMeter.VcClientIP = clientIP;
                            //mMeter.VcContractNo=
                            mMeter.VcIDNo = VcIDNo;
                            mMeter.VcMobile = VcMobile;
                            mMeter.VcTelNo = VcTelNo;

                            BLLFactory<Core.BLL.MidCustomerMeter>.Instance.Insert(mMeter);
                            //BLLFactory<Core.BLL.MidCustomerMeter>.Instance.Insert(mMeter, dbTransaction); 
                        }

                        //dbTransaction.Commit();

                        //@sClientIP VARCHAR(64), --< !--客户端IP-- >
                        //@iConcFlag  INTEGER, --< !--采集器标志0:如果已存在相同采集器，则提示错误，终止导入档案信息 1:如果已存在同地址采集器，将客户与表信息导入，表挂接在同地址采集器下-- >
                        //@iMeterFlag INTEGER,    --< !--一户多表标志0:一户一表，MidCustomerMeter.IntCustCode唯一 1:一户多表，MidCustomerMeter.IntCustCode相同的记录认为是同一客户-- >
                        List<SqlParameter> param = new List<SqlParameter>();
                        param.Add(new SqlParameter("@sClientIP", SqlDbType.VarChar, 64) { Value = clientIP });
                        param.Add(new SqlParameter("@iConcFlag", SqlDbType.VarChar, 64) { Value = 1 });
                        param.Add(new SqlParameter("@iMeterFlag", SqlDbType.VarChar, 64) { Value = 1 });
                        param.Add(new SqlParameter("@sReturn", SqlDbType.VarChar, 256) { Direction = ParameterDirection.Output });
                        //param[0].Value = clientIP;
                        //param[1].Value = 1;
                        //param[2].Value = 1;
                        //param[3].Direction = ParameterDirection.Output;

                        BLLFactory<Core.BLL.ArcConcentratorInfo>.Instance.ExecStoreProc("up_ImportArchive", param);
                        if (param[3].Value.ToString() == "0")
                        {
                            result.Success = true;
                        }
                        else
                        {
                            result.ErrorMessage = "执行up_ImportArchive存储过程出错!错误如下:" + param[3].Value.ToString();
                        }
                    }
                    else
                    {
                        result.ErrorMessage = err;
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
            }
            return ToJsonContent(result);
        }


        public ActionResult FindMaxIntMPByIntConID(int IntConID)
        {
            CommonResult result = new CommonResult();
            result.Success = true;
            result.Data1 = "1";
            try
            {
                var sql = "select IsNULL(max(intMP),0)+1 IntMP from ArcMeterInfo where IntConID=" + IntConID;
                var dt = BLLFactory<Core.BLL.ArcMeterInfo>.Instance.SqlTable(sql);
                if (dt.Rows.Count > 0)
                {
                    result.Data1 = dt.Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                LogTextHelper.Error(ex);//错误记录
                result.ErrorMessage = ex.Message;
            }
            return ToJsonContent(result);
        }
        [HttpPost]
        public ActionResult submit()
        {

            string IntCustNO = Request["IntCustNO"];
            string NvcName = Request["NvcName"];
            string NvcAddr = Request["NvcAddr"];
            string VcAddr = Request["VcAddr"];
            string where = " (1=1) ";
            if (IntCustNO != "")
            {
                where += " and IntCustNO=" + Convert.ToInt32(IntCustNO);
            }
            if (NvcName != "")
            {
                where += " and c.NvcName='" + NvcName + "'";
            }
            if (NvcAddr != "")
            {
                where += " and a.NvcAddr like'%" + NvcAddr + "%'";
            }
            if (VcAddr != "")
            {
                where += " and a.VcAddr='" + VcAddr + "'";
            }



            var sb = new System.Text.StringBuilder();
            sb.Append(" SELECT A.* ");
            sb.Append(" ,C.NvcAddr VcCustAddr,C.NvcName VcCustName ");
            sb.AppendFormat(" FROM  ArcMeterInfo A ");
            sb.AppendFormat(" left join ArcCustomerInfo C on C.IntNo=A.IntCustNo ");
            sb.Append(" WHERE " + where + " ORDER BY A.IntMP ");


            var dt = BLLFactory<Core.BLL.ArcCustomerInfo>.Instance.SqlTable(sb.ToString());
            var listCustNo = new List<string>();
            foreach (DataRow item in dt.Rows)
            {
                var custNo = item["IntCustNo"].ToString();
                if (!listCustNo.Contains(custNo)) listCustNo.Add(custNo);
            }
            //上次抄表用量
            sb = new System.Text.StringBuilder();
            sb.Append("select * from ( ");
            sb.Append("select IntID, VcAddr, IntCustNo, DteReading,DtCreate,VcStatus, NumReading,");
            sb.Append("ROW_NUMBER() over(partition by vcaddr, intcustNo order by DteReading desc) RowNum ");
            sb.Append("from ArcMeterReading ) T where RowNum=1 and IntCustNo in (0");
            foreach (var no in listCustNo)
            {
                sb.AppendFormat(",{0}", no);
            }
            sb.Append(" ) order by IntID desc ");
            var dtReading = BLLFactory<Core.BLL.ArcCustomerInfo>.Instance.SqlTable(sb.ToString());
            dt.Columns.Add("LastAmount", typeof(decimal));
            dt.Columns.Add("LastCopyTime", typeof(string));
            dt.Columns.Add("StatusWord", typeof(string));
            foreach (DataRow item in dt.Rows)
            {
                var custNo = item["IntCustNo"].ToString();
                var rows = dtReading.Select("IntCustNo=" + custNo);
                if (rows.Count() > 0)
                {
                    item["LastAmount"] = rows[0]["NumReading"];
                    item["LastCopyTime"] = rows[0]["DtCreate"];
                    item["StatusWord"] = rows[0]["VcStatus"];
                }
                else
                {
                    item["LastAmount"] = 0;
                    item["LastCopyTime"] = "";
                }
            }
            //用户余额  
            dt.Columns.Add("MonSum", typeof(decimal));
            if (listCustNo.Count > 0)
            {
                var strWhere = " and AA.IntNo in ({0})".FormatWith(string.Join(",", listCustNo));
                var dtBalance = new AccDepositController().GetMoneyInfo(strWhere);
                foreach (DataRow item in dt.Rows)
                {
                    var custNo = item["IntCustNo"].ToString();
                    var rows = dtBalance.Select("IntCustNo=" + custNo);
                    if (rows.Count() > 0)
                    {
                        item["MonSum"] = rows[0]["MonSum"];
                    }
                    else
                    {
                        item["MonSum"] = 0;
                    }
                }
            }
            return ToJsonContentDate(dt);

        }

        public class ArcMeterChange
        {
            public int InCustNo { set; get; }
            public string VcAddrOld { set; get; }
            public string VcAddrNew { set; get; }
            public string NumReading { set; get; }

            public int IntType { set; get; }
            public string NVcDesc { set; get; }
            public string VcUserID { set; get; }
            public DateTime DteChange { set; get; }
            public DateTime DtCreate { set; get; }

        }
        public ActionResult ChangeTB()
        {

            return View();
        }


        public static string HttpPost(string url, string body)
        {
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.ContentType = "application/json";

            byte[] buffer = encoding.GetBytes(body);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
            public ActionResult SettingMeterValve(String sAddr, int iCmdType)
        {
            CommonResult result = new CommonResult();
            result.Success = false;
            List<SqlParameter> settingmetervale = new List<SqlParameter>();
            try
            {
                //把需要改的数据以list形式带入到存储过程（有参）
                settingmetervale.Add(new SqlParameter("@sAddrs", SqlDbType.NVarChar) { Value = sAddr });
                settingmetervale.Add(new SqlParameter("@iCmdType", SqlDbType.Int) { Value = iCmdType });
                settingmetervale.Add(new SqlParameter("@sReturn", SqlDbType.VarChar,256) { Direction = ParameterDirection.Output });
                BLLFactory<Core.BLL.ArcMeterInfo>.Instance.ExecStoreProc("up_TaskValveBatch_NB", settingmetervale);

                if (settingmetervale[2].Value.ToString() == "0")
                {
                    result.Success = true;
                }
                else
                {
                    result.Success = false;
                }
                
            }
            catch (Exception ex)
            {
                var err= ex.ToString();
                result.Success = false;
            }

            return ToJsonContent(result);
        }
 
        public ActionResult SettingMeterInfo(String sAddr, String ChrAllowUsed, String ChrFreezeDay, String ChrValveMaint, String ChrUpTiming, String ChrUpTimingUnit, String ChrUpAmount)
        {
            CommonResult result = new CommonResult();
            result.Success = false;
            List<SqlParameter> settingmetervale = new List<SqlParameter>();
            try
            {
                if (ChrAllowUsed != "") {
                    ChrAllowUsed = Ten2Hex(ChrAllowUsed).PadLeft(2, '0');
                };
                if (ChrFreezeDay != "")
                {
                    ChrFreezeDay = Ten2Hex(ChrFreezeDay).PadLeft(4, '0');
                };
                if (ChrValveMaint != "")
                {
                    ChrValveMaint = Ten2Hex(ChrValveMaint).PadLeft(4, '0');
                };
                if (ChrUpTiming != "")
                {
                    ChrUpTiming = Ten2Hex(ChrUpTiming).PadLeft(4, '0');
                };
                if (ChrUpAmount != "")
                {
                    ChrUpAmount = Ten2Hex(ChrUpAmount).PadLeft(4, '0');
                };


                //把需要改的数据以list形式带入到存储过程（有参）
                settingmetervale.Add(new SqlParameter("@sAddrs", SqlDbType.NVarChar) { Value = sAddr });
                settingmetervale.Add(new SqlParameter("@ChrPoint", SqlDbType.VarChar,2) { Value = "" });
                settingmetervale.Add(new SqlParameter("@ChrInitReading", SqlDbType.VarChar,8) { Value = "" });
                settingmetervale.Add(new SqlParameter("@ChrAlertVolt", SqlDbType.VarChar, 2) { Value = "" });
                settingmetervale.Add(new SqlParameter("@ChrCloseVolt", SqlDbType.VarChar, 2) { Value = "" });
                settingmetervale.Add(new SqlParameter("@ChrAllowUsed", SqlDbType.VarChar, 2) { Value = ChrAllowUsed });
                settingmetervale.Add(new SqlParameter("@ChrFreezeDay", SqlDbType.VarChar, 2) { Value = ChrFreezeDay });
                settingmetervale.Add(new SqlParameter("@ChrValveMaint", SqlDbType.VarChar, 4) { Value = ChrValveMaint });
                settingmetervale.Add(new SqlParameter("@ChrUpTiming", SqlDbType.VarChar, 4) { Value = ChrUpTiming });
                settingmetervale.Add(new SqlParameter("@ChrUpTimingUnit", SqlDbType.VarChar, 2) { Value = ChrUpTimingUnit.PadLeft(2, '0') });
                settingmetervale.Add(new SqlParameter("@ChrUpAmount", SqlDbType.VarChar, 4) { Value = ChrUpAmount });
                settingmetervale.Add(new SqlParameter("@ChrValveRuning", SqlDbType.VarChar, 2) { Value = "" });
                settingmetervale.Add(new SqlParameter("@sReturn", SqlDbType.VarChar, 256) { Direction = ParameterDirection.Output });
                BLLFactory<Core.BLL.ArcMeterInfo>.Instance.ExecStoreProc("up_TaskConfigBatch_NB", settingmetervale);

                if (settingmetervale[12].Value.ToString() == "0")
                {
                    result.Success = true;
                }
                else
                {
                    result.Success = false;
                }

            }
            catch (Exception ex)
            {
                var err = ex.ToString();
                result.Success = false;
            }

            return ToJsonContent(result);
        }


        /// <summary>
        /// 从十进制转换到十六进制
        /// </summary>
        /// <param name="ten"></param>
        /// <returns></returns>
        public static string Ten2Hex(string ten)
        {
            ulong tenValue = Convert.ToUInt64(ten);
            ulong divValue, resValue;
            string hex = "";
            do
            {
                //divValue = (ulong)Math.Floor(tenValue / 16);

                divValue = (ulong)Math.Floor((decimal)(tenValue / 16));

                resValue = tenValue % 16;
                hex = tenValue2Char(resValue) + hex;
                tenValue = divValue;
            }
            while (tenValue >= 16);
            if (tenValue != 0)
                hex = tenValue2Char(tenValue) + hex;
            return hex;
        }

        public static string tenValue2Char(ulong ten)
        {
            switch (ten)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    return ten.ToString();
                case 10:
                    return "A";
                case 11:
                    return "B";
                case 12:
                    return "C";
                case 13:
                    return "D";
                case 14:
                    return "E";
                case 15:
                    return "F";
                default:
                    return "";
            }
        }

        public ActionResult ChangeTBL()
        {
            CommonResult result = new CommonResult();
            result.Success = false;
            List<SqlParameter> lichange = new List<SqlParameter>();

            var IntCustNo = Convert.ToInt32(Request["IntCustNo"]);
            var IntType = Convert.ToInt32(Request["IntType"]);
            var VcAddrOld= Request["VcAddrOld"];
            var NumReading = Request["NumReading"];
            var NVcDesc = Request["NVcDesc"];
            var VcAddrNew = Request["VcAddrNew"];
            var addNameNew= Request["addNameNew"];
            var addNvcAddrNew= Request["addNvcAddrNew"];
            var addVcBarCodeNew= Request["addVcBarCodeNew"];
            var addVcAssetNoNew= Request["addVcAssetNoNew"];
            var addIntCycleNew=Convert.ToInt32( Request["addIntCycleNew"]);
            var addIntOrigNew= Request["addIntOrigNew"];
            var UserID = Session["Identity"];
            var DteChange = Request["DteChange"];
            //把需要改的数据以list形式带入到存储过程（有参）
            lichange.Add(new SqlParameter("@iCustNo", SqlDbType.Int) { Value = IntCustNo });
            lichange.Add(new SqlParameter("@iType", SqlDbType.Int) { Value = IntType });
            lichange.Add(new SqlParameter("@sAddrOld", SqlDbType.VarChar, 16) { Value = VcAddrOld });
            lichange.Add(new SqlParameter("@fReading", SqlDbType.NVarChar, 16) { Value = NumReading });
            lichange.Add(new SqlParameter("@sDesc", SqlDbType.VarChar, 64) { Value = NVcDesc });
            lichange.Add(new SqlParameter("@sAddrNew", SqlDbType.VarChar, 16) { Value = VcAddrNew });
            lichange.Add(new SqlParameter("@sName", SqlDbType.VarChar, 64) { Value = addNameNew });
            lichange.Add(new SqlParameter("@sAddrIns", SqlDbType.VarChar, 64) { Value = addNvcAddrNew });
            lichange.Add(new SqlParameter("@sBarCode", SqlDbType.VarChar, 64) { Value = addVcBarCodeNew });
            lichange.Add(new SqlParameter("@sAssetNo", SqlDbType.VarChar, 64) { Value = addVcAssetNoNew });
            lichange.Add(new SqlParameter("@iCycle", SqlDbType.Int) { Value = addIntCycleNew });
            lichange.Add(new SqlParameter("@fOrig", SqlDbType.VarChar, 64) { Value = addIntOrigNew });
            lichange.Add(new SqlParameter("@sUserID", SqlDbType.VarChar, 64) { Value = UserID });
            lichange.Add(new SqlParameter("@dChange", SqlDbType.VarChar, 64) { Value = DteChange });
            lichange.Add(new SqlParameter("@sReturn", SqlDbType.VarChar, 64) { Direction = ParameterDirection.Output });

            BLLFactory<Core.BLL.ArcConcentratorInfo>.Instance.ExecStoreProc("up_MeterChange", lichange);

            if (lichange[14].Value.ToString() == "0")
            {
                result.Success = true;
            }
            else
            {
                result.ErrorMessage = "执行up_ImportArchive存储过程出错!错误如下:" + lichange[14].Value.ToString();
            }
            return ToJsonContent(result);
        }


        public ActionResult SwitchValveTask()
        {
            InitSignalrScript();


            return View();
        }
        public ActionResult TaskValveGet_Failure(string WHC_IntConID)
        {

            InitSignalrScript();
            var sb = new System.Text.StringBuilder();
            sb.Append(" declare @sReturn varchar  ");
            sb.Append("exec up_TaskValveGet_Failure  ");
            sb.AppendFormat("'"+ WHC_IntConID + "', ");
            sb.AppendFormat(" '2010-06-01','2999-07-10',@sReturn output ");
            sb.Append(" select @sReturn  ");
            var dt = BLLFactory<Core.BLL.ArcCustomerInfo>.Instance.SqlTable(sb.ToString());

            int rows = Request["rows"] == null ? 10 : int.Parse(Request["rows"]);
            int page = Request["page"] == null ? 1 : int.Parse(Request["page"]);

            DataTable dat = new DataTable();
            //复制源的架构和约束
            dat = dt.Clone();
            // 清除目标的所有数据
            dat.Clear();
            //对数据进行分页
            for (int i = (page - 1) * rows; i < page * rows && i < dt.Rows.Count; i++)
            {
                dat.ImportRow(dt.Rows[i]);
            }
            //最重要的是在后台取数据放在json中要添加个参数total来存放数据的总行数，如果没有这个参数则不能分页
            int total = dt.Rows.Count;
            var result = new { total = total, rows = dat };
            return ToJsonContentDate(result);
            //return ToJsonContentDate(dt);
        }
        public ActionResult SwitchValveTaskSubmit() {
            InitSignalrScript();
            string VcConceAddr = Request["VcConceAddr"];
            string DteStart = Request["DteStart"];
            string DteEnd = Request["DteEnd"];
            int OrdertType = Convert.ToInt16(Request["OrderType"]);
            var sb = new System.Text.StringBuilder();
            if (DteStart == "" || DteEnd == "")
            {
                DteStart = "2018-1-1";
                DteEnd = "2099-1-1";
            }
            sb.Append(" declare @sReturn varchar  ");
            sb.Append("exec up_TaskValveGet_Failure_Ex  ");
            sb.AppendFormat("'" + VcConceAddr + "', ");
            sb.AppendFormat(OrdertType + ",");
            sb.AppendFormat(" '" + DteStart + "','" + DteEnd + "',@sReturn output ");
            var dt = BLLFactory<Core.BLL.ArcCustomerInfo>.Instance.SqlTable(sb.ToString());
            int rows = Request["rows"] == null ? 10 : int.Parse(Request["rows"]);
            int page = Request["page"] == null ? 1 : int.Parse(Request["page"]);

            DataTable dat = new DataTable();
            //复制源的架构和约束
            dat = dt.Clone();
            // 清除目标的所有数据
            dat.Clear();
            //对数据进行分页
            for (int i = (page - 1) * rows; i < page * rows && i < dt.Rows.Count; i++)
            {
                dat.ImportRow(dt.Rows[i]);
            }
            //最重要的是在后台取数据放在json中要添加个参数total来存放数据的总行数，如果没有这个参数则不能分页
            int total = dt.Rows.Count;
            var result = new { total = total, rows = dat };
            return ToJsonContentDate(result);
        }
        private void InitSignalrScript()
        {
            var signalrUrl = DBLib.Common.ConfigHelper.GetConfigString("SignalrUrl");
            var commandTimeout = DBLib.Common.ConfigHelper.GetConfigString("CommandTimeout");
            ViewBag.SignalrScript = string.Format(@"<script src=""{0}/signalr/hubs""></script><script>var signalrUrl = ""{0}"";var CommandTimeout={1}*1000;</script>", signalrUrl, commandTimeout);
        }
    }
}
