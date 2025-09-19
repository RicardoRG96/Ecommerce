﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SharedKernel;

namespace Infrastructure.Persistence.Database.Interceptors
{
    public class AuditableEntityInterceptor : SaveChangesInterceptor
    {
        // private readonly IUserRepository _user;
        private readonly TimeProvider _dateTime;

        public AuditableEntityInterceptor(TimeProvider dateTime)
        {
            _dateTime = dateTime;
        }

        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventdata,
            InterceptionResult<int> result)
        {
            UpdateEntities(eventdata.Context);
            return base.SavingChanges(eventdata, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventdata,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventdata.Context);
            return base.SavingChangesAsync(eventdata, result, cancellationToken);
        }

        public void UpdateEntities(DbContext? context)
        {
            if (context == null) return;

            foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
            {
                if (entry.State is EntityState.Added or EntityState.Modified || entry.HasChangedOwnedEntities())
                {
                    var utcNow = _dateTime.GetUtcNow();
                    if (entry.State == EntityState.Added)
                    {
                        // entry.Entity.CreatedBy = _user.UserId;
                        entry.Entity.CreatedAt = utcNow;
                    }
                    // entry.Entity.LastModifiedBy = _user.UserId;
                    entry.Entity.LastModified = utcNow;
                }
            }
        }
    }

    public static class Extensions
    {
        public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
            entry.References.Any(r =>
                r.TargetEntry != null &&
                r.TargetEntry.Metadata.IsOwned() &&
                (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
    }
}
