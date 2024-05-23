using eXtensionSharp;
using Hangfire;
using Jina.Database.Abstract;
using Jina.Domain.Entity;
using Jina.Domain.Service.Infra.Services;
using Jina.Lang.Abstract;
using Jina.Session.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

namespace Jina.Domain.Service.Infra
{
	public class SessionContext : ISessionContext
    {
        public string TenantId { get; init; }

        public ISessionCurrentUser CurrentUser { get; init; }

        public ISessionDateTime CurrentTime { get; init; }

        public ILocalizer Localizer { get; init; }
        
        public IDbContext DbContext { get; init;}
        
        public IDbProviderBase DbProvider { get; init;}
        public IHttpContextAccessor HttpContextAccessor { get; init;}
        public IHttpClientFactory HttpClientFactory { get; init;}
        
        public IBackgroundJobClient JobClient { get; init;}
        public IDistributedCache DistributedCache { get; init; }

        public bool IsDecrypt { get; set; }

        public SessionContext(
            AppDbContext dbContext
            , IDbProviderBase dbProvider
            , ISessionCurrentUser user
            , ISessionDateTime time
            , ILocalizer localizer
            , IHttpClientFactory factory
            , IHttpContextAccessor accessor
            , IBackgroundJobClient jobClient
            , IDistributedCache cache)
        {
            this.DbContext = dbContext;
            this.Localizer = localizer;
            this.DbProvider = dbProvider;
            this.CurrentUser = user;
            this.CurrentTime = time;
            this.Localizer = localizer;
            this.HttpContextAccessor = accessor;
            this.HttpClientFactory = factory;
            this.JobClient = jobClient;
            this.DistributedCache = cache;

            if (this.CurrentUser.xIsNotEmpty())
            {
                if (this.CurrentUser.TenantId.xIsNotEmpty())
                {
                    this.TenantId = this.CurrentUser.TenantId;    
                }                
            }
        }
    }
}