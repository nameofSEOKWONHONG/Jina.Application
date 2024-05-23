using eXtensionSharp;
using Jina.Base.Service;
using Jina.Domain.Abstract.Example;
using Jina.Domain.Example;
using Jina.Domain.Service.Infra;
using Jina.Domain.Shared;
using Jina.Session.Abstract;
using QuestPDF.Fluent;

namespace Jina.Domain.Service.Example.Weather;


public class GetPdfWeathersService : ServiceImplBase<GetPdfWeathersService, PaginatedRequest<WeatherForecastRequest>, Results<byte[]>>
, IGetPdfWeathersService
{
    private readonly IGetWeathersService _getWeathersService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="pipe"></param>
    public GetPdfWeathersService(ISessionContext context, ServicePipeline pipe) : base(context, pipe)
    {
    }

    public override Task<bool> OnExecutingAsync()
    {
        return Task.FromResult(true);
    }

    public override async Task OnExecuteAsync()
    {
        PaginatedResult<WeatherForecastResult> result = null;
        
        this.Pipe.Register(_getWeathersService)
            .SetParameter(() => this.Request)
            .OnExecuted(r => result = r);

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