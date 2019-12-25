using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.Mvc;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;
using WHC.WaterFeeWeb.DbServiceReference;

namespace WHC.WaterFeeWeb.Controllers
{
    public class PricePropertyController : BusinessControllerNew<Core.BLL.PriceProperty, Core.Entity.PriceProperty>
    {
        [DataContract]
        [Serializable]
        public class TreeData : EasyTreeData
        {
            [DataMember]
            // public Core.Entity.PriceProperty data { get; set; }
            public PriceProperty data { get; set; }

        }

        // GET: CustomerInfo
        public ActionResult List()
        {
            //ViewBag.Title = "档案列表";
            return View();
        }
      
        public ActionResult GetListJson_Server()
        {
            ServiceDbClient DbServer = new ServiceDbClient();
            var endcode = Session["EndCode"] ?? "0";
            var list = DbServer.PriceProperty_GetList(endcode.ToString().ToInt32());
            var tree = new List<TreeData>();
            foreach (var item in list)
            {
                tree.Add(new TreeData()
                {
                    id = item.IntNo.ToString(),
                    text = item.NvcDesc,
                    data = item
                });
            }
            return ToJsonContentDate(tree);
        }
        public ActionResult GetTreeJson_Server()
        {
            var endcode = Session["EndCode"] ?? "0";
            ServiceDbClient DbServer = new ServiceDbClient();
            var tree = DbServer.PriceProperty_GetTreeJson(endcode.ToString().ToInt32());
            return ToJsonContentDate(tree);
        }
    
        public ActionResult Setting()
        {
            //ViewBag.Title = "档案列表";
            return View();
        }
   
        public ActionResult GetInfoByIntPropertyNo_Server()
        {
            var IntPropertyNo = RRequest("IntPropertyNo").ToInt32();
            //var IntPropertyNo = 1003;
            ServiceDbClient DbServer = new ServiceDbClient();
            var dt = DbServer.PriceProperty_GetByNo(IntPropertyNo);
            return ToJsonContentDate(dt);
        }

        public ActionResult AddOrUpdate_Server(PriceProperty info)
        {
            Framework.Commons.CommonResult result = new Framework.Commons.CommonResult();

            //价格明细
            var lstPrice = new List<PriceDetail>();
            var stepCount = RRequest("IntStepCount").ToInt32();
            //阶梯,如果是则是页面的阶梯数,不是则为1
            stepCount = info.IntStep == 1 ? stepCount : 1;
            for (int i = 1; i <= stepCount; i++)
            {
                PriceDetail price_detail_info = new PriceDetail
                {
                    //阶梯数
                    IntStepOrder = (uint)i,
                    //阶梯起始量
                    IntStepStart = (uint)RRequest("IntStepStart" + i).ToIntOrZero(),
                    // 阶梯增量
                    IntStepInc = (uint)RRequest("IntStepInc" + i).ToIntOrZero(),
                    // 总价格
                    TotalPrice = (double)RRequest("NumTotalPrice" + i).ToDecimalOrZero()
                };
                //价格分项
                Dictionary<int, double> price_info = new Dictionary<int, double>();
                var arrTypeNo = Request["ArrTypeNo"].Split(',');
                foreach (var intTypeNo in arrTypeNo)
                {
                    price_info.Add(intTypeNo.ToInt32(), (double)RRequest("NumPrice" + i + "_" + intTypeNo).ToDecimalOrZero());
                }
                price_detail_info.Price = price_info;
                lstPrice.Add(price_detail_info);
            }
            //厂家编码
            var endcode = Session["EndCode"] ?? "0";
            info.IntEndCode = endcode.ToString().ToInt32();
            //操作员
            info.IntUserNo = CurrentUser.ID;
            try
            {
                ServiceDbClient DbServer = new ServiceDbClient();
                var flag = DbServer.PriceProperty_AddOrUpdate(info, lstPrice.ToArray());
                if (flag == "0")
                {
                    result.Success = true;
                }
                else
                {
                    result.ErrorMessage = flag;
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
    }
}
