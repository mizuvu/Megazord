using System.Threading;
using System.Threading.Tasks;
using Zord.Mail;

namespace Zord.Graph
{
    public interface IGraphMailService
    {
        Task SendAsync(Sender sender, MailMessage mail, CancellationToken cancellationToken = default);
    }
}