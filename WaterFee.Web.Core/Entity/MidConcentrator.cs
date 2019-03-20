using System;
using System.Runtime.Serialization;

namespace WHC.WaterFeeWeb.Core.Entity
{
    /// <summary>
    /// MidConcentrator:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [DataContract]
    [Serializable]
    public partial class MidConcentrator : WHC.Framework.ControlUtil.BaseEntity
    {
		public MidConcentrator()
		{}
		#region Model
		private string _vcclientip;
		private string _nvcname="";
		private string _nvcaddr="";
		private string _vcaddr;
		private int _intprotocol;
		private int _intcount;
		private int _intcommmode;
		private int _intcom;
		private string _vcparam;
		private string _vcsimno;
		private int _intstatus;
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string VcClientIP
		{
			set{ _vcclientip=value;}
			get{return _vcclientip;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string NvcName
		{
			set{ _nvcname=value;}
			get{return _nvcname;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string NvcAddr
		{
			set{ _nvcaddr=value;}
			get{return _nvcaddr;}
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
        public int IntProtocol
		{
			set{ _intprotocol=value;}
			get{return _intprotocol;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntCount
		{
			set{ _intcount=value;}
			get{return _intcount;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntCommMode
		{
			set{ _intcommmode=value;}
			get{return _intcommmode;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntCOM
		{
			set{ _intcom=value;}
			get{return _intcom;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string VcParam
		{
			set{ _vcparam=value;}
			get{return _vcparam;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string VcSimNo
		{
			set{ _vcsimno=value;}
			get{return _vcsimno;}
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
		#endregion Model

	}
}

