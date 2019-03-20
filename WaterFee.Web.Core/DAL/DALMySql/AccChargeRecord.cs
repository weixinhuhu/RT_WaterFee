using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using WHC.WaterFeeWeb.Core.IDAL;
using WHC.Framework.Commons;

namespace WHC.WaterFeeWeb.Core.DALMySql
{
    public class AccChargeRecord : WHC.Framework.ControlUtil.BaseDALMySql<Entity.AccChargeRecord>, IAccChargeRecord
    {
        #region 对象实例及构造函数

        public static AccChargeRecord Instance
        {
            get
            {
                return new AccChargeRecord();
            }
        }
        public AccChargeRecord() : base("AccChargeRecord", "IntID")
        {
            
        }

        #endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override Entity.AccChargeRecord DataReaderToEntity(IDataReader dataReader)
        {
            Entity.AccChargeRecord info = new Entity.AccChargeRecord();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.IntID = reader.GetInt32("IntID");
            info.VcChargeNo = reader.GetString("VcChargeNo");
            info.IntFeeID = reader.GetInt32("IntFeeID");
            info.IntCustNo = reader.GetInt32("IntCustNo");
            info.DteFee = reader.GetDateTime("DteFee");
            info.VcFlowNo = reader.GetString("VcFlowNo");
            info.VcInvNo = reader.GetString("VcInvNo");
            info.IntInvFlag = reader.GetInt32("IntInvFlag");
            info.DteAccount = reader.GetDateTime("DteAccount");
            info.NumFee = reader.GetDecimal("NumFee");
            info.NumPenalty = reader.GetDecimal("NumPenalty");
            info.NumLastPlus = reader.GetDecimal("NumLastPlus");
            info.NumThisPlus = reader.GetDecimal("NumThisPlus");
            info.IntChargeWay = reader.GetInt32("IntChargeWay");
            info.IntFlag = reader.GetInt32("IntFlag");
            info.DtLastUpd = reader.GetDateTime("DtLastUpd");
            info.DtCreate = reader.GetDateTime("DtCreate");

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(Entity.AccChargeRecord obj)
        {
            Entity.AccChargeRecord info = obj as Entity.AccChargeRecord;
            Hashtable hash = new Hashtable();

            hash.Add("IntID", info.IntID);
            hash.Add("VcChargeNo", info.VcChargeNo);
            hash.Add("IntFeeID", info.IntFeeID);
            hash.Add("IntCustNo", info.IntCustNo);
            hash.Add("DteFee", info.DteFee);
            hash.Add("VcFlowNo", info.VcFlowNo);
            hash.Add("VcInvNo", info.VcInvNo);
            hash.Add("IntInvFlag", info.IntInvFlag);
            hash.Add("DteAccount", info.DteAccount);
            hash.Add("NumFee", info.NumFee);
            hash.Add("NumPenalty", info.NumPenalty);
            hash.Add("NumLastPlus", info.NumLastPlus);
            hash.Add("NumThisPlus", info.NumThisPlus);
            hash.Add("IntChargeWay", info.IntChargeWay);
            hash.Add("IntFlag", info.IntFlag);
            hash.Add("DtLastUpd", info.DtLastUpd);
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
            dict.Add("VcChargeNo", "VcChargeNo");
            dict.Add("IntFeeID", "IntFeeID");
            dict.Add("IntCustNo", "IntCustNo");
            dict.Add("DteFee", "DteFee");
            dict.Add("VcFlowNo", "VcFlowNo");
            dict.Add("VcInvNo", "VcInvNo");
            dict.Add("IntInvFlag", "IntInvFlag");
            dict.Add("DteAccount", "DteAccount");
            dict.Add("NumFee", "NumFee");
            dict.Add("NumPenalty", "NumPenalty");
            dict.Add("NumLastPlus", "NumLastPlus");
            dict.Add("NumThisPlus", "NumThisPlus");
            dict.Add("IntChargeWay", "IntChargeWay");
            dict.Add("IntFlag", "IntFlag");
            dict.Add("DtLastUpd", "DtLastUpd");
            dict.Add("DtCreate", "DtCreate");
            #endregion

            return dict;
        }

    }
}
