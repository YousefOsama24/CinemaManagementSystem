using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace CinemaSystemManagement.Utility
{
    public class EmailSender : IEmailSender
    {
        private const string V = "EmailSettings:Email";
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }



        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(
                    "moyousefhashad@gmail.com",
                    "vkto jxtk nlgy bigp"         
                )
            };

            return client.SendMailAsync(
                new MailMessage(
                    from: "moyousefhashad@gmail.com",
                    to: email,
                    subject,
                    htmlMessage
                )
            );
        }
        
    }
    
}
