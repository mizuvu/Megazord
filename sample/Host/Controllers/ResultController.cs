using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Host.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ResultController : ControllerBase
    {
        private readonly List<int> _list;

        public ResultController()
        {
            var list = new List<int>();
            for (int i = 0; i < 20; i++)
                list.Add(i);

            _list = list;
        }


        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_list.ToPagedResult(1, _list.Count));
        }

        [HttpGet("mapper-list")]
        public IActionResult GetPaged(int page, int size)
        {
            var paged = _list.ToPagedResult(page, size);
            var result = paged.Adapt<PagedResult<int>>();
            return Ok(result);
        }

        [HttpGet("deserialize-list")]
        public IActionResult DeserializePaged(int page, int size)
        {
            var list = _list.ToPagedResult(page, size);
            var json = JsonSerializer.Serialize(list);

            var result = JsonSerializer.Deserialize<PagedResult<int>>(json);

            return Ok(result);
        }

        [HttpGet("error")]
        public IActionResult Error()
        {
            IEnumerable<int>? list = null;

            var result = Result.Error("Error");
            var resultOfT = Result<int>.Error("Error of T");


            var resultOfList = list.ToPagedResult(1, 5);


            return Ok(new { result, resultOfT, resultOfList });
        }
    }
}
