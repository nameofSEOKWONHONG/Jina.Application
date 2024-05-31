using Blazored.SessionStorage;
using Jina.Passion.Client.Share.Common;
using Jina.Passion.Mud.Client.Features.Http;
using Jina.Passion.Mud.Client.Features.Http.Abstract;
using Microsoft.AspNetCore.Components.Authorization;

namespace Jina.Passion.Mud.Client.Features.Common;

public abstract class ServiceBase
{
    protected readonly IRestClient Client;
    protected readonly ISessionStorageWrapperService SessionStorageWrapperService;
    protected readonly AuthenticationStateProvider AuthenticationStateProvider;
    //TODO : localstorage 추가

    /// <summary>
    /// 
    /// </summary>
    /// <param name="client"></param>
    protected ServiceBase(IRestClient client)
    {
        Client = client;
    }

    protected ServiceBase(IRestClient client, ISessionStorageWrapperService sessionStorageService) : this(client)
    {
        SessionStorageWrapperService = sessionStorageService;
    }

    protected ServiceBase(IRestClient client, ISessionStorageWrapperService sessionStorageService,
        AuthenticationStateProvider authenticationStateProvider) : this(client, sessionStorageService)
    {
        AuthenticationStateProvider = authenticationStateProvider;
    }
        
}