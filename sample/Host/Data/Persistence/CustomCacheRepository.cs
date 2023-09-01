using Microsoft.EntityFrameworkCore;
using Zord.Extensions.Caching;

namespace Host.Data.Persistence;

public class CustomCacheRepository<TDbContext> : CacheRepositoryBase<TDbContext>
    where TDbContext : DbContext
{
    public CustomCacheRepository(DbContext context, ICacheService cacheService) : base(context, cacheService)
    {
    }

    public override string DatabaseName => "CustomDatabase";
}
