using System.Net.Http.Json;
using eXtensionSharp;
using Jina.Base.Service;
using Jina.Domain.Abstract.Net.ExchangeRate;
using Jina.Domain.Entity;
using Jina.Domain.Entity.Net.ExchangeRate;
using Jina.Domain.Net.ExchangeRate;
using Jina.Domain.Net.ExchangeRate.Enums;
using Uri = System.Uri;
using Mapster;

namespace Jina.Domain.Service.Net.ExchangeRate;


/*
 * [로직 설명]
 * - 분당 1회 호출시 24시간 기준 1440회 호출함.
 * - open api call 제한은 일 1000회 이므로 인증키는 최소 2개 이상 필요.
 * 
 * BackgroundService -> Running ISaveExchangeRateService (per one minutes)
 *                               |
 *                         (Saved Result)
 *                               
 * Api Service ->     Get IGetAllExchangeRateService
 *                    Get IGetExchangeRateService
 * 
 */


/// <summary>
/// 환율 조회 서비스 (일 1000회 제한)
/// singleton으로 동작해야 함.
/// https://www.koreaexim.go.kr/site/program/financial/exchangeJSON?authkey=AUTHKEY1234567890&searchdate=20180102&data=AP01
/// </summary>
public class SaveExchangeRateService : ServiceImplBase<SaveExchangeRateService, ExchangeRequest, ENUM_EXCHANGE_RESULT_TYPE>, ISaveExchangeRateService
{
    private readonly string _host = "https://www.koreaexim.go.kr";
    private readonly string _url = "site/program/financial/exchangeJSON";
    private readonly AppDbContext _dbContext;
    
    public SaveExchangeRateService(AppDbContext dbContext, IHttpClientFactory factory) : base(factory)
    {
        _dbContext = dbContext;
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
        var res = await this.HttpClientFactory.CreateClient().GetAsync(uri);
        
        if (res.IsSuccessStatusCode)
        {
            var result = await res.Content.ReadFromJsonAsync<IEnumerable<ExchangeResult>>();
            if (ENUM_EXCHANGE_RESULT_TYPE.TryFromValue(result.First().Result, out ENUM_EXCHANGE_RESULT_TYPE valueResult))
            {
                this.Result = valueResult;
            }
            
            var convertedList = new List<Exchange>();
            result.xForEach(item =>
            {
                var converted = item.Adapt<Exchange>();
                converted.CreatedBy = "SYSTEM";
                converted.CreatedOn = DateTime.UtcNow;
                convertedList.Add(converted);
            });

            await _dbContext.Exchanges.AddRangeAsync(convertedList);
            await _dbContext.SaveChangesAsync();
        }
    }
}


