using System;
using System.Runtime.Serialization;

namespace WHC.WaterFeeWeb.Core.Entity
{
    /// <summary>
    /// AccDeposit:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [DataContract]
    [Serializable]
    public partial class AccDeposit : WHC.Framework.ControlUtil.BaseEntity
    {
		public AccDeposit()
		{}
		#region Model
		private int _intcustno;
		private decimal _monsum;
		private DateTime _dtlastupd= DateTime.Now;
		private DateTime _dtcreate= DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntCustNO
		{
			set{ _intcustno=value;}
			get{return _intcustno;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public decimal MonSum
		{
			set{ _monsum=value;}
			get{return _monsum;}
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
        public Entity.ArcCustomerInfo ArcCustomerInfo { get; set; }

		#endregion Model

	}
}

