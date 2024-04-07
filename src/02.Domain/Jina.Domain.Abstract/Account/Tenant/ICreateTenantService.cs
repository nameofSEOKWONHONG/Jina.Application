using Jina.Base.Service.Abstract;
using Jina.Domain.Account.Request;
using Jina.Domain.SharedKernel;
using Jina.Domain.SharedKernel.Abstract;

namespace Jina.Domain.Abstract.Account
{
    public interface ICreateTenantService : IServiceImplBase<CreateTenantRequest, IResultBase<bool>>, IScopeService
    {

    }
}
