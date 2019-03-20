using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using WHC.WaterFeeWeb.Core.IDAL;
using WHC.Framework.Commons;

namespace WHC.WaterFeeWeb.Core.DALMySql
{
    public class MessageTemplate : WHC.Framework.ControlUtil.BaseDALMySql<Entity.MessageTemplate>, IMessageTemplate
    {
        #region 对象实例及构造函数

        public static MessageTemplate Instance
        {
            get
            {
                return new MessageTemplate();
            }
        }
        public MessageTemplate() : base("MessageTemplate", "ID")
        {
        }

        #endregion

        /// <summary>
        /// 将DataReader的属性值转化为实体类的属性值，返回实体类
        /// </summary>
        /// <param name="dr">有效的DataReader对象</param>
        /// <returns>实体类对象</returns>
        protected override Entity.MessageTemplate DataReaderToEntity(IDataReader dataReader)
        {
            Entity.MessageTemplate info = new Entity.MessageTemplate();
            SmartDataReader reader = new SmartDataReader(dataReader);

            info.ID = reader.GetInt32("ID");
            info.TempType = reader.GetString("TempType");
            info.TempName = reader.GetString("TempName");
            info.TempID = reader.GetString("TempID");
            info.TempSQL = reader.GetString("TempSQL");
            info.TempContent = reader.GetString("TempContent");
            info.Status = reader.GetInt32("Status");
            info.DtCreate = reader.GetDateTime("DtCreate");

            return info;
        }

        /// <summary>
        /// 将实体对象的属性值转化为Hashtable对应的键值
        /// </summary>
        /// <param name="obj">有效的实体对象</param>
        /// <returns>包含键值映射的Hashtable</returns>
        protected override Hashtable GetHashByEntity(Entity.MessageTemplate obj)
        {
            Entity.MessageTemplate info = obj as Entity.MessageTemplate;
            Hashtable hash = new Hashtable();

            hash.Add("ID", info.ID);
            hash.Add("TempType", info.TempType);
            hash.Add("TempName", info.TempName);
            hash.Add("TempID", info.TempID);
            hash.Add("TempSQL", info.TempSQL);
            hash.Add("TempContent", info.TempContent);
            hash.Add("Status", info.Status);
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
            dict.Add("ID", string.Empty);
            dict.Add("TempType", string.Empty);
            dict.Add("TempName", string.Empty);
            dict.Add("TempID", string.Empty);
            dict.Add("TempSQL", string.Empty);
            dict.Add("TempContent", string.Empty);
            dict.Add("Status", string.Empty);
            dict.Add("DtCreate", string.Empty);
            #endregion

            return dict;
        }

    }
}
