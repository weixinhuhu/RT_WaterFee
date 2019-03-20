using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using WHC.WaterFeeWeb.Core.IDAL;
using WHC.Framework.Commons;

namespace WHC.WaterFeeWeb.Core.BLL
{
    public class PriceList : WHC.Framework.ControlUtil.BaseBLL<Entity.PriceList>
    {
        public PriceList() : base()
        { 
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }
        public int GetMaxIntNo(System.Data.Common.DbTransaction trans = null)
        {
            //return GetMaxID(trans) + 1;
            var sql = "select Max(intNo) from PriceList";
            var r = SqlTable(sql, trans).Rows[0][0].ToString();
            int intNo = 1001;
            if (string.IsNullOrWhiteSpace(r) == false)
            {
                intNo = Convert.ToInt32(r) + 1;
            }
            return intNo;
        }
    }
}