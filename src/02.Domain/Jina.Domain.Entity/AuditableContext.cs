﻿using eXtensionSharp;
using Jina.Domain.Account;
using Jina.Domain.Entity.Account;
using Jina.Domain.Entity.Base;
using Jina.Session.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Jina.Domain.Entity;

public abstract class AuditableContext : IdentityDbContext<User, Role, string, IdentityUserClaim<string>,
    UserRole, IdentityUserLogin<string>, RoleClaim, IdentityUserToken<string>>
{
    protected readonly ISessionCurrentUser User;
    protected readonly ISessionDateTime Date;
    protected AuditableContext(DbContextOptions options, ISessionCurrentUser user,
        ISessionDateTime date) : base(options)
    {
        User = user;
        Date = date;
    }

    protected virtual async Task<int> SaveChangesAsync(string tenantId, string userId = null, CancellationToken cancellationToken = new())
    {
        var auditEntries = OnBeforeSaveChanges(tenantId, userId);
        var result = await base.SaveChangesAsync(cancellationToken);
        await OnAfterSaveChanges(auditEntries, cancellationToken);
        return result;
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Audit>(entity =>
        {
            entity.ToTable(name: "AuditTrails", "dbo");
        });
        base.OnModelCreating(builder);
    }
    
    private List<AuditEntry> OnBeforeSaveChanges(string tenantId, string userId)
    {
        ChangeTracker.DetectChanges();
        var auditEntries = new List<AuditEntry>();
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is Audit && (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged))
                continue;
            
            if (entry.Entity is not IAuditableEntity)
                continue;

            var auditEntry = new AuditEntry(entry)
            {
                TableName = entry.Entity.GetType().Name,
                TenantId = tenantId,
                UserId = userId
            };
            auditEntries.Add(auditEntry);
            foreach (var property in entry.Properties)
            {
                if (property.IsTemporary)
                {
                    auditEntry.TemporaryProperties.Add(property);
                    continue;
                }

                string propertyName = property.Metadata.Name;
                if (property.Metadata.IsPrimaryKey())
                {
                    auditEntry.KeyValues[propertyName] = property.CurrentValue;
                    continue;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        auditEntry.AuditType = ENUM_AUDIT_TYPE.Create;
                        auditEntry.NewValues[propertyName] = property.CurrentValue;
                        break;

                    case EntityState.Deleted:
                        auditEntry.AuditType = ENUM_AUDIT_TYPE.Delete;
                        auditEntry.OldValues[propertyName] = property.OriginalValue;
                        break;

                    case EntityState.Modified:
                        if (property.IsModified && property.OriginalValue?.Equals(property.CurrentValue) == false)
                        {
                            auditEntry.ChangedColumns.Add(propertyName);
                            auditEntry.AuditType = ENUM_AUDIT_TYPE.Update;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                        }
                        break;
                }
            }
        }
        foreach (var auditEntry in auditEntries.Where(_ => !_.HasTemporaryProperties && _.AuditType.xIsNotEmpty()))
        {
            AuditTrails.Add(auditEntry.ToAudit());
        }
        return auditEntries.Where(_ => _.HasTemporaryProperties).ToList();
    }
            
    private Task OnAfterSaveChanges(List<AuditEntry> auditEntries, CancellationToken cancellationToken = new())
    {
        if (auditEntries == null || auditEntries.Count == 0)
            return Task.CompletedTask;

        foreach (var auditEntry in auditEntries)
        {
            foreach (var prop in auditEntry.TemporaryProperties)
            {
                if (prop.Metadata.IsPrimaryKey())
                {
                    auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                }
                else
                {
                    auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                }
            }
            AuditTrails.Add(auditEntry.ToAudit());
        }
        return SaveChangesAsync(cancellationToken);
    }

    public DbSet<Audit> AuditTrails { get; set; }
}
