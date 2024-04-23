using eXtensionSharp;
using Jina.Base.Service;
using Jina.Domain.Abstract.Net;
using Jina.Domain.Net;
using Jina.Domain.Service.Infra;
using Jina.Domain.SharedKernel.Abstract;

namespace Jina.Domain.Service.Net;

public class EmailJob : JobBase
{
    private readonly IEmailService _emailService;
    
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="spl"></param>
    /// <param name="emailService"></param>
    public EmailJob(ServicePipeline spl, IEmailService emailService) : base(spl)
    {
        _emailService = emailService;
    }

    public async Task ExecuteAsync(EmailRequest request)
    {
        IResults<bool> result;
        this.Spl.Register(_emailService)
            .AddFilter(request.xIsNotEmpty)
            .SetParameter(() => request)
            .OnExecuted(r => result = r);

        await this.Spl.ExecuteAsync();
    }
}