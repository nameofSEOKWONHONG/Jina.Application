using eXtensionSharp;
using Jina.Session.Abstract;

namespace Jina.Domain.Service.Infra
{
    public class SessionDateTime : ISessionDateTime
    {
        public DateTime Now
        {
            get
            {
                var utcDateTime = DateTime.UtcNow;
                var tenantTimeZone = TimeZoneInfo.FindSystemTimeZoneById(_user.TimeZone.xValue("Korea Standard Time"));
                return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, tenantTimeZone);
            }
        }

        private readonly ISessionCurrentUser _user;

        public SessionDateTime(ISessionCurrentUser user)
        {
            _user = user;
        }

        public DateTime ToLocal(DateTime utc)
        {
            DateTime convertedDate = DateTime.SpecifyKind(
                utc,
                DateTimeKind.Utc);
            var kind = convertedDate.Kind; // will equal DateTimeKind.Utc
            if (kind != DateTimeKind.Utc) throw new Exception("param not utc datetime");

            return convertedDate.ToLocalTime();
        }

        public DateTime ToUtc(DateTime local) => local.ToUniversalTime();
	}
}