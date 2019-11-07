using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Aspose.Cells;

using Newtonsoft.Json;
using WHC.Pager.Entity;
using WHC.Framework.Commons;
using WHC.Framework.ControlUtil;
using WHC.MVCWebMis.BLL;
using WHC.MVCWebMis.Entity;

namespace WHC.MVCWebMis.Controllers
{
    public class DaClientInfoController : BusinessController<DaClientInfo, DaClientInfoInfo>
    {
        public DaClientInfoController() : base()
        {
        }

        #region 导入Excel数据操作
 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 		 
	    //导入或导出的字段列表   
        string columnString = "Zhh,Qy,Sh,Cbbh,Bxh,Hmcm,Dzcm,Dhhm,Mobile,Lxr,Hth,Hm,Dw,Dwdh,Dz,Yhxz,Sffs,Sfzh,Ysxz,Jhrq,Xhrq,Yjsl,Yhzt,Gh,Rks,Fphm,Fpdz,Fkdwqc,Fkdwzh,Fkkhyh,Yhlx,Cby";

        /// <summary>
        /// 检查Excel文件的字段是否包含了必须的字段
        /// </summary>
        /// <param name="guid">附件的GUID</param>
        /// <returns></returns>
        public ActionResult CheckExcelColumns(string guid)
        {
            CommonResult result = new CommonResult();

            try
            {
                DataTable dt = ConvertExcelFileToTable(guid);
                if (dt != null)
                {
                    //检查列表是否包含必须的字段
                    result.Success = DataTableHelper.ContainAllColumns(dt, columnString);
                }
            }
            catch (Exception ex)
            {
                LogTextHelper.Error(ex);
                result.ErrorMessage = ex.Message;
            }

            return ToJsonContent(result);
        }

        /// <summary>
        /// 获取服务器上的Excel文件，并把它转换为实体列表返回给客户端
        /// </summary>
        /// <param name="guid">附件的GUID</param>
        /// <returns></returns>
        public ActionResult GetExcelData(string guid)
        {
            if (string.IsNullOrEmpty(guid))
            {
                return null;
            }

            List<DaClientInfoInfo> list = new List<DaClientInfoInfo>();

            DataTable table = ConvertExcelFileToTable(guid);
            if (table != null)
            {
                #region 数据转换
                int i = 1;
                foreach (DataRow dr in table.Rows)
                {
                    bool converted = false;
                    DateTime dtDefault = Convert.ToDateTime("1900-01-01");
                    DateTime dt;                    
                    DaClientInfoInfo info = new DaClientInfoInfo();
                    
                     info.Zhh = dr["Zhh"].ToString().ToInt32();
                      info.Qy = dr["Qy"].ToString();
                      info.Sh = dr["Sh"].ToString();
                      info.Cbbh = dr["Cbbh"].ToString();
                      info.Bxh = dr["Bxh"].ToString().ToInt32();
                      info.Hmcm = dr["Hmcm"].ToString();
                      info.Dzcm = dr["Dzcm"].ToString();
                      info.Dhhm = dr["Dhhm"].ToString();
                      info.Mobile = dr["Mobile"].ToString();
                      info.Lxr = dr["Lxr"].ToString();
                      info.Hth = dr["Hth"].ToString();
                      info.Hm = dr["Hm"].ToString();
                      info.Dw = dr["Dw"].ToString();
                      info.Dwdh = dr["Dwdh"].ToString();
                      info.Dz = dr["Dz"].ToString();
                      info.Yhxz = dr["Yhxz"].ToString();
                      info.Sffs = dr["Sffs"].ToString();
                      info.Sfzh = dr["Sfzh"].ToString();
                      info.Ysxz = dr["Ysxz"].ToString();
                      converted = DateTime.TryParse(dr["Jhrq"].ToString(), out dt);
                    if (converted && dt > dtDefault)
                    {
                         info.Jhrq = dt;
                    }
                      converted = DateTime.TryParse(dr["Xhrq"].ToString(), out dt);
                    if (converted && dt > dtDefault)
                    {
                         info.Xhrq = dt;
                    }
                      info.Yjsl = dr["Yjsl"].ToString().ToInt32();
                      info.Yhzt = dr["Yhzt"].ToString();
                      info.Gh = dr["Gh"].ToString();
                      info.Rks = dr["Rks"].ToString().ToInt32();
                      info.Fphm = dr["Fphm"].ToString();
                      info.Fpdz = dr["Fpdz"].ToString();
                      info.Fkdwqc = dr["Fkdwqc"].ToString();
                      info.Fkdwzh = dr["Fkdwzh"].ToString();
                      info.Fkkhyh = dr["Fkkhyh"].ToString();
                      info.Yhlx = dr["Yhlx"].ToString();
                      info.Cby = dr["Cby"].ToString();
  
                    //info.Creator = CurrentUser.ID.ToString();
                    //info.CreateTime = DateTime.Now;
                    //info.Editor = CurrentUser.ID.ToString();
                    //info.EditTime = DateTime.Now;

                    list.Add(info);
                }
                #endregion
            }

            var result = new { total = list.Count, rows = list };
            return ToJsonContentDate(result);
        }

        /// <summary>
        /// 保存客户端上传的相关数据列表
        /// </summary>
        /// <param name="list">数据列表</param>
        /// <returns></returns>
        public ActionResult SaveExcelData(List<DaClientInfoInfo> list)
        {
            CommonResult result = new CommonResult();
            if (list != null && list.Count > 0)
            {
                #region 采用事务进行数据提交

                DbTransaction trans = BLLFactory<DaClientInfo>.Instance.CreateTransaction();
                if (trans != null)
                {
                    try
                    {
                        //int seq = 1;
                        foreach (DaClientInfoInfo detail in list)
                        {
                            //detail.Seq = seq++;//增加1
                            //detail.CreateTime = DateTime.Now;
                            //detail.Creator = CurrentUser.ID.ToString();
                            //detail.Editor = CurrentUser.ID.ToString();
                            //detail.EditTime = DateTime.Now;

                            BLLFactory<DaClientInfo>.Instance.Insert(detail, trans);
                        }
                        trans.Commit();
                        result.Success = true;
                    }
                    catch (Exception ex)
                    {
                        LogTextHelper.Error(ex);
                        result.ErrorMessage = ex.Message;
                        trans.Rollback();
                    }
                }
                #endregion
            }
            else
            {
                result.ErrorMessage = "导入信息不能为空";
            }

            return ToJsonContent(result);
        } 
        
        /// <summary>
        /// 根据查询条件导出列表数据
        /// </summary>
        /// <returns></returns>
        public ActionResult Export()
        {
            #region 根据参数获取List列表
            string where = GetPagerCondition();
            string CustomedCondition = Request["CustomedCondition"] ?? "";
            List<DaClientInfoInfo> list = new List<DaClientInfoInfo>();

            if (!string.IsNullOrWhiteSpace(CustomedCondition))
            {
            	//如果为自定义的json参数列表，那么可以使用字典反序列化获取参数，然后处理
                //Dictionary<string, string> dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(CustomedCondition);
                
                //如果是条件的自定义，可以使用Find查找
                list = baseBLL.Find(CustomedCondition);
            }
            else
            {
                list = baseBLL.Find(where);
            }
            
            #endregion

            #region 把列表转换为DataTable
            DataTable datatable = DataTableHelper.CreateTable("序号|int," + columnString);
            DataRow dr;
            int j = 1;
            for (int i = 0; i < list.Count; i++)
            {
                dr = datatable.NewRow();
                dr["序号"] = j++;                
                 dr["Zhh"] = list[i].Zhh;
                 dr["Qy"] = list[i].Qy;
                 dr["Sh"] = list[i].Sh;
                 dr["Cbbh"] = list[i].Cbbh;
                 dr["Bxh"] = list[i].Bxh;
                 dr["Hmcm"] = list[i].Hmcm;
                 dr["Dzcm"] = list[i].Dzcm;
                 dr["Dhhm"] = list[i].Dhhm;
                 dr["Mobile"] = list[i].Mobile;
                 dr["Lxr"] = list[i].Lxr;
                 dr["Hth"] = list[i].Hth;
                 dr["Hm"] = list[i].Hm;
                 dr["Dw"] = list[i].Dw;
                 dr["Dwdh"] = list[i].Dwdh;
                 dr["Dz"] = list[i].Dz;
                 dr["Yhxz"] = list[i].Yhxz;
                 dr["Sffs"] = list[i].Sffs;
                 dr["Sfzh"] = list[i].Sfzh;
                 dr["Ysxz"] = list[i].Ysxz;
                 dr["Jhrq"] = list[i].Jhrq;
                 dr["Xhrq"] = list[i].Xhrq;
                 dr["Yjsl"] = list[i].Yjsl;
                 dr["Yhzt"] = list[i].Yhzt;
                 dr["Gh"] = list[i].Gh;
                 dr["Rks"] = list[i].Rks;
                 dr["Fphm"] = list[i].Fphm;
                 dr["Fpdz"] = list[i].Fpdz;
                 dr["Fkdwqc"] = list[i].Fkdwqc;
                 dr["Fkdwzh"] = list[i].Fkdwzh;
                 dr["Fkkhyh"] = list[i].Fkkhyh;
                 dr["Yhlx"] = list[i].Yhlx;
                 dr["Cby"] = list[i].Cby;
                 //如果为外键，可以在这里进行转义，如下例子
                //dr["客户名称"] = BLLFactory<Customer>.Instance.GetCustomerName(list[i].Customer_ID);//转义为客户名称

                datatable.Rows.Add(dr);
            } 
            #endregion

            #region 把DataTable转换为Excel并输出
            Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();
            //为单元格添加样式    
            Aspose.Cells.Style style = workbook.Styles[workbook.Styles.Add()];
            //设置居中
            style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;
            //设置背景颜色
            style.ForegroundColor = System.Drawing.Color.FromArgb(153, 204, 0);
            style.Pattern = BackgroundType.Solid;
            style.Font.IsBold = true;

            int rowIndex = 0;
            for (int i = 0; i < datatable.Columns.Count; i++)
            {
                DataColumn col = datatable.Columns[i];
                string columnName = col.Caption ?? col.ColumnName;
                workbook.Worksheets[0].Cells[rowIndex, i].PutValue(columnName);
                workbook.Worksheets[0].Cells[rowIndex, i].SetStyle(style);
            }
            rowIndex++;

            foreach (DataRow row in datatable.Rows)
            {
                for (int i = 0; i < datatable.Columns.Count; i++)
                {
                    workbook.Worksheets[0].Cells[rowIndex, i].PutValue(row[i].ToString());
                }
                rowIndex++;
            }

            for (int k = 0; k < datatable.Columns.Count; k++)
            {
                workbook.Worksheets[0].AutoFitColumn(k, 0, 150);
            }
            workbook.Worksheets[0].FreezePanes(1, 0, 1, datatable.Columns.Count);

            //根据用户创建目录，确保生成的文件不会产生冲突
            string filePath = string.Format("/GenerateFiles/{0}/DaClientInfo.xls", CurrentUser.Name);
            string realPath = Server.MapPath(filePath);
            string parentPath = Directory.GetParent(realPath).FullName;
            DirectoryUtil.AssertDirExist(parentPath);

            workbook.Save(realPath, Aspose.Cells.SaveFormat.Excel97To2003); 

            #endregion

            //返回生成后的文件路径，让客户端根据地址下载
            return Content(filePath);
        }
        
        #endregion
		
        #region 写入数据前修改部分属性
        protected override void OnBeforeInsert(DaClientInfoInfo info)
        {
            //子类对参数对象进行修改
            //info.CreateTime = DateTime.Now;
            //info.Creator = CurrentUser.ID.ToString();
            //info.Company_ID = CurrentUser.Company_ID;
            //info.Dept_ID = CurrentUser.Dept_ID;
        }

        protected override void OnBeforeUpdate(DaClientInfoInfo info)
        {
            //子类对参数对象进行修改
            //info.Editor = CurrentUser.ID.ToString();
            //info.EditTime = DateTime.Now;
        } 
        #endregion

        public override ActionResult FindWithPager()
        {
            //检查用户是否有权限，否则抛出MyDenyAccessException异常
            base.CheckAuthorized(AuthorizeKey.ListKey);

            string where = GetPagerCondition();
            PagerInfo pagerInfo = GetPagerInfo();
            List<DaClientInfoInfo> list = baseBLL.FindWithPager(where, pagerInfo);

			//如果需要修改字段显示，则参考下面代码处理
            //foreach(DaClientInfoInfo info in list)
            //{
            //    info.PID = BLLFactory<DaClientInfo>.Instance.GetFieldValue(info.PID, "Name");
            //    if (!string.IsNullOrEmpty(info.Creator))
            //    {
            //        info.Creator = BLLFactory<User>.Instance.GetFullNameByID(info.Creator.ToInt32());
            //    }
            //}

            //Json格式的要求{total:22,rows:{}}
            //构造成Json的格式传递
            var result = new { total = pagerInfo.RecordCount, rows = list };
            return ToJsonContentDate(result);
        }

    }
}
