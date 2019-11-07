using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using WHC.Attachment.BLL;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;
using WHC.Pager.Entity;
using WHC.WaterFeeWeb.DbServiceReference;

namespace WHC.WaterFeeWeb.Controllers
{
    public class ArcConcentratorInfoController : BusinessControllerNew<Core.BLL.ArcConcentratorInfo, Core.Entity.ArcConcentratorInfo>
    {
        [DataContract]
        [Serializable]
        public class TreeData : EasyTreeData
        {
            [DataMember]
            public string ConcentratorAddr { get; set; }

            [DataMember]
            public int IntConID { get; set; }

            [DataMember]
            public new List<TreeData> children { get; set; }
        }

        public ActionResult GetTreeJson()
        {
            var list = BLLFactory<Core.BLL.ArcConcentratorInfo>.Instance.GetAll();
            return ToJsonContent(list);
        }

        // GET: CustomerInfo
        public ActionResult List()
        {
            var dt = BLLFactory<Core.BLL.AccPayment>.Instance.SqlTable("select top(1) * from Settings  ");
            if (dt.Rows.Count > 0)
            { 
                ViewBag.FactoryCode = dt.Rows[0]["FactoryCode"].ToString();
                ViewBag.AreaCode = dt.Rows[0]["AreaCode"].ToString();
            }
            else
            { 
                ViewBag.FactoryCode = string.Empty;
                ViewBag.AreaCode = string.Empty;
            }
            ViewBag.Title = "档案列表";
            return View();
        }

        public ActionResult ListJson()
        {
            //string where = GetPagerCondition();
            string where = "";
            PagerInfo pagerInfo = GetPagerInfo();
            var list = baseBLL.FindWithPager(where, pagerInfo);

            var result = new { total = pagerInfo.RecordCount, rows = list };

            return ToJsonContentDate(result);
        }
        public ActionResult ListJson_Server()           
        {

            var info = new Concentrator()
            {
                NvcName = Request["WHC_NvcName"] ?? "",
                NvcAddr = Request["WHC_NvcAddr"] ?? "",
                VcSimNo = Request["WHC_VcSimNo"] ?? "",
                VcAddr = Request["WHC_VcAddr"] ?? ""
            };
          
            //调用后台服务获取集中器信息
            ServiceDbClient DbServer = new ServiceDbClient();
             
            var dts = DbServer.ArcConcentrator_Qry(0, info);       

            int rows = Request["rows"] == null ? 10 : int.Parse(Request["rows"]);

            int page = Request["page"] == null ? 1 : int.Parse(Request["page"]);
         
            //复制源的架构和约束
            var dat = dts.Clone();
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

        public ActionResult ListByIntUpID(string intUpID)
        {
            var listChildIntID = new List<int>();
            GetChilden(intUpID, ref listChildIntID);

            string where = "IntID in ({0})".FormatWith(string.Join(",", listChildIntID));
            PagerInfo pagerInfo = GetPagerInfo();
            var list = baseBLL.FindWithPager(where, pagerInfo);

            var result = new { total = pagerInfo.RecordCount, rows = list };

            return ToJsonContentDate(result);
        }

        public ActionResult SelectJson()
        {
            var listAll = BLLFactory<Core.BLL.ArcConcentratorInfo>.Instance.GetAll();
            var treeList = new List<EasyTreeData>();
            var d = new EasyTreeData();
            d.id = "0";
            d.text = "无";
            d.children = null;
            treeList.Add(d);
            foreach (var item in listAll)
            {
                var dd = new EasyTreeData();
                dd.id = item.IntID.ToString();
                dd.text = item.NvcName;
                dd.children = null;
                treeList.Add(dd);
            }
            return ToJsonContentDate(treeList);
        }

        public ActionResult TreeJson()
        {
            //   var listAll = BLLFactory<Core.BLL.ArcConcentratorInfo>.Instance.GetAll();

            //调用后台服务获取集中器信息
            ServiceDbClient DbServer = new ServiceDbClient();
            var listAll = DbServer.ArcConcentrator_GetTree_Level(0).ToList();

            var children = new List<EasyTreeData>();
            var listFirst = listAll.Where(n => n.IntUpID == 0);
            foreach (var item in listFirst)
            {
                var dd = new EasyTreeData();
                //if (item.IntOnline == 1) d.iconCls = "icon-online";
                dd.iconCls = "icon-online";
                dd.id = item.IntID.ToString();
                dd.text = item.NvcName;
                dd.children = getSubItem(item.IntID, listAll);
                children.Add(dd);
            }

            var treeList = new List<EasyTreeData>();
            //ROOT
            var d = new EasyTreeData();
            d.iconCls = "icon-house";
            d.id = "0";
            d.text = "根";
            d.children = children;
            treeList.Add(d);

            return ToJsonContentDate(treeList);
        }

        private List<EasyTreeData> getSubItem(int intUpID, IEnumerable<Concentrator> listAll)
        {
            List<EasyTreeData> treeList = new List<EasyTreeData>();
            var arr = listAll.Where(n => n.IntUpID == intUpID);
            foreach (var item in arr)
            {
                var d = new EasyTreeData();
                //if (item.IntOnline == 1) d.iconCls = "icon-online";
                d.iconCls = "icon-online";
                d.id = item.IntID.ToString();
                d.text = item.NvcName;
                d.children = getSubItem(item.IntID, listAll);
                treeList.Add(d);
            }
            return treeList;
        }

        private void InitSignalrScript()
        {
            var signalrUrl = DBLib.Common.ConfigHelper.GetConfigString("SignalrUrl");
            var commandTimeout = DBLib.Common.ConfigHelper.GetConfigString("CommandTimeout");
            ViewBag.SignalrScript = string.Format(@"<script src=""{0}/signalr/hubs""></script><script>var signalrUrl = ""{0}"";var CommandTimeout={1}*1000;</script>", signalrUrl, commandTimeout);
        }

        public ActionResult TerminalMeasure()
        {
            InitSignalrScript();
            return View();
        }
        public ActionResult RemoteFunction()
        {
            InitSignalrScript();
            return View();
        }


        public ActionResult ManagerList()
        {
            InitSignalrScript();
            return View();
        }

        public ActionResult GetRootListJson()
        {
            var key = SqlTextClear(Request["key"]);

            var sb = new System.Text.StringBuilder();
            sb.AppendFormat("select * from ArcConcentratorInfo where IntUpID=0 ");
            if (key.IsNotNullOrEmpty())
            {
                sb.AppendFormat(" and IntID in (select IntConID from ArcMeterInfo ");
                sb.AppendFormat(" where VcAddr='{0}' or NvcName like '%{0}%' ", key);
                sb.AppendFormat(" or IntCustNO in  ");
                sb.AppendFormat(" (select IntNo from ArcCustomerInfo ");
                sb.AppendFormat(" where IntNo like '%{0}%' or NvcName = '{0}' or NvcAddr like '%{0}%' ", key);
                sb.AppendFormat(" or NvcVillage like '%{0}%' or VcMobile = '{0}' or VcTelNo = '{0}')) ", key);
                sb.AppendFormat(" or VcAddr like '%{0}%' or NvcName like '%{0}%' or NvcAddr like '%{0}%' or VcSimNo='{0}' ", key);
            }

            var data = baseBLL.SqlTable(sb.ToString());
            var treeList = new List<TreeData>();

            foreach (DataRow item in data.Rows)
            {
                var d = new TreeData();
                if (item["IntOnline"].ToString() == "1") d.iconCls = "icon-online";
                else d.iconCls = "icon-offline";
                d.id = item["VcAddr"].ToString();
                d.text = item["NvcName"].ToString();
                d.IntConID = item["IntID"].ToString().ToInt32();
                d.ConcentratorAddr = item["VcAddr"].ToString();

                treeList.Add(d);
            }

            return ToJsonContentDate(treeList);
        }

        public ActionResult ManagerJson()
        {
            var key = SqlTextClear(Request["key"]);

            var sb = new System.Text.StringBuilder();
            sb.AppendFormat("select * from ArcConcentratorInfo ");
            if (key.IsNotNullOrEmpty())
            {
                sb.AppendFormat(" where IntID in (select IntConID from ArcMeterInfo ");
                sb.AppendFormat(" where VcAddr='{0}' or NvcName like '%{0}%' ", key);
                sb.AppendFormat(" or IntCustNO in  ");
                sb.AppendFormat(" (select IntNo from ArcCustomerInfo ");
                sb.AppendFormat(" where IntNo like '%{0}%' or NvcName = '{0}' or NvcAddr like '%{0}%' ", key);
                sb.AppendFormat(" or NvcVillage like '%{0}%' or VcMobile = '{0}' or VcTelNo = '{0}')) ", key);
                sb.AppendFormat(" or VcAddr like '%{0}%' or NvcName like '%{0}%' or NvcAddr like '%{0}%' or VcSimNo='{0}' ", key);
            }

            var data = baseBLL.SqlTable(sb.ToString());
            var treeList = new List<TreeData>();

            foreach (DataRow item in data.Rows)
            {
                var d = new TreeData();
                if (item["IntOnline"].ToString() == "1") d.iconCls = "icon-online";
                else d.iconCls = "icon-offline";
                d.id = item["VcAddr"].ToString();
                d.text = item["NvcName"].ToString();
                d.IntConID = item["IntID"].ToString().ToInt32();
                d.ConcentratorAddr = item["VcAddr"].ToString();

                treeList.Add(d);
            }

            return ToJsonContentDate(treeList);
        }

        public ActionResult ManagerJsonTree()
        {
            //var key = SqlTextClear(Request["key"]);
            var sb = new System.Text.StringBuilder();
            sb.AppendFormat("select * from ArcConcentratorInfo ");
            //if (key.IsNotNullOrEmpty())
            //{
            //    sb.AppendFormat(" where IntID in (select IntConID from ArcMeterInfo ");
            //    sb.AppendFormat(" where VcAddr='{0}' or NvcName like '%{0}%' ", key);
            //    sb.AppendFormat(" or IntCustNO in  ");
            //    sb.AppendFormat(" (select IntNo from ArcCustomerInfo ");
            //    sb.AppendFormat(" where IntNo like '%{0}%' or NvcName = '{0}' or NvcAddr like '%{0}%' ", key);
            //    sb.AppendFormat(" or NvcVillage like '%{0}%' or VcMobile = '{0}' or VcTelNo = '{0}')) ", key);
            //    sb.AppendFormat(" or VcAddr like '%{0}%' or NvcName like '%{0}%' or NvcAddr like '%{0}%' or VcSimNo='{0}' ", key);
            //}
            var dt = baseBLL.SqlTable(sb.ToString());
            var children = new List<TreeData>();
            var drs = dt.Select("intUpID=0");
            foreach (DataRow item in drs)
            {
                var d = new TreeData();
                var isOnline = item["IntOnline"].ToString() == "1";
                if (isOnline) d.iconCls = "icon-online";
                else d.iconCls = "icon-offline";
                d.id = item["VcAddr"].ToString();
                d.text = item["NvcName"].ToString();
                d.IntConID = item["IntID"].ToString().ToInt32();
                d.ConcentratorAddr = item["VcAddr"].ToString();
                d.children = getTreeChildren2(d.IntConID, dt, isOnline);
                children.Add(d);
            }

            var list = new List<TreeData>();
            var root = new TreeData();
            root.iconCls = "icon-house";
            root.id = "0";
            root.text = "根";
            root.IntConID = 0;
            root.ConcentratorAddr = "";
            root.children = children;
            list.Add(root);

            return ToJsonContentDate(list);
        }
        private List<TreeData> getTreeChildren2(int intUpID, DataTable dt, bool isOnline)
        {
            List<TreeData> treeList = new List<TreeData>();
            var arr = dt.Select("IntUpID=" + intUpID);
            foreach (DataRow item in arr)
            {
                var d = new TreeData();
                if (!isOnline)
                {
                    isOnline = item["IntOnline"].ToString() == "1";
                }
                if (isOnline) d.iconCls = "icon-online";
                else d.iconCls = "icon-offline";
                d.id = item["VcAddr"].ToString();
                d.text = item["NvcName"].ToString();
                d.IntConID = item["IntID"].ToString().ToInt32();
                d.ConcentratorAddr = item["VcAddr"].ToString();
                d.children = getTreeChildren2(d.IntConID, dt, isOnline);
                treeList.Add(d);
            }
            return treeList;
        }


        public ActionResult ValidateData()
        {
            CommonResult result = new CommonResult() { Success = true };
            try
            {
                var fn = Request["fn"];
                switch (fn)
                {
                    case "1":
                        Fn1(result, fn);
                        break;
                    case "3":
                        Fn3(result, fn);
                        break;
                    case "58":
                        Fn58(result, fn);
                        break;
                    case "89":
                        break;
                    case "10":
                        break;
                    case "33":
                        break;
                    default:
                        result.Success = false;
                        result.ErrorMessage = ERR.ArcConcentrator.UnkownFnParams;
                        break;
                }

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ERR.ArcConcentrator.ParamError + ex.Message;
            }
            return ToJsonContent(result);
        }

        private List<SqlParameter> GetParams(string fn, int order)
        {
            var listPara = new List<SqlParameter>();
            listPara.Add(new SqlParameter("@iFunc", SqlDbType.Int) { Value = fn });
            listPara.Add(new SqlParameter("@iOrder", SqlDbType.Int) { Value = order });
            listPara.Add(new SqlParameter("@sReturn", SqlDbType.VarChar, 255) { Direction = ParameterDirection.Output });
            return listPara;
        }

        private void Fn1(CommonResult result, string fn)
        {
            var heartCycle = Request["txtHeartCycle"].ToDecimal();

            var listPara = GetParams(fn, 6);
            var ds = baseBLL.ExecStoreProcToDataSet("up_GetAFN04DataRang", listPara);
            var ret = listPara[listPara.Count - 1].Value.ToString();
            if (ret == "0")
            {
                if (ds.Tables[0].Rows.Count == 1)
                {
                    var min = ds.Tables[0].Rows[0]["min"].ToString().ToDecimalOrZero();
                    var max = ds.Tables[0].Rows[0]["max"].ToString().ToDecimalOrZero();

                    if (min > heartCycle || max < heartCycle)
                    {
                        result.Success = false;
                        result.ErrorMessage = "心跳周期超出范围,应在{0}和{1}之间."
                                       .FormatWith(min.ToString("0"), max.ToString("0"));
                    }
                }
            }
            else
            {
                result.Success = false;
                result.ErrorMessage = ERR.ArcConcentrator.ExecStoreProcErr + ret;
            }
        }

        private void Fn3(CommonResult result, string fn)
        {
            var mainIP = Request["txtMainIP"];
            var mainPort = Request["txtMainPort"].ToInt();
            var backIP = Request["txtBackIP"];
            var backPort = Request["txtBackPort"].ToInt();

            //主用主站地址
            var listPara = GetParams(fn, 1);
            var ds = baseBLL.ExecStoreProcToDataSet("up_GetAFN04DataRang", listPara);
            var ret = listPara[listPara.Count - 1].Value.ToString();

            if (ret == "0")
            {
                if (ds.Tables[0].Rows.Count > 1)
                {
                    var startIP = ds.Tables[0].Rows[0][0].ToString();
                    var endIP = ds.Tables[0].Rows[1][0].ToString();

                    if (!DBLib.Validate.IsIP(mainIP, startIP, endIP))
                    {
                        result.Success = false;
                        result.ErrorMessage = "主用主站地址 超出范围,应在{0}和{1}之间.".FormatWith(startIP, endIP);
                        return;
                    }
                }
            }
            else
            {
                result.Success = false;
                result.ErrorMessage = ERR.ArcConcentrator.ExecStoreProcErr + ret;
                return;
            }


            //备用主站地址
            if (backIP != "0.0.0.0")
            {
                listPara = GetParams(fn, 3);
                ds = baseBLL.ExecStoreProcToDataSet("up_GetAFN04DataRang", listPara);
                ret = listPara[listPara.Count - 1].Value.ToString();

                if (ret == "0")
                {
                    if (ds.Tables[0].Rows.Count > 1)
                    {
                        var startIP = ds.Tables[0].Rows[0][0].ToString();
                        var endIP = ds.Tables[0].Rows[1][0].ToString();

                        if (!DBLib.Validate.IsIP(backIP, startIP, endIP))
                        {
                            result.Success = false;
                            result.ErrorMessage = "备用主站地址 超出范围,应在{0}和{1}之间.".FormatWith(startIP, endIP);
                            return;
                        }
                    }
                }
                else
                {
                    result.Success = false;
                    result.ErrorMessage = ERR.ArcConcentrator.ExecStoreProcErr + ret;
                    return;
                }
            }

            //主用主站端口
            listPara = GetParams(fn, 2);
            ds = baseBLL.ExecStoreProcToDataSet("up_GetAFN04DataRang", listPara);
            ret = listPara[listPara.Count - 1].Value.ToString();

            if (ret == "0")
            {
                if (ds.Tables[0].Rows.Count == 1)
                {
                    var min = ds.Tables[0].Rows[0]["min"].ToString().ToDecimalOrZero();
                    var max = ds.Tables[0].Rows[0]["max"].ToString().ToDecimalOrZero();

                    if (min > mainPort || max < mainPort)
                    {
                        result.Success = false;
                        result.ErrorMessage = "主用主站端口 超出范围,应在{0}和{1}之间.".FormatWith(min.ToString(), max.ToString());
                        return;
                    }
                }
            }
            else
            {
                result.Success = false;
                result.ErrorMessage = ERR.ArcConcentrator.ExecStoreProcErr + ret;
                return;
            }


            //备用主站端口
            if (backPort != 0)
            {
                listPara = GetParams(fn, 4);
                ds = baseBLL.ExecStoreProcToDataSet("up_GetAFN04DataRang", listPara);
                ret = listPara[listPara.Count - 1].Value.ToString();

                if (ret == "0")
                {
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        var min = ds.Tables[0].Rows[0]["min"].ToString().ToDecimalOrZero();
                        var max = ds.Tables[0].Rows[0]["max"].ToString().ToDecimalOrZero();

                        if (min > backPort || max < backPort)
                        {
                            result.Success = false;
                            result.ErrorMessage = "备用主站端口 超出范围,应在{0}和{1}之间.".FormatWith(min.ToString(), max.ToString());
                            return;
                        }
                    }
                }
                else
                {
                    result.Success = false;
                    result.ErrorMessage = ERR.ArcConcentrator.ExecStoreProcErr + ret;
                    return;
                }
            }

        }

        private void Fn58(CommonResult result, string fn)
        {
            var txtConnetTime = Request["txtConnetTime"].ToDecimal();

            var listPara = GetParams(fn, 1);
            var ds = baseBLL.ExecStoreProcToDataSet("up_GetAFN04DataRang", listPara);
            var ret = listPara[listPara.Count - 1].Value.ToString();
            if (ret == "0")
            {
                if (ds.Tables[0].Rows.Count == 1)
                {
                    var min = ds.Tables[0].Rows[0]["min"].ToString().ToDecimalOrZero();
                    var max = ds.Tables[0].Rows[0]["max"].ToString().ToDecimalOrZero();

                    if (min > txtConnetTime || max < txtConnetTime)
                    {
                        result.Success = false;
                        result.ErrorMessage = "允许与主站连续无通信时间 超出范围,应在{0}和{1}之间.".FormatWith(min.ToString(), max.ToString());
                    }
                }
            }
            else
            {
                result.Success = false;
                result.ErrorMessage = ERR.ArcConcentrator.ExecStoreProcErr + ret;
            }
        }
        private void Fn89(CommonResult result, string fn)
        {
            //var txtConnetTime = Request["txtConnetTime"].ToDecimal();

            //var listPara = GetParams(fn, 1);
            //var ds = baseBLL.ExecStoreProcToDataSet("up_GetAFN04DataRang", listPara);
            //var ret = listPara[listPara.Count - 1].Value.ToString();
            //if (ret == "0")
            //{
            //    if (ds.Tables[0].Rows.Count == 1)
            //    {
            //        var min = ds.Tables[0].Rows[0]["min"].ToString().ToDecimalOrZero();
            //        var max = ds.Tables[0].Rows[0]["max"].ToString().ToDecimalOrZero();

            //        if (min > txtConnetTime || max < txtConnetTime)
            //        {
            //            result.Success = false;
            //            result.ErrorMessage = "允许与主站连续无通信时间 超出范围,应在{0}和{1}之间.".FormatWith(min.ToString(), max.ToString());
            //        }
            //    }
            //}
            //else
            //{
            //    result.Success = false;
            //    result.ErrorMessage = ERR.ArcConcentrator.ExecStoreProcErr + ret;
            //}
        }

        public override ActionResult Insert(Core.Entity.ArcConcentratorInfo info)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.InsertKey);

            CommonResult result = new CommonResult();
            DbTransaction dbTransaction = BLLFactory<Core.BLL.ArcConcentratorInfo>.Instance.CreateTransaction();
            try
            {
                var flg = baseBLL.Insert(info, dbTransaction);

                if (flg)
                {
                    //var layerParent = GetLayerParent(info.IntUpID.ToString(), dbTransaction);
                    //var layerChilden = GetLayerChilden(info.IntID.ToString(), dbTransaction);
                    ////层次关系不能大于3
                    //if (layerChilden + layerParent > 3)
                    //{
                    //    dbTransaction.Rollback();
                    //    result.ErrorMessage = "父级设备层次关系不能大于3,即一个集中器下最多能挂三个中继器!";
                    //}
                    //else
                    //{
                    dbTransaction.Commit();
                    result.Success = true;
                    //}
                }
                else
                {
                    dbTransaction.Rollback();
                }
            }
            catch (Exception ex)
            {
                dbTransaction.Rollback();
                LogTextHelper.Error(ex);//错误记录
                result.ErrorMessage = ex.Message;
            }
            return ToJsonContent(result);
        }

        public  ActionResult Insert_server(Concentrator info)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.InsertKey);

            CommonResult result = new CommonResult();

            info.IntEndCode = 0;

            try
            {
                //调用后台服务获取集中器信息
                ServiceDbClient DbServer = new ServiceDbClient();

                var flg = DbServer.ArcConcentrator_Ins(info);
              
                if (flg=="0")
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

        [HttpPost]
        public new ActionResult Update(string id, Core.Entity.ArcConcentratorInfo info)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.UpdateKey);

            CommonResult result = new CommonResult();
            if (info.IntUpID == info.IntID)
            {
                result.ErrorMessage = "不能选择自己作为父级设备";
                return ToJsonContent(result);
            }
            //判断当前 选择的父级 是否为当前设备下的子级
            var listChilden = new List<int>();
            GetChilden(info.IntID.ToString(), ref listChilden);
            if (listChilden.Contains(info.IntUpID))
            {
                result.ErrorMessage = "父级设备不能是当前设备下的子级设备!请重新选择!";
                return ToJsonContent(result);
            }
             
            DbTransaction dbTransaction = BLLFactory<Core.BLL.ArcConcentratorInfo>.Instance.CreateTransaction();
            try
            {
                var flg = baseBLL.Update(info, info.IntID, dbTransaction);

                //if (flg)
                //{
                //    var layerParent = GetLayerParent(info.IntUpID.ToString(), dbTransaction);
                //    var layerChilden = GetLayerChilden(info.IntID.ToString(), dbTransaction);
                //    //层次关系不能大于3
                //    if (layerChilden + layerParent > 3)
                //    {
                //        dbTransaction.Rollback();
                //        result.ErrorMessage = "父级设备层次关系不能大于3,即一个集中器下最多能挂三个中继器!";
                //    }
                //    else
                //    {
                dbTransaction.Commit();
                result.Success = flg;
                //}
                //}
            }
            catch (Exception ex)
            {
                dbTransaction.Rollback();
                LogTextHelper.Error(ex);//错误记录
                result.ErrorMessage = ex.Message;
            }
            return ToJsonContent(result);
        }

        public  ActionResult Update_Server(string id, Concentrator info)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.UpdateKey);

            //赋值
            info.IntID = Convert.ToInt32(id);
           
            CommonResult result = new CommonResult();
            if (info.IntUpID == info.IntID)
            {
                result.ErrorMessage = "不能选择自己作为父级设备";
                return ToJsonContent(result);
            }
            //判断当前 选择的父级 是否为当前设备下的子级
            var listChilden = new List<int>();
            GetChilden(info.IntID.ToString(), ref listChilden);
            if (listChilden.Contains(info.IntUpID))
            {
                result.ErrorMessage = "父级设备不能是当前设备下的子级设备!请重新选择!";
                return ToJsonContent(result);
            }

            //厂家编码
            info.IntEndCode = 0;
        
            try
            {
                //调用后台服务获取集中器信息
                ServiceDbClient DbServer = new ServiceDbClient();

                var flg = DbServer.ArcConcentrator_Upd(info);

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


        private int GetLayerParent(string intID, DbTransaction dbTransaction)
        {
            if (intID.ToInt() == 0) return 0;
            var sql = "select IntID,IntUpID from ArcConcentratorInfo where IntID={0}";
            int layer = 0;
            var rows = BLLFactory<Core.BLL.ArcConcentratorInfo>.Instance.SqlTable(sql.FormatWith(intID), dbTransaction).Rows;
            if (rows.Count > 0)
            {
                layer++;
                layer += GetLayerParent(rows[0]["IntUpID"].ToString(), dbTransaction);
            }
            return layer;
        }
        private int GetLayerChilden(string intUpID, DbTransaction dbTransaction)
        {
            var sql = "select IntID,IntUpID from ArcConcentratorInfo where IntUpID={0}";
            int layer = 0;
            var dt = BLLFactory<Core.BLL.ArcConcentratorInfo>.Instance.SqlTable(sql.FormatWith(intUpID), dbTransaction);
            if (dt.Rows.Count > 0)
            {
                var listLayer = new List<int>();
                foreach (DataRow item in dt.Rows)
                {
                    listLayer.Add(GetLayerChilden(item["IntID"].ToString(), dbTransaction));
                }
                var maxLayer = listLayer.Max();
                layer = maxLayer + 1;
            }
            return layer;
        }

        private List<int> GetChilden(string intUpID, ref List<int> list, DbTransaction dbTransaction = null)
        {
            var sql = "select IntID,IntUpID from ArcConcentratorInfo where IntUpID={0}";
            var dt = BLLFactory<Core.BLL.ArcConcentratorInfo>.Instance.SqlTable(sql.FormatWith(intUpID), dbTransaction);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    list.Add(item["IntID"].ToString().ToInt());
                    GetChilden(item["IntID"].ToString(), ref list, dbTransaction);
                }
            }
            return list;
        }
    }
}
