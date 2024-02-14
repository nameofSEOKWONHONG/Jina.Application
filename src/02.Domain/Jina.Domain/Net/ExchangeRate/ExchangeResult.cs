using System.Text.Json.Serialization;

namespace Jina.Domain.Net.ExchangeRate;


/// <summary>
/// <see href="link">https://www.koreaexim.go.kr/ir/HPHKIR020M01?apino=2&viewtype=C&searchselect=&searchword=</see>
/// 환율 반환 결과
/// </summary>
public class ExchangeResult
{
    [JsonPropertyName("result")]
    public int Result { get; set; }
    [JsonPropertyName("cur_unit")]
    public string CurUnit { get; set; }
    [JsonPropertyName("ttb")]
    public string Ttb { get; set; }
    [JsonPropertyName("tts")]
    public string Tts { get; set; }
    [JsonPropertyName("deal_bas_r")]
    public string DealBasR { get; set; }
    [JsonPropertyName("bkpr")]
    public string Bkpr { get; set; }
    [JsonPropertyName("yy_efee_r")]
    public string YyEfeeR { get; set; }
    [JsonPropertyName("ten_dd_efee_r")]
    public string TenDdEfeeR { get; set; }
    [JsonPropertyName("kftc_bkpr")]
    public string KftcBkpr { get; set; }
    [JsonPropertyName("kftc_deal_bas_r")]
    public string KftcDealBasR { get; set; }
    [JsonPropertyName("cur_nm")]
    public string CurNm { get; set; }
}