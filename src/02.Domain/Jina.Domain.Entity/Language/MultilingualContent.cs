using eXtensionSharp;
using Jina.Domain.Entity.Base;
using Microsoft.EntityFrameworkCore;

namespace Jina.Domain.Entity.Language;

/// <summary>
/// 다국어 번역 저장
/// </summary>
public class MultilingualContent : GuidEntity
{
    public string CultureType { get; set; }
    public string Input { get; set; }
    public virtual ICollection<MultilingualContentDetail> Details { get; set; }
}

public class MultilingualContentModelBuilder : IModelBuilder
{
    public void Build(ModelBuilder builder)
    {
        builder.Entity<MultilingualContent>(e =>
        {
            e.ToTable($"{nameof(MultilingualContent)}s", "language");
            e.HasKey(m => new { m.TenantId, Guid = m.Guid});
            e.Property(m => m.Guid)
                .ValueGeneratedOnAdd();
            e.Property(m => m.CultureType)
                .HasMaxLength(5)
                .IsRequired();
            e.Property(m => m.Input)
                .HasMaxLength(4000)
                .IsNullableType();
            e.HasMany(m => m.Details)
                .WithOne(m => m.Master)
                .HasForeignKey(m => new
                    { m.MasterTenantId, m.MasterGuid })
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}


