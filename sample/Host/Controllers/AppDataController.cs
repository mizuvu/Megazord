using Host.Data;
using Microsoft.AspNetCore.Mvc;
using Zord.Core.Repositories;

namespace Host.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AppDataController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        //private readonly IRepository<RetailLocation> _locationRepo;

        public AppDataController(
            IUnitOfWork uow)
        //IRepository<RetailLocation> locationRepo)
        {
            _uow = uow;
            //_locationRepo = locationRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {
            var list = await _uow.Repository<RetailLocation>(true).ToListAsync(cancellationToken);
            //var list1 = await _locationRepo.GetAllAsync(cancellationToken);
            return Ok(list);
        }
    }
}
