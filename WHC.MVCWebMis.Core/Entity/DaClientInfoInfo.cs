using System;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using WHC.Framework.ControlUtil;

namespace WHC.MVCWebMis.Entity
{
    /// <summary>
    /// DaClientInfoInfo
    /// </summary>
    [DataContract]
    public class DaClientInfoInfo : BaseEntity
    { 
        /// <summary>
        /// 默认构造函数（需要初始化属性的在此处理）
        /// </summary>
	    public DaClientInfoInfo()
		{
            this.ID= 0;
             this.Zhh= 0;
                this.Bxh= 0;
                             this.Yjsl= 0;
               this.Rks= 0;
         
		}

        #region Property Members
        
		[DataMember]
        public virtual int ID { get; set; }

		[DataMember]
        public virtual int Zhh { get; set; }

		[DataMember]
        public virtual string Qy { get; set; }

		[DataMember]
        public virtual string Sh { get; set; }

		[DataMember]
        public virtual string Cbbh { get; set; }

		[DataMember]
        public virtual int Bxh { get; set; }

		[DataMember]
        public virtual string Hmcm { get; set; }

		[DataMember]
        public virtual string Dzcm { get; set; }

		[DataMember]
        public virtual string Dhhm { get; set; }

		[DataMember]
        public virtual string Mobile { get; set; }

		[DataMember]
        public virtual string Lxr { get; set; }

		[DataMember]
        public virtual string Hth { get; set; }

		[DataMember]
        public virtual string Hm { get; set; }

		[DataMember]
        public virtual string Dw { get; set; }

		[DataMember]
        public virtual string Dwdh { get; set; }

		[DataMember]
        public virtual string Dz { get; set; }

		[DataMember]
        public virtual string Yhxz { get; set; }

		[DataMember]
        public virtual string Sffs { get; set; }

		[DataMember]
        public virtual string Sfzh { get; set; }

		[DataMember]
        public virtual string Ysxz { get; set; }

		[DataMember]
        public virtual DateTime Jhrq { get; set; }

		[DataMember]
        public virtual DateTime Xhrq { get; set; }

		[DataMember]
        public virtual int Yjsl { get; set; }

		[DataMember]
        public virtual string Yhzt { get; set; }

		[DataMember]
        public virtual string Gh { get; set; }

		[DataMember]
        public virtual int Rks { get; set; }

		[DataMember]
        public virtual string Fphm { get; set; }

		[DataMember]
        public virtual string Fpdz { get; set; }

		[DataMember]
        public virtual string Fkdwqc { get; set; }

		[DataMember]
        public virtual string Fkdwzh { get; set; }

		[DataMember]
        public virtual string Fkkhyh { get; set; }

		[DataMember]
        public virtual string Yhlx { get; set; }

		[DataMember]
        public virtual string Cby { get; set; }


        #endregion

    }
}