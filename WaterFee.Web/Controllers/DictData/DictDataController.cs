using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Web.Mvc;
using WHC.Dictionary.BLL;
using WHC.Dictionary.Entity;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;

namespace WHC.MVCWebMis.Controllers
{
    /// <summary>
    /// 数据字典控制器
    /// </summary>
    public class DictDataController : BusinessController<DictData, DictDataInfo>
    {
        public DictDataController() : base()
        {
        }

        #region 写入数据前修改部分属性
        protected override void OnBeforeInsert(DictDataInfo info)
        {
            //留给子类对参数对象进行修改
            info.Editor = CurrentUser.ID.ToString();
            info.LastUpdated = DateTime.Now;
        }

        protected override void OnBeforeUpdate(DictDataInfo info)
        {
            //留给子类对参数对象进行修改
            info.Editor = CurrentUser.ID.ToString();
            info.LastUpdated = DateTime.Now;
        }
        #endregion

        /// <summary>
        /// 批量添加字典数据操作
        /// </summary>
        /// <param name="DictType_ID">字典类型</param>
        /// <param name="Seq">排序开始或前缀</param>
        /// <param name="Data">批量插入的内容</param>
        /// <param name="SplitType">分开类型，分隔符分开（Split）还是行分割（Line）</param>
        /// <param name="Remark">备注</param>
        /// <returns></returns>
        public ActionResult BatchInsert(string DictType_ID, string Seq, string Data, string SplitType, string Remark)
        {
            CommonResult result = new CommonResult();
            if (string.IsNullOrEmpty(DictType_ID) || string.IsNullOrEmpty(Data))
            {
                result.ErrorMessage = "DictType_ID或Data参数为空";
                return ToJsonContent(result);
            }

            string[] arrayItems = Data.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            int intSeq = -1;
            int seqLength = 3;
            string strSeq = Seq;
            if (int.TryParse(strSeq, out intSeq))
            {
                seqLength = strSeq.Length;
            }

            if (arrayItems != null && arrayItems.Length > 0)
            {
                DbTransaction trans = BLLFactory<DictData>.Instance.CreateTransaction();
                if (trans != null)
                {
                    try
                    {
                        #region MyRegion
                        foreach (string strItem in arrayItems)
                        {
                            if (SplitType.Equals("split", StringComparison.OrdinalIgnoreCase))
                            {
                                if (!string.IsNullOrWhiteSpace(strItem))
                                {
                                    string[] dataItems = strItem.Split(new char[] { ',', '，', ';', '；', '/', '、' });
                                    foreach (string dictData in dataItems)
                                    {
                                        #region 保存数据
                                        string seq = "";
                                        if (intSeq > 0)
                                        {
                                            seq = (intSeq++).ToString().PadLeft(seqLength, '0');
                                        }
                                        else
                                        {
                                            seq = string.Format("{0}{1}", strSeq, intSeq++);
                                        }

                                        InsertDictData(DictType_ID, dictData, seq, Remark, trans);
                                        #endregion
                                    }
                                }
                            }
                            else
                            {
                                #region 保存数据
                                if (!string.IsNullOrWhiteSpace(strItem))
                                {
                                    string seq = "";
                                    if (intSeq > 0)
                                    {
                                        seq = (intSeq++).ToString().PadLeft(seqLength, '0');
                                    }
                                    else
                                    {
                                        seq = string.Format("{0}{1}", strSeq, intSeq++);
                                    }

                                    InsertDictData(DictType_ID, strItem, seq, Remark, trans);
                                }
                                #endregion
                            }
                        }
                        #endregion

                        trans.Commit();
                        result.Success = true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        LogTextHelper.Error(ex);
                        result.ErrorMessage = ex.Message;
                    }
                }
            }

            return ToJsonContent(result);
        }

        /// <summary>
        /// 使用事务参数，插入数据，最后统一提交事务处理
        /// </summary>
        /// <param name="dictData">字典数据</param>
        /// <param name="seq">排序</param>
        /// <param name="trans">事务对象</param>
        private void InsertDictData(string dictTypeId, string dictData, string seq, string note, DbTransaction trans)
        {
            if (!string.IsNullOrWhiteSpace(dictData))
            {
                DictDataInfo info = new DictDataInfo();
                info.Editor = CurrentUser.ID.ToString();
                info.LastUpdated = DateTime.Now;

                info.DictType_ID = dictTypeId;
                info.Name = dictData.Trim();
                info.Value = dictData.Trim();
                info.Remark = note;
                info.Seq = seq;

                bool succeed = BLLFactory<DictData>.Instance.Insert(info, trans);
            }
        }

        /// <summary>
        /// 根据字典类型获取对应的字典数据，方便UI控件的绑定
        /// </summary>
        /// <param name="dictTypeName">字典类型名称</param>
        /// <returns></returns>
        public ActionResult GetDictJson(string dictTypeName)
        {
            List<CListItem> treeList = new List<CListItem>();
            CListItem pNode = new CListItem("", "");
            treeList.Insert(0, pNode);

            Dictionary<string, string> dict = BLLFactory<DictData>.Instance.GetDictByDictType(dictTypeName);
            foreach (string key in dict.Keys)
            {
                treeList.Add(new CListItem(key, dict[key]));
            }
            return ToJsonContent(treeList);
        }

        /// <summary>
        /// 根据字典类型获取对应的字典数据，方便UI控件的绑定
        /// </summary>
        /// <param name="dictTypeName">字典类型名称</param>
        /// <returns></returns>
        public ActionResult GetDictTreeJson(string dictTypeName)
        {
            List<EasyTreeData> treeList = new List<EasyTreeData>();
            EasyTreeData pNode = new EasyTreeData("", "请选择");
            treeList.Insert(0, pNode);

            Dictionary<string, string> dict = BLLFactory<DictData>.Instance.GetDictByDictType(dictTypeName);
            foreach (string key in dict.Keys)
            {
                treeList.Add(new EasyTreeData(key, dict[key]));
            }
            return ToJsonContent(treeList);
        }
    }
}
