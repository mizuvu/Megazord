using System.Threading.Tasks;

namespace Zord.Extensions.Telegram
{
    public interface ITelegramBotService
    {
        Task SendMessage(string message);
    }
}


