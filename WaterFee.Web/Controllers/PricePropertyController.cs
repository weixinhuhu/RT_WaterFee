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
using WHC.Attachment.BLL;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;
using WHC.Pager.Entity;

namespace WHC.WaterFeeWeb.Controllers
{
    public class PricePropertyController : BusinessControllerNew<Core.BLL.PriceProperty, Core.Entity.PriceProperty>
    {
        [DataContract]
        [Serializable]
        public class TreeData : EasyTreeData
        {
            [DataMember]
            public Core.Entity.PriceProperty data { get; set; }
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

        public ActionResult GetTreeJson()
        {
            var list = BLLFactory<Core.BLL.PriceProperty>.Instance.GetAll(" IntNo ");
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
           // tree.Add(new TreeData { id="0",text="0"});
            return ToJsonContentDate(tree);
        }

        public override ActionResult Insert(Core.Entity.PriceProperty info)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.InsertKey);

            CommonResult result = new CommonResult();
            try
            {
                //string filter = string.Format("Name='{0}' ", info.Name);
                //bool isExist = BLLFactory<User>.Instance.IsExistRecord(filter);
                //if (isExist)
                //{
                //    result.ErrorMessage = "指定用户名重复，请重新输入！";
                //}
                //else
                //{
                info.IntNo = BLLFactory<Core.BLL.PriceProperty>.Instance.GetMaxIntNo();
                result.Success = baseBLL.Insert(info);
                //}
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

            CommonResult result = new CommonResult();
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


        public ActionResult GetInfoByIntPropertyNo()
        {
            var IntPropertyNo = RRequest("IntPropertyNo");
            var sql = @"select IntPropertyNo,NumTotalPrice,IntStepOrder,IntStepStart,IntStepEnd,IntStepInc,IntCalcNo,IntListNo,IntTypeNo,NumPrice,PriceList.NvcDesc PriceListDesc  
from PriceCalc left join PriceComposition on PriceCalc.IntNo=PriceComposition.IntCalcNo
left join PriceList on PriceComposition.IntListNo=PriceList.IntNo
where PriceCalc.IntPropertyNo=" + IntPropertyNo;
            var dt = BLLFactory<Core.BLL.PriceProperty>.Instance.SqlTable(sql);
            return ToJsonContentDate(dt);
        }

        public ActionResult AddOrUpdate(Core.Entity.PriceProperty info)
        {
            CommonResult result = new CommonResult();
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
                            calc.IntStepEnd = RRequest("IntStepStart" + (i + 1)).ToIntOrZero() ;
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
