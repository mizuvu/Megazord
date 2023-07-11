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
            var list = await _cacheRepository.GetAllAsync(cancellationToken);

            var bon = list.Where(x => x.Code == "BonGrocer").First();
            bon.Name = "---- 44444";

            _cacheRepository.Update(bon);
            _cacheRepository.SaveChanges();

            return Ok(list);
        }
    }
}
