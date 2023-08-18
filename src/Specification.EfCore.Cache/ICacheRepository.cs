using Zord.Specification;

namespace Zord.Specification.EntityFrameworkCore.Cache;

public interface ICacheRepository<TEntity, TContext> : ICacheRepository<TEntity>
    where TEntity : class
    where TContext : DbContext
{
}