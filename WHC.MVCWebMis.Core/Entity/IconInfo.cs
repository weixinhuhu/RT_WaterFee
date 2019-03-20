using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using WHC.Framework.ControlUtil;

namespace WHC.MVCWebMis.Entity
{
    /// <summary>
    /// 系统图表库
    /// </summary>
    [DataContract]
    public class IconInfo : BaseEntity
    {
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
        public IconInfo()
        {
            this.ID = System.Guid.NewGuid().ToString();
            this.IconSize = 16;
            this.CreateTime = DateTime.Now;
        }

        #region Property Members

        [DataMember]
        public virtual string ID { get; set; }

        /// <summary>
        /// 样式名称
        /// </summary>
        [DataMember]
        public virtual string IconCls { get; set; }

        /// <summary>
        /// URL地址
        /// </summary>
        [DataMember]
        public virtual string IconUrl { get; set; }

        /// <summary>
        /// 尺寸
        /// </summary>
        [DataMember]
        public virtual int IconSize { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember]
        public virtual DateTime CreateTime { get; set; }


        #endregion

    }
}