using Jina.Validate;

namespace Jina.Domain.Notification;

public class SendMessageRequest
{
    public string Method { get; set; } = "ReceiveMessage";
    public string UserId { get; set; } = "test";
    public string Message { get; set; } = "test";
}

public class SendMessageRequestValidator : Validator<SendMessageRequest>
{
    public SendMessageRequestValidator()
    {
        NotEmpty(m => m.Method);
        NotEmpty(m => m.UserId);
        NotEmpty(m => m.Method);
    }
}