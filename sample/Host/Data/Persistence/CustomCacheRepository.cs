using Zord.Extensions.Caching;
using Zord.Repository.Cache;

namespace Host.Data.Persistence;

public class CustomCacheRepository : CacheRepositoryBase<AlphaDbContext>
{
    public CustomCacheRepository(AlphaDbContext context, ICacheService cacheService) : base(context, cacheService)
    {
    }

    public override string CacheKey => "CustomDatabase";
}
