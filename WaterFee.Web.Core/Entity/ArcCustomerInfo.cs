using System;
using System.Runtime.Serialization;

namespace WHC.WaterFeeWeb.Core.Entity
{
    /// <summary>
    /// ArcConcentratorInfo:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [DataContract]
    [Serializable]
    public partial class ArcCustomerInfo : WHC.Framework.ControlUtil.BaseEntity
    {
		public ArcCustomerInfo()
		{}
		#region Model
		private int _intid;
		private int _intno;
		private string _nvcname="";
		private string _nvcaddr="";
		private string _nvcvillage="";
		private string _vcbuilding="";
		private int _intunitnum=0;
		private int _introomnum=0;
		private string _vcnamecode="";
		private string _vcaddrcode="";
		private string _vcmobile="";
		private string _vctelno="";
		private string _vcidno="";
		private string _vccontractno="";
		private string _nvcinvname="";
		private string _nvcinvaddr="";
		private int _intnumber=3;
		private int _intpriceno=0;
		private string _nvccusttype="";
		private int _intuserid;
		private int _intstatus=0;
		private string _vcwechatno="";
		private int _intaccmode=0;
		private int _inthelper=-1;
		private DateTime? _dteopen;
		private DateTime _dtecancel= Convert.ToDateTime("9999-9-9");
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
        public int IntNo
		{
			set{ _intno=value;}
			get{return _intno;}
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
        public string NvcVillage
		{
			set{ _nvcvillage=value;}
			get{return _nvcvillage;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string VcBuilding
		{
			set{ _vcbuilding=value;}
			get{return _vcbuilding;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntUnitNum
		{
			set{ _intunitnum=value;}
			get{return _intunitnum;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntRoomNum
		{
			set{ _introomnum=value;}
			get{return _introomnum;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string VcNameCode
		{
			set{ _vcnamecode=value;}
			get{return _vcnamecode;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string VcAddrCode
		{
			set{ _vcaddrcode=value;}
			get{return _vcaddrcode;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string VcMobile
		{
			set{ _vcmobile=value;}
			get{return _vcmobile;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string VcTelNo
		{
			set{ _vctelno=value;}
			get{return _vctelno;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string VcIDNo
		{
			set{ _vcidno=value;}
			get{return _vcidno;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string VcContractNo
		{
			set{ _vccontractno=value;}
			get{return _vccontractno;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string NvcInvName
		{
			set{ _nvcinvname=value;}
			get{return _nvcinvname;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string NvcInvAddr
		{
			set{ _nvcinvaddr=value;}
			get{return _nvcinvaddr;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntNumber
		{
			set{ _intnumber=value;}
			get{return _intnumber;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntPriceNo
		{
			set{ _intpriceno=value;}
			get{return _intpriceno;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string NvcCustType
		{
			set{ _nvccusttype=value;}
			get{return _nvccusttype;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntUserID
		{
			set{ _intuserid=value;}
			get{return _intuserid;}
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
        public string VcWechatNo
		{
			set{ _vcwechatno=value;}
			get{return _vcwechatno;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntAccMode
		{
			set{ _intaccmode=value;}
			get{return _intaccmode;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntHelper
		{
			set{ _inthelper=value;}
			get{return _inthelper;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public DateTime? DteOpen
		{
			set{ _dteopen=value;}
			get{return _dteopen;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public DateTime DteCancel
		{
			set{ _dtecancel=value;}
			get{return _dtecancel;}
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

