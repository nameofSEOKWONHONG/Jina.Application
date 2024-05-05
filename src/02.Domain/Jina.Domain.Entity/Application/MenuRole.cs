using Jina.Domain.Entity.Base;
using Microsoft.EntityFrameworkCore;

namespace Jina.Domain.Entity.Application;

public class MenuRole : TenantEntity
{
    public Guid MenuRoleId { get; set; }
    
    public string RoleId { get; set; }

    public virtual ICollection<MenuGroup> MenuGroups { get; set; }
}

public class MenuRoleModelBuilder : IModelBuilder
{
    public void Build(ModelBuilder builder)
    {
        builder.Entity<MenuRole>(entity =>
        {
            entity.ToTable($"{nameof(MenuRole)}s", "application");
            
            entity.HasKey(e => new {e.TenantId, e.MenuRoleId}); // 기본 키 설정
            
            entity.Property(e => e.MenuRoleId)
                .ValueGeneratedOnAdd()
                .IsRequired(); // 기본 키 속성 설정
            
            entity.Property(e => e.RoleId)
                .HasMaxLength(450)
                .IsRequired(); // RoleId 속성 설정

            entity.HasMany(e => e.MenuGroups)
                .WithOne(e => e.MenuRole)
                .HasForeignKey(e => new {e.TenantId, e.MenuRoleId})
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}