using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Jina.Domain.Entity.Base;
using Microsoft.EntityFrameworkCore;

namespace Jina.Domain.Entity.Application;

[Table("MenuGroups", Schema = "application")]
public class MenuGroup : TenantBase
{
    public Guid MenuGroupId { get; set; }
    
    [Required, MaxLength(4000)]
    public string Name { get; set; }

    [Required, MaxLength(4000)]
    public string Icon { get; set; }
    
    public bool IsVisible { get; set; } = true;
    
    public Guid MenuRoleId { get; set; }
    public virtual MenuRole MenuRole { get; set; }
    
    public virtual ICollection<Menu> Menus { get; set; }

    [Required]
    public int SortNo { get; set; }
}

public class MenuGroupModelBuilder : IModelBuilder
{
    public void Build(ModelBuilder builder)
    {
        builder.Entity<MenuGroup>(entity =>
        {   
            entity.HasKey(e => new {e.TenantId, e.MenuGroupId}); // 기본 키 설정
            
            entity.Property(e => e.MenuGroupId)
                .ValueGeneratedOnAdd()
                .IsRequired(); // 기본 키 속성 설정
            
            entity.Property(e => e.Name)
                .HasMaxLength(4000)
                .IsRequired(); // Name 속성 설정
            
            entity.Property(e => e.Icon)
                .HasMaxLength(4000)
                .IsRequired(); // Icon 속성 설정
            
            entity.Property(e => e.IsVisible)
                .IsRequired(); // IsVisible 속성 설정
            
            entity.Property(e => e.SortNo)
                .IsRequired(); // SortNo 속성 설정

            entity.HasOne(e => e.MenuRole)
                .WithMany(e => e.MenuGroups)
                .HasForeignKey(e => new {e.TenantId, e.MenuRoleId})
                .OnDelete(DeleteBehavior.Restrict); // MenuRole 외래 키 설정

            entity.HasMany(e => e.Menus)
                .WithOne(e => e.MenuGroup)
                .HasForeignKey(e => new {e.TenantId, e.MenuGroupId})
                .OnDelete(DeleteBehavior.Cascade); // Menus 컬렉션과의 관계 설정
        });
    }
}