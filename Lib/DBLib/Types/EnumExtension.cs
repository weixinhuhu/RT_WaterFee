using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace System
{
    public static class EnumExtension
    {

        public static int GetEnumValue(this Enum @this, string enumVar = null)
        {
            if (string.IsNullOrEmpty(enumVar))
                return Convert.ToInt32(@this);
            else
                return (int)Enum.Parse(@this.GetType(), enumVar);
        }

        public static int GetEnumValue<TEnum>(string enumVar)
        {
            return Convert.ToInt32((TEnum)Enum.Parse(typeof(TEnum), enumVar));
        }


        /// <summary>
        /// 获取enum的描述
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum @this)
        {
            //string enumItem = Enum.GetName(enumType, Convert.ToInt32(value));
            //if (enumItem == null)
            //{
            //    return string.Empty;
            //}

            object[] objs = @this.GetType().GetField(@this.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (objs == null || objs.Length == 0)
            {
                return string.Empty;
            }
            else
            {
                DescriptionAttribute attr = objs[0] as DescriptionAttribute;

                return attr.Description;
            }
        }

        /// <summary>
        /// 获取枚举项的描述
        /// </summary>
        /// <param name="enumItem">具体枚举类型</param>
        /// <returns></returns>
        public static string GetEnumDescription<TEnum>(object value)
        {
            Type enumType = typeof(TEnum);

            if (!enumType.IsEnum)
            {
                throw new ArgumentException("不是枚举类型");
            }

            string enumItem = Enum.GetName(enumType, Convert.ToInt32(value));
            if (enumItem == null)
            {
                return string.Empty;
            }

            object[] objs = enumType.GetField(enumItem).GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (objs == null || objs.Length == 0)
            {
                return string.Empty;
            }
            else
            {
                DescriptionAttribute attr = objs[0] as DescriptionAttribute;

                return attr.Description;
            }
        }

        #region Get enum Description http://www.cnblogs.com/anding/p/5129178.html
        ///// <summary>
        ///// 获取枚举的描述信息(Descripion)。
        ///// 支持位域，如果是位域组合值，多个按分隔符组合。
        ///// </summary>
        //public static string GetDescription(this Enum @this)
        //{
        //    return _ConcurrentDictionary.GetOrAdd(@this, (key) =>
        //    {
        //        var type = key.GetType();
        //        var field = type.GetField(key.ToString());
        //        //如果field为null则应该是组合位域值，
        //        return field == null ? key.GetDescriptions() : GetDescription(field);
        //    });
        //}

        ///// <summary>
        ///// 获取位域枚举的描述，多个按分隔符组合
        ///// </summary>
        //public static string GetDescriptions(this Enum @this, string separator = ",")
        //{
        //    var names = @this.ToString().Split(',');
        //    string[] res = new string[names.Length];
        //    var type = @this.GetType();
        //    for (int i = 0; i < names.Length; i++)
        //    {
        //        var field = type.GetField(names[i].Trim());
        //        if (field == null) continue;
        //        res[i] = GetDescription(field);
        //    }
        //    return string.Join(separator, res);
        //}

        //private static string GetDescription(FieldInfo field)
        //{
        //    var att = System.Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute), false);
        //    return att == null ? field.Name : ((DescriptionAttribute)att).Description;
        //}

        ///****************** test methods ******************/

        //public static string GetDescriptionOriginal(this Enum @this)
        //{
        //    var name = @this.ToString();
        //    var field = @this.GetType().GetField(name);
        //    if (field == null) return name;
        //    var att = System.Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute), false);
        //    return att == null ? field.Name : ((DescriptionAttribute)att).Description;
        //}

        //private static Dictionary<Enum, string> _LockDictionary = new Dictionary<Enum, string>();
        //public static string GetDescriptionByDictionaryWithLock(this Enum @this)
        //{
        //    if (_LockDictionary.ContainsKey(@this)) return _LockDictionary[@this];
        //    Monitor.Enter(_obj);
        //    if (!_LockDictionary.ContainsKey(@this))
        //    {
        //        var value = @this.GetDescriptionOriginal();
        //        _LockDictionary.Add(@this, value);
        //    }
        //    Monitor.Exit(_obj);
        //    return _LockDictionary[@this];
        //}

        //private static Dictionary<Enum, string> _ExceptionDictionary = new Dictionary<Enum, string>();
        //public static string GetDescriptionByDictionaryWithException(this Enum @this)
        //{
        //    try
        //    {
        //        return _ExceptionDictionary[@this];
        //    }
        //    catch (KeyNotFoundException)
        //    {
        //        Monitor.Enter(_obj);
        //        if (!_ExceptionDictionary.ContainsKey(@this))
        //        {
        //            var value = @this.GetDescriptionOriginal();
        //            _ExceptionDictionary.Add(@this, value);
        //        }
        //        Monitor.Exit(_obj);
        //        return _ExceptionDictionary[@this];
        //    }
        //}

        //public static object _obj = new object();
        //private static System.Collections.Concurrent.ConcurrentDictionary<Enum, string> _ConcurrentDictionary
        //           = new System.Collections.Concurrent.ConcurrentDictionary<Enum, string>();
        //public static string GetDescriptionByConcurrentDictionary(this Enum @this)
        //{
        //    return _ConcurrentDictionary.GetOrAdd(@this, (key) =>
        //    {
        //        var type = key.GetType();
        //        var field = type.GetField(key.ToString());
        //        return field == null ? key.ToString() : GetDescription(field);
        //    });
        //}
        #endregion
    }


    //  DescriptionAttribute 的定义:  
    public class DescriptionAttribute : Attribute
    {
        public DescriptionAttribute(string desc)
        {
            this._description = desc;
        }
        private string _description;

        public string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        //public DescriptionAttribute(string desc);

        //public string Description { get; set; }
    }

    public class DescriptionAttributeDemo
    {
        // Filter枚举的描述: 
        public enum Filter
        {
            /// <summary>
            /// 全部
            /// </summary>
            [Description("全部")]
            All = 1,

            /// <summary>
            /// 已分配
            /// </summary>
            [Description("已分配")]
            Assigned = 2,

            /// <summary>
            /// 待分配
            /// </summary>
            [Description("待分配")]
            Assigning = 3,

            /// <summary>
            /// 返回调整
            /// </summary>
            [Description("返回调整")]
            Adjusting = 4,

            /// <summary>
            /// 已拒绝
            /// </summary>
            [Description("已拒绝")]
            Reject = 5,

            /// <summary>
            /// 公共
            /// </summary>
            [Description("公共")]
            Public = 6,
            /// <summary>
            /// Sales把发回调整的客户编辑后不先提交只保存信息
            /// </summary>
            [Description("草稿")]
            Draft = 7,
            /// <summary>
            /// 排队
            /// </summary>
            [Description("排队")]
            InQueue = 8,
            /// <summary>
            /// 发放给大区经理审批
            /// </summary>
            [Description("审批转移")]
            ToManagerCheck = 9,

            /// <summary>
            /// 不返回任何值的条件
            /// </summary>
            None = -1,

        }

        protected void Page_Load(object sender, EventArgs e)
        {   // 调用: 
            //string desc = GetEnumDescription<Filter>(Filter.Public); //得到Filter(enum)中,为Public 所定义的描述信息.
            //string desc = GetEnumDescription<MyColorEnum>(MyColorEnum.red);
            ////desc = GetEnumDescription<MyColorEnum>(1);
            //Response.Write(desc); 
        }

        public enum MyColorEnum
        {
            [DescriptionAttribute("红色")]
            red,
            [DescriptionAttribute("蓝色")]
            blue,
            [DescriptionAttribute("绿色")]
            green,
            [DescriptionAttribute("白色")]
            white
        }
    }
}
