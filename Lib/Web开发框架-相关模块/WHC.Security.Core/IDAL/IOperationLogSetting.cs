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
    /// 记录操作日志的数据表配置
    /// </summary>
	public interface IOperationLogSetting : IBaseDAL<OperationLogSettingInfo>
	{
    }
}