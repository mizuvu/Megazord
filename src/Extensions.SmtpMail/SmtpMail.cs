using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Zord.Core.Mailing;

namespace Zord.Extensions.SmtpMail
{
    public class SmtpMail
    {
        public async Task SendAsync(Sender sender, Core.Mailing.MailMessage mail, SmtpSettings settings, CancellationToken cancellationToken = default)
        {
            var message = new System.Net.Mail.MailMessage
            {
                From = new MailAddress(sender.Address, sender.DisplayName),
                Subject = mail.Subject,
                IsBodyHtml = true,
                Body = mail.Content,
            };

            // add address mail to send
            foreach (var address in mail.Recipients)
            {
                message.To.Add(new MailAddress(address));
            }

            // add CC
            foreach (var address in mail.CcRecipients)
            {
                message.CC.Add(new MailAddress(address));
            }

            // add BCC
            foreach (var address in mail.BccRecipients)
            {
                message.Bcc.Add(new MailAddress(address));
            }

            using var smtp = new SmtpClient(settings.Host, settings.Port)
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = settings.UseSsl
            };

            await smtp.SendMailAsync(message);
            smtp.Dispose();
        }
    }
}
