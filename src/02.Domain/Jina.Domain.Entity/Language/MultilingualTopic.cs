using Jina.Domain.Entity.Base;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI;

namespace Jina.Domain.Entity.Language;

public class MultilingualTopic : NumberEntity
{
    /// <summary>
    /// 주제명
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 메인 입력 언어 타입
    /// </summary>
    public string PrimaryCultureType { get; set; }
    
    public virtual ICollection<MultilingualContent> MultilingualContents { get; set; }
}

public class MultilingualTopicModelBuilder : IModelBuilder
{
    public void Build(ModelBuilder builder)
    {
        builder.Entity<MultilingualTopic>(e =>
        {
            e.ToTable($"{nameof(MultilingualTopic)}s", "language");
            e.HasKey(m => new { m.TenantId, m.Id, m.PrimaryCultureType });
            e.Property(m => m.Id)
                .ValueGeneratedOnAdd();
            e.Property(m => m.Name)
                .HasMaxLength(500)
                .IsRequired()
                .HasComment("번역 주제");
            e.Property(m => m.PrimaryCultureType)
                .HasMaxLength(5)
                .IsRequired()
                .HasComment("메인 입력 언어 타입");
            e.HasMany(e => e.MultilingualContents)
                .WithOne(e => e.MultilingualTopic)
                .HasForeignKey(e => new
                    { e.MultilingualTopicTenantId, e.MultilingualTopicId, e.MultilingualTopicPrimaryCultureType })
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}