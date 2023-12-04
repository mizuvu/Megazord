﻿using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Sample.Data;

namespace Sample.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
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
