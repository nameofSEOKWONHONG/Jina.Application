using Ardalis.SmartEnum;

namespace Jina.Domain.Net.ExchangeRate.Enums;

public class ENUM_EXCHANGE_DATA_TYPE : SmartEnum<ENUM_EXCHANGE_DATA_TYPE>
{
    /// <summary>
    /// 환율
    /// </summary>
    private static ENUM_EXCHANGE_DATA_TYPE AP01 = new ENUM_EXCHANGE_DATA_TYPE(nameof(AP01), 1);
    /// <summary>
    /// 대출금리
    /// </summary>
    private static ENUM_EXCHANGE_DATA_TYPE AP02 = new ENUM_EXCHANGE_DATA_TYPE(nameof(AP01), 1);
    /// <summary>
    /// 국제금리
    /// </summary>
    private static ENUM_EXCHANGE_DATA_TYPE AP03 = new ENUM_EXCHANGE_DATA_TYPE(nameof(AP01), 1);
    
    public ENUM_EXCHANGE_DATA_TYPE(string name, int value) : base(name, value)
    {
    }
}
