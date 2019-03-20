using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using WHC.WaterFeeWeb.Core.IDAL;
using WHC.Framework.Commons;

namespace WHC.WaterFeeWeb.Core.DALMySql
{
    public class PriceComposition : WHC.Framework.ControlUtil.BaseDALMySql<Entity.PriceComposition>, IPriceComposition
    {
        #region 对象实例及构造函数

        public static PriceComposition Instance
        {
            get
            {
                return new PriceComposition();
            }
        }
        public PriceComposition() : base("PriceComposition", "IntListNo")
        {
        }

        #endregion 

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override Entity.PriceComposition DataReaderToEntity(IDataReader dataReader)
        {
            Entity.PriceComposition info = new Entity.PriceComposition();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.IntCalcNo = reader.GetInt32("IntCalcNo");
            info.IntListNo = reader.GetInt32("IntListNo");
            info.IntUserNo = reader.GetInt32("IntUserNo");
            info.DtCreate = reader.GetDateTime("DtCreate");

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(Entity.PriceComposition obj)
        {
            Entity.PriceComposition info = obj as Entity.PriceComposition;
            Hashtable hash = new Hashtable();

            hash.Add("IntCalcNo", info.IntCalcNo);
            hash.Add("IntListNo", info.IntListNo);
            hash.Add("IntUserNo", info.IntUserNo);
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
            dict.Add("IntCalcNo", "IntCalcNo");
            dict.Add("IntListNo", "IntListNo");
            dict.Add("IntUserNo", "IntUserNo");
            dict.Add("DtCreate", "DtCreate"); 
            #endregion

            return dict;
        }

    }
}




