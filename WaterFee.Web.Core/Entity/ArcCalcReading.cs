using System;
using System.Runtime.Serialization;

namespace WHC.WaterFeeWeb.Core.Entity
{
    /// <summary>
    /// ArcCalcReading:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [DataContract]
    [Serializable]
    public partial class ArcCalcReading : WHC.Framework.ControlUtil.BaseEntity
    {
        public ArcCalcReading()
        { }
        #region Model
        private int _intfeeid;
        private string _vcaddr;
        private int _intyearmon;
        private DateTime _dtefee;
        private decimal _numprior;
        private decimal _numlast;
        private decimal _numused;
        private int _intstatus = 0;
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
        public string VcAddr
        {
            set { _vcaddr = value; }
            get { return _vcaddr; }
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
        public decimal NumPrior
        {
            set { _numprior = value; }
            get { return _numprior; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public decimal NumLast
        {
            set { _numlast = value; }
            get { return _numlast; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public decimal NumUsed
        {
            set { _numused = value; }
            get { return _numused; }
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
        public DateTime DtCreate
        {
            set { _dtcreate = value; }
            get { return _dtcreate; }
        }
        #endregion Model

        [DataMember]
        public virtual ArcCustomerInfo ArcCustomerInfo { get; set; }

        [DataMember]
        public virtual ArcMeterInfo ArcMeterInfo { get; set; }
    }
}
