using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using WHC.WaterFeeWeb.Core.IDAL;
using WHC.Framework.Commons;

namespace WHC.WaterFeeWeb.Core.DALMySql
{
    public class AccDetail : WHC.Framework.ControlUtil.BaseDALMySql<Entity.AccDetail>, IAccDetail
    {
        #region 对象实例及构造函数

        public static AccDetail Instance
        {
            get
            {
                return new AccDetail();
            }
        }
        public AccDetail() : base("AccDetail", "IntFeeID")
        {
        }

        #endregion
 
        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override Entity.AccDetail DataReaderToEntity(IDataReader dataReader)
        {
            Entity.AccDetail info = new Entity.AccDetail();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.IntFeeID= reader.GetInt32("IntFeeID"); 
            info.IntCustNo= reader.GetInt32("IntCustNo"); 
            info.IntYearMon= reader.GetInt32("IntYearMon"); 
            info.DteFee= reader.GetDateTime("DteFee"); 
            info.VcAddr= reader.GetString("VcAddr"); 
            info.IntStepOrd= reader.GetInt32("IntStepOrd"); 
            info.IntType= reader.GetInt32("IntType"); 
            info.NumFee= reader.GetDecimal("NumFee"); 
            info.IntInvFlag= reader.GetInt32("IntInvFlag"); 
            info.VcDesc= reader.GetString("VcDesc"); 
            info.DtCreate = reader.GetDateTime("DtCreate");  

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(Entity.AccDetail obj)
        {
            Entity.AccDetail info = obj as Entity.AccDetail;
            Hashtable hash = new Hashtable();

            hash.Add("IntFeeID", info.IntFeeID);
            hash.Add("IntCustNo", info.IntCustNo);
            hash.Add("IntYearMon", info.IntYearMon);
            hash.Add("DteFee", info.DteFee);
            hash.Add("VcAddr", info.VcAddr);
            hash.Add("IntStepOrd", info.IntStepOrd);
            hash.Add("IntType", info.IntType);
            hash.Add("NumFee", info.NumFee);
            hash.Add("IntInvFlag", info.IntInvFlag);
            hash.Add("VcDesc", info.VcDesc);
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
            dict.Add("VcAddr", "VcAddr"); 
            dict.Add("IntStepOrd", "IntStepOrd"); 
            dict.Add("IntType", "IntType"); 
            dict.Add("NumFee", "NumFee"); 
            dict.Add("IntInvFlag", "IntInvFlag"); 
            dict.Add("VcDesc", "VcDesc"); 
            dict.Add("DtCreate", "DtCreate"); 
            #endregion

            return dict;
        }

    }
}
