using Ardalis.SmartEnum;

namespace Jina.Domain.Net.ExchangeRate.Enums;

public class ENUM_EXCHANGE_DATA_TYPE : SmartEnum<ENUM_EXCHANGE_DATA_TYPE>
{
    /// <summary>
    /// 환율
    /// </summary>
    public static readonly ENUM_EXCHANGE_DATA_TYPE AP01 = new ENUM_EXCHANGE_DATA_TYPE(nameof(AP01), 1);
    /// <summary>
    /// 대출금리
    /// </summary>
    public static readonly ENUM_EXCHANGE_DATA_TYPE AP02 = new ENUM_EXCHANGE_DATA_TYPE(nameof(AP01), 1);
    /// <summary>
    /// 국제금리
    /// </summary>
    public static readonly ENUM_EXCHANGE_DATA_TYPE AP03 = new ENUM_EXCHANGE_DATA_TYPE(nameof(AP01), 1);
    
    public ENUM_EXCHANGE_DATA_TYPE(string name, int value) : base(name, value)
    {
    }
}

public class ENUM_EXCHANGE_RESULT_TYPE : SmartEnum<ENUM_EXCHANGE_RESULT_TYPE>
{
    /// <summary>
    /// 성공
    /// </summary>
    public static readonly ENUM_EXCHANGE_RESULT_TYPE SUCCESS = new ENUM_EXCHANGE_RESULT_TYPE(nameof(SUCCESS), 1);
    
    /// <summary>
    /// DATA코드 오류
    /// </summary>
    public static readonly ENUM_EXCHANGE_RESULT_TYPE ERROR = new ENUM_EXCHANGE_RESULT_TYPE(nameof(ERROR), 2);
    
    /// <summary>
    /// 인증코드 오류
    /// </summary>
    public static readonly ENUM_EXCHANGE_RESULT_TYPE NOT_AUTH = new ENUM_EXCHANGE_RESULT_TYPE(nameof(NOT_AUTH), 3);
    
    /// <summary>
    /// 일일제한횟수 마감
    /// </summary>
    public static readonly ENUM_EXCHANGE_RESULT_TYPE LIMIT = new ENUM_EXCHANGE_RESULT_TYPE(nameof(LIMIT), 4);
    
    public ENUM_EXCHANGE_RESULT_TYPE(string name, int value) : base(name, value)
    {
    }
}
