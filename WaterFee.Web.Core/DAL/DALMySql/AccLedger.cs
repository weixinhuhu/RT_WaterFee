using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using WHC.WaterFeeWeb.Core.IDAL;
using WHC.Framework.Commons;

namespace WHC.WaterFeeWeb.Core.DALMySql
{
    public class AccLedger : WHC.Framework.ControlUtil.BaseDALMySql<Entity.AccLedger>, IAccLedger
    {
        #region 对象实例及构造函数

        public static AccLedger Instance
        {
            get
            {
                return new AccLedger();
            }
        }
        public AccLedger() : base("AccLedger", "IntFeeID")
        {
        }

        #endregion

   
        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override Entity.AccLedger DataReaderToEntity(IDataReader dataReader)
        {
            Entity.AccLedger info = new Entity.AccLedger();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.IntFeeID = reader.GetInt32("IntFeeID");
            info.IntCustNo = reader.GetInt32("IntCustNo");
            info.IntYearMon = reader.GetInt32("IntYearMon");
            info.DteFee = reader.GetDateTime("DteFee");
            info.IntCalcSum = reader.GetInt32("IntCalcSum");
            info.MonFeeSum = reader.GetDecimal("MonFeeSum");
            info.DtLock = reader.GetDateTime("DtLock");
            info.DtCalc = reader.GetDateTime("DtCalc");
            info.IntInvFlag = reader.GetInt32("IntInvFlag");
            info.DtCreate = reader.GetDateTime("DtCreate");

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(Entity.AccLedger obj)
        {
            Entity.AccLedger info = obj as Entity.AccLedger;
            Hashtable hash = new Hashtable();

            hash.Add("IntFeeID", info.IntFeeID); 
            hash.Add("IntCustNo", info.IntCustNo); 
            hash.Add("IntYearMon", info.IntYearMon); 
            hash.Add("DteFee", info.DteFee); 
            hash.Add("IntCalcSum", info.IntCalcSum); 
            hash.Add("MonFeeSum", info.MonFeeSum); 
            hash.Add("DtLock", info.DtLock); 
            hash.Add("DtCalc", info.DtCalc); 
            hash.Add("IntInvFlag", info.IntInvFlag); 
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
            dict.Add("IntCustNo", "IntCustNo"); 
            dict.Add("IntYearMon", "IntYearMon"); 
            dict.Add("DteFee", "DteFee"); 
            dict.Add("IntCalcSum", "IntCalcSum"); 
            dict.Add("MonFeeSum", "MonFeeSum"); 
            dict.Add("DtLock", "DtLock"); 
            dict.Add("DtCalc", "DtCalc"); 
            dict.Add("IntInvFlag", "IntInvFlag"); 
            dict.Add("DtCreate", "DtCreate");  
            #endregion

            return dict;
        }

    }
}
