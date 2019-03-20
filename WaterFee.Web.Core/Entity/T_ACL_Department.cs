using System;
using System.Runtime.Serialization;

namespace WHC.WaterFeeWeb.Core.Entity
{
    /// <summary>
    /// 部门表
    /// </summary>
    [DataContract]
    [Serializable]
    public partial class T_ACL_Department : WHC.Framework.ControlUtil.BaseEntity
    {
		public T_ACL_Department()
		{}
		#region Model
		private int _departmentid;
		private string _name;
		private int? _sort=99;
		private string _remark;
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int DepartmentID
		{
			set{ _departmentid=value;}
			get{return _departmentid;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
        /// <summary>
        /// 排序
        /// </summary>
        [DataMember]
        public int? Sort
		{
			set{ _sort=value;}
			get{return _sort;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		#endregion Model

	}
}

