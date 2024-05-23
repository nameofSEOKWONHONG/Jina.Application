using Jina.Domain.Entity.Base;
using Microsoft.EntityFrameworkCore;

namespace Jina.Domain.Entity.Common;

public class Sequence : TenantEntity
{
    public string TableName { get; set; }
    public int NextValue { get; set; } = 1;
}

public class SequenceModelBuilder: IModelBuilder
{
    public void Build(ModelBuilder builder)
    {
        builder.Entity<Sequence>(e =>
        {
            e.ToTable($"{nameof(Sequence)}s", "common");
            e.HasKey(m => new { m.TenantId, m.TableName, m.NextValue });
            e.Property(m => m.TableName)
                .HasMaxLength(100);
            e.Property(m => m.TenantId)
                .HasMaxLength(5);
        });
    }
}