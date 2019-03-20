using System;
using System.Runtime.Serialization;

namespace WHC.WaterFeeWeb.Core.Entity
{
    /// <summary>
    /// 短信模板表
    /// </summary>
    [DataContract]
    [Serializable]
    public partial class MessageTemplate : WHC.Framework.ControlUtil.BaseEntity
    {
        public MessageTemplate()
        { }
        #region Model
        private int _id;
        private string _temptype;
        private string _tempname;
        private string _tempid;
        private string _tempsql;
        private string _tempcontent;
        private DateTime? _dtcreate = DateTime.Now;
        private int? _status = 1;
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 模板类型
        /// </summary>
        [DataMember]
        public string TempType
        {
            set { _temptype = value; }
            get { return _temptype; }
        }
        /// <summary>
        /// 模板名称
        /// </summary>
        [DataMember]
        public string TempName
        {
            set { _tempname = value; }
            get { return _tempname; }
        }
        /// <summary>
        /// 模板ID
        /// </summary>
        [DataMember]
        public string TempID
        {
            set { _tempid = value; }
            get { return _tempid; }
        }
        /// <summary>
        /// SQL语句
        /// </summary>
        [DataMember]
        public string TempSQL
        {
            set { _tempsql = value; }
            get { return _tempsql; }
        }
        /// <summary>
        /// 模板内容
        /// </summary>
        [DataMember]
        public string TempContent
        {
            set { _tempcontent = value; }
            get { return _tempcontent; }
        }
        /// <summary>
        /// 创建日期
        /// </summary>
        [DataMember]
        public DateTime? DtCreate
        {
            set { _dtcreate = value; }
            get { return _dtcreate; }
        }
        /// <summary>
        /// 模板状态:1为启用,0为禁用
        /// </summary>
        [DataMember]
        public int? Status
        {
            set { _status = value; }
            get { return _status; }
        }
        #endregion Model

    }
}

