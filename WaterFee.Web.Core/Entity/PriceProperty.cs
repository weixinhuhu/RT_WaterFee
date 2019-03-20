using System;
using System.Runtime.Serialization;

namespace WHC.WaterFeeWeb.Core.Entity
{
    /// <summary>
    /// PriceProperty:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [DataContract]
    [Serializable]
	public partial class PriceProperty : WHC.Framework.ControlUtil.BaseEntity
    {
		public PriceProperty()
		{}
		#region Model
		private int _intno;
		private string _nvcdesc;
		private int _intstep=0;
		private int _intinterval=0;
		private int _intstepnum=3;
		private string _vcstattype="";
		private int _intuserno;
		private DateTime _dtestart= DateTime.Now;
		private DateTime _dteend= Convert.ToDateTime("9999-9-9");
		private DateTime _dtcreate= DateTime.Now;
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
        public int IntStep
		{
			set{ _intstep=value;}
			get{return _intstep;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntInterval
		{
			set{ _intinterval=value;}
			get{return _intinterval;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntStepNum
		{
			set{ _intstepnum=value;}
			get{return _intstepnum;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string VcStatType
		{
			set{ _vcstattype=value;}
			get{return _vcstattype;}
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
        public DateTime DteStart
		{
			set{ _dtestart=value;}
			get{return _dtestart;}
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
        public DateTime DtCreate
		{
			set{ _dtcreate=value;}
			get{return _dtcreate;}
		}
		#endregion Model

	}
}

