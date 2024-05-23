using eXtensionSharp;
using Jina.Base.Service;
using Jina.Base.Service.Abstract;
using Jina.Database.Abstract;
using Jina.Session.Abstract;

namespace Jina.Domain.Service.Infra
{
	public abstract class ServiceImplBase<TSelf, TRequest, TResult> 
        : ServiceImplCore<TSelf, TRequest, TResult>
            , IServiceImplBase<TRequest, TResult>
        where TSelf : class
    {
        public ISessionContext Context { get; }
        protected readonly ServicePipeline Pipe;

        protected ServiceImplBase(ISessionContext context, ServicePipeline pipe)
        {
            this.Self = this;
            this.Context = context;
            this.Pipe = pipe;
        }
    }

    public abstract class ServiceImplBase<TSelf, TDbContext, TRequest, TResult> 
        : ServiceImplBase<TSelf, TRequest, TResult>
        where TSelf : class
        where TDbContext : IDbContext
    {
        protected TDbContext Db => this.Context.DbContext.xAs<TDbContext>();

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
        /// <param name="pipe"></param>
        protected ServiceImplBase(ISessionContext context, ServicePipeline pipe) : base(context, pipe)
        {
        }
    }
}