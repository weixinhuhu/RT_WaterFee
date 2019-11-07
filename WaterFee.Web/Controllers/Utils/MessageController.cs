using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WHC.Framework.Commons;

namespace WHC.WaterFeeWeb.Controllers
{
    public class MessageController : BusinessControllerNew<Core.BLL.MessageTemplate, Core.Entity.MessageTemplate>
    {
        // GET: Message
        public ActionResult TemplateSetting()
        {
            return View();
        }

        public ActionResult TemplateList()
        {
            return base.FindWithPager();
        }

        public override ActionResult Insert(Core.Entity.MessageTemplate info)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.InsertKey);

            CommonResult result = new CommonResult();
            //DbTransaction dbTransaction = BLLFactory<Core.BLL.ArcConcentratorInfo>.Instance.CreateTransaction();
            try
            {
                info.DtCreate = DateTime.Now;
                result.Success = baseBLL.Insert(info);
            }
            catch (Exception ex)
            {
                LogTextHelper.Error(ex);//错误记录
                result.ErrorMessage = ex.Message;
            }
            return ToJsonContent(result);
        }


        public ActionResult Update(Core.Entity.MessageTemplate info)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.UpdateKey);

            CommonResult result = new CommonResult();
            try
            {
                result.Success = baseBLL.Update(info, info.ID);
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