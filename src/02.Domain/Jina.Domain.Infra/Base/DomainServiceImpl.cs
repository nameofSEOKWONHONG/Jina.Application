using Jina.Base.Service;
using Jina.Domain.Entity;
using Jina.Session.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jina.Domain.Infra.Base
{
    public abstract class DomainServiceImpl<TSelf, TRequest, TResult> : ServiceImplBase<TSelf, TRequest, TResult>
    {
        protected AppDbContext DbContext;
        protected ISessionContext SessionContext;

        protected DomainServiceImpl(AppDbContext db, ISessionContext context)
        {
            this.DbContext = db;
            this.SessionContext = context;
        }
    }
}