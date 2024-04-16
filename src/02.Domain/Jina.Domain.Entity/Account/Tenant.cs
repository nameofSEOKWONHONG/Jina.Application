using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Jina.Domain.Entity.Account;

[Table("Tenants", Schema = "dbo")]
public sealed class Tenant
{
    [Key, Column(Order = 0), MaxLength(5), Comment("테넌트 ID")]
    public string TenantId { get; set; }
    
    [Column(Order = 1), Comment("테넌트 명")]
    [MaxLength(400)]
    public string Name { get; set; }
    
    [Column(Order = 2), Comment("이동 주소 URL")]
    [MaxLength(1000)]
    public string RedirectUrl { get; set; }
    
    /// <summary>
    /// ex: "Korea Standard Time"
    /// </summary>
    [Column(Order = 3), Comment("시스템 시간")]
    [MaxLength(60)]
    public string TimeZone { get; set; }
}