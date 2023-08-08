using Host.Data;
using Microsoft.AspNetCore.Mvc;
using Zord.EntityFrameworkCore;
using Zord.Extensions;

namespace Host.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IUnitOfWork<AlphaDbContext> _unitOfWork;
        private readonly IRepository<RetailLocation, AlphaDbContext> _locationRepo;

        public DataController(
            IUnitOfWork<AlphaDbContext> unitOfWork,
            IRepository<RetailLocation, AlphaDbContext> locationRepo)
        {
            _unitOfWork = unitOfWork;
            _locationRepo = locationRepo;
        }

        [HttpGet("locations")]
        public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.Repository<RetailLocation>().ToListAsync(cancellationToken);
            return Ok(data);
        }

        [HttpGet("locations/{id}")]
        public async Task<IActionResult> GetAsync(string id, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.Repository<RetailLocation>().FindByKeyAsync(id, cancellationToken);

            data.ThrowIfNull();

            return Ok(data);
        }

        [HttpPut("locations")]
        public async Task<IActionResult> PutAsync(CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.Repository<RetailLocation>().FindByKeyAsync("111", cancellationToken);
            data!.Name = "---111----";

            var data2 = await _locationRepo.FindByKeyAsync("222", cancellationToken);
            data2!.Name = "----222";

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok();
        }
    }
}
