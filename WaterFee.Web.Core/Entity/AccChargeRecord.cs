using System;
using System.Runtime.Serialization;

namespace WHC.WaterFeeWeb.Core.Entity
{
    /// <summary>
    /// AccChargeRecord:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [DataContract]
    [Serializable]
    public partial class AccChargeRecord : WHC.Framework.ControlUtil.BaseEntity
    {
		public AccChargeRecord()
		{}
		#region Model
		private int _intid;
		private string _vcchargeno;
		private int _intfeeid;
		private int _intcustno;
		private DateTime _dtefee;
		private string _vcflowno;
		private string _vcinvno;
		private int _intinvflag=0;
		private DateTime _dteaccount;
		private decimal _numfee;
		private decimal _numpenalty;
		private decimal _numlastplus;
		private decimal _numthisplus;
		private int _intchargeway;
		private int _intflag=0;
		private DateTime _dtlastupd= DateTime.Now;
		private DateTime _dtcreate= DateTime.Now;
        /// <summary>
        /// 
        /// </summary> 
          [DataMember]
        public int IntID
		{
			set{ _intid=value;}
			get{return _intid;}
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
        public DateTime DteFee
		{
			set{ _dtefee=value;}
			get{return _dtefee;}
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
        public string VcInvNo
		{
			set{ _vcinvno=value;}
			get{return _vcinvno;}
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
        public DateTime DteAccount
		{
			set{ _dteaccount=value;}
			get{return _dteaccount;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public decimal NumFee
		{
			set{ _numfee=value;}
			get{return _numfee;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public decimal NumPenalty
		{
			set{ _numpenalty=value;}
			get{return _numpenalty;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public decimal NumLastPlus
		{
			set{ _numlastplus=value;}
			get{return _numlastplus;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public decimal NumThisPlus
		{
			set{ _numthisplus=value;}
			get{return _numthisplus;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntChargeWay
		{
			set{ _intchargeway=value;}
			get{return _intchargeway;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntFlag
		{
			set{ _intflag=value;}
			get{return _intflag;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public DateTime DtLastUpd
		{
			set{ _dtlastupd=value;}
			get{return _dtlastupd;}
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

