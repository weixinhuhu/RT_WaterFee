using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using WHC.WaterFeeWeb.Core.IDAL;
using WHC.Framework.Commons;

namespace WHC.WaterFeeWeb.Core.DALMySql
{
    public class AccDepositDetail : WHC.Framework.ControlUtil.BaseDALMySql<Entity.AccDepositDetail>, IAccDepositDetail
    {
        #region 对象实例及构造函数

        public static AccDepositDetail Instance
        {
            get
            {
                return new AccDepositDetail();
            }
        }
        public AccDepositDetail() : base("AccDepositDetail", "IntID")
        {
        }

        #endregion
     
        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override Entity.AccDepositDetail DataReaderToEntity(IDataReader dataReader)
        {
            Entity.AccDepositDetail info = new Entity.AccDepositDetail();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.IntID = reader.GetInt32("IntID"); 
            info.IntCustNo = reader.GetInt32("IntCustNo"); 
            info.MonAmount = reader.GetDecimal("MonAmount");
            info.IntType = reader.GetInt32("IntType"); 
            info.VcFlowNo = reader.GetString("VcFlowNo"); 
            info.VcUserID = reader.GetString("VcUserID");
            info.VcReceiptNo = reader.GetString("VcReceiptNo"); 
            info.DteAccount = reader.GetDateTime("DteAccount"); 
            info.IntFlag = reader.GetInt32("IntFlag");
            info.VcDesc = reader.GetString("VcDesc"); 
            info.DtCreate = reader.GetDateTime("DtCreate");  

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(Entity.AccDepositDetail obj)
        {
            Entity.AccDepositDetail info = obj as Entity.AccDepositDetail;
            Hashtable hash = new Hashtable();

            hash.Add("IntID", info.IntID); 
            hash.Add("IntCustNo", info.IntCustNo);
            hash.Add("MonAmount", info.MonAmount);
            hash.Add("IntType", info.IntType);
            hash.Add("VcFlowNo", info.VcFlowNo); 
            hash.Add("VcUserID", info.VcUserID);
            hash.Add("VcReceiptNo", info.VcReceiptNo);
            hash.Add("DteAccount", info.DteAccount);
            hash.Add("IntFlag", info.IntFlag); 
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
            dict.Add("IntID", "IntID"); 
            dict.Add("IntCustNo", "IntCustNo"); 
            dict.Add("MonAmount", "MonAmount"); 
            dict.Add("IntType", "IntType"); 
            dict.Add("VcFlowNo", "VcFlowNo"); 
            dict.Add("VcUserID", "VcUserID"); 
            dict.Add("VcReceiptNo", "VcReceiptNo"); 
            dict.Add("DteAccount", "DteAccount"); 
            dict.Add("IntFlag", "IntFlag"); 
            dict.Add("VcDesc", "VcDesc"); 
            dict.Add("DtCreate", "DtCreate");  
            #endregion

            return dict;
        }

    }
}
