using eXtensionSharp;
using Jina.Domain.Entity.Base;
using Microsoft.EntityFrameworkCore;

namespace Jina.Domain.Entity.Language;

public class MultilingualContentDetail : GuidEntity
{
    public string CultureType { get; set; }
    
    /// <summary>
    /// 번역 TEXT
    /// </summary>
    public string Input { get; set; }
    
    /// <summary>
    /// ai 질의 기록
    /// </summary>
    public string Comment { get; set; }
    
    public virtual MultilingualContent Master { get; set; }
    public string MasterTenantId { get; set; }
    public Guid MasterGuid { get; set; }
}

public class MultilingualContentDetailModelBuilder : IModelBuilder
{
    public void Build(ModelBuilder builder)
    {
        builder.Entity<MultilingualContentDetail>(e =>
        {
            e.ToTable($"{nameof(MultilingualContentDetail)}s", "language");
            e.HasKey(m => new { m.TenantId, m.Guid});
            e.Property(m => m.Guid)
                .ValueGeneratedOnAdd();
            e.Property(m => m.CultureType)
                .HasMaxLength(5)
                .IsRequired();
            e.Property(m => m.Input)
                .HasMaxLength(4000)
                .IsNullableType();
            e.Property(m => m.Comment)
                .HasMaxLength(4000)
                .IsNullableType();
            e.HasOne(m => m.Master)
                .WithMany(m => m.Details)
                .HasForeignKey(m => new
                    { m.MasterTenantId, m.MasterGuid })
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}