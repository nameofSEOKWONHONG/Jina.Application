using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Jina.Domain.Entity.Base;

namespace Jina.Domain.Entity.Application;

[Table("MenuGroups", Schema = "Application")]
public class MenuGroup : NumberEntityBase
{
    [Required, MaxLength(4000)]
    public string Name { get; set; }

    [Required, MaxLength(4000)]
    public string Icon { get; set; }
    
    public bool IsVisible { get; set; } = true;
    
    public virtual MenuRole MenuRole { get; set; }
    
    public virtual ICollection<Menu> Menus { get; set; }

    [Required]
    public int SortNo { get; set; }
}