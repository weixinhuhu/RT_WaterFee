using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using WHC.CRM.Entity;
using WHC.CRM.IDAL;
using WHC.Pager.Entity;
using WHC.Framework.ControlUtil;

namespace WHC.CRM.BLL
{
    /// <summary>
    /// 联系人组别
    /// </summary>
	public class ContactGroup : BaseBLL<ContactGroupInfo>
    {
        public ContactGroup() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            baseDal.OnOperationLog += new OperationLogEventHandler(WHC.Security.BLL.OperationLog.OnOperationLog);//如果需要记录操作日志，则实现这个事件
        }

        /// <summary>
        /// 根据用户，获取树形结构的分组列表
        /// </summary>
        public List<ContactGroupNodeInfo> GetTree(string creator)
        {
            IContactGroup dal = baseDal as IContactGroup;
            return dal.GetTree(creator);
        }

        /// <summary>
        /// 根据联系人ID，获取客户对应的分组集合
        /// </summary>
        /// <param name="contactId">联系人ID</param>
        /// <returns></returns>
        public List<ContactGroupInfo> GetByContact(string contactId)
        {
            IContactGroup dal = baseDal as IContactGroup;
            return dal.GetByContact(contactId);
        }

        /// <summary>
        /// 根据用户标识，获取对应用户的分组集合
        /// </summary>
        /// <param name="creator">用户标识（用户登陆名称）</param>
        /// <returns></returns>
        public List<ContactGroupInfo> GetAllByUser(string creator)
        {
            string condition = string.Format("creator='{0}'", creator);
            return Find(condition);
        }
    }
}
