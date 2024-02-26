﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Sample.Data;
using Sample.Models;
using Zord.Exceptions;
using Zord.Extensions;

namespace Sample.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestController(
        IMemoryCache memoryCache,
        AlphaDbContext context) : ControllerBase
    {
        private readonly string _cacheKey = "Key";

        [HttpGet("mem")]
        public async Task<IActionResult> GetAsync()
        {
            var memObj = await memoryCache.GetOrCreateAsync(_cacheKey,
                async factory =>
                {
                    var data = await context.RetailCategories.FirstAsync();
                    Console.WriteLine("Get data from DB");
                    return data;
                });

            return Ok(memObj);
        }

        [HttpGet("dis")]
        public IActionResult GetDisAsync()
        {
            return Ok();
        }

        [HttpGet("throw")]
        public IActionResult ThrowAsync()
        {
            throw new InternalServerErrorException("Server error");
        }
    }
}
