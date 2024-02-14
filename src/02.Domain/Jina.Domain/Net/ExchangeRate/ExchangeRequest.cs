using Jina.Domain.Net.ExchangeRate.Enums;

namespace Jina.Domain.Net.ExchangeRate;

public class ExchangeRequest
{
    public string AuthKey { get; set; }
    public DateTime SearchDate { get; set; }
    public ENUM_EXCHANGE_DATA_TYPE SearchType { get; set; }
} 