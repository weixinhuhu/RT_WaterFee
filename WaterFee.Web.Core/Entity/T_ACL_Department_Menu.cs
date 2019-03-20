using System;
using System.Runtime.Serialization;

namespace WHC.WaterFeeWeb.Core.Entity
{
    /// <summary>
    /// 权限分配:部分与菜单关系表
    /// </summary>
    [DataContract]
    [Serializable]
    public partial class T_ACL_Department_Menu : WHC.Framework.ControlUtil.BaseEntity
    {
		public T_ACL_Department_Menu()
		{}
		#region Model
		private int _id;
		private int? _departmentid;
		private string _menuid;
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
        public int? DepartmentID
		{
			set{ _departmentid=value;}
			get{return _departmentid;}
		}
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string MenuID
		{
			set{ _menuid=value;}
			get{return _menuid;}
		}
		#endregion Model

	}
}

