using Microsoft.EntityFrameworkCore;
using Zord.Core.Repositories;
using Zord.Extensions.Caching;
using Zord.Extensions.Logging;

namespace Zord.EntityFrameworkCore.Cache;

public abstract class CacheRepositoryBase<T> : RepositoryBase<T>, ICacheRepository<T>
    where T : class
{
    private readonly string _tableName = typeof(T).Name;
    private readonly ICacheService _cacheService;
    private readonly ILogger _logger;

    public CacheRepositoryBase(DbContext context,
        ICacheService cacheService,
        ILogger logger) : base(context)
    {
        _cacheService = cacheService;
        _logger = logger;
    }

    private async Task<IEnumerable<T>> LoadDataToCacheAsync(CancellationToken cancellationToken = default)
    {
        await _cacheService.RemoveAsync(_tableName, cancellationToken);

        var data = await base.GetAllAsync(cancellationToken);

        await _cacheService.SetAsync(_tableName, data, cancellationToken: cancellationToken);

        _logger.Information("Table {name} loaded to cache", _tableName);

        return data;
    }

    public override async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default, bool tracking = false)
    {
        var data = await _cacheService.GetAsync<IEnumerable<T>>(_tableName, cancellationToken);

        data ??= await LoadDataToCacheAsync(cancellationToken);

        return data;
    }

    public override int SaveChanges()
    {
        var result = base.SaveChanges();
        Task.Run(async () => await ReloadCacheAsync());
        return result;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var result = await base.SaveChangesAsync(cancellationToken);
        await ReloadCacheAsync(cancellationToken: cancellationToken);
        return result;
    }


    public async Task ReloadCacheAsync(CancellationToken cancellationToken = default)
    {
        await LoadDataToCacheAsync(cancellationToken);
    }

    public virtual async Task RemoveCacheAsync(CancellationToken cancellationToken = default)
    {
        await _cacheService.RemoveAsync(_tableName, cancellationToken);
    }
}
