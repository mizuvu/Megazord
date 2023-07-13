using Host.Events;
using Host.Extensions;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Zord.Result.Models;

namespace Host.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ResultController : ControllerBase
    {
        private readonly List<int> _list = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        [HttpGet]
        public IActionResult Get()
        {
            TestEvent test = default;

            int id = default;


            return Result<TestEvent>.Object(test).ToResponse();
        }

        private PagedList<int> ResultList()
        {
            var result = _list.ToPagedList();
            return result;
        }

        private PagedList<int> MapperList()
        {
            var paged = _list.ToPagedList(1, 5);
            var result = paged.Adapt<PagedList<int>>();
            return result;
        }

        private PagedList<int> DeserializeList()
        {
            var list = _list.ToPagedList(1, 5);
            var json = JsonSerializer.Serialize(list);

            var result = JsonSerializer.Deserialize<PagedList<int>>(json);
            return result!;
        }

        private IResult<List<int>> MapperObject()
        {
            var data = new List<int>() { 1, 2, 3, 4 };

            var list = Result<List<int>>.Object(data);

            var result = list.Adapt<IResult<List<int>>>();
            return result;
        }
    }
}
