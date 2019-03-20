using System;
using System.Runtime.Serialization;

namespace WHC.WaterFeeWeb.Core.Entity
{
    /// <summary>
    /// AccDetail:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary> 
    [DataContract]
    [Serializable]
    public partial class AccDetail : WHC.Framework.ControlUtil.BaseEntity
    {
		public AccDetail()
		{}
		#region Model
		private int _intfeeid;
		private int _intcustno;
		private int _intyearmon;
		private DateTime _dtefee;
		private string _vcaddr;
		private int _intstepord=0;
		private int _inttype;
		private decimal _numfee;
		private int _intinvflag=0;
		private string _vcdesc;
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
        public string VcAddr
		{
			set{ _vcaddr=value;}
			get{return _vcaddr;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntStepOrd
		{
			set{ _intstepord=value;}
			get{return _intstepord;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntType
		{
			set{ _inttype=value;}
			get{return _inttype;}
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
        public int IntInvFlag
		{
			set{ _intinvflag=value;}
			get{return _intinvflag;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string VcDesc
		{
			set{ _vcdesc=value;}
			get{return _vcdesc;}
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

