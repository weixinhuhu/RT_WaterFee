using System;
using System.Runtime.Serialization;

namespace WHC.WaterFeeWeb.Core.Entity
{
    /// <summary>
    /// ArcMeterReading:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [DataContract]
    [Serializable]
    public partial class ArcMeterReading : WHC.Framework.ControlUtil.BaseEntity
    {
		public ArcMeterReading()
		{}
		#region Model
		private int _intid;
		private string _vcaddr;
		private int _intcustno;
		private DateTime _dtereading= DateTime.Now;
		private DateTime _dtefreeze;
		private decimal _numreading;
		private string _vcstatus="";
		private int _intstatus=0;
		private DateTime _dtlastupd= DateTime.Now;
		private DateTime _dtcreate= DateTime.Now;
        private int _IntFlag;
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
        public string VcAddr
		{
			set{ _vcaddr=value;}
			get{return _vcaddr;}
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
        public DateTime DteReading
		{
			set{ _dtereading=value;}
			get{return _dtereading;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public DateTime DteFreeze
		{
			set{ _dtefreeze=value;}
			get{return _dtefreeze;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public decimal NumReading
		{
			set{ _numreading=value;}
			get{return _numreading;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string VcStatus
		{
			set{ _vcstatus=value;}
			get{return _vcstatus;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntStatus
		{
			set{ _intstatus=value;}
			get{return _intstatus;}
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
        [DataMember]
        public virtual ArcCustomerInfo ArcCustomerInfo { get; set; }
        #endregion Model
        [DataMember]
        public int IntFlag {
            set { _IntFlag = value; }
            get { return _IntFlag; }
        }

	}
}

