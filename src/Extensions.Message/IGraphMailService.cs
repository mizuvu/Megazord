using System.Threading;
using System.Threading.Tasks;
using Zord.Core.Mailing;

namespace Zord.Core
{
    public interface IGraphMailService
    {
        Task SendAsync(Sender sender, MailMessage mail, CancellationToken cancellationToken = default);
    }
}