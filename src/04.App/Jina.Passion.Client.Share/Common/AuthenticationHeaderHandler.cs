using System.Net.Http.Headers;
using eXtensionSharp;
using Jina.Passion.Client.Share.Consts;

namespace Jina.Passion.Client.Share.Common;

public class AuthenticationHeaderHandler : DelegatingHandler
{
    private readonly AuthenticationStateProviderImpl _authenticationStateProviderImpl;
    private readonly ISessionStorageWrapperService _sessionStorageWrapperService;

    public AuthenticationHeaderHandler(ISessionStorageWrapperService sessionStorageWrapperService, AuthenticationStateProviderImpl authenticationStateProviderImpl)
    {
        this._sessionStorageWrapperService = sessionStorageWrapperService;
        this._authenticationStateProviderImpl = authenticationStateProviderImpl;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (request.Headers.Authorization?.Scheme != "Bearer")
        {
            var savedToken = await this._sessionStorageWrapperService.GetAsync(StorageConsts.Local.AuthToken);

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