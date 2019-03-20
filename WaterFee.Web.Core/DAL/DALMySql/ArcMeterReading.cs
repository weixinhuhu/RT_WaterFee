using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using WHC.WaterFeeWeb.Core.IDAL;
using WHC.Framework.Commons;

namespace WHC.WaterFeeWeb.Core.DALMySql
{
    public partial class ArcMeterReading : WHC.Framework.ControlUtil.BaseDALMySql<Entity.ArcMeterReading>, IArcMeterReading
    {
        #region 对象实例及构造函数

        public static ArcMeterReading Instance
        {
            get
            {
                return new ArcMeterReading();
            }
        }
        public ArcMeterReading() : base("ArcMeterReading", "IntID")
        {
        }

        #endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override Entity.ArcMeterReading DataReaderToEntity(IDataReader dataReader)
        {
            Entity.ArcMeterReading info = new Entity.ArcMeterReading();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.IntID = reader.GetInt32("IntID");
            info.VcAddr = reader.GetString("VcAddr");
            info.IntCustNo = reader.GetInt32("IntCustNo");
            info.DteReading = reader.GetDateTime("DteReading");
            info.DteFreeze = reader.GetDateTime("DteFreeze");
            info.NumReading = reader.GetDecimal("NumReading");
            info.VcStatus = reader.GetString("VcStatus");
            info.IntStatus = reader.GetInt32("IntStatus");
            info.DtLastUpd = reader.GetDateTime("DtLastUpd");
            info.DtCreate = reader.GetDateTime("DtCreate");

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(Entity.ArcMeterReading obj)
        {
            Entity.ArcMeterReading info = obj as Entity.ArcMeterReading;
            Hashtable hash = new Hashtable();

            hash.Add("IntID", info.IntID);
            hash.Add("VcAddr", info.VcAddr);
            hash.Add("IntCustNo", info.IntCustNo);
            hash.Add("DteReading", info.DteReading);
            hash.Add("DteFreeze", info.DteFreeze);
            hash.Add("NumReading", info.NumReading);
            hash.Add("VcStatus", info.VcStatus);
            hash.Add("IntStatus", info.IntStatus);
            hash.Add("DtLastUpd", info.DtLastUpd);
            hash.Add("DtCreate", info.DtCreate);
            hash.Add("IntFlag", info.IntFlag);
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

            dict.Add("IntID", "IntID");
            dict.Add("VcAddr", "VcAddr");
            dict.Add("IntCustNo", "IntCustNo");
            dict.Add("DteReading", "DteReading");
            dict.Add("DteFreeze", "DteFreeze");
            dict.Add("NumReading", "NumReading");
            dict.Add("VcStatus", "VcStatus");
            dict.Add("IntStatus", "IntStatus");
            dict.Add("DtLastUpd", "DtLastUpd");
            dict.Add("DtCreate", "DtCreate");

            #endregion

            return dict;
        }

    }
}
