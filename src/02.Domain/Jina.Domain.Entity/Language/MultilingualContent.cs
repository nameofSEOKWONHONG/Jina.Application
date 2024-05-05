using eXtensionSharp;
using Jina.Domain.Entity.Base;
using Microsoft.EntityFrameworkCore;

namespace Jina.Domain.Entity.Language;

/// <summary>
/// 다국어 번역 저장
/// </summary>
public class MultilingualContent : NumberEntity
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
    
    public MultilingualTopic MultilingualTopic { get; set; }
    public string MultilingualTopicTenantId { get; set; }
    public int MultilingualTopicId { get; set; }
    public string MultilingualTopicPrimaryCultureType { get; set; }    
}

public class MultilingualContentModelBuilder : IModelBuilder
{
    public void Build(ModelBuilder builder)
    {
        builder.Entity<MultilingualContent>(e =>
        {
            e.ToTable($"{nameof(MultilingualContent)}s", "language");
            e.HasKey(m => new { m.TenantId, m.Id, m.CultureType});
            e.Property(m => m.Id)
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
            e.HasOne(m => m.MultilingualTopic)
                .WithMany(m => m.MultilingualContents)
                .HasForeignKey(m => new {m.MultilingualTopicTenantId, m.MultilingualTopicId, m.MultilingualTopicPrimaryCultureType })
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}