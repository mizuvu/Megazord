using Microsoft.AspNetCore.Mvc;
using Zord.Serilog;

namespace Sample.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OptionsController(IConfiguration configuration) : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var config = SerilogOptionsExtensions.GetWriteToOptions(configuration);
            return Ok(config);
        }
    }
}
