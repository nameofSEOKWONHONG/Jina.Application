using Jina.Base.Service.Abstract;
using Jina.Domain.Account.Request;
using Jina.Domain.Shared.Abstract;

namespace Jina.Domain.Abstract.Account
{
    public interface IRegisterUserService : IServiceImplBase<RegisterRequest, IResults<string>>, IScopeService
    {

    }
}
