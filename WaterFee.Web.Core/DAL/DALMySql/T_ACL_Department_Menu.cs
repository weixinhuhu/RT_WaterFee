using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using WHC.WaterFeeWeb.Core.IDAL;
using WHC.Framework.Commons;

namespace WHC.WaterFeeWeb.Core.DALMySql
{
    public class T_ACL_Department_Menu : WHC.Framework.ControlUtil.BaseDALMySql<Entity.T_ACL_Department_Menu>, IT_ACL_Department_Menu
    {
        #region 对象实例及构造函数

        public static T_ACL_Department_Menu Instance
        {
            get
            {
                return new T_ACL_Department_Menu();
            }
        }
        public T_ACL_Department_Menu() : base("T_ACL_Department_Menu", "ID")
        {
        }

        #endregion 

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override Entity.T_ACL_Department_Menu DataReaderToEntity(IDataReader dataReader)
        {
            Entity.T_ACL_Department_Menu info = new Entity.T_ACL_Department_Menu();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.ID = reader.GetInt32("ID");
            info.DepartmentID = reader.GetInt32("DepartmentID");
            info.MenuID = reader.GetString("MenuID");

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(Entity.T_ACL_Department_Menu obj)
        {
            Entity.T_ACL_Department_Menu info = obj as Entity.T_ACL_Department_Menu;
            Hashtable hash = new Hashtable();

            hash.Add("ID", info.ID);
            hash.Add("DepartmentID", info.DepartmentID);
            hash.Add("MenuID", info.MenuID);

            return hash;
        }

        /// <summary>
        /// 获取字段中文别名（用于界面显示）的字典集合
        /// </summary>
        /// <returns></returns>
        public override Dictionary<string, string> GetColumnNameAlias()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            #region 添加别名解析
            dict.Add("ID", "ID");
            dict.Add("DepartmentID", "DepartmentID");
            dict.Add("MenuID", "MenuID"); 
            #endregion

            return dict;
        }

    }
}






