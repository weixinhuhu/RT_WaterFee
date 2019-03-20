using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using WHC.Pager.Entity;
using WHC.Framework.ControlUtil;
using WHC.CRM.Entity;

namespace WHC.CRM.IDAL
{
    /// <summary>
    /// 通讯录分组
    /// </summary>
	public interface IAddressGroup : IBaseDAL<AddressGroupInfo>
	{
        /// <summary>
        /// 根据用户，获取树形结构的分组列表
        /// </summary>
        List<AddressGroupNodeInfo> GetTree(string addressType, string creator = null);

        /// <summary>
        /// 根据联系人ID，获取客户对应的分组集合
        /// </summary>
        /// <param name="contactId">联系人ID</param>
        /// <returns></returns>
        List<AddressGroupInfo> GetByContact(string contactId);
    }
}