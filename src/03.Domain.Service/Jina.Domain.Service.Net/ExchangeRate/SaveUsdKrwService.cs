using eXtensionSharp;
using Jina.Base.Service;
using Jina.Domain.Abstract.Net.ExchangeRate;
using Jina.Domain.Entity;
using Jina.Domain.Entity.Net.ExchangeRate;
using Jina.Domain.Net.ExchangeRate;
using Mapster;
using System.Net.Http.Json;

namespace Jina.Domain.Service.Net.ExchangeRate;


/*
 * [로직 설명]
 * - 분당 1회 호출시 24시간 기준 1440회 호출함.
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
/// 두나무 API 환율 조회 서비스, 한국산업은행 API는 정상동작 안함.
/// </summary>
public class SaveUsdKrwService : ServiceImplBase<SaveUsdKrwService, ExchangeRequest, bool>, ISaveExchangeRateService
{
    private readonly string _url = "https://quotation-api-cdn.dunamu.com/v1/forex/recent?codes=FRX.KRWUSD";
    private readonly AppDbContext _dbContext;
    
    public SaveUsdKrwService(AppDbContext dbContext, IHttpClientFactory factory) : base(factory)
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
        var uri = new Uri(_url);
        var res = await this.HttpClientFactory.CreateClient().GetAsync(uri);
        
        if (res.IsSuccessStatusCode)
        {
            this.Result = true;
            var result = await res.Content.ReadFromJsonAsync<IEnumerable<ExchangeResult>>();            
            
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


