namespace Jina.Domain.Entity.Base;

public abstract class Entity : IAuditableEntity
{
    public string CreatedBy { get; set; }

    public string CreatedName { get; set; }

    public DateTime CreatedOn { get; set; }

    public string LastModifiedBy { get; set; }

    public string LastModifiedName { get; set; }

    public DateTime? LastModifiedOn { get; set; }
}

public interface ITenantEntity
{
    string TenantId { get; set; }
}

public abstract class TenantEntity : Entity, ITenantEntity
{
    public string TenantId { get; set; }

    public bool IsActive { get; set; }
}

public abstract class NumberEntity : TenantEntity
{
    public int Id { get; set; }
}

public abstract class GuidEntity : TenantEntity
{
    public Guid Guid { get; set; }
}