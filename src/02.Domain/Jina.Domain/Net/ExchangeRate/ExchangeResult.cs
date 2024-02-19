using System.Text.Json.Serialization;

namespace Jina.Domain.Net.ExchangeRate;


/// <summary>
/// <see href="link">https://quotation-api-cdn.dunamu.com/v1/forex/recent?codes=FRX.KRWUSD</see>
/// 환율 반환 결과
/// </summary>
public class ExchangeResult
{
    public string Code { get; set; }
    public string CurrencyCode { get; set; }
    public string CurrencyName { get; set; }
    public string Country { get; set; }
    public string Name { get; set; }
    public string Date { get; set; }
    public string Time { get; set; }
    public int RecurrenceCount { get; set; }
    public double BasePrice { get; set; }
    public double OpeningPrice { get; set; }
    public double HighPrice { get; set; }
    public double LowPrice { get; set; }
    public string Change { get; set; }
    public double ChangePrice { get; set; }
    public double CashBuyingPrice { get; set; }
    public double CashSellingPrice { get; set; }
    public double TtBuyingPrice { get; set; }
    public double TtSellingPrice { get; set; }
    public double? TcBuyingPrice { get; set; }
    public double? FcSellingPrice { get; set; }
    public double ExchangeCommission { get; set; }
    public double UsDollarRate { get; set; }
    public double High52wPrice { get; set; }
    public string High52wDate { get; set; }
    public double Low52wPrice { get; set; }
    public string Low52wDate { get; set; }
    public int CurrencyUnit { get; set; }
    public string Provider { get; set; }
    public long Timestamp { get; set; }

    [JsonPropertyName("id")]
    public int Idx { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public double SignedChangePrice { get; set; }
    public double SignedChangeRate { get; set; }
    public double ChangeRate { get; set; }
}