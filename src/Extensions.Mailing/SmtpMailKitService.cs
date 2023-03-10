using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;
using Zord.Core.Mailing;

namespace Zord.Extensions.Mailing
{
    public class SmtpMailKitService : ISmtpMailService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly ILogger<SmtpMailKitService> _logger;

        public SmtpMailKitService(IOptions<SmtpSettings> smtpSettings,
            ILogger<SmtpMailKitService> logger)
        {
            _smtpSettings = smtpSettings.Value;
            _logger = logger;
        }

        public async Task SendAsync(Sender sender, MailMessage mail)
        {
            var email = new MimeMessage
            {
                Sender = new MailboxAddress(sender.DisplayName, sender.Address),
                Subject = mail.Subject,
                Body = new BodyBuilder
                {
                    HtmlBody = mail.Content
                }.ToMessageBody(),
            };

            // add address mail to send
            foreach (var address in mail.Recipients)
            {
                email.To.Add(MailboxAddress.Parse(address));
            }

            // add CC
            foreach (var address in mail.CcRecipients)
            {
                email.Cc.Add(MailboxAddress.Parse(address));
            }

            // add BCC
            foreach (var address in mail.BccRecipients)
            {
                email.Bcc.Add(MailboxAddress.Parse(address));
            }

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, _smtpSettings.UseSsl);
            await smtp.AuthenticateAsync(_smtpSettings.UserName, _smtpSettings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

#if DEBUG
            _logger.LogInformation("----- Mail to <{email}> [{subject}] succeeded.",
                mail.Recipients.JoinToString(), mail.Subject);
#endif
        }
    }
}