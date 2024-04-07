using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jina.Domain.Account.Request
{
	public class CreateTenantRequest
	{
		public string TenantId { get; set; }
		/// <summary>
		/// company name or tenant name
		/// </summary>
		public string Name { get; set; }
		public string RedirectUrl { get; set; }

		/// <summary>
		/// 국가 코드에 매칭되는 시간대 표기
		/// </summary>
		public string TimeZone { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public string UserName { get; set; }	
	}
}
