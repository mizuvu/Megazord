using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Threading.Tasks;
using Zord.Core.Mailing;

namespace Zord.Extensions.Mailing
{
    public class SmtpMailService : ISmtpMailService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly ILogger<SmtpMailService> _logger;

        public SmtpMailService(IOptions<SmtpSettings> smtpSettings,
            ILogger<SmtpMailService> logger)
        {
            _smtpSettings = smtpSettings.Value;
            _logger = logger;
        }

        public async Task SendAsync(Sender sender, Core.Mailing.MailMessage mail)
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

            using var smtpClient = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            //smtpClient.EnableSsl = true;

            await smtpClient.SendMailAsync(message);
            smtpClient.Dispose();

#if DEBUG
            _logger.LogInformation("----- Mail to <{email}> [{subject}] succeeded.",
                mail.Recipients.JoinToString(), mail.Subject);
#endif   
        }
    }
}
