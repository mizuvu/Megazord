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
            var value = "";
            return Ok(value.LeftToChar("-"));
        }

        [HttpGet("null-checker")]
        public IActionResult NullCheck()
        {
            Result result = default;

            result.ThrowIfNull();

            return Ok();
        }
    }
}
