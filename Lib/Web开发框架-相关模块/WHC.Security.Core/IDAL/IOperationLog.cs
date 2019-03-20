using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using WHC.Pager.Entity;
using WHC.Framework.ControlUtil;
using WHC.Security.Entity;

namespace WHC.Security.IDAL
{
    /// <summary>
    /// 用户关键操作记录
    /// </summary>
	public interface IOperationLog : IBaseDAL<OperationLogInfo>
	{
    }
}