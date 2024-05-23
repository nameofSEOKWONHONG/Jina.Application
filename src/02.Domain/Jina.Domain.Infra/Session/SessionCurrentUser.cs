using System.Security.Claims;
using eXtensionSharp;
using Jina.Session.Abstract;
using Microsoft.AspNetCore.Http;

namespace Jina.Domain.Service.Infra
{
	public class SessionCurrentUser : ISessionCurrentUser
    {
        public string TenantId { get; private set; }

        public string TimeZone { get; private set; }

        public string UserId { get; private set; }
        public string Email { get; private set; }

        public string UserName { get; private set; }

        public string RoleName { get; private set; }

        public List<KeyValuePair<string, string>> Claims { get; private set; }

        public SessionCurrentUser(IHttpContextAccessor accessor)
        {
            var user = accessor.HttpContext?.User;
            if (user.xIsNotEmpty())
            {
                if (user.Identity.IsAuthenticated)
                {
                    TenantId = user.FindFirst(ApplicationClaimTypes.TenantId).Value;
                    TimeZone = user.FindFirst(ApplicationClaimTypes.TimeZone)?.Value;
                    UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    Email = user.FindFirst(ClaimTypes.Email)?.Value;
                    UserName = user.FindFirst(ClaimTypes.Name)?.Value;
                    RoleName = user.FindFirst(ClaimTypes.Role)?.Value;
                    Claims = user.Claims.AsEnumerable().Select(item => new KeyValuePair<string, string>(item.Type, item.Value)).ToList();                
                }    
            }
        }
    }
}