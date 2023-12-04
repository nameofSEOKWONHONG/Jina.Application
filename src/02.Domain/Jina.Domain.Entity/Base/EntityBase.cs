using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Jina.Domain.Entity.Base;

public abstract class EntityBase
{
    [Key, Column(Order = 0), Comment("테넌트 ID")]
    [Required, MaxLength(5)]
    public string TenantId { get; set; }

    [Comment("활성화 여부")]
    public bool IsActive { get; set; }

    [Required, MaxLength(140), Comment("생성자")]
    public string CreatedBy { get; set; }

    [MaxLength(30)]
    public string CreatedName { get; set; }

    [Required, Comment("생성일")]
    public DateTime? CreatedOn { get; set; }

    [MaxLength(140), Comment("수정자")]
    public string LastModifiedBy { get; set; }

    [MaxLength(30)]
    public string LastModifiedName { get; set; }

    [Comment("수정일")]
    public DateTime? LastModifiedOn { get; set; }
}