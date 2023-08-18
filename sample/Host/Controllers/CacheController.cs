using Microsoft.AspNetCore.Mvc;
using Zord.Extensions.Caching;

namespace Host.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private const string _cacheKey = "CacheValues";

        private readonly ICacheService _cacheService;

        public CacheController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var data = await GetDataOnCache();
            return Ok(data);
        }

        private async Task<Dictionary<string, int>> LoadDataToCache()
        {
            var data = CacheData();
            await _cacheService.SetAsync(_cacheKey, data);
            return data;
        }

        private async Task<Dictionary<string, int>> GetDataOnCache()
        {
            var data = await _cacheService.GetAsync<Dictionary<string, int>>(_cacheKey);
            data ??= await LoadDataToCache();
            return data;
        }

        private static Dictionary<string, int> CacheData()
        {
            var data = new Dictionary<string, int>();
            for (var i = 1; i <= 10; i++)
            {
                data.Add($"Key{i}", i);
            }

            return data;
        }
    }
}
