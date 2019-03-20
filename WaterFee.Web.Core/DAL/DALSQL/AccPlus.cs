using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using WHC.WaterFeeWeb.Core.IDAL;
using WHC.Framework.Commons;

namespace WHC.WaterFeeWeb.Core.DALSQL
{
    public class AccPlus : WHC.Framework.ControlUtil.BaseDALSQL<Entity.AccPlus>, IAccPlus
    {
        #region 对象实例及构造函数

        public static AccPlus Instance
        {
            get
            {
                return new AccPlus();
            }
        }
        public AccPlus() : base("AccPlus", "IntCustNo")
        {
        }

        #endregion
 
        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override Entity.AccPlus DataReaderToEntity(IDataReader dataReader)
        {
            Entity.AccPlus info = new Entity.AccPlus();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.IntCustNo = reader.GetInt32("IntCustNo");
            info.NumPlus = reader.GetDecimal("NumPlus");
            info.DtLastUpd = reader.GetDateTime("DtLastUpd");
            info.DtCreate = reader.GetDateTime("DtCreate");

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(Entity.AccPlus obj)
        {
            Entity.AccPlus info = obj as Entity.AccPlus;
            Hashtable hash = new Hashtable();

            hash.Add("IntCustNo", info.IntCustNo);
            hash.Add("NumPlus", info.NumPlus);
            hash.Add("DtLastUpd", info.DtLastUpd);
            hash.Add("DtCreate", info.DtCreate);

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
            dict.Add("IntCustNo", "IntCustNo");
            dict.Add("NumPlus", "NumPlus");
            dict.Add("DtLastUpd", "DtLastUpd");
            dict.Add("DtCreate", "DtCreate");
            #endregion

            return dict;
        }

    }
}
