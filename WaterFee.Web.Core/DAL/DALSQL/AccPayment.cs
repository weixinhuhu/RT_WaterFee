using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using WHC.WaterFeeWeb.Core.IDAL;
using WHC.Framework.Commons;

namespace WHC.WaterFeeWeb.Core.DALSQL
{
    public class AccPayment : WHC.Framework.ControlUtil.BaseDALSQL<Entity.AccPayment>, IAccPayment
    {
        #region 对象实例及构造函数

        public static AccPayment Instance
        {
            get
            {
                return new AccPayment();
            }
        }
        public AccPayment() : base("AccPayment", "IntFeeID")
        {
        }

        #endregion
 
        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override Entity.AccPayment DataReaderToEntity(IDataReader dataReader)
        {
            Entity.AccPayment info = new Entity.AccPayment();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.IntFeeID = reader.GetInt32("IntFeeID"); 
            info.IntCustNo = reader.GetInt32("IntCustNo"); 
            info.IntYearMon = reader.GetInt32("IntYearMon"); 
            info.DteFee = reader.GetDateTime("DteFee"); 
            info.DtePay = reader.GetDateTime("DtePay"); 
            info.DteEnd = reader.GetDateTime("DteEnd"); 
            info.DteEndExec = reader.GetDateTime("DteEndExec"); 
            info.MonFee = reader.GetDecimal("MonFee"); 
            info.MonFeeExec = reader.GetDecimal("MonFeeExec"); 
            info.MonPenalty = reader.GetDecimal("MonPenalty"); 
            info.IntDays = reader.GetInt32("IntDays"); 
            info.IntPayUnit = reader.GetInt32("IntPayUnit"); 
            info.IntPayMode = reader.GetInt32("IntPayMode"); 
            info.VcChargeNo = reader.GetString("VcChargeNo"); 
            info.VcFlowNo = reader.GetString("VcFlowNo"); 
            info.DtCreate = reader.GetDateTime("DtCreate"); 

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(Entity.AccPayment obj)
        {
            Entity.AccPayment info = obj as Entity.AccPayment;
            Hashtable hash = new Hashtable();

            hash.Add("IntFeeID", info.IntFeeID); 
            hash.Add("IntCustNo", info.IntCustNo); 
            hash.Add("IntYearMon", info.IntYearMon); 
            hash.Add("DteFee", info.DteFee); 
            hash.Add("DtePay", info.DtePay); 
            hash.Add("DteEnd", info.DteEnd); 
            hash.Add("DteEndExec", info.DteEndExec); 
            hash.Add("MonFee", info.MonFee); 
            hash.Add("MonFeeExec", info.MonFeeExec); 
            hash.Add("MonPenalty", info.MonPenalty); 
            hash.Add("IntDays", info.IntDays); 
            hash.Add("IntPayUnit", info.IntPayUnit); 
            hash.Add("IntPayMode", info.IntPayMode); 
            hash.Add("VcChargeNo", info.VcChargeNo); 
            hash.Add("VcFlowNo", info.VcFlowNo); 
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
            dict.Add("DtePay", "DtePay"); 
            dict.Add("DteEnd", "DteEnd"); 
            dict.Add("DteEndExec", "DteEndExec"); 
            dict.Add("MonFee", "MonFee"); 
            dict.Add("MonFeeExec", "MonFeeExec"); 
            dict.Add("MonPenalty", "MonPenalty"); 
            dict.Add("IntDays", "IntDays"); 
            dict.Add("IntPayUnit", "IntPayUnit"); 
            dict.Add("IntPayMode", "IntPayMode"); 
            dict.Add("VcChargeNo", "VcChargeNo"); 
            dict.Add("VcFlowNo", "VcFlowNo"); 
            dict.Add("DtCreate", "DtCreate");  
            #endregion

            return dict;
        }

    }
}
