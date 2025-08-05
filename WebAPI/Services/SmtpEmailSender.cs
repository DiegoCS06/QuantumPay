using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace WebAPI.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly IConfiguration _config;

        public SmtpEmailSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var s = _config.GetSection("Smtp");
            using var client = new SmtpClient(s["Host"], int.Parse(s["Port"]))
            {
                Credentials = new NetworkCredential(s["User"], s["Pass"]),
                EnableSsl = true
            };

            var msg = new MailMessage(
                from: new MailAddress(s["FromEmail"], s["FromName"]),
                to: new MailAddress(to)
            )
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };

            await client.SendMailAsync(msg);
        }
    }
}
