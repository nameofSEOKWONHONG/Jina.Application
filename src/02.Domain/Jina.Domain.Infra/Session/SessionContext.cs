using Jina.Lang.Abstract;
using Jina.Session.Abstract;

namespace Jina.Domain.Service.Infra.Session
{
	public class SessionContext : ISessionContext
    {
        public string TenantId { get; private set; }

        public ISessionCurrentUser CurrentUser { get; private set; }

        public ISessionDateTime CurrentTime { get; private set; }

        public ILocalizer Localizer { get; private set; }

        public bool IsDecrypt { get; private set; }

        public SessionContext(ISessionCurrentUser user
            , ISessionDateTime time
            , ILocalizer localizer)
        {
            this.Localizer = localizer;
#if DEBUG
            this.TenantId = "00000";
            this.CurrentUser = user;
            this.CurrentTime = time;
            this.IsDecrypt = false;
#endif
        }
    }
}