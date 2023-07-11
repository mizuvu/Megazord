using Host.Models;
using Microsoft.AspNetCore.Mvc;
using Zord.Extensions;

namespace Host.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            IQueryable<TestModel> query = new HashSet<TestModel>().AsQueryable();

            var value = "";
            return Ok(value.LeftToChar("-"));
        }
    }
}
