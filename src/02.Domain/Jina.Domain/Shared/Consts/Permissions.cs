using System.ComponentModel;
using System.Reflection;

namespace Jina.Domain.Service.Infra
{
	public class Permissions
	{
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
			foreach (var prop in typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
			{
				var propertyValue = prop.GetValue(null);
				if (propertyValue is not null)
					permissions.Add(propertyValue.ToString());
			}
			return permissions;
		}
	}
}
