namespace Zord.Repository;

/// <summary>
/// Cache repository with multi DbContext
/// </summary>
public interface ICacheRepository<TEntity, TContext> : ICacheRepository<TEntity>
    where TEntity : class
    where TContext : DbContext
{
}