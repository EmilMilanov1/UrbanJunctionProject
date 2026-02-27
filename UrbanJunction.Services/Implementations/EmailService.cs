using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using UrbanJunction.Services.Interfaces;

namespace UrbanJunction.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendContactEmailAsync(string name, string email, string message)
        {
            var smtpHost  = _config["Email:SmtpHost"]  ?? "smtp.gmail.com";
            var smtpPort  = int.Parse(_config["Email:SmtpPort"] ?? "587");
            var smtpUser  = _config["Email:Username"]  ?? "";
            var smtpPass  = _config["Email:Password"]  ?? "";
            var toAddress = _config["Email:ToAddress"] ?? smtpUser;

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true
            };

            var mail = new MailMessage
            {
                From       = new MailAddress(smtpUser, "Urban Junction Contact"),
                Subject    = $"[Urban Junction] Message from {name}",
                Body       = $"From: {name} <{email}>\n\n{message}",
                IsBodyHtml = false
            };
            mail.To.Add(toAddress);

            await client.SendMailAsync(mail);
        }
    }
}
