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
using WHC.Pager.Entity;


namespace WHC.WaterFeeWeb.Controllers
{
    public class PriceTypeController : BusinessControllerNew<Core.BLL.PriceType, Core.Entity.PriceType>
    {
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

        public ActionResult ListJson_Server()
        {
            var endcode = Session["EndCode"] ?? "0";
            //调用后台服务获取集中器信息
            DbServiceReference.ServiceDbClient DbServer = new DbServiceReference.ServiceDbClient();

            var dts = DbServer.PriceType_Qry(endcode.ToString().ToInt32());

            return ToJsonContentDate(dts);
        }

        public ActionResult GetTreeJson()
        {
            //System.Threading.Thread.Sleep(2000);
            var list = BLLFactory<Core.BLL.PriceType>.Instance.GetAll(" IntNo ");
            return ToJsonContentDate(list);
        }
        public ActionResult GetTreeJson_Server()
        {
            var endcode = Session["EndCode"] ?? "0";
            //调用后台服务获取集中器信息
            DbServiceReference.ServiceDbClient DbServer = new DbServiceReference.ServiceDbClient();

            var dts = DbServer.PriceType_GetTreeJson(endcode.ToString().ToInt32());

            return ToJsonContentDate(dts);
        }

        public ActionResult Insert_Server(DbServiceReference.PriceType info)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.InsertKey);
            CommonResult result = new CommonResult();
            try
            {
                var endcode = Session["EndCode"] ?? "0";
                //调用后台服务获取集中器信息
                DbServiceReference.ServiceDbClient DbServer = new DbServiceReference.ServiceDbClient();
                var flag = DbServer.PriceType_Ins(endcode.ToString().ToInt32(), info);
                if (flag == "0")
                {
                    result.Success = true;
                }
                else {
                    result.ErrorMessage = flag;
                }
            }
            catch (Exception ex)
            {
                LogTextHelper.Error(ex);//错误记录              
            }
            return ToJsonContent(result);
        }

        [HttpPost]
        public ActionResult Update_Server(DbServiceReference.PriceType info)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.UpdateKey);
            CommonResult result = new CommonResult();
            try
            {
                //调用后台服务获取集中器信息
                DbServiceReference.ServiceDbClient DbServer = new DbServiceReference.ServiceDbClient();
                var flag = DbServer.PriceType_Upd(info);
                if (flag == "0")
                {
                    result.Success = true;
                }
                else
                {
                    result.ErrorMessage = flag;
                }
            }
            catch (Exception ex)
            {
                LogTextHelper.Error(ex);//错误记录 
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
    }
}
