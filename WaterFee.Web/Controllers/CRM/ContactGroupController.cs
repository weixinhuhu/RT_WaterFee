using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WHC.CRM.BLL;
using WHC.CRM.Entity;
using WHC.Framework.ControlUtil;

namespace WHC.MVCWebMis.Controllers
{
    public class ContactGroupController : BusinessController<ContactGroup, ContactGroupInfo>
    {
        public ContactGroupController() : base()
        {
        }

        #region 写入数据前修改部分属性
        protected override void OnBeforeInsert(ContactGroupInfo info)
        {
            //留给子类对参数对象进行修改
            info.CreateTime = DateTime.Now;
            info.Creator = CurrentUser.ID.ToString();
            info.Company_ID = CurrentUser.Company_ID;
            info.Dept_ID = CurrentUser.Dept_ID;
        }

        protected override void OnBeforeUpdate(ContactGroupInfo info)
        {
            //留给子类对参数对象进行修改
            info.Editor = CurrentUser.ID.ToString();
            info.EditTime = DateTime.Now;
        }
        #endregion

        /// <summary>
        /// 获取联系人分组树Json字符串
        /// </summary>
        /// <returns></returns>
        public ActionResult GetGroupTreeJson(string userId)
        {
            //添加一个未分类和全部客户的组别
            List<EasyTreeData> treeList = new List<EasyTreeData>();
            EasyTreeData pNode = new EasyTreeData("", "所有联系人");
            treeList.Insert(0, pNode);
            treeList.Add(new EasyTreeData("", "未分组联系人"));

            List<ContactGroupNodeInfo> groupList = BLLFactory<ContactGroup>.Instance.GetTree(userId);
            AddContactGroupTree(groupList, pNode);

            return ToJsonContent(treeList);
        }

        /// <summary>
        /// 初始化并绑定客户个人分组信息
        /// </summary>
        public ActionResult GetMyContactGroup(string contactId, string userId)
        {
            List<ContactGroupInfo> myGroupList = BLLFactory<ContactGroup>.Instance.GetByContact(contactId);
            List<string> groupIdList = new List<string>();
            foreach (ContactGroupInfo info in myGroupList)
            {
                groupIdList.Add(info.ID);
            }

            List<ContactGroupNodeInfo> groupList = BLLFactory<ContactGroup>.Instance.GetTree(userId);

            List<EasyTreeData> treeList = new List<EasyTreeData>();
            foreach (ContactGroupNodeInfo nodeInfo in groupList)
            {
                bool check = groupIdList.Contains(nodeInfo.ID);
                EasyTreeData treeData = new EasyTreeData(nodeInfo.ID, nodeInfo.Name);
                treeData.Checked = check;

                treeList.Add(treeData);
            }

            return ToJsonContent(treeList);
        }

        /// <summary>
        /// 获取客户分组并绑定
        /// </summary>
        private void AddContactGroupTree(List<ContactGroupNodeInfo> nodeList, EasyTreeData treeNode)
        {
            foreach (ContactGroupNodeInfo nodeInfo in nodeList)
            {
                EasyTreeData subNode = new EasyTreeData(nodeInfo.ID, nodeInfo.Name, "icon-view");
                treeNode.children.Add(subNode);

                AddContactGroupTree(nodeInfo.Children, subNode);
            }
        }
    }
}
