using Jina.Domain.Entity.Account;
using Jina.Domain.Entity.Application;
using Jina.Domain.Entity.Common;
using Jina.Domain.Entity.Example;
using Jina.Session.Abstract;
using Microsoft.EntityFrameworkCore;
using SmartEnum.EFCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Jina.Database.Abstract;
using Jina.Domain.Entity.Language;

namespace Jina.Domain.Entity;

public class AppDbContext : AuditableContext, IDbContext
{
    private readonly ISessionCurrentUser _user;
    private readonly ISessionDateTime _date;

    public AppDbContext(DbContextOptions<AppDbContext> options
     , ISessionCurrentUser user
     , ISessionDateTime date
    ) : base(options)
    {
        _user = user;
        _date = date;
    }

    public AppDbContext(string connection) : base(CreateOption(connection))
    {
    }

	private static DbContextOptions CreateOption(string connection)
    {
        return new DbContextOptionsBuilder().UseSqlServer(connection).Options;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region [setting conversion smartenum]

        modelBuilder.ConfigureSmartEnum();

        #endregion [setting conversion smartenum]

        #region [setting decimal default]

        foreach (var property in modelBuilder.Model.GetEntityTypes()
                     .SelectMany(t => t.GetProperties())
                     .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetColumnType("decimal(18,6)");
        }

        #endregion [setting decimal default]

        #region [get all composite keys (entity decorated by more than 1 [Key] attribute]

        foreach (var entity in modelBuilder.Model.GetEntityTypes()
                     .Where(t =>
                         t.ClrType.GetProperties()
                             .Count(p => p.CustomAttributes.Any(a => a.AttributeType == typeof(KeyAttribute))) > 1))
        {
            // get the keys in the appropriate order
            var orderedKeys = entity.ClrType
                .GetProperties()
                .Where(p => p.CustomAttributes.Any(a => a.AttributeType == typeof(KeyAttribute)))
                .OrderBy(p =>
                    p.CustomAttributes.Single(x => x.AttributeType == typeof(ColumnAttribute))?
                        .NamedArguments?.Single(y => y.MemberName == nameof(ColumnAttribute.Order))
                        .TypedValue.Value ?? 0)
                .Select(x => x.Name)
                .ToArray();

            // apply the keys to the model builder
            modelBuilder.Entity(entity.ClrType).HasKey(orderedKeys);
        }

        #endregion [get all composite keys (entity decorated by more than 1 [Key] attribute]

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Tenant> Tenants { get; set; }

    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    public DbSet<Role> Roles { get; set; }
    public DbSet<RoleClaim> RoleClaims { get; set; }

    public DbSet<MenuRole> MenuRoles { get; set; }
    public DbSet<MenuGroup> MenuGroups { get; set; }
    public DbSet<Menu> Menus { get; set; }

    public DbSet<Code> Codes { get; set; }
    public DbSet<CodeGroup> CodeGroups { get; set; }

    public DbSet<WeatherForecast> WeatherForecasts { get; set; }
    
    public DbSet<MultilingualConfig> MultilingualConfigs { get; set; }
    public DbSet<MultilingualContent> MultilingualContents { get; set; }
}