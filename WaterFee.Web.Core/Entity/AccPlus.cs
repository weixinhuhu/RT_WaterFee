using System;
using System.Runtime.Serialization;

namespace WHC.WaterFeeWeb.Core.Entity
{
    /// <summary>
    /// AccPlus:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary> 
    [DataContract]
    [Serializable]
    public partial class AccPlus : WHC.Framework.ControlUtil.BaseEntity
    {
		public AccPlus()
		{}
		#region Model
		private int _intcustno;
		private decimal _numplus;
		private DateTime _dtlastupd= DateTime.Now;
		private DateTime _dtcreate= DateTime.Now;
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
        public decimal NumPlus
		{
			set{ _numplus=value;}
			get{return _numplus;}
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

