using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using WHC.WaterFeeWeb.Core.IDAL;
using WHC.Framework.Commons;

namespace WHC.WaterFeeWeb.Core.DALSQL
{
    public partial class ArcCalcReading : WHC.Framework.ControlUtil.BaseDALSQL<Entity.ArcCalcReading>, IArcCalcReading
    {
        #region 对象实例及构造函数

        public static ArcCalcReading Instance
        {
            get
            {
                return new ArcCalcReading();
            }
        }
        public ArcCalcReading() : base("ArcCalcReading", "IntFeeID")
        {
        }

        #endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override Entity.ArcCalcReading DataReaderToEntity(IDataReader dataReader)
        {
            Entity.ArcCalcReading info = new Entity.ArcCalcReading();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.IntFeeID = reader.GetInt32("IntFeeID");
            info.VcAddr = reader.GetString("VcAddr");
            info.IntYearMon = reader.GetInt32("IntYearMon");
            info.DteFee = reader.GetDateTime("DteFee");
            info.NumPrior = reader.GetDecimal("NumPrior");
            info.NumLast = reader.GetDecimal("NumLast");
            info.NumUsed = reader.GetDecimal("NumUsed");
            info.IntStatus = reader.GetInt32("IntStatus");
            info.DtCreate = reader.GetDateTime("DtCreate");

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(Entity.ArcCalcReading obj)
        {
            Entity.ArcCalcReading info = obj as Entity.ArcCalcReading;
            Hashtable hash = new Hashtable();

            hash.Add("IntFeeID", info.IntFeeID);
            hash.Add("VcAddr", info.VcAddr);
            hash.Add("IntYearMon", info.IntYearMon);
            hash.Add("DteFee", info.DteFee);
            hash.Add("NumPrior", info.NumPrior);
            hash.Add("NumLast", info.NumLast);
            hash.Add("NumUsed", info.NumUsed);
            hash.Add("IntStatus", info.IntStatus);
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

            dict.Add("IntFeeID", "IntFeeID");
            dict.Add("VcAddr", "VcAddr");
            dict.Add("IntYearMon", "IntYearMon");
            dict.Add("DteFee", "DteFee");
            dict.Add("NumPrior", "NumPrior");
            dict.Add("NumLast", "NumLast");
            dict.Add("NumUsed", "NumUsed");
            dict.Add("IntStatus", "IntStatus");
            dict.Add("DtCreate", "DtCreate");

            #endregion

            return dict;
        }

    }
}
