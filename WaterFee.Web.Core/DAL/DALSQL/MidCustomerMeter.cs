using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using WHC.WaterFeeWeb.Core.IDAL;
using WHC.Framework.Commons;

namespace WHC.WaterFeeWeb.Core.DALSQL
{
    public class MidCustomerMeter : WHC.Framework.ControlUtil.BaseDALSQL<Entity.MidCustomerMeter>, IMidCustomerMeter
    {
        #region 对象实例及构造函数

        public static MidCustomerMeter Instance
        {
            get
            {
                return new MidCustomerMeter();
            }
        }
        public MidCustomerMeter() : base("MidCustomerMeter", "VcAddr")
        {
        }

        #endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override Entity.MidCustomerMeter DataReaderToEntity(IDataReader dataReader)
        {
            Entity.MidCustomerMeter info = new Entity.MidCustomerMeter();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.VcClientIP = reader.GetString("VcClientIP");
            info.IntCustCode = reader.GetInt32("IntCustCode");
            info.NvcName = reader.GetString("NvcName");
            info.NvcAddr = reader.GetString("NvcAddr");
            info.NvcVillage = reader.GetString("NvcVillage");
            info.VcBuilding = reader.GetString("VcBuilding");
            info.IntUnitNum = reader.GetInt32("IntUnitNum");
            info.IntRoomNum = reader.GetInt32("IntRoomNum");
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
            info.DteOpen = reader.GetDateTime("DteOpen");
            info.VcAddr = reader.GetString("VcAddr");
            info.NvcAddrIns = reader.GetString("NvcAddrIns");
            info.VcBarCode = reader.GetString("VcBarCode");
            info.VcAssetNo = reader.GetString("VcAssetNo");
            info.IntChannal = reader.GetInt32("IntChannal");
            info.IntProtocol = reader.GetInt32("IntProtocol");
            info.IntCycle = reader.GetInt32("IntCycle");
            info.IntOrig = reader.GetInt32("IntOrig");
            info.IntState = reader.GetInt32("IntState");
            info.DtCreate = reader.GetDateTime("DtCreate");

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(Entity.MidCustomerMeter obj)
        {
            Entity.MidCustomerMeter info = obj as Entity.MidCustomerMeter;
            Hashtable hash = new Hashtable();

            hash.Add("VcClientIP", info.VcClientIP);
            hash.Add("IntCustCode", info.IntCustCode);
            hash.Add("NvcName", info.NvcName);
            hash.Add("NvcAddr", info.NvcAddr);
            hash.Add("NvcVillage", info.NvcVillage);
            hash.Add("VcBuilding", info.VcBuilding);
            hash.Add("IntUnitNum", info.IntUnitNum);
            hash.Add("IntRoomNum", info.IntRoomNum);
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
            hash.Add("IntStatus", info.IntStatus);
            hash.Add("DteOpen", info.DteOpen);
            hash.Add("VcAddr", info.VcAddr);
            hash.Add("NvcAddrIns", info.NvcAddrIns);
            hash.Add("VcBarCode", info.VcBarCode);
            hash.Add("VcAssetNo", info.VcAssetNo);
            hash.Add("IntChannal", info.IntChannal);
            hash.Add("IntProtocol", info.IntProtocol);
            hash.Add("IntCycle", info.IntCycle);
            hash.Add("IntOrig", info.IntOrig);
            hash.Add("IntState", info.IntState);
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

            dict.Add("VcClientIP", "VcClientIP");
            dict.Add("IntCustCode", "IntCustCode");
            dict.Add("NvcName", "NvcName");
            dict.Add("NvcAddr", "NvcAddr");
            dict.Add("NvcVillage", "NvcVillage");
            dict.Add("VcBuilding", "VcBuilding");
            dict.Add("IntUnitNum", "IntUnitNum");
            dict.Add("IntRoomNum", "IntRoomNum");
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
            dict.Add("IntStatus", "IntStatus");
            dict.Add("DteOpen", "DteOpen");
            dict.Add("VcAddr", "VcAddr");
            dict.Add("NvcAddrIns", "NvcAddrIns");
            dict.Add("VcBarCode", "VcBarCode");
            dict.Add("VcAssetNo", "VcAssetNo");
            dict.Add("IntChannal", "IntChannal");
            dict.Add("IntProtocol", "IntProtocol");
            dict.Add("IntCycle", "IntCycle");
            dict.Add("IntOrig", "IntOrig");
            dict.Add("IntState", "IntState");
            dict.Add("DtCreate", "DtCreate");

            #endregion

            return dict;
        }

    }
}
