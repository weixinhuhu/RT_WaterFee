using System;
using System.Runtime.Serialization;

namespace WHC.WaterFeeWeb.Core.Entity
{
    /// <summary>
    /// AccPayment:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary> 
    [DataContract]
    [Serializable]
    public partial class AccPayment : WHC.Framework.ControlUtil.BaseEntity
    {
		public AccPayment()
		{}
		#region Model
		private int _intfeeid;
		private int _intcustno;
		private int _intyearmon;
		private DateTime _dtefee;
		private DateTime _dtepay;
		private DateTime _dteend;
		private DateTime _dteendexec;
		private decimal _monfee;
		private decimal _monfeeexec;
		private decimal _monpenalty;
		private int _intdays;
		private int _intpayunit;
		private int _intpaymode;
		private string _vcchargeno;
		private string _vcflowno;
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
        public DateTime DtePay
		{
			set{ _dtepay=value;}
			get{return _dtepay;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public DateTime DteEnd
		{
			set{ _dteend=value;}
			get{return _dteend;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public DateTime DteEndExec
		{
			set{ _dteendexec=value;}
			get{return _dteendexec;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public decimal MonFee
		{
			set{ _monfee=value;}
			get{return _monfee;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public decimal MonFeeExec
		{
			set{ _monfeeexec=value;}
			get{return _monfeeexec;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public decimal MonPenalty
		{
			set{ _monpenalty=value;}
			get{return _monpenalty;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntDays
		{
			set{ _intdays=value;}
			get{return _intdays;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntPayUnit
		{
			set{ _intpayunit=value;}
			get{return _intpayunit;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntPayMode
		{
			set{ _intpaymode=value;}
			get{return _intpaymode;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string VcChargeNo
		{
			set{ _vcchargeno=value;}
			get{return _vcchargeno;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string VcFlowNo
		{
			set{ _vcflowno=value;}
			get{return _vcflowno;}
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
        [DataMember]
        public virtual Entity.ArcCustomerInfo ArcCustomerInfo { get; set; }
    }
}

