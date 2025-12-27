using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Pos.Web.Shared.Abstractions;

namespace Pos.Web.Infrastructure.Persistence
{
    public sealed class AuditingInterceptor : SaveChangesInterceptor
    {
        // Can inject a CurrentUserService here to get the User ID from the Token

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            if (eventData.Context is not null)
            {
                UpdateAuditEntities(eventData.Context);
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void UpdateAuditEntities(DbContext context)
        {
            var entries = context.ChangeTracker.Entries<AuditableEntity>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedOnUtc = DateTime.UtcNow;
                    // entry.Entity.CreatedBy = _currentUserService.UserId; 
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.ModifiedOnUtc = DateTime.UtcNow;
                    // entry.Entity.ModifiedBy = _currentUserService.UserId;
                }
            }
        }
    }
}
