namespace Jina.Domain.Entity.Base;

public interface IEntity<TId> : IEntity
{
    public TId Id { get; set; }
}

public interface IEntity
{
}