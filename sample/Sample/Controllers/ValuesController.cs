using Microsoft.AspNetCore.Mvc;
using Sample.Models;
using Zord.Extensions;

namespace Sample.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var value = "";
            return Ok(value.Left("-"));
        }

        [HttpGet("right")]
        public IActionResult Get(string data, int length)
        {
            return Ok(data.Right(length));
        }

        [HttpGet("null-checker")]
        public IActionResult NullCheck()
        {
            Result? result = default;

            result.ThrowIfNull();

            return Ok();
        }

        [HttpGet("obj-to-string")]
        public IActionResult ObjToString()
        {
            var obj = new TestModel
            {
                Id = 1,
                Name = "Test",
                CreatedOn = DateTime.Now,
            };

            var stringData = obj.ToJsonString();

            return Ok(stringData);
        }

        [HttpGet("read-string-as-obj")]
        public IActionResult StringToObj(string value)
        {
            var obj = value.ReadJsonAs<TestModel>();

            return Ok(obj);
        }
    }
}
