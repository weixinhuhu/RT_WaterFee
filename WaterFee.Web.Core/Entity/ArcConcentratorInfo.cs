using System;
using System.Runtime.Serialization;

namespace WHC.WaterFeeWeb.Core.Entity
{
    /// <summary>
    /// ArcConcentratorInfo:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [DataContract]
    [Serializable]
    public partial class ArcConcentratorInfo : WHC.Framework.ControlUtil.BaseEntity
    {
        public ArcConcentratorInfo()
        { }
        #region Model
        private int _intid;
        private string _vcaddr;
        private string _nvcname = "";
        private string _nvcaddr = "";
        private string _vcsimno = "";
        private int _intonline = 0;
        private int _intstatus = 0;
        private int _intprotocol = 0;
        private int _intcount = 1;
        private int _intcommmode = 0;
        private int _intcom = 1;
        private string _vcparam = "";
        private DateTime _dtlastupd = DateTime.Now;
        private DateTime _dtcreate = DateTime.Now;
        private int intupid = 0;
        private int intcmpcode = 0;
        private int intareacode = 0;

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntID
        {
            set { _intid = value; }
            get { return _intid; }
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
        public string VcSimNo
        {
            set { _vcsimno = value; }
            get { return _vcsimno; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntOnline
        {
            set { _intonline = value; }
            get { return _intonline; }
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
        public int IntProtocol
        {
            set { _intprotocol = value; }
            get { return _intprotocol; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntCount
        {
            set { _intcount = value; }
            get { return _intcount; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntCommMode
        {
            set { _intcommmode = value; }
            get { return _intcommmode; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int IntCOM
        {
            set { _intcom = value; }
            get { return _intcom; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string VcParam
        {
            set { _vcparam = value; }
            get { return _vcparam; }
        }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public DateTime DtLastUpd
        {
            set { _dtlastupd = value; }
            get { return _dtlastupd; }
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


        /// <summary>
        /// 父级ID,为0则表示当前为顶级
        /// </summary>
        [DataMember]
        public int IntUpID
        {
            set { intupid = value; }
            get { return intupid; }
        }
        /// <summary>
        /// 厂家码
        /// </summary>
        [DataMember]
        public int IntCmpCode
        {
            set { intcmpcode = value; }
            get { return intcmpcode; }
        }
        /// <summary>
        /// 区域码
        /// </summary>
        [DataMember]
        public int IntAreaCode
        {
            set { intareacode = value; }
            get { return intareacode; }
        }


        #endregion Model

    }
}

