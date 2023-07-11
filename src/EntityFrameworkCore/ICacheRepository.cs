namespace Zord.EntityFrameworkCore;

public interface ICacheRepository<TEntity, TContext> : ICacheRepository<TEntity>
    where TEntity : class
    where TContext : DbContext
{
}