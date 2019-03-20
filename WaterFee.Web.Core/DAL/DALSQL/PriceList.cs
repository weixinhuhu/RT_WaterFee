using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using WHC.WaterFeeWeb.Core.IDAL;
using WHC.Framework.Commons;

namespace WHC.WaterFeeWeb.Core.DALSQL
{
    public class PriceList : WHC.Framework.ControlUtil.BaseDALSQL<Entity.PriceList>, IPriceList
    {
        #region 对象实例及构造函数

        public static PriceList Instance
        {
            get
            {
                return new PriceList();
            }
        }
        public PriceList() : base("PriceList", "IntNo")
        {
        }

        #endregion 

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override Entity.PriceList DataReaderToEntity(IDataReader dataReader)
        {
            Entity.PriceList info = new Entity.PriceList();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.IntTypeNo = reader.GetInt32("IntTypeNo");
            info.IntNo = reader.GetInt32("IntNo");
            info.NvcDesc = reader.GetString("NvcDesc");
            info.NumPrice = reader.GetDecimal("NumPrice");
            info.IntUserNo = reader.GetInt32("IntUserNo");
            info.DteStart = reader.GetDateTime("DteStart");
            info.DteEnd = reader.GetDateTime("DteEnd");
            info.DtCreate = reader.GetDateTime("DtCreate"); ;

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(Entity.PriceList obj)
        {
            Entity.PriceList info = obj as Entity.PriceList;
            Hashtable hash = new Hashtable();

            hash.Add("IntTypeNo", info.IntTypeNo);
            hash.Add("IntNo", info.IntNo);
            hash.Add("NvcDesc", info.NvcDesc);
            hash.Add("NumPrice", info.NumPrice);
            hash.Add("IntUserNo", info.IntUserNo);
            hash.Add("DteStart", info.DteStart);
            hash.Add("DteEnd", info.DteEnd);
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

            dict.Add("IntTypeNo", "IntTypeNo"); 
            dict.Add("IntNo", "IntNo"); 
            dict.Add("NvcDesc", "NvcDesc"); 
            dict.Add("NumPrice", "NumPrice"); 
            dict.Add("IntUserNo", "IntUserNo"); 
            dict.Add("DteStart", "DteStart"); 
            dict.Add("DteEnd", "DteEnd"); 
            dict.Add("DtCreate", "DtCreate");  

            #endregion

            return dict;
        }

    }
}








