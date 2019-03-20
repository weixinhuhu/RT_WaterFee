using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using WHC.WaterFeeWeb.Core.IDAL;
using WHC.Framework.Commons;

namespace WHC.WaterFeeWeb.Core.DALMySql
{
    public partial class ArcMeterInfo : WHC.Framework.ControlUtil.BaseDALMySql<Entity.ArcMeterInfo>, IArcMeterInfo
    {
        #region 对象实例及构造函数

        public static ArcMeterInfo Instance
        {
            get
            {
                return new ArcMeterInfo();
            }
        }
        public ArcMeterInfo() : base("ArcMeterInfo", "IntID")
        {
        }

        #endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override Entity.ArcMeterInfo DataReaderToEntity(IDataReader dataReader)
        {
            Entity.ArcMeterInfo info = new Entity.ArcMeterInfo();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.IntID = reader.GetInt32("IntID");
            info.VcAddr = reader.GetString("VcAddr");
            info.NvcName = reader.GetString("NvcName");
            info.NvcAddr = reader.GetString("NvcAddr");
            info.VcBarCode = reader.GetString("VcBarCode");
            info.VcAssetNo = reader.GetString("VcAssetNo");
            info.IntProtocol = reader.GetInt32("IntProtocol");
            info.IntCycle = reader.GetInt32("IntCycle");
            info.IntOrig = reader.GetInt32("IntOrig");
            info.IntReadFlag = reader.GetInt32("IntReadFlag");
            info.IntValveState = reader.GetInt32("IntValveState");
            info.IntConID = reader.GetInt32("IntConID");
            info.NumRatio = reader.GetInt32("NumRatio").ToString();
            info.IntMP = reader.GetInt32("IntMP");
            info.IntPriceNo2 = reader.GetInt32("IntRate");
            info.IntCustNO = reader.GetInt32("IntCustNO");
            info.IntStatus = reader.GetInt32("IntStatus");
            info.DtLastUpd = reader.GetDateTime("DtLastUpd");
            info.DtCreate = reader.GetDateTime("DtCreate");
            info.IntPriceNo = reader.GetInt32("IntPriceNo");

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(Entity.ArcMeterInfo obj)
        {
            Entity.ArcMeterInfo info = obj as Entity.ArcMeterInfo;
            Hashtable hash = new Hashtable();

            hash.Add("IntID", info.IntID);
            hash.Add("VcAddr", info.VcAddr);
            hash.Add("NvcName", info.NvcName);
            hash.Add("NvcAddr", info.NvcAddr);
            hash.Add("VcBarCode", info.VcBarCode);
            hash.Add("VcAssetNo", info.VcAssetNo);
            hash.Add("IntProtocol", info.IntProtocol);
            hash.Add("IntCycle", info.IntCycle);
            hash.Add("IntOrig", info.IntOrig);
            hash.Add("IntReadFlag", info.IntReadFlag);
            hash.Add("IntValveState", info.IntValveState);
            hash.Add("IntConID", info.IntConID);
            hash.Add("NumRatio", info.NumRatio);
            hash.Add("IntMP", info.IntMP);
            hash.Add("IntPriceNo2", info.IntPriceNo2);
            hash.Add("IntCustNO", info.IntCustNO);
            hash.Add("IntStatus", info.IntStatus);
            hash.Add("DtLastUpd", info.DtLastUpd);
            hash.Add("DtCreate", info.DtCreate);
            hash.Add("IntPriceNo", info.IntPriceNo);
            hash.Add("IntAccountWay",info.IntAccountWay);
            hash.Add("IntAutoSwitch", info.IntAutoSwitch);


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
            dict.Add("NvcName", "NvcName");
            dict.Add("NvcAddr", "NvcAddr");
            dict.Add("VcBarCode", "VcBarCode");
            dict.Add("VcAssetNo", "VcAssetNo");
            dict.Add("IntProtocol", "IntProtocol");
            dict.Add("IntCycle", "IntCycle");
            dict.Add("IntOrig", "IntOrig");
            dict.Add("IntReadFlag", "IntReadFlag");
            dict.Add("IntValveState", "IntValveState");
            dict.Add("IntConID", "IntConID");
            dict.Add("IntChannal", "IntChannal");
            dict.Add("IntMP", "IntMP");
            dict.Add("IntRate", "IntRate");
            dict.Add("IntCustNO", "IntCustNO");
            dict.Add("IntStatus", "IntStatus");
            dict.Add("DtLastUpd", "DtLastUpd");
            dict.Add("DtCreate", "DtCreate");
            dict.Add("IntPriceNo", "IntPriceNo");
            #endregion

            return dict;
        }

    }
}

