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
    public class AccDepositController : BusinessControllerNew<Core.BLL.AccDeposit, Core.Entity.AccDeposit>
    {
        /// <summary>
        /// 预存款数据
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetMoneyInfo(string strWhere)
        {
            var sb = new System.Text.StringBuilder();
            sb.Append(" select AA.NvcName,AA.IntNo IntCustNo,AA.IntUnitNum,AA.NvcVillage,AA.VcBuilding,AA.IntRoomNum,IsNULL(BB.MonSum,0) MonSum,BB.DtLastUpd ,CC.IntMP,DD.VcAddr from ArcCustomerInfo AA ");
            sb.Append(" left join AccDeposit BB on AA.IntNo = BB.IntCustNO left join ArcMeterInfo CC on AA.IntNo=CC.IntCustNO and CC.IntStatus=0 left join ArcConcentratorInfo DD  on CC.IntTopConID=DD.IntID where 1 =1 ");
            sb.Append(strWhere);
            var list = BLLFactory<Core.DALSQL.ArcCustomerInfo>.Instance.SqlTable(sb.ToString());
            
            //非要减去欠费的账单 
            var listCustNo = new List<int>();
            foreach (DataRow item in list.Rows)
            {
                listCustNo.Add(item["IntCustNo"].ToString().ToInt());
            }
            if (listCustNo.Count > 0)
            {
                var sql = "SELECT SUM(MonFeeExec) MonFeeExec,IntCustNo FROM AccDebt WHERE IntStatus=0 and IntCustNo in ({0}) group by IntCustNo".FormatWith(string.Join(",", listCustNo));
                var dt = BLLFactory<Core.DALSQL.ArcCustomerInfo>.Instance.SqlTable(sql);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in list.Rows)
                    {
                        var r = dt.Select("IntCustNo=" + item["IntCustNo"].ToString());
                        if (r.Count() > 0)
                        {
                            item["MonSum"] = item["MonSum"].ToString().ToDecimalOrZero() - r[0]["MonFeeExec"].ToString().ToDecimalOrZero();
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 预存款数据
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataTable GetMoneyInfoBak(string strWhere)
        {
            var sb = new System.Text.StringBuilder();
            sb.Append(" select AA.NvcName,AA.IntNo,AA.IntCustNo,AA.IntUnitNum,IsNULL(BB.MonSum,0) MonSum,BB.DtLastUpd from ArcCustomerInfo AA ");
            sb.Append(" left join AccDeposit BB on AA.IntNo = BB.IntCustNO where 1=1 ");
            sb.Append(strWhere);
            var list = BLLFactory<Core.DALSQL.ArcCustomerInfo>.Instance.SqlTable(sb.ToString());

            return list;
        }

        //预存款数据
        public ActionResult GetMoneyJson()
        {   
            //weixin 2019.2.28修改分页bug
            var key = SqlTextClear(Request["key"]);
            var fuji = Request["WHC_Fuji"];
            var Text = Request["WHC_Text"];
            var Strlevel = Request["WHC_Treelevel"];
            var ParentText = Request["WHC_TreePrentText"];

            var sb = new System.Text.StringBuilder();
            sb.Append(" select AA.NvcName,AA.IntNo IntCustNo,AA.IntUnitNum,AA.NvcVillage,AA.VcBuilding,AA.IntRoomNum,IsNULL(BB.MonSum,0) MonSum,BB.DtLastUpd ,CC.IntMP,DD.VcAddr from ArcCustomerInfo AA ");
            sb.Append(" left join AccDeposit BB on AA.IntNo = BB.IntCustNO left join ArcMeterInfo CC on AA.IntNo=CC.IntCustNO and CC.IntStatus=0 left join ArcConcentratorInfo DD  on CC.IntTopConID=DD.IntID where 1 =1 ");
           
            if (Strlevel == "1")
            {
                sb.AppendFormat( " and NvcVillage = '所有小区' ");
            }

            if (Strlevel == "2")
            {
                sb.AppendFormat(" and NvcVillage = '" + Text + "' ");
            }

            if (Strlevel == "3")
            {
                sb.AppendFormat(" and NvcVillage = '" + fuji + "' ");
                sb.AppendFormat(" and VcBuilding='" + Text + "'");
            }

            if (Strlevel == "4")
            {
                sb.AppendFormat(" and NvcVillage = '" + ParentText + "' ");
                sb.AppendFormat(" and VcBuilding = '" + fuji + "' ");
                sb.AppendFormat(" and IntUnitNum='" + Text + "'");
            }

            if (key.IsNotNullOrEmpty())
            {
                sb.AppendFormat(" and AA.IntNo like '%{0}%' or AA.NvcName like '%{0}%'  or AA.NvcVillage like '%{0}%'or AA.VcBuilding like '%{0}%' or AA.IntUnitNum like '%{0}%' or AA.IntRoomNum like '%{0}%' or AA.VcMobile like '%{0}%' or AA.VcTelNo like '%{0}%' ", key);
            }
            var list = BLLFactory<Core.DALSQL.ArcCustomerInfo>.Instance.SqlTable(sb.ToString());
            int rows = Request["rows"] == null ? 10 : int.Parse(Request["rows"]);
            int page = Request["page"] == null ? 1 : int.Parse(Request["page"]);
            DataTable dat = new DataTable();
            //复制源的架构和约束
            dat = list.Clone();
            // 清除目标的所有数据
            dat.Clear();
            //对数据进行分页
            for (int i = (page - 1) * rows; i < page * rows && i < list.Rows.Count; i++)
            {
                dat.ImportRow(list.Rows[i]);
            }
            //最重要的是在后台取数据放在json中要添加个参数total来存放数据的总行数，如果没有这个参数则不能分页
            int total = list.Rows.Count;
            var result = new { total = total, rows = dat };
            return ToJsonContentDate(result);
        }       
    }
}
