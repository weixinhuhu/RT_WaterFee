using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using WHC.WaterFeeWeb.Core.IDAL;
using WHC.Framework.Commons;

namespace WHC.WaterFeeWeb.Core.DALMySql
{
    public partial class ArcCustomerInfo : WHC.Framework.ControlUtil.BaseDALMySql<Entity.ArcCustomerInfo>, IArcCustomerInfo
    {
        #region 对象实例及构造函数

        public static ArcCustomerInfo Instance
        {
            get
            {
                return new ArcCustomerInfo();
            }
        }
        public ArcCustomerInfo() : base("ArcCustomerInfo", "IntID")
        {
        }

        #endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override Entity.ArcCustomerInfo DataReaderToEntity(IDataReader dataReader)
        {
            Entity.ArcCustomerInfo info = new Entity.ArcCustomerInfo();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.IntID = reader.GetInt32("IntID");
            info.IntNo = reader.GetInt32("IntNo");
            info.NvcName = reader.GetString("NvcName");
            info.NvcAddr = reader.GetString("NvcAddr");
            info.NvcVillage = reader.GetString("NvcVillage");
            info.VcBuilding = reader.GetString("VcBuilding");
            info.IntUnitNum = reader.GetInt32("IntUnitNum");
            info.IntRoomNum = reader.GetInt32("IntRoomNum");
            info.VcNameCode = reader.GetString("VcNameCode");
            info.VcAddrCode = reader.GetString("VcAddrCode");
            info.VcMobile = reader.GetString("VcMobile");
            info.VcTelNo = reader.GetString("VcTelNo");
            info.VcIDNo = reader.GetString("VcIDNo");
            info.VcContractNo = reader.GetString("VcContractNo");
            info.NvcInvName = reader.GetString("NvcInvName");
            info.NvcInvAddr = reader.GetString("NvcInvAddr");
            info.IntNumber = reader.GetInt32("IntNumber");
            info.IntPriceNo = reader.GetInt32("IntPriceNo");
            info.NvcCustType = reader.GetString("NvcCustType");
            info.IntUserID = reader.GetInt32("IntUserID");
            info.IntStatus = reader.GetInt32("IntStatus");
            info.IntAccMode = reader.GetInt32("IntAccMode");
            info.DteOpen = reader.GetDateTime("DteOpen");
            info.DteCancel = reader.GetDateTime("DteCancel");
            info.DtCreate = reader.GetDateTime("DtCreate");
            info.IntHelper = reader.GetInt32("IntHelper");

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(Entity.ArcCustomerInfo obj)
        {
            Entity.ArcCustomerInfo info = obj as Entity.ArcCustomerInfo;
            Hashtable hash = new Hashtable();

            hash.Add("IntID", info.IntID);
            hash.Add("IntNo", info.IntNo);
            hash.Add("NvcName", info.NvcName);
            hash.Add("NvcAddr", info.NvcAddr);
            hash.Add("NvcVillage", info.NvcVillage);
            hash.Add("VcBuilding", info.VcBuilding);
            hash.Add("IntUnitNum", info.IntUnitNum);
            hash.Add("IntRoomNum", info.IntRoomNum);
            hash.Add("VcNameCode", info.VcNameCode);
            hash.Add("VcAddrCode", info.VcAddrCode);
            hash.Add("VcMobile", info.VcMobile);
            hash.Add("VcTelNo", info.VcTelNo);
            hash.Add("VcIDNo", info.VcIDNo);
            hash.Add("VcContractNo", info.VcContractNo);
            hash.Add("NvcInvName", info.NvcInvName);
            hash.Add("NvcInvAddr", info.NvcInvAddr);
            hash.Add("IntNumber", info.IntNumber);
            hash.Add("IntPriceNo", info.IntPriceNo);
            hash.Add("NvcCustType", info.NvcCustType);
            hash.Add("IntUserID", info.IntUserID);
            hash.Add("IntAccMode", info.IntAccMode);
            hash.Add("IntStatus", info.IntStatus);
            hash.Add("DteOpen", info.DteOpen);
            hash.Add("DteCancel", info.DteCancel);
            hash.Add("DtCreate", info.DtCreate);
            hash.Add("IntHelper", info.IntHelper);
    

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
            dict.Add("IntNo", "IntNo");
            dict.Add("NvcName", "NvcName");
            dict.Add("NvcAddr", "NvcAddr");
            dict.Add("NvcVillage", "NvcVillage");
            dict.Add("VcBuilding", "VcBuilding");
            dict.Add("IntUnitNum", "IntUnitNum");
            dict.Add("IntRoomNum", "IntRoomNum");
            dict.Add("VcNameCode", "VcNameCode");
            dict.Add("VcAddrCode", "VcAddrCode");
            dict.Add("VcMobile", "VcMobile");
            dict.Add("VcTelNo", "VcTelNo");
            dict.Add("VcIDNo", "VcIDNo");
            dict.Add("VcContractNo", "VcContractNo");
            dict.Add("NvcInvName", "NvcInvName");
            dict.Add("NvcInvAddr", "NvcInvAddr");
            dict.Add("IntNumber", "IntNumber");
            dict.Add("IntPriceNo", "IntPriceNo");
            dict.Add("NvcCustType", "NvcCustType");
            dict.Add("IntUserID", "IntUserID");
            dict.Add("IntAccMode", "IntAccMode");
            dict.Add("IntStatus", "IntStatus");
            dict.Add("DteOpen", "DteOpen");
            dict.Add("DteCancel", "DteCancel");
            dict.Add("DtCreate", "DtCreate");
            dict.Add("IntHelper", "IntHelper");

            #endregion

            return dict;
        }

    }
}
