using Microsoft.Extensions.Logging;
using Zord.Extensions.Caching;

namespace Zord.Specification.EntityFrameworkCore.Cache;

public class CacheRepository<TEntity, TContext> : CacheRepositoryBase<TEntity>, ICacheRepository<TEntity, TContext>
    where TEntity : class
    where TContext : DbContext
{
    public CacheRepository(TContext context, ICacheService cacheService, ILogger<ICacheService> logger)
        : base(context, cacheService, logger)
    {
    }
}