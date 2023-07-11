using System.Threading;
using System.Threading.Tasks;
using Zord.Extensions.Mailing.Models;

namespace Zord.Extensions.Graph.Abstractions
{
    public interface IGraphMailService
    {
        Task SendAsync(Sender sender, MailMessage mail, CancellationToken cancellationToken = default);
    }
}