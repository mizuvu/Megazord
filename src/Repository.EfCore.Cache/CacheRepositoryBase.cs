using Zord.Extensions.Caching;

namespace Zord.Repository;

public abstract class CacheRepositoryBase<T> : RepositoryBase<T>, ICacheRepository<T>
    where T : class
{
    private readonly ICacheService _cacheService;
    private readonly DbContext _context;

    protected CacheRepositoryBase(DbContext context, ICacheService cacheService) : base(context)
    {
        _context = context;
        _cacheService = cacheService;
    }

    public virtual string DatabaseName => _context.Database.GetDbConnection().Database;

    public virtual string BuildCacheKey() => $"[{DatabaseName}]_[{typeof(T).Name}]";

    private async Task<IEnumerable<T>> LoadDataToCacheAsync(CancellationToken cancellationToken = default)
    {
        var key = BuildCacheKey();

        await _cacheService.RemoveAsync(key, cancellationToken);

        var data = await base.ToListAsync(cancellationToken);

        await _cacheService.SetAsync(key, data, cancellationToken: cancellationToken);

        return data;
    }

    public override async Task<IEnumerable<T>> ToListAsync(CancellationToken cancellationToken = default)
    {
        var key = BuildCacheKey();
        
        var data = await _cacheService.GetAsync<IEnumerable<T>>(key, cancellationToken);

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
        var key = BuildCacheKey();
        await _cacheService.RemoveAsync(key, cancellationToken);
    }
}
