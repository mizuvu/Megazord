using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Zord.Specification.EntityFrameworkCore;

/// <summary>
///     A DbContext instance represents a session with the database
/// </summary>
public interface IDbContext : ISaveChanges, IDisposable, IAsyncDisposable
{
    ChangeTracker ChangeTracker { get; }

    DatabaseFacade Database { get; }

    EntityEntry<TEntity> EntityEntry<TEntity>(TEntity entity) where TEntity : class;
}