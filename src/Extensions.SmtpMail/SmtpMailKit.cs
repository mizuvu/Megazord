using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Zord.Extensions.Mailing.Models;

namespace Zord.Extensions.SmtpMail
{
    public class SmtpMailKit
    {
        public async Task SendAsync(Sender sender, MailMessage mail, SmtpSettings settings, CancellationToken cancellationToken = default)
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
            await smtp.ConnectAsync(settings.Host, settings.Port, settings.UseSsl, cancellationToken);
            await smtp.AuthenticateAsync(settings.UserName, settings.Password, cancellationToken);
            await smtp.SendAsync(email, cancellationToken);
            await smtp.DisconnectAsync(true, cancellationToken);
            smtp.Dispose();

#if DEBUG
            Console.WriteLine($"----- Mail to <{mail.Recipients.JoinToString()}> [{mail.Subject}] succeeded.");
#endif
        }
    }
}