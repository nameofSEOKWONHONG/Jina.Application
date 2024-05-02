using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Jina.Domain.Entity.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Jina.Domain.Entity.Account;

public class Role : IdentityRole, IAuditableEntity
{
    [MaxLength(5)]
    public string TenantId { get; set; }

    public Tenant Tenant { get; set; }

    [MaxLength(4000)]
    public string Description { get; set; }

    public bool IsActive { get; set; } = true;

    [Required, MaxLength(140), Comment("생성자")]
    public string CreatedBy { get; set; }

    [Required, Comment("생성일")]
    public DateTime CreatedOn { get; set; }

    [MaxLength(140), Comment("수정자")]
    public string LastModifiedBy { get; set; }

    [Comment("수정일")]
    public DateTime? LastModifiedOn { get; set; }

    public virtual ICollection<RoleClaim> RoleClaims { get; set; }

	public Role() : base()
	{
		RoleClaims = new HashSet<RoleClaim>();
	}

	public Role(string tenantId, string roleName, string roleDescription = null) : base(roleName)
	{
		TenantId = tenantId;
		RoleClaims = new HashSet<RoleClaim>();
		Description = roleDescription;
	}

	public static void EntityModelBuild(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<IdentityRole>()
        //    .ToTable("Roles", "Account");
    }
}