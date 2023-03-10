using System.Threading.Tasks;

namespace Zord.Core.Mailing
{
    public interface ISmtpMailService
    {
        Task SendAsync(Sender sender, MailMessage mail);
    }
}