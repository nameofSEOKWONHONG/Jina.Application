using eXtensionSharp;
using Jina.Base.Service;
using Jina.Domain.Abstract.Language;
using Jina.Domain.Multilingual;
using Jina.Domain.Service.Infra;
using Jina.Domain.Shared;
using Jina.Domain.Shared.Abstract;
using Jina.Session.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Jina.Domain.Service.Language;

public class GetMultilingualTopicService : ServiceImplBase<GetMultilingualTopicService
    , AppDbContext
    , GetMultilingualTopicRequest
    , IResults<GetMultilingualTopicResult>>
    , IGetMultilingualTopicService
{
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="context"></param>
    /// <param name="svc"></param>
    public GetMultilingualTopicService(ISessionContext context, ServicePipeline svc) : base(context, svc)
    {
    }

    public override Task OnExecutingAsync()
    {
        return Task.CompletedTask;
    }

    public override async Task OnExecuteAsync()
    {
        var result = await this.Db.MultilingualTopics
            .Include(m => m.MultilingualTopicConfigs)
            .FirstOrDefaultAsync(m => m.Id == this.Request.Id);

        if(result.xIsEmpty()) return;
        
        var data = new GetMultilingualTopicResult();
        data.Id = result.Id;
        data.PrimaryCultureType = result.PrimaryCultureType;
        data.MultilingualTopicConfigResults = new();
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