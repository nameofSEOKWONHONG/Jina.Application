using System.Globalization;
using eXtensionSharp;
using Jina.Passion.Client.Share.Consts;
using Microsoft.AspNetCore.Components;

namespace Jina.Passion.Client.Share.Common;

public class CultureInfoViewModel : ICultureInfoViewModel
{
    private readonly NavigationManager _navigationManager;
    private readonly ILocalStorageWrapperService _state;

    private string _selectedLanguageCode;

    public string SelectedLanguageCode
    {
        get
        {
            return _selectedLanguageCode;
        }
        set
        {
            _selectedLanguageCode = value;
            _state.SetAsync(ApplicationConsts.Client.CULTURE_CODE_NAME, value);
            this.Culture = new CultureInfo(_selectedLanguageCode);
        }
    }

    public CultureInfoViewModel(ILocalStorageWrapperService state, NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
        _state = state;
    }

    public async Task InitializeCoreAsync()
    {
        _selectedLanguageCode = await _state.GetAsync(ApplicationConsts.Client.CULTURE_CODE_NAME);
        if (_selectedLanguageCode.xIsEmpty())
        {
            _selectedLanguageCode = LocalizationConstants.SUPPORTED_COUNTRIES.First().LanguageCode;
        }
    }

    public CultureInfo Culture
    {
        get => CultureInfo.CurrentCulture;
        set
        {
            if (CultureInfo.CurrentCulture != value)
            {
                _state.SetAsync(ApplicationConsts.Client.CULTURE_CODE_NAME, value.Name);
                _navigationManager.NavigateTo(_navigationManager.Uri, forceLoad: true);
            }
        }
    }

    public async Task ChangeLanguageAsync(string languageCode)
    {
        await Task.Run(() => this.Culture = new CultureInfo(languageCode));
    }
}