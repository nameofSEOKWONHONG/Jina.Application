using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Jina.Domain.Entity.Base;

public interface IModelBuilder
{
    void Build(ModelBuilder builder);
}