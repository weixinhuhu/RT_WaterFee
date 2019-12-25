namespace WHC.WaterFeeWeb
{
    public class ERR
    {
        public class ArcConcentrator
        {
            public const string ParamError = "ERR-1001:参数验证错误!<br/>";
            public const string ExecStoreProcErr = "ERR-1002:执行存储过程出错!<br/>";
            public const string UnkownFnParams = "ERR-1003:未知fn参数!<br/>";
        }

        public class ArcMeterInfo
        {
            public const string CustomerNoNotExist = "ERR-2001:客户编号不存在!<br/>";
            public const string ConcentratorNoNotExist = "ERR-2002:采集器编号不存在!<br/>";
            public const string ArcMeterInfoNotExist = "ERR-2003:表不存在!<br/>";

        }

    }
}