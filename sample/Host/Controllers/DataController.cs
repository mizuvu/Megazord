using Asp.Versioning;
using Host.Data;
using Microsoft.AspNetCore.Mvc;
using Zord.Extensions;
using Zord.Repository.EntityFrameworkCore;

namespace Host.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
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
        public async Task<IActionResult> PutAsync(RetailLocation location, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.Repository<RetailLocation>().FindByKeyAsync(location.Code, cancellationToken);
            data!.Name = location.Name;
            data.Phone = location.Phone;
            data.Address = location.Address;
            data.City = location.City;

            //var data2 = await _locationRepo.FindByKeyAsync("222", cancellationToken);
            //data2!.Name = "----222";

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok();
        }
    }
}
