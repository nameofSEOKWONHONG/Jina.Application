using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Jina.Domain.Entity.Base;
using Microsoft.EntityFrameworkCore;

namespace Jina.Domain.Entity.Application;

public class Menu : TenantEntity
{
    public Guid MenuId { get; set; }
    
    [MaxLength(4000)]
    public string Url { get; set; }
    
    [MaxLength(4000)]
    public string Title { get; set; }
    
    [MaxLength(4000)]
    public string Icon { get; set; }
    
    public int Level { get; set; }
    
    public bool IsVisible { get; set; } = true;
    
    public int SortNo { get; set; }
        
    public Guid MenuGroupId { get; set; }
    public virtual MenuGroup MenuGroup { get; set; }

    // 부모 메뉴
    public Guid? ParentMenuId { get; set; }
    public Menu ParentMenu { get; set; }

    // 자식 메뉴
    public ICollection<Menu> ChildMenus { get; set; }
}

public class MenuModelBuilder : IModelBuilder
{
    public void Build(ModelBuilder builder)
    {
        builder.Entity<Menu>(entity =>
        {
            entity.ToTable($"{nameof(Menu)}s", "application");
            
            entity.HasKey(e => new {e.TenantId, e.MenuId}); // 기본 키 설정

            entity.Property(e => e.MenuId)
                .ValueGeneratedOnAdd()
                .IsRequired(); // 기본 키 속성 설정

            entity.Property(e => e.Url)
                .HasMaxLength(4000)
                .IsRequired(); // Url 속성 설정

            entity.Property(e => e.Title)
                .HasMaxLength(4000)
                .IsRequired(); // Title 속성 설정

            entity.Property(e => e.Icon)
                .HasMaxLength(4000)
                .IsRequired(); // Icon 속성 설정

            entity.Property(e => e.Level)
                .IsRequired(); // Level 속성 설정

            entity.Property(e => e.IsVisible)
                .IsRequired(); // IsVisible 속성 설정

            entity.Property(e => e.SortNo)
                .IsRequired(); // SortNo 속성 설정

            entity.HasOne(e => e.ParentMenu)
                .WithMany(e => e.ChildMenus)
                .HasForeignKey(e => new {e.TenantId, e.ParentMenuId})
                .OnDelete(DeleteBehavior.Restrict); // 부모 메뉴 외래 키 설정
            
            entity.HasOne(e => e.MenuGroup)
                .WithMany(e => e.Menus)
                .HasForeignKey(e => new {e.TenantId, e.MenuGroupId})
                .OnDelete(DeleteBehavior.Restrict); // MenuGroup 외래 키 설정
        });
    }
}