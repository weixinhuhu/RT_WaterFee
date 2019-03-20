using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WHC.MVCWebMis.Common
{
    /// <summary>
    /// 文件管理状态
    /// </summary>
    public enum FileManagerStatus
    {
        NotStarted,
        Aborted,
        Complete,
        InProgress
    }
}
