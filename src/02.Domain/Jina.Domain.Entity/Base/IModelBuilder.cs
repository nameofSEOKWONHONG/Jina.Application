using Microsoft.EntityFrameworkCore;

namespace Jina.Domain.Entity.Base;

public interface IModelBuilder
{
    void Build(ModelBuilder builder);
}