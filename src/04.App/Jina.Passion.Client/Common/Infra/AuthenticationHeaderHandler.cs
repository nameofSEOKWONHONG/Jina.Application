using Jina.Passion.Client.Base.Abstract;
using Jina.Passion.Client.Common.Consts;
using System.Net.Http.Headers;

namespace Jina.Passion.Client.Common.Infra
{
    public class AuthenticationHeaderHandler : DelegatingHandler
    {
        private readonly ISessionStorageHandler _sessionStorageHandler;

        public AuthenticationHeaderHandler(ISessionStorageHandler sessionStorageHandler)
        {
            this._sessionStorageHandler = sessionStorageHandler;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (request.Headers.Authorization?.Scheme != "Bearer")
            {
                var savedToken = await this._sessionStorageHandler.GetAsync(StorageConsts.Local.AuthToken);

                if (!string.IsNullOrWhiteSpace(savedToken))
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
