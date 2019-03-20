using System;
using System.Runtime.Serialization;

namespace WHC.WaterFeeWeb.Core.Entity
{
    /// <summary>
    /// ArcMeterInfo:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [DataContract]
    [Serializable]
    public partial class ArcMeterInfo : WHC.Framework.ControlUtil.BaseEntity
    {
		public ArcMeterInfo()
		{}
		#region Model
		private int _intid;
		private string _vcaddr;
		private string _nvcname="";
		private string _nvcaddr="";
		private string _vcbarcode="";
		private string _vcassetno="";
		private int _intprotocol=0;
		private int _intcycle=0;
		private int _intorig;
		private int _intreadflag=0;
		private int _intvalvestate=0;
        private int _IntAccountWay = 0;
        private int _intconid=0;
		private string _numratio = "";
		private int _intmp=0;
		private int _intpriceno2 = 0;
		private int _intcustno=0;
        private int _intstatus = 0;
        private int _IntPriceNo = 0;
        private int _IntAutoSwitch = 0;



        private DateTime _dtlastupd= DateTime.Now;
		private DateTime _dtcreate= DateTime.Now;
        /// <summary>
        /// 
        /// </summary>
        /// 
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
        public string VcBarCode
		{
			set{ _vcbarcode=value;}
			get{return _vcbarcode;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string VcAssetNo
		{
			set{ _vcassetno=value;}
			get{return _vcassetno;}
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
        public int IntCycle
		{
			set{ _intcycle=value;}
			get{return _intcycle;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntOrig
		{
			set{ _intorig=value;}
			get{return _intorig;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntReadFlag
		{
			set{ _intreadflag=value;}
			get{return _intreadflag;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntValveState
		{
			set{ _intvalvestate=value;}
			get{return _intvalvestate;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntConID
		{
			set{ _intconid= value; }
			get{return _intconid;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string NumRatio
        {
			set{ _numratio = value;}
			get{return _numratio; }
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntMP
		{
			set{ _intmp=value;}
			get{return _intmp;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntPriceNo2
        {
			set{ _intpriceno2 = value;}
			get{return _intpriceno2;}
		}
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
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntPriceNo
        {
			set{ _IntPriceNo = value; }
			get{return _IntPriceNo; }
		}
        [DataMember]
        public int IntAccountWay
        {
            set { _IntAccountWay = value; }
            get { return _IntAccountWay; }
        }
        [DataMember]
        public int IntAutoSwitch {
            set { _IntAutoSwitch = value; }
            get { return _IntAutoSwitch; }
        }
        #endregion Model

    }
}

