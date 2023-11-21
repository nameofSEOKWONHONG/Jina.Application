using Jina.Base.Service.Abstract;
using Jina.Domain.Account.Token;
using Jina.Domain.SharedKernel.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jina.Domain.Abstract.Account.Token
{
    public interface IGetTokenRefreshService : IServiceImplBase<RefreshTokenRequest, IResultBase<TokenResponse>>, IScopeService
    {
    }
}