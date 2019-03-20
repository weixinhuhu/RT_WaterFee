using System;
using System.Runtime.Serialization;

namespace WHC.WaterFeeWeb.Core.Entity
{
    /// <summary>
    /// AccDepositDetail:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [DataContract]
    [Serializable]
    public partial class AccDepositDetail : WHC.Framework.ControlUtil.BaseEntity
    {
		public AccDepositDetail()
		{}
		#region Model
		private int _intid;
		private int _intcustno;
		private decimal _monamount;
		private int _inttype;
		private string _vcflowno;
		private string _vcuserid;
		private string _vcreceiptno="";
		private DateTime _dteaccount= DateTime.Now;
		private int _intflag=0;
		private string _vcdesc="";
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
        public int IntCustNo
		{
			set{ _intcustno=value;}
			get{return _intcustno;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public decimal MonAmount
		{
			set{ _monamount=value;}
			get{return _monamount;}
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
        public string VcFlowNo
		{
			set{ _vcflowno=value;}
			get{return _vcflowno;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string VcUserID
		{
			set{ _vcuserid=value;}
			get{return _vcuserid;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string VcReceiptNo
		{
			set{ _vcreceiptno=value;}
			get{return _vcreceiptno;}
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
        public int IntFlag
		{
			set{ _intflag=value;}
			get{return _intflag;}
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

