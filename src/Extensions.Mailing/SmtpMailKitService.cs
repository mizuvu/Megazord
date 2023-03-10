using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
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

        public async Task SendAsync(Sender sender, MailMessage mail, CancellationToken cancellationToken = default)
        {
            var email = new MimeMessage
            {
                Sender = new MailboxAddress(sender.DisplayName, sender.Address),
                Subject = mail.Subject,
            };

            var bodyBuilder = new BodyBuilder { HtmlBody = mail.Content };

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

            var attachments = new List<MimeEntity>();

            foreach (var attachment in mail.Attachments)
            {
                // file from stream
                bodyBuilder.Attachments.Add(attachment.FileName, attachment.FileToBytes);
            }

            // build message body
            email.Body = bodyBuilder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, _smtpSettings.UseSsl, cancellationToken);
            await smtp.AuthenticateAsync(_smtpSettings.UserName, _smtpSettings.Password, cancellationToken);
            await smtp.SendAsync(email, cancellationToken);
            await smtp.DisconnectAsync(true, cancellationToken);

#if DEBUG
            _logger.LogInformation("----- Mail to <{email}> [{subject}] succeeded.",
                mail.Recipients.JoinToString(), mail.Subject);
#endif
        }
    }
}