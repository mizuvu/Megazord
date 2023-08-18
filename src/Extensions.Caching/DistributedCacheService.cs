using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace Zord.Extensions.Caching;

public class DistributedCacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly ILogger<DistributedCacheService> _logger;

    public DistributedCacheService(IDistributedCache cache, ILogger<DistributedCacheService> logger) =>
        (_cache, _logger) = (cache, logger);

    public T Get<T>(string key) =>
        Get(key) is { } data
            ? JsonSerializer.Deserialize<T>(Encoding.Default.GetString(data))
            : default;

    private byte[] Get(string key)
    {
        ArgumentNullException.ThrowIfNull(key);

        try
        {
            return _cache.Get(key);
        }
        catch
        {
            return null;
        }
    }

    public async Task<T> GetAsync<T>(string key, CancellationToken token = default) =>
        await GetAsync(key, token) is { } data
            ? JsonSerializer.Deserialize<T>(Encoding.Default.GetString(data))
            : default;

    private async Task<byte[]> GetAsync(string key, CancellationToken token = default)
    {
        try
        {
            return await _cache.GetAsync(key, token);
        }
        catch
        {
            return null;
        }
    }

    public void Refresh(string key)
    {
        try
        {
            _cache.Refresh(key);
            _logger.LogInformation("Cache refreshed: {key}", key);
        }
        catch
        {
        }
    }

    public async Task RefreshAsync(string key, CancellationToken token = default)
    {
        try
        {
            await _cache.RefreshAsync(key, token);
            _logger.LogInformation("Cache refreshed: {key}", key);
        }
        catch
        {
        }
    }

    public void Remove(string key)
    {
        try
        {
            _cache.Remove(key);
            _logger.LogInformation("Cache removed: {key}", key);
        }
        catch
        {
        }
    }

    public async Task RemoveAsync(string key, CancellationToken token = default)
    {
        try
        {
            await _cache.RemoveAsync(key, token);
            _logger.LogInformation("Cache removed: {key}", key);
        }
        catch
        {
        }
    }

    public void Set<T>(string key, T value, TimeSpan? slidingExpiration = null) =>
        Set(key, Encoding.Default.GetBytes(JsonSerializer.Serialize(value)), slidingExpiration);

    private void Set(string key, byte[] value, TimeSpan? slidingExpiration = null)
    {
        try
        {
            var options = new DistributedCacheEntryOptions();
            options.SetSlidingExpiration(slidingExpiration ?? TimeSpan.FromMinutes(30)); // Default expiration time is 30 minutes.

            _cache.Set(key, value, options);
            _logger.LogInformation("Cache loaded: {key}", key);
        }
        catch
        {
        }
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default) =>
        SetAsync(key, Encoding.Default.GetBytes(JsonSerializer.Serialize(value)), slidingExpiration, cancellationToken);

    private async Task SetAsync(string key, byte[] value, TimeSpan? slidingExpiration = null, CancellationToken token = default)
    {
        try
        {
            var options = new DistributedCacheEntryOptions();
            options.SetSlidingExpiration(slidingExpiration ?? TimeSpan.FromMinutes(30)); // Default expiration time is 30 minutes.

            await _cache.SetAsync(key, value, options, token);

            _logger.LogInformation("Cache loaded: {key}", key);
        }
        catch
        {
        }
    }
}