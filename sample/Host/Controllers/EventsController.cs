using Host.Models;
using Microsoft.AspNetCore.Mvc;
using Zord.Extensions.EventBus.Abstractions;
using Zord.Extensions.EventBus.Events;

namespace Host.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventBus _eventBus;

        public EventsController(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _eventBus.Publish("TestQueue", new MessageQueue<TestModel>(new TestModel { }));
            return Ok();
        }
    }
}
