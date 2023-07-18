using Host.Events;
using Microsoft.AspNetCore.Mvc;
using Zord.Extensions;
using Zord.Extensions.Telegram;

namespace Host.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TelegramController : ControllerBase
    {
        private readonly ITelegramBotService _botService;

        public TelegramController(ITelegramBotService botService)
        {
            _botService = botService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await _botService.SendMessage("123");

            return Ok();
        }
    }
}
