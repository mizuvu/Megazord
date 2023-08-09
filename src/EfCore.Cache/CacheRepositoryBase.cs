using Zord.Core.Repositories;

namespace Zord.EntityFrameworkCore.Cache;

public abstract class CacheRepositoryBase<T> : RepositoryBase<T>, ICacheRepository<T>
    where T : class
{
    private readonly string _tableName;
    private readonly ICacheService _cacheService;
    private readonly Serilog.ILogger _logger;

    protected CacheRepositoryBase(DbContext context,
        ICacheService cacheService,
        Serilog.ILogger logger) : base(context)
    {
        var dbName = context.Database.GetDbConnection().Database;
        _tableName = $"[{dbName}]_[{typeof(T).Name}]";
        _cacheService = cacheService;
        _logger = logger;
    }

    private async Task<IEnumerable<T>> LoadDataToCacheAsync(CancellationToken cancellationToken = default)
    {
        await _cacheService.RemoveAsync(_tableName, cancellationToken);

        var data = await base.ToListAsync(cancellationToken);

        await _cacheService.SetAsync(_tableName, data, cancellationToken: cancellationToken);

        _logger.Information("{name} loaded to cache", _tableName);

        return data;
    }

    public override async Task<IEnumerable<T>> ToListAsync(CancellationToken cancellationToken = default)
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
