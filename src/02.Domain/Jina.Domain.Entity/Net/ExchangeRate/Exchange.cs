using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Jina.Domain.Entity.Base;

namespace Jina.Domain.Entity.Net.ExchangeRate;

/// <summary>
/// <see href="link">https://www.koreaexim.go.kr/ir/HPHKIR020M01?apino=2&viewtype=C&searchselect=&searchword=</see>
/// 환율
/// </summary>
public class Exchange : EntityCore
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public string Code { get; set; }
    public string CurrencyCode { get; set; }
    public string CurrencyName { get; set; }
    public string Country { get; set; }
    public string Name { get; set; }
    public string Date { get; set; }
    public string Time { get; set; }
    public int RecurrenceCount { get; set; }
    public int BasePrice { get; set; }
    public int OpeningPrice { get; set; }
    public double HighPrice { get; set; }
    public double LowPrice { get; set; }
    public string Change { get; set; }
    public double ChangePrice { get; set; }
    public double CashBuyingPrice { get; set; }
    public double CashSellingPrice { get; set; }
    public int TtBuyingPrice { get; set; }
    public int TtSellingPrice { get; set; }
    public object TcBuyingPrice { get; set; }
    public object FcSellingPrice { get; set; }
    public double ExchangeCommission { get; set; }
    public int UsDollarRate { get; set; }
    public double High52wPrice { get; set; }
    public string High52wDate { get; set; }
    public double Low52wPrice { get; set; }
    public string Low52wDate { get; set; }
    public int CurrencyUnit { get; set; }
    public string Provider { get; set; }
    public long Timestamp { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedAt { get; set; }
    public double SignedChangePrice { get; set; }
    public double SignedChangeRate { get; set; }
    public double ChangeRate { get; set; }
}