using Host.Data;
using Microsoft.AspNetCore.Mvc;
using Zord.EntityFrameworkCore;

namespace Host.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CacheDataController : ControllerBase
    {
        private readonly ICacheRepository<RetailLocation, AlphaDbContext> _cacheRepository;

        public CacheDataController(ICacheRepository<RetailLocation, AlphaDbContext> cacheRepository)
        {
            _cacheRepository = cacheRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {
            var list = await _cacheRepository.ToListAsync(cancellationToken);

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
    }
}
