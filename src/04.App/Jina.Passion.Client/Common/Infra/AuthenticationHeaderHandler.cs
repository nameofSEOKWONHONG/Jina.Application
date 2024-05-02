using eXtensionSharp;
using Jina.Passion.Client.Base.Abstract;
using Jina.Passion.Client.Common.Consts;
using System.Net.Http.Headers;

namespace Jina.Passion.Client.Common.Infra
{
    public class AuthenticationHeaderHandler : DelegatingHandler
    {
        private readonly AuthenticationStateProviderImpl _authenticationStateProviderImpl;
        private readonly ISessionStorageService _sessionStorageService;

        public AuthenticationHeaderHandler(ISessionStorageService sessionStorageService, AuthenticationStateProviderImpl authenticationStateProviderImpl)
        {
            this._sessionStorageService = sessionStorageService;
            this._authenticationStateProviderImpl = authenticationStateProviderImpl;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (request.Headers.Authorization?.Scheme != "Bearer")
            {
                var savedToken = await this._sessionStorageService.GetAsync(StorageConsts.Local.AuthToken);

                if (savedToken.xIsNotEmpty())
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);
                }
            }

            var result = await base.SendAsync(request, cancellationToken);
            if (result.StatusCode == System.Net.HttpStatusCode.Unauthorized ||
                result.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                return new HttpResponseMessage { StatusCode = System.Net.HttpStatusCode.Unauthorized };
            }
            return result;
        }
    }
}
