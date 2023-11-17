using Jina.Session.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jina.Domain.Infra.Session
{
    public class SessionContext : ISessionContext
    {
        public string TenantId { get; private set; }

        public ISessionCurrentUser CurrentUser { get; private set; }

        public ISessionDateTime CurrentTime { get; private set; }

        public SessionContext(ISessionCurrentUser user
            , ISessionDateTime time)
        {
            CurrentUser = user;
            CurrentTime = time;
        }
    }
}