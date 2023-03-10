namespace Zord.Extensions.Caching
{
    public class CacheConfiguration
    {
        public bool UseDistributedCache { get; set; }
        public bool PreferRedis { get; set; }
        public string? RedisURL { get; set; }
        public string? RedisPassword { get; set; }
    }
}