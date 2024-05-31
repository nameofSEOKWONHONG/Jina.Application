using System.Globalization;

namespace Jina.Passion.Client.Share.Consts;

public static class LocalizationConstants
{
    public static readonly CountryInfo[] SUPPORTED_COUNTRIES =
    {
        new CountryInfo
        {
            Name = "United States",
            LanguageDisplayName= "English",
            TwoISOCode = "US",
            ThreeISOCode = "USA",
            LanguageCode = "en-US",
            TimeZone = "Pacific Standard Time"
        },
        new CountryInfo
        {
            Name = "Korea",
            LanguageDisplayName = "한국어",
            TwoISOCode = "KR",
            ThreeISOCode = "KOR",
            LanguageCode = "ko-KR",
            TimeZone = "Korea Standard Time"
        },
        //new LanguageCode
        //{
        //    Code = "ja-JP",
        //    DisplayName = "日本語"
        //},
        //new LanguageCode
        //{
        //    Code = "zh-CN",
        //    DisplayName = "简体中文"
        //}

        //new LanguageCode
        //{
        //    Code = "fr-FR",
        //    DisplayName = "French"
        //},
        //// new LanguageCode
        //// {
        ////     Code = "km_KH",
        ////     DisplayName= "Khmer"
        //// },
        //new LanguageCode
        //{
        //    Code = "de-DE",
        //    DisplayName = "German"
        //},
        //new LanguageCode
        //{
        //    Code = "es-ES",
        //    DisplayName = "Español"
        //},
        //new LanguageCode
        //{
        //    Code = "ru-RU",
        //    DisplayName = "Русский"
        //},
        //new LanguageCode
        //{
        //    Code = "sv-SE",
        //    DisplayName = "Swedish"
        //},
        //new LanguageCode
        //{
        //    Code = "id-ID",
        //    DisplayName = "Indonesia"
        //},
        //new LanguageCode
        //{
        //    Code = "it-IT",
        //    DisplayName = "Italian"
        //},
    };
}

public class CountryInfo
{
    public string Name { get; set; }
    public string LanguageDisplayName { get; set; }
    
    public string TwoISOCode { get; set; }
    public string ThreeISOCode { get; set; }
    public string LanguageCode { get; set; }
    
    public string TimeZone { get; set; }

    public CultureInfo CultureInfo
    {
        get
        {
            return new CultureInfo(this.Name);
            
        }
    }
}