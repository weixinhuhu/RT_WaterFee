using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;

using WHC.Pager.Entity;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;
using Microsoft.Practices.EnterpriseLibrary.Data;
using WHC.MVCWebMis.Entity;
using WHC.MVCWebMis.IDAL;

namespace WHC.MVCWebMis.DALSQL
{
    /// <summary>
    /// DaClientInfo
    /// </summary>
	public class DaClientInfo : BaseDALSQL<DaClientInfoInfo>, IDaClientInfo
	{
		#region 对象实例及构造函数

		public static DaClientInfo Instance
		{
			get
			{
				return new DaClientInfo();
			}
		}
		public DaClientInfo() : base("DaClientInfo","ID")
		{
		}

		#endregion

		/// <summary>
		/// 将DataReader的属性值转化为实体类的属性值，返回实体类
		/// </summary>
		/// <param name="dr">有效的DataReader对象</param>
		/// <returns>实体类对象</returns>
		protected override DaClientInfoInfo DataReaderToEntity(IDataReader dataReader)
		{
			DaClientInfoInfo info = new DaClientInfoInfo();
			SmartDataReader reader = new SmartDataReader(dataReader);
			
			info.ID = reader.GetInt32("ID");
			info.Zhh = reader.GetInt32("ZHH");
			info.Qy = reader.GetString("QY");
			info.Sh = reader.GetString("SH");
			info.Cbbh = reader.GetString("CBBH");
			info.Bxh = reader.GetInt32("BXH");
			info.Hmcm = reader.GetString("HMCM");
			info.Dzcm = reader.GetString("DZCM");
			info.Dhhm = reader.GetString("DHHM");
			info.Mobile = reader.GetString("MOBILE");
			info.Lxr = reader.GetString("LXR");
			info.Hth = reader.GetString("HTH");
			info.Hm = reader.GetString("HM");
			info.Dw = reader.GetString("DW");
			info.Dwdh = reader.GetString("DWDH");
			info.Dz = reader.GetString("DZ");
			info.Yhxz = reader.GetString("YHXZ");
			info.Sffs = reader.GetString("SFFS");
			info.Sfzh = reader.GetString("SFZH");
			info.Ysxz = reader.GetString("YSXZ");
			info.Jhrq = reader.GetDateTime("JHRQ");
			info.Xhrq = reader.GetDateTime("XHRQ");
			info.Yjsl = reader.GetInt32("YJSL");
			info.Yhzt = reader.GetString("YHZT");
			info.Gh = reader.GetString("GH");
			info.Rks = reader.GetInt32("RKS");
			info.Fphm = reader.GetString("FPHM");
			info.Fpdz = reader.GetString("FPDZ");
			info.Fkdwqc = reader.GetString("FKDWQC");
			info.Fkdwzh = reader.GetString("FKDWZH");
			info.Fkkhyh = reader.GetString("FKKHYH");
			info.Yhlx = reader.GetString("YHLX");
			info.Cby = reader.GetString("CBY");
			
			return info;
		}

		/// <summary>
		/// 将实体对象的属性值转化为Hashtable对应的键值
		/// </summary>
		/// <param name="obj">有效的实体对象</param>
		/// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(DaClientInfoInfo obj)
		{
		    DaClientInfoInfo info = obj as DaClientInfoInfo;
			Hashtable hash = new Hashtable(); 
			
 			hash.Add("ZHH", info.Zhh);
 			hash.Add("QY", info.Qy);
 			hash.Add("SH", info.Sh);
 			hash.Add("CBBH", info.Cbbh);
 			hash.Add("BXH", info.Bxh);
 			hash.Add("HMCM", info.Hmcm);
 			hash.Add("DZCM", info.Dzcm);
 			hash.Add("DHHM", info.Dhhm);
 			hash.Add("MOBILE", info.Mobile);
 			hash.Add("LXR", info.Lxr);
 			hash.Add("HTH", info.Hth);
 			hash.Add("HM", info.Hm);
 			hash.Add("DW", info.Dw);
 			hash.Add("DWDH", info.Dwdh);
 			hash.Add("DZ", info.Dz);
 			hash.Add("YHXZ", info.Yhxz);
 			hash.Add("SFFS", info.Sffs);
 			hash.Add("SFZH", info.Sfzh);
 			hash.Add("YSXZ", info.Ysxz);
 			hash.Add("JHRQ", info.Jhrq);
 			hash.Add("XHRQ", info.Xhrq);
 			hash.Add("YJSL", info.Yjsl);
 			hash.Add("YHZT", info.Yhzt);
 			hash.Add("GH", info.Gh);
 			hash.Add("RKS", info.Rks);
 			hash.Add("FPHM", info.Fphm);
 			hash.Add("FPDZ", info.Fpdz);
 			hash.Add("FKDWQC", info.Fkdwqc);
 			hash.Add("FKDWZH", info.Fkdwzh);
 			hash.Add("FKKHYH", info.Fkkhyh);
 			hash.Add("YHLX", info.Yhlx);
 			hash.Add("CBY", info.Cby);
 				
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
            //dict.Add("ID", "编号");
             dict.Add("Zhh", "");
             dict.Add("Qy", "");
             dict.Add("Sh", "");
             dict.Add("Cbbh", "");
             dict.Add("Bxh", "");
             dict.Add("Hmcm", "");
             dict.Add("Dzcm", "");
             dict.Add("Dhhm", "");
             dict.Add("Mobile", "");
             dict.Add("Lxr", "");
             dict.Add("Hth", "");
             dict.Add("Hm", "");
             dict.Add("Dw", "");
             dict.Add("Dwdh", "");
             dict.Add("Dz", "");
             dict.Add("Yhxz", "");
             dict.Add("Sffs", "");
             dict.Add("Sfzh", "");
             dict.Add("Ysxz", "");
             dict.Add("Jhrq", "");
             dict.Add("Xhrq", "");
             dict.Add("Yjsl", "");
             dict.Add("Yhzt", "");
             dict.Add("Gh", "");
             dict.Add("Rks", "");
             dict.Add("Fphm", "");
             dict.Add("Fpdz", "");
             dict.Add("Fkdwqc", "");
             dict.Add("Fkdwzh", "");
             dict.Add("Fkkhyh", "");
             dict.Add("Yhlx", "");
             dict.Add("Cby", "");
             #endregion

            return dict;
        }

    }
}