using System.Threading.Tasks;

namespace Zord.Core
{
    public interface ITelegramService
    {
        Task SendMessage(string message);
    }
}


