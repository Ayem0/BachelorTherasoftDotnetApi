using BachelorTherasoftDotnetApi.src.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

public class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
        {
            return base.SavingChangesAsync(
                eventData, result, cancellationToken);
        }

        IEnumerable<EntityEntry<BaseEntity>> entries =
            eventData
                .Context
                .ChangeTracker
                .Entries<BaseEntity>()
                .Where(e => e.State == EntityState.Deleted);

        foreach (EntityEntry<BaseEntity> softDeletable in entries)
        {
            softDeletable.State = EntityState.Modified;
            softDeletable.Entity.DeletedAt = DateTime.UtcNow;
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}