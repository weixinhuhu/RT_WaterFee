using System.Collections;
using WHC.Security.BLL;
using WHC.Security.Common;
using WHC.Security.Entity;

namespace WHC.Security.Adapter
{
	public class SecurityManager : ISecurityManager
	{
		private ArrayList m_AdminNames = new ArrayList();
		private string m_SessionID = "";

		public SecurityManager(string sessionID)
		{
			this.m_SessionID = sessionID;
			RoleInfo roleByName = new Role().GetRoleByName(RoleInfo.SuperAdminName);
			if (roleByName == null)
			{
				throw new MyException("管理员被非法删除，请重新安装系统！");
			}
			User user = new User();
			foreach (SimpleUserInfo info2 in user.GetSimpleUsersByRole(roleByName.ID))
			{
				this.m_AdminNames.Add(info2.Name);
			}
		}

		public int AddFunction(string identity, FunctionInfo functionInfo)
		{
			this.VerifyUser(identity);
			Function function = new Function();
			return function.Insert2(functionInfo);
		}

		public void AddFunctions(string identity, int roleID, string[] functionIDs)
		{
			this.VerifyUser(identity);
			new Function();
			Role role = new Role();
			for (int i = 0; i < functionIDs.Length; i++)
			{
				role.AddFunction(functionIDs[i], roleID);
			}
		}

		public int AddOU(string identity, OUInfo ouInfo)
		{
			this.VerifyUser(identity);
			OU ou = new OU();
			return ou.Insert2(ouInfo);
		}

		public void AddOUs(string identity, int roleID, int[] ouIDs)
		{
			this.VerifyUser(identity);
			new OU();
			Role role = new Role();
			for (int i = 0; i < ouIDs.Length; i++)
			{
				role.AddOU(ouIDs[i], roleID);
			}
		}

		public int AddRole(string identity, RoleInfo roleInfo)
		{
			this.VerifyUser(identity);
			Role role = new Role();
			return role.Insert2(roleInfo);
		}

		public void AddUser(string identity, UserInfo userInfo)
		{
			this.VerifyUser(identity);
			new User().Insert(userInfo);
		}

		public void AddUsersToOU(string identity, int ouID, string[] userNames)
		{
			this.VerifyUser(identity);
			User user = new User();
			OU ou = new OU();
			UserInfo userByName = null;
			for (int i = 0; i < userNames.Length; i++)
			{
				userByName = user.GetUserByName(userNames[i]);
				ou.AddUser(userByName.ID, ouID);
			}
		}

		public void AddUsersToRole(string identity, int roleID, string[] userNames)
		{
			this.VerifyUser(identity);
			User user = new User();
			UserInfo userByName = new UserInfo();
			Role role = new Role();
			for (int i = 0; i < userNames.Length; i++)
			{
				userByName = user.GetUserByName(userNames[i]);
				role.AddUser(userByName.ID, roleID);
			}
		}

		public void DeleteFunction(string identity, string[] functionIDs)
		{
			this.VerifyUser(identity);
			Function function = new Function();
			for (int i = 0; i < functionIDs.Length; i++)
			{
				function.Delete(functionIDs[i].ToString());
			}
		}

		public void DeleteOU(string identity, int[] ouIDs)
		{
			this.VerifyUser(identity);
			OU ou = new OU();
			for (int i = 0; i < ouIDs.Length; i++)
			{
				ou.Delete(ouIDs[i].ToString());
			}
		}

		public void DeleteRole(string identity, string[] roleNames)
		{
			this.VerifyUser(identity);
			int length = roleNames.Length;
			int[] numArray1 = new int[length];
			Role role = new Role();
			RoleInfo roleByName = null;
			for (int i = 0; i < length; i++)
			{
				roleByName = role.GetRoleByName(roleNames[i]);
				role.Delete(roleByName.ID.ToString());
			}
		}

		public void DeleteUser(string identity, string[] userNames)
		{
			this.VerifyUser(identity);
			int length = userNames.Length;
			User user = new User();
			UserInfo userByName = null;
			for (int i = 0; i < length; i++)
			{
				userByName = user.GetUserByName(userNames[i]);
				user.Delete(userByName.ID.ToString());
			}
		}

		public void RemoveFunctions(string identity, int roleID, string[] functionIDs)
		{
			this.VerifyUser(identity);
			new Function();
			Role role = new Role();
			for (int i = 0; i < functionIDs.Length; i++)
			{
				role.RemoveFunction(functionIDs[i], roleID);
			}
		}

		public void RemoveOUs(string identity, int roleID, int[] ouIDs)
		{
			this.VerifyUser(identity);
			new OU();
			Role role = new Role();
			for (int i = 0; i < ouIDs.Length; i++)
			{
				role.RemoveUser(ouIDs[i], roleID);
			}
		}

		public void RemoveUsersFromOU(string identity, int ouID, string[] userNames)
		{
			this.VerifyUser(identity);
			User user = new User();
			OU ou = new OU();
			UserInfo userByName = null;
			for (int i = 0; i < userNames.Length; i++)
			{
				userByName = user.GetUserByName(userNames[i]);
				ou.RemoveUser(userByName.ID, ouID);
			}
		}

		public void RemoveUsersFromRole(string identity, int roleID, string[] userNames)
		{
			this.VerifyUser(identity);
			User user = new User();
			UserInfo userByName = new UserInfo();
			Role role = new Role();
			for (int i = 0; i < userNames.Length; i++)
			{
				userByName = user.GetUserByName(userNames[i]);
				role.RemoveUser(userByName.ID, roleID);
			}
		}

		public void UpdateFunction(string identity, FunctionInfo functionInfo)
		{
			this.VerifyUser(identity);
            new Function().Update(functionInfo, functionInfo.ID.ToString());
		}

		public void UpdateOU(string identity, OUInfo ouInfo)
		{
			this.VerifyUser(identity);
			new OU().Update(ouInfo, ouInfo.ID.ToString());
		}

		public void UpdateRole(string identity, RoleInfo roleInfo)
		{
			this.VerifyUser(identity);
			new Role().Update(roleInfo, roleInfo.ID.ToString());
		}

		public void UpdateUser(string identity, UserInfo userInfo)
		{
			this.VerifyUser(identity);
			new User().Update(userInfo, userInfo.ID.ToString());
		}

		private void VerifyUser(string identity)
		{
			if ((this.m_SessionID == null) || (this.m_SessionID == string.Empty))
			{
				throw new MyException("SessionID未设置！");
			}
			string userName = new User().GetUserName(identity, this.m_SessionID);
			if (userName == string.Empty)
			{
				throw new MyException("用户未授权或验证未通过！");
			}
			if (this.m_AdminNames.IndexOf(userName) < 0)
			{
				throw new MyException("没有操作此功能的权限，请与管理员联系！");
			}
		}
	}
}