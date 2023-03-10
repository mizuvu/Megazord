using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Zord.EntityFrameworkCore
{
    public interface IDbFactory : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        ChangeTracker ChangeTracker { get; }

        DatabaseFacade Database { get; }

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
