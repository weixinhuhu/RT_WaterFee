using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using WHC.WaterFeeWeb.Core.IDAL;
using WHC.Framework.Commons;

namespace WHC.WaterFeeWeb.Core.BLL
{
    public class T_ACL_Department : WHC.Framework.ControlUtil.BaseBLL<Entity.T_ACL_Department>
    {
        public T_ACL_Department() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

        //public int GetMaxIntNo()
        //{
        //    var sql = "select Max(intNo) from T_ACL_Department";
        //    var r = SqlTable(sql).Rows[0][0].ToString();
        //    int intNo = 1001;
        //    if (string.IsNullOrWhiteSpace(r) == false)
        //    {
        //        intNo = Convert.ToInt32(r) + 1;
        //    }
        //    return intNo;
        //}
    }
}