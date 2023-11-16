﻿using Zord.Extensions.Caching;

namespace Zord.Repository.Cache;

public class CacheRepository<TEntity, TContext> : CacheRepositoryBase<TEntity>, ICacheRepository<TEntity, TContext>
    where TEntity : class
    where TContext : DbContext
{
    public CacheRepository(TContext context, ICacheService cacheService) : base(context, cacheService)
    {
    }
}