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
    
    public int RESULT { get; set; }
    public string CUR_UNIT { get; set; }
    public string TTB { get; set; }
    public string TTS { get; set; }
    public string DEAK_BAS_R { get; set; }
    public string BKPR { get; set; }
    public string YY_EFEE_R { get; set; }
    public string TEB_DD_EFEE_R { get; set; }
    public string KFTC_BKPR { get; set; }
    public string KFTC_DEAL_BAS_R { get; set; }
    public string CUR_NM { get; set; }
}