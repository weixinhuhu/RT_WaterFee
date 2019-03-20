using System;
using System.Runtime.Serialization;

namespace WHC.WaterFeeWeb.Core.Entity
{
    /// <summary>
    /// AccLedger:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary> 
    [DataContract]
    [Serializable]
    public partial class AccLedger : WHC.Framework.ControlUtil.BaseEntity
    {
		public AccLedger()
		{}
		#region Model
		private int _intfeeid;
		private int _intcustno;
		private int _intyearmon;
		private DateTime _dtefee;
		private int _intcalcsum;
		private decimal _monfeesum;
		private DateTime _dtlock;
		private DateTime _dtcalc;
		private int _intinvflag=0;
		private DateTime _dtcreate= DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntFeeID
		{
			set{ _intfeeid=value;}
			get{return _intfeeid;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntCustNo
		{
			set{ _intcustno=value;}
			get{return _intcustno;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntYearMon
		{
			set{ _intyearmon=value;}
			get{return _intyearmon;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public DateTime DteFee
		{
			set{ _dtefee=value;}
			get{return _dtefee;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntCalcSum
		{
			set{ _intcalcsum=value;}
			get{return _intcalcsum;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public decimal MonFeeSum
		{
			set{ _monfeesum=value;}
			get{return _monfeesum;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public DateTime DtLock
		{
			set{ _dtlock=value;}
			get{return _dtlock;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public DateTime DtCalc
		{
			set{ _dtcalc=value;}
			get{return _dtcalc;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntInvFlag
		{
			set{ _intinvflag=value;}
			get{return _intinvflag;}
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

