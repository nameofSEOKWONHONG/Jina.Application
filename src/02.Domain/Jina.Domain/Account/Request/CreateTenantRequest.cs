using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jina.Validate;

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
		/// 이름
		/// </summary>
		public string FirstName { get; set; }
		
		/// <summary>
		/// 성
		/// </summary>
		public string LastName { get; set; }
		
		public string Email { get; set; }
		
		/// <summary>
		/// 최초 사용자 등록자명
		/// </summary>
		public string UserName => $"{LastName} {FirstName}";

		/// <summary>
		/// 국가 코드에 매칭되는 시간대 표기
		/// 국가 코드는 연락처은 국가 코드와 매칭된다. (미국 : +1 / en-US)
		/// </summary>
		public string TimeZone { get; set; }	
		
		/// <summary>
		/// 지역번호 (서울:02) 
		/// </summary>
		public int RegionCode { get; set; }
		
		/// <summary>
		/// 전화번호
		/// </summary>
		public int PhoneNumber { get; set; }
		
		public class Validator : Validator<CreateTenantRequest>
		{
			public Validator()
			{
				NotEmpty(m => m.TenantId);
				NotEmpty(m => m.Email);
				NotEmpty(m => m.Name);
				NotEmpty(m => m.FirstName);
				NotEmpty(m => m.LastName);
			}
		}
	}
}
