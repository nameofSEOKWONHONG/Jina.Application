using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Jina.Domain.Entity.Account;

public class RoleClaim : IdentityRoleClaim<string>
{
    [MaxLength(5)]
    public string TenantId { get; set; }

    public Tenant Tenant { get; set; }

    [MaxLength(4000), Comment("설명")]
    public string Description { get; set; }

    [MaxLength(140), Comment("그룹명")]
    public string Group { get; set; }

    [Required, MaxLength(140), Comment("생성자")]
    public string CreatedBy { get; set; }

    [Required, Comment("생성일")]
    public DateTime CreatedOn { get; set; }

    [MaxLength(140), Comment("수정자")]
    public string LastModifiedBy { get; set; }

    [Comment("수정일")]
    public DateTime? LastModifiedOn { get; set; }

    public virtual Role Role { get; set; }

    public static void EntityModelBuild(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<IdentityRoleClaim<string>>()
        //    .ToTable("RoleClaims", "Account");
    }
}