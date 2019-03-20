using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using WHC.WaterFeeWeb.Core.IDAL;
using WHC.Framework.Commons;

namespace WHC.WaterFeeWeb.Core.DALSQL
{
    public class AccDebt : WHC.Framework.ControlUtil.BaseDALSQL<Entity.AccDebt>, IAccDebt
    {
        #region 对象实例及构造函数

        public static AccDebt Instance
        {
            get
            {
                return new AccDebt();
            }
        }
        public AccDebt() : base("AccDebt", "IntFeeID")
        {
        }

        #endregion
 
        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override Entity.AccDebt DataReaderToEntity(IDataReader dataReader)
        {
            Entity.AccDebt info = new Entity.AccDebt();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.IntFeeID = reader.GetInt32("IntFeeID"); 
            info.IntCustNo = reader.GetInt32("IntCustNo"); 
            info.IntYearMon = reader.GetInt32("IntYearMon"); 
            info.DteFee = reader.GetDateTime("DteFee"); 
            info.MonFee = reader.GetDecimal("MonFee"); 
            info.MonFeeExec = reader.GetDecimal("MonFeeExec"); 
            info.DteEnd = reader.GetDateTime("DteEnd"); 
            info.DteEndExec = reader.GetDateTime("DteEndExec"); 
            info.IntStatus = reader.GetInt32("IntStatus");
            info.IntInvFlag = reader.GetInt32("IntInvFlag"); 
            info.VcRemitCause = reader.GetString("VcRemitCause"); 
            info.DtUpLast = reader.GetDateTime("DtUpLast");  
            info.DtCreate = reader.GetDateTime("DtCreate");  

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(Entity.AccDebt obj)
        {
            Entity.AccDebt info = obj as Entity.AccDebt;
            Hashtable hash = new Hashtable();

            hash.Add("IntFeeID", info.IntFeeID); 
            hash.Add("IntCustNo", info.IntCustNo); 
            hash.Add("IntYearMon", info.IntYearMon); 
            hash.Add("DteFee", info.DteFee); 
            hash.Add("MonFee", info.MonFee); 
            hash.Add("MonFeeExec", info.MonFeeExec); 
            hash.Add("DteEnd", info.DteEnd); 
            hash.Add("DteEndExec", info.DteEndExec); 
            hash.Add("IntStatus", info.IntStatus); 
            hash.Add("IntInvFlag", info.IntInvFlag); 
            hash.Add("VcRemitCause", info.VcRemitCause); 
            hash.Add("DtUpLast", info.DtUpLast); 
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
            dict.Add("MonFee", "MonFee"); 
            dict.Add("MonFeeExec", "MonFeeExec"); 
            dict.Add("DteEnd", "DteEnd"); 
            dict.Add("DteEndExec", "DteEndExec"); 
            dict.Add("IntStatus", "IntStatus"); 
            dict.Add("IntInvFlag", "IntInvFlag"); 
            dict.Add("VcRemitCause", "VcRemitCause"); 
            dict.Add("DtUpLast", "DtUpLast"); 
            dict.Add("DtCreate", "DtCreate");  
            #endregion

            return dict;
        }

    }
}
