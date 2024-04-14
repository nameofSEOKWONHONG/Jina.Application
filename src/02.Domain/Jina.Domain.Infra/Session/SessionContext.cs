using Jina.Base.Service;
using Jina.Database.Abstract;
using Jina.Domain.Entity;
using Jina.Lang.Abstract;
using Jina.Session.Abstract;
using Microsoft.AspNetCore.Http;

namespace Jina.Domain.Service.Infra
{
	public class SessionContext : ISessionContext
    {
        public string TenantId { get; private set; }

        public ISessionCurrentUser CurrentUser { get; private set; }

        public ISessionDateTime CurrentTime { get; private set; }

        public ILocalizer Localizer { get; private set; }
        
        public IDbContext DbContext { get; }
        
        public IDbProviderBase DbProvider { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }
        public IHttpClientFactory HttpClientFactory { get; }

        public CancellationToken CancellationToken { get; }

        public bool IsDecrypt { get; private set; }

        public SessionContext(
            AppDbContext dbContext
            , IDbProviderBase dbProvider
            , ISessionCurrentUser user
            , ISessionDateTime time
            , ILocalizer localizer
            , IHttpClientFactory factory
            , IHttpContextAccessor accessor)
        {
            this.DbContext = dbContext;
            this.Localizer = localizer;
            this.DbProvider = dbProvider;
            this.CurrentUser = user;
            this.CurrentTime = time;
            this.IsDecrypt = false;
            this.Localizer = localizer;
            this.HttpContextAccessor = accessor;
            this.HttpClientFactory = factory;
        }
    }
}