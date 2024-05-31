using System.Globalization;

namespace Jina.Passion.Client.Share.Common;

public interface ICultureInfoViewModel
{
    CultureInfo Culture { get; set; }

    Task ChangeLanguageAsync(string languageCode);

    string SelectedLanguageCode { get; set; }

    Task InitializeCoreAsync();
}