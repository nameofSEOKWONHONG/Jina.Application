using System.ComponentModel.DataAnnotations;

namespace Jina.Domain.Entity.Account;

public class Audit
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Type { get; set; }
    public string TableName { get; set; }
    public DateTime DateTime { get; set; }
    public string OldValues { get; set; }
    public string NewValues { get; set; }
    public string AffectedColumns { get; set; }
    public string PrimaryKey { get; set; }

    [MaxLength(5)]
    public string TenantId { get; set; }

    public virtual Tenant Tenant { get; set; }
}