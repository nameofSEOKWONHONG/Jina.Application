using eXtensionSharp;
using Jina.Application.Configuration;
using Jina.Base.Service;
using Jina.Domain.Abstract.Net;
using Jina.Domain.Net;
using Jina.Domain.Service.Infra;
using Jina.Domain.SharedKernel;
using Jina.Domain.SharedKernel.Abstract;
using Jina.Session.Abstract;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Jina.Domain.Service.Net;

public class EmailService : ServiceImplBase<EmailService, MailRequest, IResultBase<bool>>, IEmailService
{
    /// <summary>
    /// white list states
    /// </summary>
    private readonly Dictionary<string, (string mimePartType, string mediaSubType)> _minePartTypes
        = new()
        {
            {
                ".xlsx", new ("application", "vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            },
            {
                ".png", new ("image", "png")
            },
            {
                ".jpg", new ("image", "jpg")
            }
        };
    
    private readonly MailConfigOption _config;
    
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="context"></param>
    /// <param name="svc"></param>
    public EmailService(ISessionContext context, ServicePipeline svc,
        IOptions<MailConfigOption> options) : base(context, svc)
    {
        _config = options.Value;
    }

    public override Task OnExecutingAsync()
    {
        return Task.CompletedTask;
    }

    public override async Task OnExecuteAsync()
    {
        if (this.Request.FromMailers.xIsEmpty())
        {
            this.Request.FromMailers = new List<MailSender>();
            this.Request.FromMailers.Add(new MailSender() {Name = _config.DisplayName, Mail = _config.From});
        }
        var message = CreateMessage(this.Request);
        using var smtp = new SmtpClient();
        if (this.Request.SmtpInfo.xIsEmpty())
        {
            await smtp.ConnectAsync(_config.Host, _config.Port, true);
            await smtp.AuthenticateAsync(_config.UserName, _config.Password);
        }
        else
        {
            if (this.Request.SmtpInfo.Host.ToLower().Contains("office") || this.Request.SmtpInfo.Host.ToLower().Contains("outlook"))
            {
                await smtp.ConnectAsync(this.Request.SmtpInfo.Host, this.Request.SmtpInfo.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(this.Request.SmtpInfo.LoginId, this.Request.SmtpInfo.Password);
            }
            else
            {
                await smtp.ConnectAsync(this.Request.SmtpInfo.Host, this.Request.SmtpInfo.Port, this.Request.SmtpInfo.UseSsl);
                await smtp.AuthenticateAsync(this.Request.SmtpInfo.LoginId, this.Request.SmtpInfo.Password);                    
            }
        }
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);

        this.Result = await ResultBase<bool>.SuccessAsync();
    }
    
    /// <summary>
    /// 발송 메세지 생성
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    private MimeMessage CreateMessage(MailRequest request)
    {
        var message = new MimeMessage();
        request.FromMailers.xForEach(item =>
        {
            message.From.Add(new MailboxAddress(item.Name, item.Mail));    
        });
        request.ToMailers.xForEach(item =>
        {
            message.To.Add(new MailboxAddress(item.Name, item.Mail));    
        });
        request.CcMailers.xForEach(item =>
        {
            message.Cc.Add(new MailboxAddress(item.Name, item.Mail));    
        });           
        
        message.Subject = request.Subject;
        if (!request.IsBodyHtml)
        {
            message.Body = new BodyBuilder()
            {
                TextBody = request.Body
            }.ToMessageBody();
        }
        else
        {
            message.Body = new BodyBuilder()
            {
                HtmlBody = request.Body
            }.ToMessageBody();
        };

        if (request.Files.xIsNotEmpty())
        {
            var multipart = new Multipart("mixed");
            multipart.Add(message.Body);
            
            request.Files.xForEach(item =>
            {
                var selectedMinePartType = _minePartTypes[item.FileName.xGetExtension()];
                var attachment = new MimePart (selectedMinePartType.mimePartType, selectedMinePartType.mediaSubType) {
                    Content = new MimeContent(new MemoryStream(item.File)),
                    ContentDisposition = new ContentDisposition (ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = item.FileName
                };
                multipart.Add(attachment);
            });     
            
            message.Body = multipart;
            return message;
        }
        
        return message;
    }        
}