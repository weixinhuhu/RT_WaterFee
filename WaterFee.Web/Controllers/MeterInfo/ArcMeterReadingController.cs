using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
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
        public ActionResult ListJson_Server()
        {
            var endcode = Session["EndCode"] ?? "";
            var WHC_StartDteFreeze = Request["WHC_StratDteFreeze"].ToDateTime();
            var WHC_EndDteFreeze = Request["WHC_EndDteFreeze"].ToDateTime();
            var fuji = Request["WHC_Fuji"];
            var Text = Request["WHC_Text"];
            var Strlevel = Request["WHC_Treelevel"];
            var ParentText = Request["WHC_TreePrentText"];
            var customerinfo = new DbServiceReference.Customer()
            {
                NvcName = Request["WHC_NvcName"] ?? "",
                VcMobile = Request["WHC_VcMobile"] ?? "",
            };
            var custno = Request["WHC_IntCustNo"] ?? "0";
            customerinfo.IntNo = custno == "" ? 0 : custno.ToInt();

            if (Strlevel == "1")
            {
                customerinfo.NvcVillage = "所有小区";
            };
            if (Strlevel == "2")
            {
                customerinfo.NvcVillage = Text;
            }
            if (Strlevel == "3")
            {
                customerinfo.NvcVillage = fuji;
                customerinfo.VcBuilding = Text;
            }
            if (Strlevel == "4")
            {
                customerinfo.NvcVillage = ParentText;
                customerinfo.VcBuilding = fuji;
                customerinfo.VcUnitNum = Text;
            }
            //调用后台服务获取集中器信息          
            var dts = new DbServiceReference.ServiceDbClient().CollectData_Qry(endcode.ToString().ToInt(), customerinfo, WHC_StartDteFreeze, WHC_EndDteFreeze);

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
        public ActionResult CollectStatJson_Server()
        {
            var endcode = Session["EndCode"] ?? "";
            var WHC_StartDteFreeze = Request["WHC_StratDteFreeze"].ToDateTime();
            var WHC_EndDteFreeze = Request["WHC_EndDteFreeze"].ToDateTime();
            var fuji = Request["WHC_Fuji"];
            var Text = Request["WHC_Text"];
            var Strlevel = Request["WHC_Treelevel"];
            var ParentText = Request["WHC_TreePrentText"];
            var customerinfo = new DbServiceReference.Customer()
            {
                NvcName = Request["WHC_NvcName"] ?? "",
                VcMobile = Request["WHC_VcMobile"] ?? "",
            };
            var custno = Request["WHC_IntCustNo"] ?? "0";
            customerinfo.IntNo = custno == "" ? 0 : custno.ToInt();

            if (Strlevel == "1")
            {
                customerinfo.NvcVillage = "所有小区";
            };
            if (Strlevel == "2")
            {
                customerinfo.NvcVillage = Text;
            }
            if (Strlevel == "3")
            {
                customerinfo.NvcVillage = fuji;
                customerinfo.VcBuilding = Text;
            }
            if (Strlevel == "4")
            {
                customerinfo.NvcVillage = ParentText;
                customerinfo.VcBuilding = fuji;
                customerinfo.VcUnitNum = Text;
            }
            //调用后台服务获取集中器信息          
            var dts = new DbServiceReference.ServiceDbClient().CollectStatus_Qry(endcode.ToString().ToInt(), customerinfo, WHC_StartDteFreeze, WHC_EndDteFreeze);

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
        public ActionResult CollectStatJsonTotal_Server()
        {
            CommonResult result = new CommonResult();
            var endcode = Session["EndCode"] ?? "";
            var WHC_StartDteFreeze = Request["WHC_StratDteFreeze"].ToDateTime();
            var WHC_EndDteFreeze = Request["WHC_EndDteFreeze"].ToDateTime();
            var fuji = Request["WHC_Fuji"];
            var Text = Request["WHC_Text"];
            var Strlevel = Request["WHC_Treelevel"];
            var ParentText = Request["WHC_TreePrentText"];
            var customerinfo = new DbServiceReference.Customer()
            {
                NvcName = Request["WHC_NvcName"] ?? "",
                VcMobile = Request["WHC_VcMobile"] ?? "",
            };
            var custno = Request["WHC_IntCustNo"] ?? "0";
            customerinfo.IntNo = custno == "" ? 0 : custno.ToInt();

            if (Strlevel == "1")
            {
                customerinfo.NvcVillage = "所有小区";
            };
            if (Strlevel == "2")
            {
                customerinfo.NvcVillage = Text;
            }
            if (Strlevel == "3")
            {
                customerinfo.NvcVillage = fuji;
                customerinfo.VcBuilding = Text;
            }
            if (Strlevel == "4")
            {
                customerinfo.NvcVillage = ParentText;
                customerinfo.VcBuilding = fuji;
                customerinfo.VcUnitNum = Text;
            }
            //调用后台服务获取集中器信息          
            var dts = new DbServiceReference.ServiceDbClient().CollectStatus_Qry(endcode.ToString().ToInt(), customerinfo, WHC_StartDteFreeze, WHC_EndDteFreeze);
            var m = new CollectStatModel();
            foreach (DataRow item in dts.Rows)
            {
                m.Used += item["Reading"].ToString().ToDecimalOrZero();
            }
            result.Data1 = dts.Rows.Count.ToString();
            result.Data2 = m.Used.ToString();
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
            var start = Request["dtStart"].ToDateTime();
            var end = Request["dtEnd"].ToDateTime();
            var endcode = Session["EndCode"] ?? "";
            var customerinfo = new DbServiceReference.Customer();
            var dt = new DbServiceReference.ServiceDbClient().CollectData_Qry(endcode.ToString().ToInt(), customerinfo, start, end);
            var chart = new ChartDataSimple()
            {
                categories = new List<string>(),
                series = new List<ChartDataSimple.SeriesItem>()
            };
            var si = new ChartDataSimple.SeriesItem() { data = new List<decimal?>(), name = "用量" };          
            for (var now = start; now <= end; now = now.AddDays(1))
            {
                chart.categories.Add(now.ToYyyyMMdd());
                var rows = dt.Select("DteFreeze='{0}'".FormatWith(now.ToYyyyMMdd()));
                if (rows.Count() > 0) si.data.Add(rows[0]["NumReading"].ToString().ToDecimalOrZero());             
                else si.data.Add(0);
            }
            chart.series.Add(si);
            return ToJsonContentDate(chart);
        }

        public ActionResult Insert()
        {
            return View();
        }

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
        
        public ActionResult ChangeTable()
        {
            return View();
        }

        public ActionResult ManualMeterReading()
        {
            return View();
        }
        [HttpPost]
        public ActionResult WriteNumReading_Server()
        {
            CommonResult result = new CommonResult();
            var endcode = Session["EndCode"] ?? "0";
            var IntCustNo = Request["IntCustNo"];
            var VcAddr = Request["VcAddr"];
            var DteFreeze = Request["DteFreeze"].ToDateTime();
            var NumReading = Request["NumReading"].ToDouble();
            var rs = new DbServiceReference.ServiceDbClient().CollectData_Ins(endcode.ToString().ToInt(), IntCustNo.ToInt32(), VcAddr, DteFreeze, NumReading, Session["UserID"].ToString());
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
    }
}
