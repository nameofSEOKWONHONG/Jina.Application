using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Jina.Domain.Entity.Base;

namespace Jina.Domain.Entity.Application;

[Table("MenuRoles", Schema = "application")]
public class MenuRole : EntityBase
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid MenuRoleId { get; set; }
    
    [Key, Column(Order = 2), MaxLength(450)]
    public string RoleId { get; set; }

    public virtual ICollection<MenuGroup> MenuGroups { get; set; }
}