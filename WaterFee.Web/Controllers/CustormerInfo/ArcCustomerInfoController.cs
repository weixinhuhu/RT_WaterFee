using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using WHC.Attachment.BLL;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;
using WHC.MVCWebMis.Controllers;

using WHC.WaterFeeWeb.DbServiceReference;
using CommonResult = WHC.Framework.Commons.CommonResult;

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

        public ActionResult ListJson_Server()
        {

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

            var endcode = Session["EndCode"] ?? "0";
            //调用后台服务获取集中器信息
            ServiceDbClient DbServer = new ServiceDbClient();
            var dts = DbServer.ArcCustomer_Qry(endcode.ToString().ToInt32(), custormerinfo);

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
            var result = new { total, rows = dat };

            return ToJsonContentDate(result);
        }

        public ActionResult Insert_Server(Customer CustomerInfo, Meter MeterInfo)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.InsertKey);
            CommonResult result = new CommonResult();

            try
            {
                var cInfo = Request["CustomerInfo"];
                var mInfo = Request["MeterInfo"];
                var setting = new Newtonsoft.Json.JsonSerializerSettings
                {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
                };
                CustomerInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Customer>(cInfo, setting);
                MeterInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Meter>(mInfo, setting);
                CustomerInfo = ReflectionHelper.ReplacePropertyValue(CustomerInfo, typeof(string), null, string.Empty);
                MeterInfo = ReflectionHelper.ReplacePropertyValue(MeterInfo, typeof(string), null, string.Empty);
                CustomerInfo.IntUserID = CurrentUser.ID;
                CustomerInfo.IntStatus = 1;
                CustomerInfo.DteOpen = DateTime.Now;
                CustomerInfo.DteCancel = DateTime.Now;
                MeterInfo.IntCustNO = CustomerInfo.IntNo;
                CustomerInfo.VcAddrCode = DBLib.PinYinHelper.GetInitials(CustomerInfo.NvcAddr);
                CustomerInfo.VcNameCode = DBLib.PinYinHelper.GetInitials(CustomerInfo.NvcName);
                var endcode = Session["EndCode"] ?? "0";
                CustomerInfo.IntEndCode = endcode.ToString().ToInt32();
                MeterInfo.IntEndCode = endcode.ToString().ToInt32();

                ServiceDbClient DbServer = new ServiceDbClient();
                var flg = DbServer.ArcCustMeter_Ins(CustomerInfo, MeterInfo);
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
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }
            return ToJsonContent(result);
        }
        public ActionResult Update_Server(Customer CustomerInfo, Meter MeterInfo)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.InsertKey);
            CommonResult result = new CommonResult();
            try
            {
                var cInfo = Request["CustomerInfo"];
                var mInfo = Request["MeterInfo"];
                var setting = new Newtonsoft.Json.JsonSerializerSettings
                {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore
                };
                CustomerInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Customer>(cInfo, setting);
                MeterInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Meter>(mInfo, setting);
                CustomerInfo = ReflectionHelper.ReplacePropertyValue(CustomerInfo, typeof(string), null, string.Empty);
                MeterInfo = ReflectionHelper.ReplacePropertyValue(MeterInfo, typeof(string), null, string.Empty);

                CustomerInfo.IntUserID = CurrentUser.ID;
                CustomerInfo.IntStatus = 1;

                CustomerInfo.DteCancel = DateTime.Now;
                CustomerInfo.DteOpen = DateTime.Now;
                MeterInfo.DtCreate = DateTime.Now;
                MeterInfo.DtOnline = DateTime.Now;

                MeterInfo.IntCustNO = CustomerInfo.IntNo;
                CustomerInfo.VcAddrCode = DBLib.PinYinHelper.GetInitials(CustomerInfo.NvcAddr);
                CustomerInfo.VcNameCode = DBLib.PinYinHelper.GetInitials(CustomerInfo.NvcName);
                var endcode = Session["EndCode"] ?? "0";
                CustomerInfo.IntEndCode = endcode.ToString().ToInt32();
                MeterInfo.IntEndCode = endcode.ToString().ToInt32();
                //调用后台服务获取集中器信息
                ServiceDbClient DbServer = new ServiceDbClient();
                var flg = DbServer.ArcCustMeter_Upd(CustomerInfo, MeterInfo);
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
                result.Success = false;
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

        public ActionResult DictAccountWay_Server()
        {
            ServiceDbClient DbServer = new ServiceDbClient();
            var dt = DbServer.GetDictAccountWay();
            return ToJsonContentDate(dt);
        }

        public ActionResult IntAutoSwitch_Server()
        {
            ServiceDbClient DbServer = new ServiceDbClient();
            var list = DbServer.GetDictValveAuto();
            return ToJsonContentDate(list);
        }
        public ActionResult IntReplaceType_Server()
        {
            ServiceDbClient DbServer = new ServiceDbClient();
            var list = DbServer.GetDictMeterReplaceType();
            return ToJsonContentDate(list);
        }

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

        public ActionResult TreeCommunity_Server()
        {
            var endcode = Session["EndCode"] ?? "0";

            var treelist = new ServiceDbClient().ArcCustomer_TreeCommunity(endcode.ToString().ToInt32());

            return ToJsonContentDate(treelist);
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
