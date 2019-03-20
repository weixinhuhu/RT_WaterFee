using System;
using System.Runtime.Serialization;

namespace WHC.WaterFeeWeb.Core.Entity
{
    /// <summary>
    /// PriceComposition:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [DataContract]
    [Serializable]
	public partial class PriceComposition : WHC.Framework.ControlUtil.BaseEntity
    {
		public PriceComposition()
		{}
		#region Model
		private int _intcalcno;
		private int _intlistno;
		private int _intuserno;
		private DateTime _dtcreate= DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntCalcNo
		{
			set{ _intcalcno=value;}
			get{return _intcalcno;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntListNo
		{
			set{ _intlistno=value;}
			get{return _intlistno;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntUserNo
		{
			set{ _intuserno=value;}
			get{return _intuserno;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public DateTime DtCreate
		{
			set{ _dtcreate=value;}
			get{return _dtcreate;}
		}
		#endregion Model

	}
}

