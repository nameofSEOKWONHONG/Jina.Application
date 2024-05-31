using System.ComponentModel.DataAnnotations.Schema;
using Jina.Domain.Entity.Base;
using Microsoft.EntityFrameworkCore;

namespace Jina.Domain.Entity.Common;

[Table("Tags", Schema = "common")]
public class Tag : GuidEntity
{
    public string TableName { get; set; }
    public Guid TagId { get; set; }
}

public class TagModelBuilder : IModelBuilder
{
    public void Build(ModelBuilder builder)
    {
        builder.Entity<Tag>(e =>
        {
            e.ToTable("common.Tags");
            e.HasKey(m => new { m.TenantId, m.Guid });
            e.Property(m => m.TenantId)
                .HasMaxLength(5)
                ;
            e.Property(m => m.TableName)
                .HasMaxLength(100)
                .IsRequired()
                ;
            e.Property(m => m.TagId)
                .IsRequired()
                ;
        });
    }
}