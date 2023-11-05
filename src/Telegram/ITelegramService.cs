namespace Telegram
{
    public interface ITelegramService
    {
        Task SendMessage(string message);
    }
}


