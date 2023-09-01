using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Zord.Extensions.Caching;

public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    private readonly ILogger<MemoryCacheService> _logger;

    public MemoryCacheService(IMemoryCache cache, ILogger<MemoryCacheService> logger) =>
        (_cache, _logger) = (cache, logger);

    public T Get<T>(string key) =>
        _cache.Get<T>(key);

    public Task<T> GetAsync<T>(string key, CancellationToken token = default) =>
        Task.FromResult(Get<T>(key));

    public void Refresh(string key)
    {
        _cache.TryGetValue(key, out _);

        _logger.LogInformation("Cache {key} refreshed", key);
    }


    public Task RefreshAsync(string key, CancellationToken token = default)
    {
        Refresh(key);

        return Task.CompletedTask;
    }

    public void Remove(string key)
    {
        _cache.Remove(key);

        _logger.LogInformation("Cache {key} removed", key);
    }

    public Task RemoveAsync(string key, CancellationToken token = default)
    {
        Remove(key);
        return Task.CompletedTask;
    }

    public void Set<T>(string key, T value, TimeSpan? slidingExpiration = null)
    {
        slidingExpiration ??= TimeSpan.FromMinutes(30); // Default expiration time is 30 minutes.

        _cache.Set(key, value, new MemoryCacheEntryOptions { SlidingExpiration = slidingExpiration });

        _logger.LogInformation("Cache {key} loaded", key);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? slidingExpiration = null, CancellationToken token = default)
    {
        Set(key, value, slidingExpiration);
        return Task.CompletedTask;
    }
}