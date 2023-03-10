using Microsoft.AspNetCore.Mvc;
using Zord.Core.Mailing;

namespace Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly IGraphMailService _graphMailService;
        private readonly ISmtpMailService _smtpMailService;

        public TestController(ILogger<TestController> logger,
            IGraphMailService graphMailService,
            ISmtpMailService smtpMailService)
        {
            _logger = logger;
            _graphMailService = graphMailService;
            _smtpMailService = smtpMailService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string filePath = @$"D:\\backups\\pexels-pixabay-268533.jpg";
            byte[] byteArray = System.IO.File.ReadAllBytes(filePath);

            var message = new MailMessage
            {
                Subject = "Test...." + DateTime.Now,
                Content = "Hello,.......... this test mail",
            };

            message.Recipients.Add("minhvd@aswatson.com");

            //message.CcRecipients.Add("minhvu.box@gmail.com");

            //message.BccRecipients.Add("kidkylin@gmail.com");
            /*
            message.Attachments.Add(new MailAttachment
            {
                FileName = "pexels-pixabay-268533.jpg",
                FileToBytes = byteArray
            });
            */
            var sender = new Sender
            {
                Address = "minhvd@aswatson.com",
                DisplayName = "Minh VD"
            };

            await _smtpMailService.SendAsync(sender, message);

            return Ok();
        }
    }
}