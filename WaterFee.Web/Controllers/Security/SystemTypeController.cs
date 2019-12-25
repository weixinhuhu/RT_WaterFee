using System.Collections.Generic;
using System.Web.Mvc;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;
using WHC.MVCWebMis.Entity;
using WHC.Security.BLL;
using WHC.Security.Entity;

namespace WHC.MVCWebMis.Controllers
{
    public class SystemTypeController : BusinessController<SystemType, SystemTypeInfo>
    {
        /// <summary>
        /// 获取系统类型的树Json
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTreeJson()
        {
            List<EasyTreeData> treeList = new List<EasyTreeData>();
            List<SystemTypeInfo> list = baseBLL.GetAll();
            foreach (SystemTypeInfo info in list)
            {
                treeList.Add(new EasyTreeData(info.OID, info.Name, "icon-computer"));
            }

            return ToJsonContent(treeList);
        }

        /// <summary>
        /// 用作下拉列表的菜单Json数据
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDictJson()
        {
            List<SystemTypeInfo> list = baseBLL.GetAll();
            List<CListItem> itemList = new List<CListItem>();
            foreach (SystemTypeInfo info in list)
            {
                itemList.Add(new CListItem(info.Name, info.OID));
            }
            return Json(itemList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据系统OID获取系统标识信息
        /// </summary>
        /// <param name="oid">系统OID</param>
        /// <returns>存在则返回指定的对象,否则返回Null</returns>
        public virtual ActionResult FindByOID(string oid)
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ViewKey);

            ActionResult result = Content("");
            SystemTypeInfo info = BLLFactory<SystemType>.Instance.FindByOID(oid);
            if (info != null)
            {
                result = ToJsonContentDate(info);
            }

            return result;
        }

    }
}
