using eXtensionSharp;
using Jina.Session.Abstract;

namespace Jina.Db.Test;

public class MockSessionDateTime : ISessionDateTime
{
    private readonly ISessionCurrentUser _user;
    public DateTime Now
    {
        get
        {
            if (_user.TimeZone.xIsEmpty())
                throw new Exception("Session not init or Timezone is empty");
                
            var utcDateTime = DateTime.UtcNow;
            var tenantTimeZone = TimeZoneInfo.FindSystemTimeZoneById(_user.TimeZone);
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, tenantTimeZone);
        }
    }

    public DateTime ToLocal(DateTime utc)
    {
        throw new NotImplementedException();
    }

    public DateTime ToUtc(DateTime local)
    {
        throw new NotImplementedException();
    }

    public MockSessionDateTime(ISessionCurrentUser user)
    {
        _user = user;
    }
}