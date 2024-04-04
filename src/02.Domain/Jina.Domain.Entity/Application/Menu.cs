using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Jina.Domain.Entity.Base;

namespace Jina.Domain.Entity.Application;

[Table("Menus", Schema = "application")]
public class Menu : NumberEntityBase
{
    [MaxLength(4000)]
    public string Url { get; set; }
    [MaxLength(4000)]
    public string Title { get; set; }
    [MaxLength(4000)]
    public string Icon { get; set; }
    public int Level { get; set; }
    public bool IsVisible { get; set; } = true;
    public int SortNo { get; set; }
        
    public virtual MenuGroup MenuGroup { get; set; }

    public virtual ICollection<Menu> SubMenus { get; set; }
}