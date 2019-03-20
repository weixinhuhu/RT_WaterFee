using System;
using System.Runtime.Serialization;

namespace WHC.WaterFeeWeb.Core.Entity
{
    /// <summary>
    /// AccDebt:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary> 
    [DataContract]
    [Serializable]
    public partial class AccDebt : WHC.Framework.ControlUtil.BaseEntity
    {
        public AccDebt()
        { }
        #region Model
        private int _intfeeid;
        private int _intcustno;
        private int _intyearmon;
        private DateTime _dtefee;
        private decimal _monfee;
        private decimal _monfeeexec;
        private DateTime _dteend;
        private DateTime _dteendexec;
        private int _intstatus = 0;
        private int _intinvflag = 0;
        private string _vcremitcause = "";
        private DateTime _dtuplast = DateTime.Now;
        private DateTime _dtcreate = DateTime.Now;
        /// <summary>
        /// 
        /// </summary> 
        [DataMember]
        public int IntFeeID
        {
            set { _intfeeid = value; }
            get { return _intfeeid; }
        }
        /// <summary>
        /// 
        /// </summary> 
        [DataMember]
        public int IntCustNo
        {
            set { _intcustno = value; }
            get { return _intcustno; }
        }
        /// <summary>
        /// 
        /// </summary> 
        [DataMember]
        public int IntYearMon
        {
            set { _intyearmon = value; }
            get { return _intyearmon; }
        }
        /// <summary>
        /// 
        /// </summary> 
        [DataMember]
        public DateTime DteFee
        {
            set { _dtefee = value; }
            get { return _dtefee; }
        }
        /// <summary>
        /// 
        /// </summary> 
        [DataMember]
        public decimal MonFee
        {
            set { _monfee = value; }
            get { return _monfee; }
        }
        /// <summary>
        /// 
        /// </summary> 
        [DataMember]
        public decimal MonFeeExec
        {
            set { _monfeeexec = value; }
            get { return _monfeeexec; }
        }
        /// <summary>
        /// 
        /// </summary> 
        [DataMember]
        public DateTime DteEnd
        {
            set { _dteend = value; }
            get { return _dteend; }
        }
        /// <summary>
        /// 
        /// </summary> 
        [DataMember]
        public DateTime DteEndExec
        {
            set { _dteendexec = value; }
            get { return _dteendexec; }
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
        public int IntInvFlag
        {
            set { _intinvflag = value; }
            get { return _intinvflag; }
        }
        /// <summary>
        /// 
        /// </summary> 
        [DataMember]
        public string VcRemitCause
        {
            set { _vcremitcause = value; }
            get { return _vcremitcause; }
        }
        /// <summary>
        /// 
        /// </summary> 
        [DataMember]
        public DateTime DtUpLast
        {
            set { _dtuplast = value; }
            get { return _dtuplast; }
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

        [DataMember]
        public virtual Entity.ArcCustomerInfo ArcCustomerInfo { get; set; }
        #endregion Model

        /// <summary>
        /// 违约金
        /// </summary> 
        [DataMember]
        public decimal MonPenalty { get; set; }
    }
}

