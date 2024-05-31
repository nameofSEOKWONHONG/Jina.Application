using System.Security.Claims;
using eXtensionSharp;
using Jina.Domain.Account.Token;
using Jina.Passion.Client.Services.Account;
using Jina.Passion.Client.Share.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using MudBlazor;

namespace Jina.Passion.Mud.Client.Pages.Account;

public partial class Login : ComponentBase
{
    [Inject] public NavigationManager NavigationManager { get; set; }
    [Inject] public IJSRuntime Js { get; set; }
    [Inject] public ICultureInfoViewModel CultureInfoViewModel { get; set; }
    [Inject] public IAccountService AccountService { get; set; }
    [Inject] public ILocalStorageWrapperService LocalStorageWrapperService { get; set; }
    [Inject] public AuthenticationStateProviderImpl AuthenticationStateProviderImpl { get; set; }
    private bool _enableUpdate;
    private bool Validated
    {
        get
        {
            if (_tokenModel.Email.xIsNotEmpty() && _tokenModel.Password.xIsNotEmpty()) return true;
            return false;
        }
    }

    private TokenRequest _tokenModel = new();

    private bool _passwordVisibility;
    private InputType _passwordInput = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

    private bool _visibleDebugAccount = false;
    
    protected override async Task OnInitializedAsync()
    {
#if DEBUG
        _visibleDebugAccount = true;
#endif
        var state = await AuthenticationStateProviderImpl.GetAuthenticationStateAsync();
        if (state != new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())))
        {
            NavigationManager.NavigateTo("/");
        }
        var remember = await LocalStorageWrapperService.GetOnceAsync(nameof(_tokenModel.RemamberMe));
        if (remember.xEquals("Y"))
        {
            _tokenModel.RemamberMe = true;
            _tokenModel.TenantId = await LocalStorageWrapperService.GetOnceAsync(nameof(_tokenModel.Email));
            _tokenModel.Email = await LocalStorageWrapperService.GetOnceAsync(nameof(_tokenModel.Email));
        }
    }

    private async Task SubmitAsync()
    {
        var result = await AccountService.Login(_tokenModel);
        if (!result.Succeeded)
        {
            foreach (var message in result.Messages)
            {
                SnakeBar.Add(message, Severity.Error);
            }
        }
    }

    private void TogglePasswordVisibility()
    {
        if (_passwordVisibility)
        {
            _passwordVisibility = false;
            _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
            _passwordInput = InputType.Password;
        }
        else
        {
            _passwordVisibility = true;
            _passwordInputIcon = Icons.Material.Filled.Visibility;
            _passwordInput = InputType.Text;
        }
    }

    private void FillAdministratorCredentials()
    {
        _tokenModel.TenantId = "00000";
        _tokenModel.Email = "admin@test.com";
        _tokenModel.Password = "1q2w3e4r";
    }
}