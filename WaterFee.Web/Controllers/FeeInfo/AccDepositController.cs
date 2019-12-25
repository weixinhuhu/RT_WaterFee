using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;
using WHC.WaterFeeWeb.DbServiceReference;

namespace WHC.WaterFeeWeb.Controllers
{
    public class AccDepositController : BusinessControllerNew<Core.BLL.AccDeposit, Core.Entity.AccDeposit>
    {
      
        public ActionResult GetMoneyJson_Server()
        {
            var Strlevel = Request["WHC_Treelevel"] ?? "";
            var fuji = Request["WHC_Fuji"] ?? "";
            var Text = Request["WHC_Text"] ?? "";
            var ParentText = Request["WHC_TreePrentText"] ?? "";
            var custormerinfo = new Customer()
            {
                NvcName = Request["WHC_NvcName"] ?? "",
                NvcAddr = Request["WHC_NvcAddr"] ?? "",
                VcMobile = Request["WHC_VcMobile"] ?? ""
            };
            var useno = Request["Key"] ?? "0";
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
            var dts = DbServer.Account_GetMoney(endcode.ToString().ToInt32(), custormerinfo);

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
    }

}

