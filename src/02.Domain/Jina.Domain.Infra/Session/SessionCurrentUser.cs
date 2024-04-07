using Jina.Session.Abstract;
using Microsoft.AspNetCore.Http;

namespace Jina.Domain.Service.Infra
{
	public class SessionCurrentUser : ISessionCurrentUser
    {
        public string TenantId { get; private set; }

        public string TimeZone { get; private set; }

        public string UserId { get; private set; }

        public string UserName { get; private set; }

        public string RoleName { get; private set; }

        public List<KeyValuePair<string, string>> Claims { get; private set; }

        public SessionCurrentUser(IHttpContextAccessor accessor)
        {
#if DEBUG
            TenantId = "00000";
            TimeZone = string.Empty;
            UserId = "test";
            UserName = "test";
            RoleName = "test";
            Claims = null;
#else
            var user = accessor.HttpContext.User;
            TenantId = user.FindFirst(ApplicationClaimTypeConst.TenantId).Value;
            TimeZone = user.FindFirst(ApplicationClaimTypeConst.TimeZone)?.Value;
            UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            UserName = user.FindFirst(ClaimTypes.Name)?.Value;
            RoleName = user.FindFirst(ClaimTypes.Role)?.Value;
            Claims = user.Claims.AsEnumerable().Select(item => new KeyValuePair<string, string>(item.Type, item.Value)).ToList();
#endif
        }
    }
}