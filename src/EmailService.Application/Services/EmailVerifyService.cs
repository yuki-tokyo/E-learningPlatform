using EmailService.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace EmailService.Application.Services
{
    public class EmailVerifyService : IEmailVerifyService
    {
        private readonly IConfiguration _config;

        public EmailVerifyService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendVerificationCode(string email, string code)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(
                    _config["Email:Login"],
                    _config["Email:Password"]),
                EnableSsl = true
            };

            var message = new MailMessage
            {
                From = new MailAddress(_config["Email:Login"]),
                Subject = "Код подтверждения",
                Body = $"Ваш код: {code}\nКод действует 10 минут.",
                IsBodyHtml = false
            };
            message.To.Add(email);

            await smtpClient.SendMailAsync(message);
        }
    }
}
