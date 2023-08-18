using Host.Data;
using Microsoft.AspNetCore.Mvc;
using Zord.EntityFrameworkCore.Cache;

namespace Host.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CacheDataController : ControllerBase
    {
        private readonly ICacheRepository<RetailLocation, AlphaDbContext> _cacheRepository;
        private readonly Serilog.ILogger _logger;

        public CacheDataController(
            ICacheRepository<RetailLocation, AlphaDbContext> cacheRepository,
            Serilog.ILogger logger)
        {
            _cacheRepository = cacheRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {
            var list = await _cacheRepository.ToListAsync(cancellationToken);
            _logger.Information("Hello......");
            return Ok(list);
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(CancellationToken cancellationToken)
        {
            var list = await _cacheRepository.ToListAsync(cancellationToken);

            var bon = list.Where(x => x.Code == "BonGrocer").First();
            bon.Name = "______";

            _cacheRepository.Update(bon);
            await _cacheRepository.SaveChangesAsync(cancellationToken);

            return Ok(list);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(CancellationToken cancellationToken)
        {
            await _cacheRepository.RemoveCacheAsync(cancellationToken);
            return Ok();
        }
    }
}
