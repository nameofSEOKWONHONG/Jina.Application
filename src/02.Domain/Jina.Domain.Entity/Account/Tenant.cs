using System.ComponentModel.DataAnnotations.Schema;
using Jina.Domain.Entity.Base;
using Microsoft.EntityFrameworkCore;

namespace Jina.Domain.Entity.Account;

[Table("Tenants", Schema = "dbo")]
public sealed class Tenant
{
    public string TenantId { get; set; }
    
    public string Name { get; set; }
    
    public string RedirectUrl { get; set; }
    
    /// <summary>
    /// ex: "Korea Standard Time"
    /// </summary>
    public string TimeZone { get; set; }
}

public class TenantModelBuilder : IModelBuilder
{
    public void Build(ModelBuilder builder)
    {
        builder.Entity<Tenant>()
            .HasKey(m => m.TenantId);
        builder.Entity<Tenant>()
            .Property(m => m.TenantId)
            .HasMaxLength(5)
            .HasColumnOrder(0)
            .HasComment("테넌트ID");
        builder.Entity<Tenant>()
            .Property(m => m.Name)
            .HasMaxLength(400)
            .HasColumnOrder(1)
            .HasComment("테넌트명");
        builder.Entity<Tenant>()
            .Property(m => m.RedirectUrl)
            .HasMaxLength(1000)
            .HasColumnOrder(2)
            .HasComment("리다이렉트 url");
        builder.Entity<Tenant>()
            .Property(m => m.TimeZone)
            .HasMaxLength(60)
            .HasColumnOrder(3)
            .HasComment("시간대");
    }
}