using System;
using System.Runtime.Serialization;

namespace WHC.WaterFeeWeb.Core.Entity
{
    /// <summary>
    /// MidCustomerMeter:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary> 
    [DataContract]
    [Serializable]
    public partial class MidCustomerMeter : WHC.Framework.ControlUtil.BaseEntity
    {
        public MidCustomerMeter()
        { }
        #region Model
        private string _vcclientip;
        private int _intcustcode;
        private string _nvcname = "";
        private string _nvcaddr = "";
        private string _nvcvillage = "";
        private string _vcbuilding = "";
        private int _intunitnum = 0;
        private int _introomnum = 0;
        private string _vcmobile = "";
        private string _vctelno = "";
        private string _vcidno = "";
        private string _vccontractno = "";
        private string _nvcinvname = "";
        private string _nvcinvaddr = "";
        private int _intnumber = 3;
        private int _intpriceno = 0;
        private string _nvccusttype = "";
        private int _intuserid;
        private int _intstatus = 0;
        private DateTime _dteopen = DateTime.Now;
        private string _vcaddr;
        private string _nvcaddrins = "";
        private string _vcbarcode = "";
        private string _vcassetno = "";
        private int _intchannal = 1;
        private int _intprotocol = 0;
        private int _intcycle = 0;
        private int _intorig;
        private int _intstate = 0;
        private DateTime _dtcreate;
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string VcClientIP
        {
            set { _vcclientip = value; }
            get { return _vcclientip; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntCustCode
        {
            set { _intcustcode = value; }
            get { return _intcustcode; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string NvcName
        {
            set { _nvcname = value; }
            get { return _nvcname; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string NvcAddr
        {
            set { _nvcaddr = value; }
            get { return _nvcaddr; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string NvcVillage
        {
            set { _nvcvillage = value; }
            get { return _nvcvillage; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string VcBuilding
        {
            set { _vcbuilding = value; }
            get { return _vcbuilding; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntUnitNum
        {
            set { _intunitnum = value; }
            get { return _intunitnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntRoomNum
        {
            set { _introomnum = value; }
            get { return _introomnum; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string VcMobile
        {
            set { _vcmobile = value; }
            get { return _vcmobile; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string VcTelNo
        {
            set { _vctelno = value; }
            get { return _vctelno; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string VcIDNo
        {
            set { _vcidno = value; }
            get { return _vcidno; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string VcContractNo
        {
            set { _vccontractno = value; }
            get { return _vccontractno; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string NvcInvName
        {
            set { _nvcinvname = value; }
            get { return _nvcinvname; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string NvcInvAddr
        {
            set { _nvcinvaddr = value; }
            get { return _nvcinvaddr; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntNumber
        {
            set { _intnumber = value; }
            get { return _intnumber; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntPriceNo
        {
            set { _intpriceno = value; }
            get { return _intpriceno; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string NvcCustType
        {
            set { _nvccusttype = value; }
            get { return _nvccusttype; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntUserID
        {
            set { _intuserid = value; }
            get { return _intuserid; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntStatus
        {
            set { _intstatus = value; }
            get { return _intstatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public DateTime DteOpen
        {
            set { _dteopen = value; }
            get { return _dteopen; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string VcAddr
        {
            set { _vcaddr = value; }
            get { return _vcaddr; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string NvcAddrIns
        {
            set { _nvcaddrins = value; }
            get { return _nvcaddrins; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string VcBarCode
        {
            set { _vcbarcode = value; }
            get { return _vcbarcode; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string VcAssetNo
        {
            set { _vcassetno = value; }
            get { return _vcassetno; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntChannal
        {
            set { _intchannal = value; }
            get { return _intchannal; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntProtocol
        {
            set { _intprotocol = value; }
            get { return _intprotocol; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntCycle
        {
            set { _intcycle = value; }
            get { return _intcycle; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntOrig
        {
            set { _intorig = value; }
            get { return _intorig; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntState
        {
            set { _intstate = value; }
            get { return _intstate; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public DateTime DtCreate
        {
            set { _dtcreate = value; }
            get { return _dtcreate; }
        }
        #endregion Model

    }
}

