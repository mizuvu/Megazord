using Zord.Extensions.Caching;
using Zord.Extensions.Logging;

namespace Zord.EntityFrameworkCore.Cache;

public class CacheRepository<TEntity, TContext> : CacheRepositoryBase<TEntity>, ICacheRepository<TEntity, TContext>
    where TEntity : class
    where TContext : DbContext
{
    public CacheRepository(TContext context,
        ICacheService cacheService,
        ILogger logger) : base(context, cacheService, logger)
    {
    }
}
