using Host.Events;
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
            IQueryable<TestEvent> query = new HashSet<TestEvent>().AsQueryable();

            var value = "";
            return Ok(value.LeftToChar("-"));
        }
    }
}
