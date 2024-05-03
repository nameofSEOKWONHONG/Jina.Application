using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Jina.Domain.Entity.Base;

public abstract class EntityCore : IAuditableEntity
{
    [Required, MaxLength(140), Comment("생성자")]
    public string CreatedBy { get; set; }

    [MaxLength(1000)]
    public string CreatedName { get; set; }

    [Required, Comment("생성일")]
    public DateTime CreatedOn { get; set; }

    [MaxLength(140), Comment("수정자")]
    public string LastModifiedBy { get; set; }

    [MaxLength(1000)]
    public string LastModifiedName { get; set; }

    [Comment("수정일")]
    public DateTime? LastModifiedOn { get; set; }
}

public interface ITenantBase
{
    string TenantId { get; set; }
}

public class TenantBase : EntityCore, ITenantBase
{
    [Column(Order = 0), Comment("테넌트 ID")]
    [MaxLength(5)]
    public string TenantId { get; set; }

    [Comment("활성화 여부")]
    public bool IsActive { get; set; }
}

public class NumberBase : TenantBase
{
    [Column(Order = 1), DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
}