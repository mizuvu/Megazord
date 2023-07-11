using Microsoft.Graph;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Zord.Extensions.Graph.Abstractions;
using Zord.Extensions.Mailing.Models;

namespace Zord.Extensions.Graph.Mailing
{
    public class GraphMailService : IGraphMailService
    {
        private readonly GraphServiceClient _graphServiceClient;

        public GraphMailService(GraphServiceClient graphServiceClient)
        {
            _graphServiceClient = graphServiceClient;
        }

        public async Task SendAsync(Sender sender, MailMessage mail, CancellationToken cancellationToken = default)
        {
            // Define a simple e-mail message.
            var message = new Message
            {
                ToRecipients = RecipientBuilder(mail.Recipients),
                Subject = mail.Subject,
                Body = new ItemBody
                {
                    ContentType = BodyType.Html,
                    Content = mail.Content
                },

                // add CC
                CcRecipients = RecipientBuilder(mail.CcRecipients),

                // add BCC
                BccRecipients = RecipientBuilder(mail.BccRecipients),

                // add attachments
                Attachments = AttachmentBuilder(mail.Attachments),
            };

            var request = new Microsoft.Graph.Users.Item.SendMail.SendMailPostRequestBody
            {
                Message = message,
                SaveToSentItems = true,
            };

            // Send mail as the given user. 
            await _graphServiceClient.Users[sender.Address].SendMail.PostAsync(request, cancellationToken: cancellationToken);

#if DEBUG
            Console.WriteLine("----- Mail to <{email}> [{subject}] succeeded.",
                string.Join("|", mail.Recipients), mail.Subject);
#endif
        }

        private List<Recipient> RecipientBuilder(List<string> addresses)
        {
            // generate recipients
            return addresses
                .Select(address => new Recipient
                {
                    EmailAddress = new EmailAddress { Address = address }
                })
                .ToList();
        }

        private List<Attachment> AttachmentBuilder(List<MailAttachment> attachments)
        {
            return attachments
                .Select(s => new Attachment
                {
                    OdataType = "#microsoft.graph.fileAttachment",
                    Name = s.FileName,
                    AdditionalData = new Dictionary<string, object>
                    {
                        {
                            "contentBytes" , Convert.ToBase64String(s.FileToBytes)
                        },
                    }
                })
                .ToList();
        }
    }
}
