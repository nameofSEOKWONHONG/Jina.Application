using eXtensionSharp;
using Jina.Base.Service;
using Jina.Domain.Abstract.Language;
using Jina.Domain.Multilingual;
using Jina.Domain.Service.Infra;
using Jina.Domain.Shared;
using Jina.Domain.Shared.Abstract;
using Jina.Session.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Jina.Domain.Service.Language;

public class GetMultilingualTopicService 
    : ServiceImplBase<GetMultilingualTopicService, AppDbContext, GetMultilingualTopicRequest, Results<GetMultilingualTopicResult>>
        , IGetMultilingualTopicService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="ctx"></param>
    public GetMultilingualTopicService(ILogger<GetMultilingualTopicService> logger, ISessionContext ctx) : base(logger, ctx)
    {
    }

    public override Task<bool> OnExecutingAsync()
    {
        return Task.FromResult(true);
    }

    public override async Task OnExecuteAsync()
    {
        var result = await this.Db.MultilingualTopics
            .Include(m => m.MultilingualTopicConfigs)
            .FirstOrDefaultAsync(m => m.Id == this.Request.Id);

        if(result.xIsEmpty()) return;
        
        var data = new GetMultilingualTopicResult
        {
            Id = result!.Id,
            PrimaryCultureType = result.PrimaryCultureType,
            MultilingualTopicConfigResults = []
        };
        foreach (var item in result.MultilingualTopicConfigs)
        {
            data.MultilingualTopicConfigResults.Add(new MultilingualTopicConfigResult()
            {
                Id = item.Id,
                CultureType = item.CultureType,
                SortNo = item.SortNo
            });
        }
        this.Result = await Results<GetMultilingualTopicResult>.SuccessAsync(data);
    }
}