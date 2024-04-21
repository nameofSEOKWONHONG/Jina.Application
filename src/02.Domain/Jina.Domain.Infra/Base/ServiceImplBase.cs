using eXtensionSharp;
using Jina.Base.Service;
using Jina.Database.Abstract;
using Jina.Session.Abstract;

namespace Jina.Domain.Service.Infra
{
	public abstract class ServiceImplBase<TSelf, TRequest, TResult> : ServiceImplCore<TSelf, TRequest, TResult>
        where TSelf : class
    {
        protected ISessionContext Ctx;
        protected ServicePipeline Svc;

        protected ServiceImplBase(ISessionContext context, ServicePipeline svc)
        {
            this.Self = this;
            this.Ctx = context;
            this.Svc = svc;
        }
    }

    public abstract class ServiceImplBase<TSelf, TDbContext, TRequest, TResult> : ServiceImplBase<TSelf, TRequest, TResult>
        where TSelf : class
        where TDbContext : IDbContext
    {
        protected TDbContext Db => this.Ctx.DbContext.xAs<TDbContext>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        protected ServiceImplBase(ISessionContext ctx) : base(ctx, null)
        {
            
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="svc"></param>
        protected ServiceImplBase(ISessionContext context, ServicePipeline svc) : base(context, svc)
        {
        }
    }
}