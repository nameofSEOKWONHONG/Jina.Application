using Jina.Base.Service;
using Jina.Domain.Entity;
using Jina.Session.Abstract;

namespace Jina.Domain.Service.Infra.Base
{
	public abstract class ServiceImplBase<TSelf, TRequest, TResult> : ServiceImplCore<TSelf, TRequest, TResult>
    {
        protected AppDbContext DbContext;
        protected ISessionContext SessionContext;

        protected ServiceImplBase(AppDbContext db, ISessionContext context)
        {
            this.DbContext = db;
            this.SessionContext = context;
        }
    }
}