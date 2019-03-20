using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using WHC.WaterFeeWeb.Core.IDAL;
using WHC.Framework.Commons;

namespace WHC.WaterFeeWeb.Core.DALSQL
{
    public class PriceCalc : WHC.Framework.ControlUtil.BaseDALSQL<Entity.PriceCalc>, IPriceCalc
    {
        #region 对象实例及构造函数

        public static PriceCalc Instance
        {
            get
            {
                return new PriceCalc();
            }
        }
        public PriceCalc() : base("PriceCalc", "IntNo")
        {
        }

        #endregion 

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override Entity.PriceCalc DataReaderToEntity(IDataReader dataReader)
        {
            Entity.PriceCalc info = new Entity.PriceCalc();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.IntPropertyNo = reader.GetInt32("IntPropertyNo");
            info.IntNo = reader.GetInt32("IntNo");
            info.NvcDesc = reader.GetString("NvcDesc");
            info.NumTotalPrice = reader.GetDecimal("NumTotalPrice");
            info.IntStepOrder = reader.GetInt32("IntStepOrder");
            info.IntStepStart = reader.GetInt32("IntStepStart");
            info.IntStepEnd = reader.GetInt32("IntStepEnd");
            info.IntStepInc = reader.GetInt32("IntStepInc");
            info.IntUserNo = reader.GetInt32("IntUserNo");
            info.DtCreate = reader.GetDateTime("DtCreate");

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(Entity.PriceCalc obj)
        {
            Entity.PriceCalc info = obj as Entity.PriceCalc;
            Hashtable hash = new Hashtable();

            hash.Add("IntPropertyNo", info.IntPropertyNo);
            hash.Add("IntNo", info.IntNo);
            hash.Add("NvcDesc", info.NvcDesc);
            hash.Add("NumTotalPrice", info.NumTotalPrice);
            hash.Add("IntStepOrder", info.IntStepOrder);
            hash.Add("IntStepStart", info.IntStepStart);
            hash.Add("IntStepEnd", info.IntStepEnd);
            hash.Add("IntStepInc", info.IntStepInc);
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

            dict.Add("IntPropertyNo", "IntPropertyNo");
            dict.Add("IntNo", "IntNo");
            dict.Add("NvcDesc", "NvcDesc");
            dict.Add("NumTotalPrice", "NumTotalPrice");
            dict.Add("IntStepOrder", "IntStepOrder");
            dict.Add("IntStepStart", "IntStepStart");
            dict.Add("IntStepEnd", "IntStepEnd");
            dict.Add("IntStepInc", "IntStepInc");
            dict.Add("IntUserNo", "IntUserNo");
            dict.Add("DtCreate", "DtCreate"); 

            #endregion

            return dict;
        }

    }
}










