using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using WHC.Attachment.BLL;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;
using WHC.MVCWebMis.Controllers;
using WHC.Pager.Entity;
using WHC.Security.BLL;
using WHC.Security.Entity;
namespace WHC.WaterFeeWeb.Controllers
{
    public class ArcCustomerInfoController : BusinessController<Core.BLL.ArcCustomerInfo, Core.Entity.ArcCustomerInfo>
    {
        // GET: CustomerInfo
        public ActionResult List()
        {
            ViewBag.Title = "档案列表";
            return View();
        }

        public ActionResult ListJson()
        {
            // return base.FindWithPager();
            string where = "";
            string sql = "";
            var WHC_IntNo = Request["WHC_IntNo"];
            var WHC_NvcName = Request["WHC_NvcName"];
            var WHC_NvcAddr = Request["WHC_NvcAddr"];
            var WHC_VcMobile = Request["WHC_VcMobile"];

            string NvcVillage = Request["NvcVillage"];
            string VcBuilding = Request["VcBuilding"];

            sql = "SELECT ROW_NUMBER() OVER ( order by a.IntID DESC) as RowNumber,  a.IntID,a.IntNo,a.NvcName,isnull(c.VcDesc,'无表')VcDesc,a.NvcAddr,a.NvcVillage,a.VcBuilding,a.IntUnitNum,a.IntRoomNum,a.VcNameCode,a.VcAddrCode,a.VcMobile,a.VcTelNo,a.VcIDNo,a.VcContractNo,a.NvcInvName,a.NvcInvAddr,a.IntNumber,b.IntPriceNo,b.IntPriceNo2,a.NvcCustType,a.IntUserID,a.IntStatus,a.VcWechatNo,a.IntAccMode,a.IntHelper,a.DteOpen,a.DteCancel,a.DtCreate,b.IntID bIntID,b.VcAddr bVcAddr,b.NvcName bNvcName ,b.NvcAddr bNvcAddr,b.VcBarCode bVcBarCode,b.VcAssetNo bVcAssetNo,b.NumRatio,b.IntAutoSwitch,j.VcDesc jVcDesc,e.IntID eIntID,e.NvcName eNvcName,b.IntMP ,f.VcDesc fVcDesc,isnull(g.IntNo,0) gIntNo,isnull(g.NvcDesc,0) gNvcDesc,isnull(h.IntNo,0) hIntNo,isnull(h.NvcDesc,0) hNvcDesc  ,d.IntCode YFINtCode,b.IntAccountWay,d.VcDesc YFVcDesc FROM ArcCustomerInfo a  left join ArcMeterInfo b on a.IntNo=b.IntCustNO and b.IntStatus=0  left join DictValveStatus c on c.IntCode=b.IntValveState left join DictAccountWay d on d.IntCode=b.IntAccountWay left join ArcConcentratorInfo e on e.IntID=b.IntConID left join DictMeterStatus f on b.IntStatus=f.IntCode left join PriceProperty g on g.IntNo=b.IntPriceNo left join PriceProperty h on h.IntNo=b.IntPriceNo2  left join DictValveAuto j on b.IntAutoSwitch=j.IntCode where ";

            where = " (1=1) ";

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

            if (WHC_IntNo != null && WHC_IntNo != "")
            {
                where += " and a.IntNo=" + WHC_IntNo;
            }
            if (WHC_NvcName != null && WHC_NvcName != "")
            {
                where += " and a.NvcName" + WHC_NvcName;
            }
            if (WHC_NvcAddr != null && WHC_NvcAddr != "")
            {
                where += " and a.NvcAddr=" + WHC_NvcAddr;
            }
            if (WHC_VcMobile != null && WHC_VcMobile != "")
            {
                where += " and a.VcMobile=" + WHC_VcMobile;
            }
            var IntID = Request["WHC_IntID"];

            var fuji = Request["WHC_Fuji"];
            var Text = Request["WHC_Text"];
            if (IntID != null && IntID != "")
            {
                // if (bolNum(IntID)) { 
                where = "  e.IntID=" + IntID;
                //}

            }
            if (fuji != null && fuji != "")
            {
                where = " a.NvcVillage='" + fuji + "'";
            }
            if (Text != "" && Text != null)
            {
                where += " and a.VcBuilding='" + Text + "'";
            }
            sql = sql + where + "  order by IntMP desc";

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

        public ActionResult ShowListByConcentratorID(string id)
        {
            //base.CheckAuthorized(AuthorizeKey.ListKey);

            //string where = GetPagerCondition();
            //PagerInfo pagerInfo = GetPagerInfo();
            //List<Core.Entity.ArcCustomerInfo> list = baseBLL.FindWithPager(where, pagerInfo);

            ////Json格式的要求{total:22,rows:{}}
            ////构造成Json的格式传递
            //var result = new { total = pagerInfo.RecordCount, rows = list };
            //return ToJsonContentDate(result);
            return View();
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
        public ActionResult Insert2(Core.Entity.ArcCustomerInfo CustomerInfo, Core.Entity.ArcMeterInfo MeterInfo)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.InsertKey);

            CommonResult result = new CommonResult();
            var dbTran = BLLFactory<Core.BLL.ArcCustomerInfo>.Instance.CreateTransaction();
            try
            {
                var cInfo = Request["CustomerInfo"];
                var mInfo = Request["MeterInfo"];
                var setting = new Newtonsoft.Json.JsonSerializerSettings
                {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
                };
                CustomerInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Core.Entity.ArcCustomerInfo>(cInfo, setting);
                MeterInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Core.Entity.ArcMeterInfo>(mInfo, setting);

                var info = ReflectionHelper.ReplacePropertyValue(CustomerInfo, typeof(string), null, string.Empty);
                MeterInfo = ReflectionHelper.ReplacePropertyValue(MeterInfo, typeof(string), null, string.Empty);

                info.IntUserID = CurrentUser.ID;
                info.IntStatus = 1;
                info.DteOpen = DateTime.Now;
                //info.DteCancel = DateTime.MaxValue;
                info.DtCreate = DateTime.Now;
                info.IntHelper = 0;

                info.VcAddrCode = DBLib.PinYinHelper.GetInitials(info.NvcAddr);
                info.VcNameCode = DBLib.PinYinHelper.GetInitials(info.NvcName);

                result.Success = BLLFactory<Core.BLL.ArcCustomerInfo>.Instance.Insert(info, dbTran);
              
                if (result.Success)
                {
                    MeterInfo.IntCustNO = info.IntNo;
                    result.Success = BLLFactory<Core.BLL.ArcMeterInfo>.Instance.Insert(MeterInfo, dbTran);
                    if (result.Success)
                    {
                        dbTran.Commit();
                    }
                    else dbTran.Rollback();
                }
                else dbTran.Rollback();
            }
            catch (Exception ex)
            {
                dbTran.Rollback();
                LogTextHelper.Error(ex);//错误记录
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return ToJsonContent(result);
        }

        [HttpPost]
        public ActionResult Update2(Core.Entity.ArcCustomerInfo CustomerInfo, Core.Entity.ArcMeterInfo MeterInfo)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.InsertKey);

            CommonResult result = new CommonResult();
            var dbTran = BLLFactory<Core.BLL.ArcCustomerInfo>.Instance.CreateTransaction();
            try
            {
                var cInfo = Request["CustomerInfo"];
                var mInfo = Request["MeterInfo"];

                var setting = new Newtonsoft.Json.JsonSerializerSettings
                {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
                };
                CustomerInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Core.Entity.ArcCustomerInfo>(cInfo, setting);
                MeterInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Core.Entity.ArcMeterInfo>(mInfo, setting);

                var info = ReflectionHelper.ReplacePropertyValue(CustomerInfo, typeof(string), null, string.Empty);
                MeterInfo = ReflectionHelper.ReplacePropertyValue(MeterInfo, typeof(string), null, string.Empty);

                ////判断计量点是否重复
                //var existMP = BLLFactory<Core.BLL.ArcMeterInfo>.Instance
                //        .IsExistRecord("IntConID={0} and IntMP={1} and IntID!={2}".FormatWith(MeterInfo.IntConID, MeterInfo.IntMP, MeterInfo.IntID));
                //if (existMP)
                //{
                //    result.ErrorMessage = "同一采集器下的计量点不能重复!";
                //    return ToJsonContent(result);
                //}

                info.VcAddrCode = DBLib.PinYinHelper.GetInitials(info.NvcAddr);
                info.VcNameCode = DBLib.PinYinHelper.GetInitials(info.NvcName);

                result.Success = baseBLL.Update(info, dbTran);
                if (result.Success)
                {
                    MeterInfo.IntCustNO = info.IntNo;
                    if (MeterInfo.IntID == 0)
                        result.Success = BLLFactory<Core.BLL.ArcMeterInfo>.Instance.Insert(MeterInfo, dbTran);
                    else
                        result.Success = BLLFactory<Core.BLL.ArcMeterInfo>.Instance.Update(MeterInfo, dbTran);
                    if (result.Success)
                    {
                        dbTran.Commit();
                    }
                    else dbTran.Rollback();
                }
                else dbTran.Rollback();
            }
            catch (Exception ex)
            {
                dbTran.Rollback();
                LogTextHelper.Error(ex);//错误记录
                result.ErrorMessage = ex.Message;
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
                var clientIP = GetClientIp();// Request.ServerVariables["REMOTE_ADDR"];
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
                            var custCode = item[0].ToString();
                            if (custCode.IsNullOrEmpty()) continue;

                            var IntCustCode = custCode.ToInt();
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

        // GET: CustomerInfo/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CustomerInfo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerInfo/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerInfo/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CustomerInfo/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerInfo/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CustomerInfo/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult DictAccountWay()
        {

            string sql = "select * from DictAccountWay";
            var dt = BLLFactory<Core.BLL.ArcCustomerInfo>.Instance.SqlTable(sql);
            return ToJsonContentDate(dt);
        }
        public ActionResult GetTreeJson()
        {
            string sql = "select * from PriceProperty";
            var list = BLLFactory<Core.BLL.PriceProperty>.Instance.SqlTable(sql);
            return ToJsonContentDate(list);
        }
        public ActionResult IntAutoSwitch()
        {

            string sql = "select * from DictValveAuto";
            var list = BLLFactory<Core.BLL.PriceProperty>.Instance.SqlTable(sql);
            return ToJsonContentDate(list);
        }
        [HttpPost]
        public ActionResult PdIntAutoSwitch()
        {
            var IntCustNo = Request["IntCustNo"];
            string sql = "select  top(1)IntAutoSwitch from ArcMeterInfo  where IntCustNO=" + IntCustNo + " order by DtLastUpd desc";
            var list = BLLFactory<Core.BLL.PriceProperty>.Instance.SqlTable(sql);
            return ToJsonContentDate(list);

        }

        //public ActionResult TreeArcConcentratorInfo(bool isAddRoot = true)
        //{
        //    //bool isAddRoot = true
        //    //string num = "";
        //    //if (isAddRoot) { 
        //    //  num = "0";
        //    //}
        //     var list = BLLFactory<Core.DALSQL.ArcConcentratorInfo>.Instance.GetAll();
        //    //string sql = "select * from ArcConcentratorInfo where IntUpID=" + num;
        //   // var list = BLLFactory<Core.DALSQL.ArcConcentratorInfo>.Instance.GetList(sql);
        //    var treeList = new List<EasyTreeData>();

        //    var root = new EasyTreeData();
        //    root.iconCls = "icon-organ";
        //    root.id = "";
        //    root.text = "所有集中器";
        //    root.state = "open";
        //    root.children = new List<EasyTreeData>();
        //    foreach (var item in list)
        //    {
        //        var d = new EasyTreeData();
        //        d.iconCls = "icon-organ";
        //        d.id = item.IntID.ToString();
        //        d.text = item.NvcName;

        //        d.state = "open";

        //        root.children.Add(d);
        //    }
        //    if (isAddRoot)
        //        treeList.Add(root);
        //    else
        //        treeList = root.children;
        //    return ToJsonContentDate(treeList);
        //}
        public ActionResult TreeArcConcentratorInfo(bool isAddRoot = true)
        {
            //bool isAddRoot = true
            string num = "";
            if (isAddRoot)
            {
                num = "0";
            }
            //var list = BLLFactory<Core.DALSQL.ArcConcentratorInfo>.Instance.GetAll();
            string sql = "select * from ArcConcentratorInfo where IntUpID=" + num;
            var list = BLLFactory<Core.DALSQL.ArcConcentratorInfo>.Instance.GetList(sql);
            var treeList = new List<EasyTreeData>();

            var root = new EasyTreeData();
            root.iconCls = "icon-organ";
            root.id = "";
            root.text = "所有集中器";
            root.state = "open";
            root.children = new List<EasyTreeData>();
            foreach (var item in list)
            {
                var d = new EasyTreeData();
                d.iconCls = "icon-organ";
                d.id = item.IntID.ToString();
                d.text = item.NvcName;
                d.state = "open";
                root.children.Add(d);
                string sql2 = "select * from ArcConcentratorInfo where IntUpID=" + d.id;
                var lists = BLLFactory<Core.DALSQL.ArcConcentratorInfo>.Instance.GetList(sql2);
                foreach (var items in lists)
                {
                    EasyTreeData info = new EasyTreeData(items.IntID.ToString(), items.NvcName.ToString(), "icon-view");
                    d.children.Add(info);
                }
            }
            if (isAddRoot)
                treeList.Add(root);
            else
                treeList = root.children;
            return ToJsonContentDate(treeList);
        }

        public ActionResult TreeCommunity(bool isAddRoot = true)
        {
            var treeList = new List<EasyTreeData>();
            var sqls = "select  DISTINCT NvcVillage,max(IntID)IntID  ,max(IntNo)IntNo,max(NvcName)NvcName,max(NvcAddr)NvcAddr,max(VcBuilding)VcBuilding,max(IntUnitNum)IntUnitNum,max(IntRoomNum)IntRoomNum,max(VcNameCode)VcNameCode,max(VcAddrCode)VcAddrCode,max(VcMobile)VcMobile,max(VcTelNo)VcTelNo,max(VcIDNo)VcIDNo,max(VcContractNo)VcContractNo,max(NvcInvName)NvcInvName,max(NvcInvAddr)NvcInvAddr ,max(IntNumber)IntNumber,max(IntPriceNo)IntPriceNo,max(NvcCustType)NvcCustType,max(IntUserID)IntUserID,max(IntStatus)IntStatus ,max(VcWechatNo)VcWechatNo,max(IntAccMode)IntAccMode ,max(IntHelper)IntHelper,max(DteOpen)DteOpen,max(DteCancel)DteCancel,max(DtCreate)DtCreate from ArcCustomerInfo where NvcVillage <>'' group by NvcVillage";
            var li = BLLFactory<Core.DALSQL.ArcCustomerInfo>.Instance.GetList(sqls);

            var treeLists = new List<EasyTreeData>();

            var roots = new EasyTreeData();
            // roots.iconCls = "icon-organ";
            roots.id = "";
            roots.text = "所有小区";
            roots.state = "open";

            roots.children = new List<EasyTreeData>();

            foreach (var info in li)
            {
                var d = new EasyTreeData();
                // d.iconCls = "icon-organ";
                d.id = info.NvcVillage.ToString();
                d.text = info.NvcVillage.ToString();
                d.state = "open";
                roots.children.Add(d);
                string BuildingSQL = "select  DISTINCT [VcBuilding],max(IntID)IntID ,max(NvcVillage) NvcVillage ,max(IntNo)IntNo,max(NvcName)NvcName,max(NvcAddr)NvcAddr,max(VcBuilding)VcBuilding,max(IntUnitNum)IntUnitNum,max(IntRoomNum)IntRoomNum,max(VcNameCode)VcNameCode,max(VcAddrCode)VcAddrCode,max(VcMobile)VcMobile,max(VcTelNo)VcTelNo,max(VcIDNo)VcIDNo,max(VcContractNo)VcContractNo,max(NvcInvName)NvcInvName,max(NvcInvAddr)NvcInvAddr ,max(IntNumber)IntNumber,max(IntPriceNo)IntPriceNo,max(NvcCustType)NvcCustType,max(IntUserID)IntUserID,max(IntStatus)IntStatus ,max(VcWechatNo)VcWechatNo,max(IntAccMode)IntAccMode ,max(IntHelper)IntHelper,max(DteOpen)DteOpen,max(DteCancel)DteCancel,max(DtCreate)DtCreate from ArcCustomerInfo where NvcVillage='" + d.text + "' group by [VcBuilding]";
                var lists = BLLFactory<Core.DALSQL.ArcCustomerInfo>.Instance.GetList(BuildingSQL);
                foreach (var items in lists)
                {
                    EasyTreeData Buildinginfo = new EasyTreeData(items.IntID.ToString(), items.VcBuilding.ToString(), "icon-view");
                    d.children.Add(Buildinginfo);
                }
            }
            if (isAddRoot)
                treeList.Add(roots);
            else
                treeList = roots.children;

            return ToJsonContentDate(treeList);

        }

        public ActionResult TreeCommunity_3(bool isAddRoot = true)
        {
            var treeList = new List<EasyTreeData>();
            var sqls = "select  DISTINCT NvcVillage,max(IntID)IntID  ,max(IntNo)IntNo,max(NvcName)NvcName,max(NvcAddr)NvcAddr,max(VcBuilding)VcBuilding,max(IntUnitNum)IntUnitNum,max(IntRoomNum)IntRoomNum,max(VcNameCode)VcNameCode,max(VcAddrCode)VcAddrCode,max(VcMobile)VcMobile,max(VcTelNo)VcTelNo,max(VcIDNo)VcIDNo,max(VcContractNo)VcContractNo,max(NvcInvName)NvcInvName,max(NvcInvAddr)NvcInvAddr ,max(IntNumber)IntNumber,max(IntPriceNo)IntPriceNo,max(NvcCustType)NvcCustType,max(IntUserID)IntUserID,max(IntStatus)IntStatus ,max(VcWechatNo)VcWechatNo,max(IntAccMode)IntAccMode ,max(IntHelper)IntHelper,max(DteOpen)DteOpen,max(DteCancel)DteCancel,max(DtCreate)DtCreate from ArcCustomerInfo where NvcVillage <>'' group by NvcVillage";
            var li = BLLFactory<Core.DALSQL.ArcCustomerInfo>.Instance.GetList(sqls);

            var treeLists = new List<EasyTreeData>();

            var roots = new EasyTreeData();
            // roots.iconCls = "icon-organ";
            roots.id = "";
            roots.text = "所有小区";
            roots.state = "open";

            roots.children = new List<EasyTreeData>();

            foreach (var info in li)
            {
                var d = new EasyTreeData();
                // d.iconCls = "icon-organ";
                d.id = info.NvcVillage.ToString();
                d.text = info.NvcVillage.ToString();
                d.state = "open";
                roots.children.Add(d);
                string BuildingSQL = "select * from ArcCustomerInfo where NvcVillage='" + d.text + "'";
                var lists = BLLFactory<Core.DALSQL.ArcCustomerInfo>.Instance.GetList(BuildingSQL);
                foreach (var items in lists)
                {
                    EasyTreeData Buildinginfo = new EasyTreeData(items.IntID.ToString(), items.VcBuilding.ToString(), "icon-view");
                    d.children.Add(Buildinginfo);
                    string IntUnitNum = "select * from ArcCustomerInfo where VcBuilding='" + items.VcBuilding.ToString() + "'";
                    var IntUnitNumlists = BLLFactory<Core.DALSQL.ArcCustomerInfo>.Instance.GetList(IntUnitNum);
                    foreach (var IntUnitNumItems in lists)
                    {
                        EasyTreeData IntUnitNumInfo = new EasyTreeData(IntUnitNumItems.IntID.ToString(), IntUnitNumItems.IntUnitNum.ToString(), "icon-view");
                        d.children.Add(IntUnitNumInfo);
                    }
                }
            }
            if (isAddRoot)
                treeList.Add(roots);
            else
                treeList = roots.children;

            return ToJsonContentDate(treeList);

        }

        public bool bolNum(string temp)
        {
            for (int i = 0; i < temp.Length; i++)
            {
                byte tempByte = Convert.ToByte(temp[i]);
                if ((tempByte < 48) || (tempByte > 57))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
