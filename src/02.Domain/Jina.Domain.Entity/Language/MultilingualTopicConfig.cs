using Jina.Domain.Entity.Base;
using Microsoft.EntityFrameworkCore;

namespace Jina.Domain.Entity.Language;

public class MultilingualTopicConfig : NumberEntity
{
    /// <summary>
    /// 언어 문화권
    /// </summary>
    public string CultureType { get; set; }
    /// <summary>
    /// 정렬 순서
    /// </summary>
    public int SortNo { get; set; }
    
    public MultilingualTopic MultilingualTopic { get; set; }
    public string MultilingualTopicTenantId { get; set; }
    public int MultilingualTopicId { get; set; }
    public string MultilingualTopicPrimaryCultureType { get; set; }   
}

public class MultilingualTopicConfigModelBuilder : IModelBuilder
{
    public void Build(ModelBuilder builder)
    {
        builder.Entity<MultilingualTopicConfig>(e =>
        {
            e.ToTable($"{nameof(MultilingualTopicConfig)}s", "language");
            e.HasKey(m => new { m.TenantId, m.Id, m.CultureType });
            e.Property(m => m.Id)
                .ValueGeneratedOnAdd();
            e.Property(m => m.CultureType)
                .IsRequired()
                .HasMaxLength(5);
            e.HasOne(m => m.MultilingualTopic)
                .WithMany(m => m.MultilingualTopicConfigs)
                .HasForeignKey(m => new
                    { m.MultilingualTopicTenantId, m.MultilingualTopicId, m.MultilingualTopicPrimaryCultureType })
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}