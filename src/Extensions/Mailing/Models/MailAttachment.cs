namespace Zord.Extensions.Mailing.Models
{
    public class MailAttachment
    {
        public string FileName { get; set; } = default!;
        public byte[] FileToBytes { get; set; } = default!;
    }
}
