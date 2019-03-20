using System;
using System.Runtime.Serialization;

namespace WHC.WaterFeeWeb.Core.Entity
{
    /// <summary>
    /// 用户与部分关系表
    /// </summary>
    [DataContract]
    [Serializable]
    public partial class T_ACL_User_Department : WHC.Framework.ControlUtil.BaseEntity
    {
		public T_ACL_User_Department()
		{}
		#region Model
		private int _id;
		private int? _userid;
		private int? _departmentid;
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int? UserID
		{
			set{ _userid=value;}
			get{return _userid;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int? DepartmentID
		{
			set{ _departmentid=value;}
			get{return _departmentid;}
		}
		#endregion Model

	}
}

