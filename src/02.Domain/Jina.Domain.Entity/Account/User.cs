﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Jina.Domain.Entity.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Jina.Domain.Entity.Account;

[Table("dbo.AspNetUsers")]
public class User : IdentityUser<string>, IAuditableEntity
{
    [MaxLength(5)]
    public string TenantId { get; set; }

    public Tenant Tenant { get; set; }

    [MaxLength(100)]
    public string FirstName { get; set; }

    [MaxLength(100)]
    public string LastName { get; set; }

    [MaxLength(256)]
    public string RefreshToken { get; set; }

    [MaxLength(4000)]
    public string ProfilePictureDataUrl { get; set; }

	public DateTime RefreshTokenExpiryTime { get; set; }

    [Comment("활성화 여부")]
    public bool IsActive { get; set; }

    [Required, MaxLength(140), Comment("생성자")]
    public string CreatedBy { get; set; }

    [Required, Comment("생성일")]
    public DateTime CreatedOn { get; set; }

    [MaxLength(140), Comment("수정자")]
    public string LastModifiedBy { get; set; }

    [Comment("수정일")]
    public DateTime? LastModifiedOn { get; set; }

    public static void EntityModelBuild(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<IdentityUser>()
        //    .ToTable("Users", "Account");
    }
}