using Jina.Session.Abstract;

namespace Jina.Db.Test;

public class MockSessionCurrentUser : ISessionCurrentUser
{
    public string TenantId { get; }
    public string Email { get; }
    public string TimeZone { get; }
    public string UserId { get; }
    public string UserName { get; }
    public string RoleName { get; }
    public List<KeyValuePair<string, string>>? Claims { get; }

    public MockSessionCurrentUser()
    {
        TenantId = "00000";
        Email = "admin@test.com";
        TimeZone = "Korea Standard Time";
        UserId = Guid.NewGuid().ToString("N");
        RoleName = "admin";
        Claims = default;
    }
}