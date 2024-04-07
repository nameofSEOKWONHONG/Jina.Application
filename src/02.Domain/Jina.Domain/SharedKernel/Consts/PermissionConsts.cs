using System.ComponentModel;
using System.Reflection;

namespace Jina.Domain.Service.Infra
{
	public class PermissionConsts
	{
		#region [시스템정보]

		[DisplayName("Company")]
		[Description("Company Permissions")]
		public static class Company
		{
			public const string View = "Permissions.Company.View";
			public const string Edit = "Permissions.Company.Edit";
		}

		#endregion [시스템정보]

		#region [계정설정]

		[DisplayName("User")]
		[Description("User Permissions")]
		public static class Users
		{
			public const string View = "Permissions.User.View";
			public const string Create = "Permissions.User.Create";
			public const string Edit = "Permissions.User.Edit";
			public const string Delete = "Permissions.User.Delete";
			public const string Export = "Permissions.User.Export";
			public const string Search = "Permissions.User.Search";
		}

		[DisplayName("User Role")]
		[Description("user Role Permissions")]
		public static class UserRoles
		{
			public const string View = "Permissions.UserRole.View";
			public const string Create = "Permissions.UserRole.Create";
			public const string Edit = "Permissions.UserRole.Edit";
			public const string Delete = "Permissions.UserRole.Delete";
			public const string Search = "Permissions.UserRole.Search";
		}

		#endregion [계정설정]

		#region [메뉴권한]

		[DisplayName("Role")]
		[Description("Role Permissions")]
		public static class Role
		{
			public const string View = "Permissions.Role.View";
			public const string Create = "Permissions.Role.Create";
			public const string Edit = "Permissions.Role.Edit";
			public const string Delete = "Permissions.Role.Delete";
			public const string Search = "Permissions.Role.Search";
		}

		[DisplayName("Role Claim")]
		[Description("Role Claim Permissions")]
		public static class RoleClaim
		{
			public const string View = "Permissions.RoleClaim.View";
			public const string Create = "Permissions.RoleClaim.Create";
			public const string Edit = "Permissions.RoleClaim.Edit";
			public const string Delete = "Permissions.RoleClaim.Delete";
			public const string Search = "Permissions.RoleClaim.Search";
		}

		#endregion [메뉴권한]

		#region [메일설정]

		[DisplayName("Mail Config")]
		[Description("Mail Config Permissions")]
		public static class MailConfig
		{
			public const string View = "Permissions.MailConfig.View";
			public const string Edit = "Permissions.MailConfig.Edit";
		}

		#endregion [메일설정]

		#region [시스템설정]

		[DisplayName("System Config")]
		[Description("System Config Permissions")]
		public static class SystemConfig
		{
			public const string View = "Permissions.SystemConfig.View";
			public const string Create = "Permissions.SystemConfig.Create";
			public const string Edit = "Permissions.SystemConfig.Edit";
			public const string Delete = "Permissions.SystemConfig.Delete";
			public const string Search = "Permissions.SystemConfig.Search";
		}

		#endregion [시스템설정]

		#region [시스템이력]

		[DisplayName("Audit Trail")]
		[Description("Audit Trail Permissions")]
		public static class AuditTrail
		{
			public const string View = "Permissions.AuditTrail.View";
			public const string Search = "Permissions.AuditTrail.Search";
		}

		#endregion [시스템이력]

		public static List<string> GetRegisteredPermissions()
		{
			var permissions = new List<string>();
			foreach (var prop in typeof(PermissionConsts).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
			{
				var propertyValue = prop.GetValue(null);
				if (propertyValue is not null)
					permissions.Add(propertyValue.ToString());
			}
			return permissions;
		}
	}
}
