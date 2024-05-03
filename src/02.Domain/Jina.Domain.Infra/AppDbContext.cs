﻿using Jina.Domain.Entity.Account;
using Jina.Domain.Entity.Application;
using Jina.Domain.Entity.Common;
using Jina.Domain.Entity.Example;
using Jina.Session.Abstract;
using Microsoft.EntityFrameworkCore;
using SmartEnum.EFCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;
using eXtensionSharp;
using Jina.Database.Abstract;
using Jina.Domain.Entity;
using Jina.Domain.Entity.Base;

namespace Jina.Domain.Service.Infra;

public class AppDbContext : AuditableContext, IDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options
     , ISessionCurrentUser user
     , ISessionDateTime date
    ) : base(options, user, date)
    {
        
    }

    public AppDbContext(string connection 
        , ISessionCurrentUser user
        , ISessionDateTime date) : base(CreateOption(connection), user, date)
    {
    }

	private static DbContextOptions CreateOption(string connection)
    {
        return new DbContextOptionsBuilder().UseSqlServer(connection).Options;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
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

        #region [tenant setting]

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var type = entity.ClrType;
            if(typeof(ITenantBase).IsAssignableFrom(type))
            {
                var método = typeof(AppDbContext)
                    .GetMethod(nameof(SetFiltroGlobalTenant),
                        BindingFlags.NonPublic | BindingFlags.Static
                    )?.MakeGenericMethod(type);

                var filtro = método?.Invoke(null, new object[] { this })!;
                entity.SetQueryFilter((LambdaExpression)filtro);
                entity.AddIndex(entity.FindProperty(nameof(ITenantBase.TenantId))!);
            }
        }        

        #endregion

        #region [modelbuilder setting]

        var types = Jina.Domain.Entity.PersistenceAssembly.Assembly.GetTypes()
            .Where(t => typeof(IModelBuilder).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var type in types)
        {
            var modelBuilderInstance = (IModelBuilder)Activator.CreateInstance(type);
            modelBuilderInstance.Build(modelBuilder);
        }        

        #endregion

    }
    
    private static LambdaExpression SetFiltroGlobalTenant<TEntity>(
        AppDbContext context)
        where TEntity : class, ITenantBase
    {
        Expression<Func<TEntity, bool>> filtro = x => x.TenantId == context.User.TenantId;
        return filtro;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        if (User.TenantId.xIsEmpty())
            return await base.SaveChangesAsync(cancellationToken);
        
        var audit = ChangeTracker.Entries<IAuditableEntity>().FirstOrDefault();
        if (audit.xIsNotEmpty())
        {
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = Date.Now;
                        entry.Entity.CreatedBy = User.UserId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedOn = Date.Now;
                        entry.Entity.LastModifiedBy = User.UserId;
                        break;
                }
            }
        }
        
        foreach (var entry in ChangeTracker.Entries<TenantBase>().ToList())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.TenantId = User.TenantId;
                    entry.Entity.CreatedBy = User.UserId;
                    entry.Entity.CreatedName = User.UserName.vToAESEncrypt();
                    entry.Entity.CreatedOn = Date.Now;
                    entry.Entity.IsActive = true;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = User.UserId;
                    entry.Entity.LastModifiedName = User.UserName.vToAESEncrypt();
                    entry.Entity.LastModifiedOn = Date.Now;
                    break;
            }
        }
        
        return await base.SaveChangesAsync(User.TenantId, User.UserId, cancellationToken);
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
    
    // public DbSet<MultilingualConfig> MultilingualConfigs { get; set; }
    // public DbSet<MultilingualContent> MultilingualContents { get; set; }
}