using eXtensionSharp;
using Jina.Base.Service;
using Jina.Domain.Abstract.Example;
using Jina.Domain.Example;
using Jina.Domain.Service.Infra;
using Jina.Domain.Shared;
using Jina.Session.Abstract;
using Microsoft.Extensions.Logging;
using QuestPDF.Fluent;

namespace Jina.Domain.Service.Example.Weather;


public class GetPdfWeathersService : ServiceImplBase<GetPdfWeathersService, PaginatedRequest<WeatherForecastRequest>, Results<byte[]>>
, IGetPdfWeathersService
{
    private readonly IGetWeathersService _getWeathersService;

    public GetPdfWeathersService(ILogger<GetPdfWeathersService> logger, ISessionContext context, ServicePipeline pipe,
        IGetWeathersService getWeathersService) : base(logger, context, pipe)
    {
        _getWeathersService = getWeathersService;
    }

    public override Task<bool> OnExecutingAsync()
    {
        return Task.FromResult(true);
    }

    public override async Task OnExecuteAsync()
    {
        PaginatedResult<WeatherForecastResult> result = null;
        
        this.Pipe.Register(_getWeathersService)
            .WithParameter(() => this.Request)
            .Then(r => result = r);

        await this.Pipe.ExecuteAsync();

        if (result.xIsEmpty())
        {
            this.Result = await Results<byte[]>.FailAsync();
            return;
        }
        
        var model = InvoiceDocumentDataSource.GetInvoiceDetails();
        var document = new InvoiceDocument(model);
        var bytes = document.GeneratePdf();
        
        this.Result = await Results<byte[]>.SuccessAsync(bytes);
    }
}