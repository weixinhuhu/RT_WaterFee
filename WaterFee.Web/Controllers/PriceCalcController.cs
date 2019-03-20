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
    public class PriceCalcController : BusinessControllerNew<Core.BLL.PriceCalc, Core.Entity.PriceCalc>
    {
        [DataContract]
        [Serializable]
        public class TreeData : EasyTreeData
        {
            [DataMember]
            public Core.Entity.PriceCalc data { get; set; }
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
            var list = BLLFactory<Core.BLL.PriceCalc>.Instance.GetAll(" IntNo ");
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

        public ActionResult GetListByIntPropertyNo()
        {
            var no = RRequest("IntPropertyNo");
            var list = BLLFactory<Core.BLL.PriceCalc>.Instance.Find(" IntPropertyNo=" + no);
            return ToJsonContentDate(list);
        }

        public override ActionResult Insert(Core.Entity.PriceCalc info)
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
                //info.IntNo = BLLFactory<Core.BLL.PriceCalc>.Instance.GetMaxIntNo();
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
        public ActionResult Update(Core.Entity.PriceCalc info)
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


    }
}
