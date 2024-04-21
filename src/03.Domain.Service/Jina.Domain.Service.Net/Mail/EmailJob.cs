using eXtensionSharp;
using Jina.Base.Service;
using Jina.Domain.Abstract.Net;
using Jina.Domain.Net;
using Jina.Domain.SharedKernel.Abstract;

namespace Jina.Domain.Service.Net;

public class EmailJob
{
    private readonly ServicePipeline _svc;
    private readonly IEmailService _emailService;
    
    public EmailJob(ServicePipeline svc, IEmailService emailService)
    {
        _svc = svc;
        _emailService = emailService;
    }

    public async Task ExecuteAsync(EmailRequest request)
    {
        IResults<bool> result;
        _svc.Register(_emailService)
            .AddFilter(() => request.xIsNotEmpty())
            .SetParameter(() => request)
            .OnExecuted(r => result = r);

        await _svc.ExecuteAsync();
    }
}