using System.Collections.Generic;

namespace Zord.Core.Mailing
{
    public class MailMessage
    {
        public List<string> Recipients { get; set; } = new List<string>();

        public string Subject { get; set; } = default!;

        public string Content { get; set; } = default!;

        public List<string> CcRecipients { get; set; } = new List<string>();

        public List<string> BccRecipients { get; set; } = new List<string>();

        public List<MailAttachment> Attachments { get; set; } = new List<MailAttachment>();
    }
}
