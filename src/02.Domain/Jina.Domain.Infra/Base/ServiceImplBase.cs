using eXtensionSharp;
using Jina.Base.Service;
using Jina.Base.Service.Abstract;
using Jina.Database.Abstract;
using Jina.Session.Abstract;
using Microsoft.Extensions.Logging;

namespace Jina.Domain.Service.Infra
{
	public abstract class ServiceImplBase<TSelf, TRequest, TResult> 
        : ServiceImplCore<TSelf, TRequest, TResult>
            , IServiceImplBase<TRequest, TResult>
        where TSelf : class
    {
        public ISessionContext Context { get; }
        protected readonly ServicePipeline Pipe;

        protected ServiceImplBase(ILogger<TSelf> logger, ISessionContext context) : base(logger: logger)
        {
            this.Self = this;
            this.Context = context;
        }
        
        protected ServiceImplBase(ILogger<TSelf> logger, ISessionContext context, ServicePipeline pipe) : this(logger, context)
        {
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
        protected ServiceImplBase(ILogger<TSelf> logger, ISessionContext ctx) : base(logger:logger, ctx, null)
        {
            
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="pipe"></param>
        protected ServiceImplBase(ILogger<TSelf> logger, ISessionContext context, ServicePipeline pipe) : base(logger:logger, context, pipe)
        {
        }
    }
}