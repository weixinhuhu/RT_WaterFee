using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using WHC.WaterFeeWeb.Core.IDAL;
using WHC.Framework.Commons;

namespace WHC.WaterFeeWeb.Core.BLL
{
    public partial class ArcCustomerInfo : WHC.Framework.ControlUtil.BaseBLL<Entity.ArcCustomerInfo>
    {
        public ArcCustomerInfo() : base()
        {
            base.Init(this.GetType().FullName, System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }

    }
}