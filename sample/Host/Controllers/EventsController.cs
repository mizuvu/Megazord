using Host.Events;
using Host.MessageQueues;
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
            _eventBus.Publish("TestQueue", new MessageQueue<TestEvent>(new TestEvent { }));
            return Ok();
        }

        [HttpGet("message-queue")]
        public IActionResult GetMessageQueue()
        {
            var message = new TestMessageQueue { Data = new[] { "123", "456" } };

            _eventBus.Publish(message);

            return Ok();
        }
    }
}
