using eXtensionSharp;
using Jina.Base.Service;
using Jina.Domain.Abstract.Net;
using Jina.Domain.Net;
using Jina.Domain.Service.Infra;

namespace Jina.Domain.Service.Net;

/// <summary>
/// 
/// </summary>
public class EmailJob : JobBase<EmailRequest>
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

    public override async Task ExecuteAsync(EmailRequest request)
    {
        this.Spl.Register(_emailService)
            .When(request.xIsNotEmpty)
            .WithParameter(() => request)
            .Then(r => _ = r);

        await this.Spl.ExecuteAsync();
    }
}