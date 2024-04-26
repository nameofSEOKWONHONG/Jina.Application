using Jina.Base.Service.Abstract;
using Jina.Domain.Account.Request;
using Jina.Domain.Shared;
using Jina.Domain.Shared.Abstract;

namespace Jina.Domain.Abstract.Account
{
    public interface ICreateTenantService : IServiceImplBase<CreateTenantRequest, IResults<bool>>, IScopeService
    {

    }
}
