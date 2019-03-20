using System;
using System.Runtime.Serialization;

namespace WHC.WaterFeeWeb.Core.Entity
{
    /// <summary>
    /// PriceCalc:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [DataContract]
    [Serializable]
	public partial class PriceCalc : WHC.Framework.ControlUtil.BaseEntity
    {
		public PriceCalc()
		{}
		#region Model
		private int _intpropertyno;
		private int _intno;
		private string _nvcdesc;
		private decimal _numtotalprice;
		private int _intsteporder=0;
		private int _intstepstart=0;
		private int _intstepend=0;
		private int _intstepinc=0;
		private int _intuserno;
		private DateTime _dtcreate= DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntPropertyNo
		{
			set{ _intpropertyno=value;}
			get{return _intpropertyno;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntNo
		{
			set{ _intno=value;}
			get{return _intno;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string NvcDesc
		{
			set{ _nvcdesc=value;}
			get{return _nvcdesc;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public decimal NumTotalPrice
		{
			set{ _numtotalprice=value;}
			get{return _numtotalprice;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntStepOrder
		{
			set{ _intsteporder=value;}
			get{return _intsteporder;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntStepStart
		{
			set{ _intstepstart=value;}
			get{return _intstepstart;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntStepEnd
		{
			set{ _intstepend=value;}
			get{return _intstepend;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntStepInc
		{
			set{ _intstepinc=value;}
			get{return _intstepinc;}
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

