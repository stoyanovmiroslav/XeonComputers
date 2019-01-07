using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace XeonComputers.Services
{
    public class EmailSender : IEmailSender
    {
        private const string SENDER_EMAIL = "no-reply@xeoncomputers.com";
        private const string NAME_OF_THE_SENDER = "Xeon Computers";

        public EmailSender(IConfiguration Configuration)
        {
            this.SendGridKey = Configuration["Authentication:SendGridKey"];
        }

        public string SendGridUser { get; set; }

        public string SendGridKey { get; set; }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(this.SendGridKey, subject, message, email);
        }

        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(SENDER_EMAIL, NAME_OF_THE_SENDER),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}