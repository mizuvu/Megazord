using Microsoft.AspNetCore.Mvc;
using Zord.Exceptions;

namespace Host.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("default-exception")]
        public IActionResult Throw()
        {
            throw new Exception("throw via default exception");
        }

        [HttpGet("zord-exception")]
        public IActionResult ThrowZord()
        {
            throw new InternalServerErrorException("throw via Zord exception");
        }
    }
}
