using System.Collections;
using WHC.Security.BLL;
using WHC.Security.Common;
using WHC.Security.Entity;
using System.Collections.Generic;

namespace WHC.Security.Adapter
{
	public class RealWorldSecurity : ISecurity
	{
		private string m_SessionID = "";

		public string GetManager(string userName)
		{
			string name = "";
			User user = new User();
			UserInfo userByName = user.GetUserByName(userName);
			if (userByName != null)
			{
				userByName = user.FindByID(userByName.PID);
				if (userByName != null)
				{
					name = userByName.Name;
				}
			}
			return name;
		}

		public OUInfo GetOU(string identity, int ouID)
		{
			OUInfo info = null;
			if (this.IdentityIsValid(identity))
			{
				OU ou = new OU();
				info = (OUInfo) ou.FindByID(ouID.ToString());
			}
			return info;
		}

		public OUInfo[] GetOUByUser(string userName)
		{
			UserInfo userByName = new User().GetUserByName(userName);
			OU ou = new OU();
			OUInfo[] infoArray = null;
			List<OUInfo> oUsByUser = ou.GetOUsByUser(userByName.ID);
			if (oUsByUser != null)
			{
				infoArray = oUsByUser.ToArray();
			}
			return infoArray;
		}

		public OUInfo[] GetOUs()
		{
			OU ou = new OU();
            List<OUInfo> all = ou.GetAll();
            return all.ToArray();
		}

		public OUInfo[] GetOUsByRole(int roleID)
		{
			OU ou = new OU();
            List<OUInfo> oUsByRole = ou.GetOUsByRole(roleID);
            return oUsByRole.ToArray();
		}

		public RoleInfo GetRole(string identity, int roleID)
		{
			RoleInfo info = null;
			if (this.IdentityIsValid(identity))
			{
				Role role = new Role();
                info = (RoleInfo)role.FindByID(roleID.ToString());
			}
			return info;
		}

		public RoleInfo[] GetRoles()
		{
			Role role = new Role();
            List<RoleInfo> all = role.GetAll();
            return all.ToArray();
		}

		public RoleInfo[] GetRolesByOU(int ouID)
		{
			Role role = new Role();
            List<RoleInfo> rolesByOU = role.GetRolesByOU(ouID);
            return rolesByOU.ToArray();
		}

		public RoleInfo[] GetRolesByUser(string userName)
		{
			UserInfo userByName = new User().GetUserByName(userName);
			Role role = new Role();
            List<RoleInfo> rolesByUser = role.GetRolesByUser(userByName.ID);
            return rolesByUser.ToArray();
		}

		public SimpleUserInfo[] GetSimpleUsers(string identity)
		{
			User user = new User();
            return user.GetSimpleUsers().ToArray();
		}

		public SimpleUserInfo[] GetSimpleUsersByIDs(string identity, string userIDs)
		{
			SimpleUserInfo[] infoArray = null;
			if (this.IdentityIsValid(identity))
			{
				User user = new User();
				infoArray = user.GetSimpleUsers(userIDs).ToArray();
			}
			return infoArray;
		}

		public SimpleUserInfo[] GetSimpleUsersByOU(string identity, int ouID)
		{
			SimpleUserInfo[] infoArray = null;
			if (this.IdentityIsValid(identity))
			{
                infoArray = new User().GetSimpleUsersByOU(ouID).ToArray();
			}
			return infoArray;
		}

		public SimpleUserInfo[] GetSimpleUsersByRole(string identity, int roleID)
		{
			SimpleUserInfo[] infoArray = null;
			if (this.IdentityIsValid(identity))
			{
                infoArray = new User().GetSimpleUsersByRole(roleID).ToArray();
			}
			return infoArray;
		}

		public FunctionInfo[] GetUserFunctions(string identity, string typeID)
		{
			List<FunctionInfo> list = new User().GetUserFunctions(identity, this.m_SessionID, typeID);
			return list.ToArray();
		}

		public UserInfo GetUserInfo(string identity)
		{
			User user = new User();
			string userName = user.GetUserName(identity, this.m_SessionID);
			return user.GetUserByName(userName);
		}

		private bool IdentityIsValid(string identity)
		{
			bool flag = false;
			string userName = new User().GetUserName(identity, this.m_SessionID);
			if ((userName != null) && (userName != string.Empty))
			{
				flag = true;
			}
			return flag;
		}

		public bool ModifyPassword(string identity, string userPassword)
		{
			User user = new User();
			return user.ModifyPassword(user.GetUserName(identity, this.m_SessionID), userPassword);
		}

		public string VerifyUser(string userName, string userPassword, string serialNumber, string typeID)
		{
			string text = "";
			bool flag = false;
			SystemType type = new SystemType();
			SystemTypeInfo info = type.FindByOID(typeID);
			if (info == null)
			{
				text = "系统[" + typeID + "]编号不存在，请与开发商联系！";
				flag = true;
			}

			if (flag)
			{
				return text;
			}

			User user2 = new User();
			return user2.VerifyUser(userName, userPassword, this.m_SessionID);
		}

		public string VerifyUserByIdentity(string identity)
		{
			User user = new User();
			return user.GetUserName(identity, this.m_SessionID);
		}

		public string SessionID
		{
			get
			{
				return this.m_SessionID;
			}
			set
			{
				if ((value == null) || (value == string.Empty))
				{
					throw new MyException("唯一标识SessionID不能为空！");
				}
				this.m_SessionID = value;
			}
		}
	}
}