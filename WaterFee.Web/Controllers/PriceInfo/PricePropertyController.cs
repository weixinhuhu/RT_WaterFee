using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.Mvc;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;
using WHC.Security.Entity;
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

        public ActionResult ListJson()
        {
            return base.FindWithPager();
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

        public override ActionResult Insert(Core.Entity.PriceProperty info)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.InsertKey);
            Framework.Commons.CommonResult result = new Framework.Commons.CommonResult();
            try
            {
                info.IntNo = BLLFactory<Core.BLL.PriceProperty>.Instance.GetMaxIntNo();
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
        public ActionResult Update(Core.Entity.PriceProperty info)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.UpdateKey);

            Framework.Commons.CommonResult result = new Framework.Commons.CommonResult();
            try
            {
                result.Success = baseBLL.Update(info, info.IntNo);
            }
            catch (Exception ex)
            {
                LogTextHelper.Error(ex);//错误记录
                result.ErrorMessage = ex.Message;
            }
            return ToJsonContent(result);
        }

        public ActionResult Setting()
        {
            //ViewBag.Title = "档案列表";
            return View();
        }

        public ActionResult SettingJson()
        {
            return base.FindWithPager();
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
        public ActionResult AddOrUpdate(Core.Entity.PriceProperty info)
        {
            Framework.Commons.CommonResult result = new Framework.Commons.CommonResult();
            var dbTran = BLLFactory<Core.BLL.PriceProperty>.Instance.CreateTransaction();
            try
            {
                var arrTypeNo = Request["ArrTypeNo"].Split(',');
                var isAdd = info.IntNo <= 0;
                //PriceProperty
                var errorCount = 0;
                if (isAdd)
                {
                    info.DteEnd = DateTime.MaxValue;
                    info.IntNo = BLLFactory<Core.BLL.PriceProperty>.Instance.GetMaxIntNo(dbTran);
                    errorCount = baseBLL.Insert(info, dbTran) ? errorCount : errorCount + 1;
                }
                else
                    errorCount = baseBLL.Update(info, info.IntNo, dbTran) ? errorCount : errorCount + 1;

                if (errorCount == 0)
                {
                    if (!isAdd)//如果是编辑,则先删除原先的数据再新增
                    {
                        var sbDelete = new System.Text.StringBuilder();
                        //sbDelete.AppendFormat("delete PriceList where IntNo in (select IntListNo from PriceComposition where IntCalcNo in (select IntNo from PriceCalc where IntPropertyNo={0}));", info.IntNo);
                        sbDelete.AppendFormat("delete PriceComposition where IntCalcNo in (select IntNo from PriceCalc where IntPropertyNo={0});", info.IntNo);
                        sbDelete.AppendFormat("delete PriceCalc where IntPropertyNo={0};", info.IntNo);
                        BLLFactory<Core.BLL.PriceProperty>.Instance.SqlExecute(sbDelete.ToString(), dbTran);
                    }

                    var stepCount = RRequest("IntStepCount").ToInt32();
                    stepCount = info.IntStep == 1 ? stepCount : 1;//阶梯,如果是则是页面的阶梯数,不是则为1
                    for (int i = 1; i <= stepCount; i++)
                    {
                        //PriceCalc
                        var calc = new Core.Entity.PriceCalc();
                        var calcNo = BLLFactory<Core.BLL.PriceCalc>.Instance.GetMaxIntNo(dbTran);
                        calc.IntNo = calcNo;
                        calc.IntPropertyNo = info.IntNo;
                        calc.IntStepInc = RRequest("IntStepInc" + i).ToIntOrZero();
                        calc.IntStepOrder = info.IntStep == 1 ? i : 0;//不是阶梯则为0
                        calc.IntStepStart = RRequest("IntStepStart" + i).ToIntOrZero();
                        if (i < stepCount)
                            calc.IntStepEnd = RRequest("IntStepStart" + (i + 1)).ToIntOrZero();
                        else
                            calc.IntStepEnd = int.MaxValue;

                        calc.IntUserNo = CurrentUser.ID;
                        calc.NumTotalPrice = RRequest("NumTotalPrice" + i).ToDecimalOrZero();
                        calc.NvcDesc = info.IntStep == 1 ? ("阶梯" + i) : "非阶梯";
                        var isOk = BLLFactory<Core.BLL.PriceCalc>.Instance.Insert(calc, dbTran);
                        if (!isOk)
                        {
                            errorCount++;
                            goto END;
                        }

                        foreach (var intTypeNo in arrTypeNo)
                        {
                            //查询同一IntTypeNo和价格的是否已存在,如存在则用,不存在新增
                            //PriceList
                            var numPrice = RRequest("NumPrice" + i + "_" + intTypeNo).ToDecimalOrZero();
                            var modelPriceList = BLLFactory<Core.BLL.PriceList>.Instance.Find("IntTypeNo={0} and NumPrice='{1}'"
                                               .FormatWith(intTypeNo, numPrice), dbTran).FirstOrDefault();
                            if (modelPriceList == null)
                            {
                                modelPriceList = new Core.Entity.PriceList();
                                modelPriceList.IntNo = BLLFactory<Core.BLL.PriceList>.Instance.GetMaxIntNo(dbTran);
                                modelPriceList.IntTypeNo = intTypeNo.ToInt();
                                modelPriceList.IntUserNo = CurrentUser.ID;
                                modelPriceList.NumPrice = RRequest("NumPrice" + i + "_" + intTypeNo).ToDecimalOrZero();
                                modelPriceList.NvcDesc = intTypeNo + "单价";
                                modelPriceList.DtCreate = DateTime.Now;
                                modelPriceList.DteEnd = info.DteEnd;
                                modelPriceList.DteStart = info.DteStart;

                                if (!BLLFactory<Core.BLL.PriceList>.Instance.Insert(modelPriceList, dbTran))
                                {
                                    errorCount++;
                                    goto END;
                                }
                            }

                            //PriceComposition
                            var modelComposition = new Core.Entity.PriceComposition();
                            modelComposition.IntListNo = modelPriceList.IntNo;
                            modelComposition.IntCalcNo = calc.IntNo;
                            modelComposition.IntUserNo = CurrentUser.ID;
                            if (!BLLFactory<Core.BLL.PriceComposition>.Instance.Insert(modelComposition, dbTran))
                            {
                                errorCount++;
                                goto END;
                            }
                        }
                    }
                }

                END:
                result.Success = errorCount == 0;
                if (result.Success)
                {
                    dbTran.Commit();
                }
                else
                {
                    dbTran.Rollback();
                    result.ErrorMessage = "ErrorCount={0}".FormatWith(errorCount);
                }
            }
            catch (Exception ex)
            {
                dbTran.Rollback();
                LogTextHelper.Error(ex);//错误记录
                result.ErrorMessage = ex.Message;
            }

            return ToJsonContent(result);
        }


    }
}
