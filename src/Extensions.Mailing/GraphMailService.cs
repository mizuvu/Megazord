using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zord.Core.Mailing;

namespace Zord.Extensions.Mailing
{
    public class GraphMailService : IGraphMailService
    {
        private readonly GraphServiceClient _graphServiceClient;
        private readonly ILogger<GraphMailService> _logger;

        public GraphMailService(GraphServiceClient graphServiceClient,
            ILogger<GraphMailService> logger)
        {
            _graphServiceClient = graphServiceClient;
            _logger = logger;
        }

        public async Task SendAsync(Sender sender, MailMessage mail)
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
            await _graphServiceClient.Users[sender.Address].SendMail.PostAsync(request);

#if DEBUG
            _logger.LogInformation("----- Mail to <{email}> [{subject}] succeeded.",
                mail.Recipients.JoinToString(), mail.Subject);
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
