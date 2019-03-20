using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using WHC.WaterFeeWeb.Core.IDAL;
using WHC.Framework.Commons;

namespace WHC.WaterFeeWeb.Core.DALSQL
{
    public class MidConcentrator : WHC.Framework.ControlUtil.BaseDALSQL<Entity.MidConcentrator>, IMidConcentrator
    {
        #region 对象实例及构造函数

        public static MidConcentrator Instance
        {
            get
            {
                return new MidConcentrator();
            }
        }
        public MidConcentrator() : base("MidConcentrator", "VcAddr")
        {
        }

        #endregion 

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override Entity.MidConcentrator DataReaderToEntity(IDataReader dataReader)
        {
            Entity.MidConcentrator info = new Entity.MidConcentrator();
            SmartDataReader reader = new SmartDataReader(dataReader);

             info.VcClientIP = reader.GetString("VcClientIP"); 
             info.NvcName = reader.GetString("NvcName"); 
             info.NvcAddr = reader.GetString("NvcAddr"); 
             info.VcAddr = reader.GetString("VcAddr"); 
             info.IntProtocol = reader.GetInt32("IntProtocol"); 
             info.IntCount = reader.GetInt32("IntCount"); 
             info.IntCommMode = reader.GetInt32("IntCommMode"); 
             info.IntCOM = reader.GetInt32("IntCOM"); 
             info.VcParam = reader.GetString("VcParam"); 
             info.VcSimNo = reader.GetString("VcSimNo"); 
             info.IntStatus = reader.GetInt32("IntStatus");  

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(Entity.MidConcentrator obj)
        {
            Entity.MidConcentrator info = obj as Entity.MidConcentrator;
            Hashtable hash = new Hashtable();

            hash.Add("VcClientIP", info.VcClientIP);
            hash.Add("NvcName", info.NvcName);
            hash.Add("NvcAddr", info.NvcAddr);
            hash.Add("VcAddr", info.VcAddr);
            hash.Add("IntProtocol", info.IntProtocol);
            hash.Add("IntCount", info.IntCount);
            hash.Add("IntCommMode", info.IntCommMode);
            hash.Add("IntCOM", info.IntCOM);
            hash.Add("VcParam", info.VcParam);
            hash.Add("VcSimNo", info.VcSimNo);
            hash.Add("IntStatus", info.IntStatus);

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
            dict.Add("NvcName", "NvcName");
            dict.Add("NvcAddr", "NvcAddr");
            dict.Add("VcAddr", "VcAddr");
            dict.Add("IntProtocol", "IntProtocol");
            dict.Add("IntCount", "IntCount");
            dict.Add("IntCommMode", "IntCommMode");
            dict.Add("IntCOM", "IntCOM");
            dict.Add("VcParam", "VcParam");
            dict.Add("VcSimNo", "VcSimNo");
            dict.Add("IntStatus", "IntStatus");
            #endregion

            return dict;
        }

    }
}

