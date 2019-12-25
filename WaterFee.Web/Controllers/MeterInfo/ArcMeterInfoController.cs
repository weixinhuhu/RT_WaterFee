using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using WHC.Attachment.BLL;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;
using WHC.MVCWebMis.Controllers;
using WHC.WaterFeeWeb.DbServiceReference;
using CommonResult = WHC.Framework.Commons.CommonResult;
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
        
        public ActionResult ListJson_Server()
        {
            var Strlevel = Request["WHC_Treelevel"];
            var fuji = Request["WHC_Fuji"];
            var Text = Request["WHC_Text"];
            var ParentText = Request["WHC_TreePrentText"];
            var QryCondi = new MeterReplaceQryCondition()
            {
                NvcName = Request["WHC_NvcName"] ?? "",
                NvcAddr = Request["WHC_NvcAddr"] ?? "",
                IntCustNo = Convert.ToInt32(Request["IntCustNO"] ?? "0"),
                VcMeterAddr = Request["WHC_VcAddr"] ?? ""
            };

            if (Strlevel == "1")
            {
                QryCondi.NvcVillage = "所有小区";
            };
            if (Strlevel == "2")
            {
                QryCondi.NvcVillage = Text;
            }
            if (Strlevel == "3")
            {
                QryCondi.NvcVillage = fuji;
                QryCondi.VcBuilding = Text;
            }
            if (Strlevel == "4")
            {
                QryCondi.NvcVillage = ParentText;
                QryCondi.VcBuilding = fuji;
                QryCondi.VcUnitNum = Text;
            }
            //调用后台服务获取集中器信息          
            var dts = new ServiceDbClient().GetMeterReplaceList(QryCondi);

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
  
        //设置参数
        public ActionResult SettingMangger_Server()
        {
            var endcode = Session["EndCode"] ?? "0";
            var Strlevel = Request["WHC_Treelevel"];
            var fuji = Request["WHC_Fuji"];
            var Text = Request["WHC_Text"];
            var ParentText = Request["WHC_TreePrentText"];
            var custormerinfo = new Customer()
            {
                NvcName = Request["WHC_NvcName"] ?? "",
                NvcAddr = Request["WHC_NvcAddr"] ?? "",
                VcMobile = Request["WHC_VcMobile"] ?? ""
            };
            var useno = Request["WHC_IntNo"] ?? "0";
            custormerinfo.IntNo = useno.Equals("") ? 0 : useno.ToInt32();

            if (Strlevel == "1")
            {
                custormerinfo.NvcVillage = "所有小区";
            };

            if (Strlevel == "2")
            {
                custormerinfo.NvcVillage = Text;
            }

            if (Strlevel == "3")
            {
                custormerinfo.NvcVillage = fuji;
                custormerinfo.VcBuilding = Text;
            }

            if (Strlevel == "4")
            {
                custormerinfo.NvcVillage = ParentText;
                custormerinfo.VcBuilding = fuji;
                custormerinfo.VcUnitNum = Text;
            }

            //调用后台服务获取集中器信息
            ServiceDbClient DbServer = new ServiceDbClient();
            var dts = DbServer.Terminal_GetMeterSetting(endcode.ToString().ToInt32(), custormerinfo, new Meter());

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

        public ActionResult FindByIntCustNo_Server(string IntCustNo)
        {
            DbServiceReference.CommonResult result = new DbServiceReference.CommonResult();
            try
            {
                result = new ServiceDbClient().FindByIntCustNo(IntCustNo);
            }
            catch (Exception ex)
            {
                LogTextHelper.Error(ex);//错误记录
                result.ErrorMsg = ex.Message;
            }
            return ToJsonContent(result);
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

        public ActionResult ChangeTB()
        {
            return View();
        }

        public ActionResult MeterSettingInfo()
        {
            return View();
        }
       
        /// <summary>
        /// 查询参数信息 用于前端下拉框
        /// </summary>
        /// <returns></returns>
        public ActionResult GetParam_MeterConfigTreeJson_Server()
        {
            var endcode = Session["EndCode"] ?? "0";
            ServiceDbClient DbServer = new ServiceDbClient();
            var tree = new ServiceDbClient().Param_MeterConfig_GetTree(endcode.ToString().ToInt());
            return ToJsonContentDate(tree);
        }
        /// <summary>
        /// 查询参数信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Param_MeterConfig_Qry()
        {
            var endcoed = Session["EndCode"] ?? "";
            var dts = new ServiceDbClient().Param_MeterConfig_Qry(endcoed.ToString().ToInt());

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

        public ActionResult Param_MeterConfig_Ins(MeterConfig MeterConf)
        {
            CommonResult result = new CommonResult();
            var endcode = Session["EndCode"] ?? "0";
            MeterConf.IntID = 0;
            MeterConf.VcUserID = Session["UserID"].ToString();
            MeterConf.VcUserIDUpd = Session["UserID"].ToString();
            MeterConf.IntEndCode = endcode.ToString().ToInt32();
            MeterConf.DtCreate = DateTime.Now;
            var rs = new ServiceDbClient().Param_MeterConfig_Opr(MeterConf);
            if (rs == "0")
            {
                result.Success = true;
            }
            else
            {
                result.Success = false;
                result.ErrorMessage = rs;
            }
            return ToJsonContent(result);
        }
        public ActionResult Param_MeterConfig_Upd(MeterConfig MeterConf)
        {
            CommonResult result = new CommonResult();
            var endcode = Session["EndCode"] ?? "0";
            MeterConf.VcUserIDUpd = Session["UserID"].ToString() ?? "";
            MeterConf.IntEndCode = endcode.ToString().ToInt32();
            MeterConf.DtLstUpd = DateTime.Now;
            var rs = new ServiceDbClient().Param_MeterConfig_Opr(MeterConf);
            if (rs == "0")
            {
                result.Success = true;
            }
            else
            {
                result.Success = false;
                result.ErrorMessage = rs;
            }
            return ToJsonContent(result);
        }
        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="sAddr"></param>
        /// <param name="ChrAllowUsed"></param>
        /// <param name="ChrFreezeDay"></param>
        /// <param name="ChrValveMaint"></param>
        /// <param name="ChrUpTiming"></param>
        /// <param name="ChrUpTimingUnit"></param>
        /// <param name="ChrUpAmount"></param>
        /// <returns></returns>
        public ActionResult SettingMeterInfo_Server(String sAddr, String sMeterInfoTypeNo)
        {
            CommonResult result = new CommonResult();
            var endcode = Session["EndCode"] ?? "0";
            var meterSettingtypeno = sMeterInfoTypeNo.ToInt();
            try
            {
                var rs = new ServiceDbClient().Terminal_SetMeterConfig(endcode.ToString().ToInt(), sAddr, meterSettingtypeno);
                if (rs == "0")
                {
                    result.Success = true;
                }
                else
                {
                    result.Success = false;
                    result.ErrorMessage = rs;
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
        public ActionResult ChangeTBL_Server(MeterReplaceInfo MeterReplace)
        {
            CommonResult result = new CommonResult();
            MeterReplace.VcUserID = Session["UserID"].ToString();
            MeterReplace.IntEndCode = 0;
            try
            {
                var flg = new ServiceDbClient().ArcMeter_Replace(MeterReplace);
                if (flg == "0")
                {
                    result.Success = true;
                }
                else
                {
                    result.ErrorMessage = flg;
                    result.Success = false;
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
            }
            return ToJsonContent(result);
        }
    }
}
