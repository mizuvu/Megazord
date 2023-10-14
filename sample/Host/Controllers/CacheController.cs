﻿using Microsoft.AspNetCore.Mvc;
using Zord.Extensions.Caching;

namespace Host.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly string _cacheKey;

        private readonly Dictionary<string, int> _data;

        private readonly ICacheService _cacheService;

        public CacheController(ICacheService cacheService)
        {
            _cacheKey = "CacheValues";

            var data = new Dictionary<string, int>();
            for (var i = 1; i <= 10; i++)
            {
                data.Add($"Key{i}", i);
            }
            _data = data;

            _cacheService = cacheService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var res = await _cacheService.GetAsync<Dictionary<string, int>>(_cacheKey);
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> Set()
        {
            await _cacheService.SetAsync(_cacheKey, _data);
            return Ok();
        }

        [HttpGet("try")]
        public async Task<IActionResult> TryGet()
        {
            var res = await _cacheService.TryGetAsync<Dictionary<string, int>>(_cacheKey);
            return Ok(res);
        }

        [HttpPost("try")]
        public async Task<IActionResult> TrySet()
        {
            await _cacheService.TrySetAsync(_cacheKey, _data);
            return Ok();
        }
    }
}
