using Jina.Domain.Entity.Base;
using Microsoft.EntityFrameworkCore;

namespace Jina.Domain.Entity.Common;

public class CodeGroup : TenantEntity
{
    public string GroupCode { get; set; }
    
    public string Key { get; set; }
    
    public string Value { get; set; }
    
    public string Desc { get; set; }
}

public class CodeGroupModelBuilder : IModelBuilder
{
    public void Build(ModelBuilder builder)
    {
        builder.Entity<CodeGroup>(entity =>
        {
            entity.ToTable($"{nameof(CodeGroup)}s", "common");
            entity.HasKey(m => new { m.TenantId, m.GroupCode, m.Key });
            entity.Property(m => m.TenantId)
                .HasMaxLength(5)
                .HasColumnOrder(0);
            entity.Property(m => m.GroupCode)
                .HasMaxLength(10)
                .HasColumnOrder(1);
            entity.Property(m => m.Key)
                .HasMaxLength(20)
                .HasColumnOrder(2);
            entity.Property(m => m.Value)
                .HasMaxLength(4000)
                .HasColumnOrder(3);
            entity.Property(m => m.Desc)
                .HasMaxLength(4000)
                .HasColumnOrder(4);
        });
    }
}