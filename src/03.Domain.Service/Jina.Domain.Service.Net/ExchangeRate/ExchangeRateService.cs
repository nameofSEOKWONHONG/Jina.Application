using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Ardalis.SmartEnum;
using eXtensionSharp;
using Jina.Base.Service;
using Jina.Domain.Abstract.Net.ExchangeRate;
using Jina.Domain.Net.ExchangeRate;
using Uri = System.Uri;

namespace Jina.Domain.Service.Net.ExchangeRate;

/// <summary>
/// 환율 조회 서비스 (일 1000회 제한)
/// singleton으로 동작해야 함.
/// https://www.koreaexim.go.kr/site/program/financial/exchangeJSON?authkey=AUTHKEY1234567890&searchdate=20180102&data=AP01
/// </summary>
public class ExchangeRateService : ServiceImplBase<ExchangeRateService, ExchangeRequest, IEnumerable<ExchangeResult>>, IExchangeRateService
{
    
    private readonly HttpClient _client;
    private readonly string _host = "https://www.koreaexim.go.kr";
    private readonly string _url = "site/program/financial/exchangeJSON";
    private readonly int _maxCount = 1000;
    private int _searchedCount = 0;
    
    public ExchangeRateService(IHttpClientFactory factory)
    {
        _client = factory.CreateClient();
    }


    public override Task<bool> OnExecutingAsync()
    {
        if (this.Request.xIsEmpty()) return Task.FromResult(false);
        if (this.Request.AuthKey.xIsEmpty()) return Task.FromResult(false);
        if (this.Request.SearchDate.xIsEmpty()) return Task.FromResult(false);
        if (this.Request.SearchType.xIsEmpty()) return Task.FromResult(false);

        return Task.FromResult(true);
    }

    public override async Task OnExecuteAsync()
    {
        var uri = new Uri($"{_host}/{_url}?authKey={this.Request.AuthKey}&searchdate={this.Request.SearchDate.ToString(ENUM_DATE_FORMAT.YYYYMMDD)}&data={this.Request.SearchType.Name}");
        var res = await _client.GetAsync(uri);
        res.EnsureSuccessStatusCode();

        _searchedCount += 1;

        //save or update, and return result;
        this.Result = await res.Content.ReadFromJsonAsync<IEnumerable<ExchangeResult>>();
    }
}



