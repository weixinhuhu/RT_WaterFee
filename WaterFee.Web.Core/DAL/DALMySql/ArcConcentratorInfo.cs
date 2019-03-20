using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using WHC.WaterFeeWeb.Core.IDAL;
using WHC.Framework.Commons;

namespace WHC.WaterFeeWeb.Core.DALMySql
{
    public class ArcConcentratorInfo : WHC.Framework.ControlUtil.BaseDALMySql<Entity.ArcConcentratorInfo>, IArcConcentratorInfo
    {
        #region 对象实例及构造函数

        public static ArcConcentratorInfo Instance
        {
            get
            {
                return new ArcConcentratorInfo();
            }
        }
        public ArcConcentratorInfo() : base("ArcConcentratorInfo", "IntID")
        {
        }

        #endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override Entity.ArcConcentratorInfo DataReaderToEntity(IDataReader dataReader)
        {
            Entity.ArcConcentratorInfo info = new Entity.ArcConcentratorInfo();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.IntID = reader.GetInt32("IntID");
            info.VcAddr = reader.GetString("VcAddr");
            info.NvcName = reader.GetString("NvcName");
            info.NvcAddr = reader.GetString("NvcAddr");
            info.VcSimNo = reader.GetString("VcSimNo");
            info.IntOnline = reader.GetInt32("IntOnline");
            info.IntStatus = reader.GetInt32("IntStatus");
            info.IntProtocol = reader.GetInt32("IntProtocol");
            info.IntCount = reader.GetInt32("IntCount");
            info.IntCommMode = reader.GetInt32("IntCommMode");
            info.IntCOM = reader.GetInt32("IntCOM");
            info.VcParam = reader.GetString("VcParam");
            info.DtLastUpd = reader.GetDateTime("DtLastUpd");
            info.DtCreate = reader.GetDateTime("DtCreate");
            info.IntUpID = reader.GetInt32("IntUpID");
            info.IntCmpCode = reader.GetInt32("IntCmpCode");
            info.IntAreaCode = reader.GetInt32("IntAreaCode");

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(Entity.ArcConcentratorInfo obj)
        {
            Entity.ArcConcentratorInfo info = obj as Entity.ArcConcentratorInfo;
            Hashtable hash = new Hashtable();

            hash.Add("IntID", info.IntID);
            hash.Add("VcAddr", info.VcAddr);
            hash.Add("NvcName", info.NvcName);
            hash.Add("NvcAddr", info.NvcAddr);
            hash.Add("VcSimNo", info.VcSimNo);
            hash.Add("IntOnline", info.IntOnline);
            hash.Add("IntStatus", info.IntStatus);
            hash.Add("IntProtocol", info.IntProtocol);
            hash.Add("IntCount", info.IntCount);
            hash.Add("IntCommMode", info.IntCommMode);
            hash.Add("IntCOM", info.IntCOM);
            hash.Add("VcParam", info.VcParam);
            hash.Add("DtLastUpd", info.DtLastUpd);
            hash.Add("DtCreate", info.DtCreate);
            hash.Add("IntUpID", info.IntUpID);
            hash.Add("IntCmpCode", info.IntCmpCode);
            hash.Add("IntAreaCode", info.IntAreaCode);

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
            dict.Add("IntID", "ID");
            dict.Add("VcAddr", "通信地址");
            dict.Add("NvcName", "采集器名称");
            dict.Add("NvcAddr", "采集器安装地址");
            dict.Add("VcSimNo", "移动号码");
            dict.Add("IntOnline", "在线状态");
            dict.Add("IntStatus", "状态");
            dict.Add("IntProtocol", "协议类型");
            dict.Add("IntCount", "通道数量");
            dict.Add("IntCommMode", "通信方式");
            dict.Add("IntCOM", "串口号");
            dict.Add("VcParam", "通讯参数");
            dict.Add("DtLastUpd", "最后更新");
            dict.Add("DtCreate", "创建日期");
            dict.Add("IntUpID", "父级ID");
            dict.Add("IntCmpCode", "厂家码");
            dict.Add("IntAreaCode", "区域码");
            #endregion

            return dict;
        }

    }
}
