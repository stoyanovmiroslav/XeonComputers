using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XeonComputers.Models;
using XeonComputers.Services.Common;
using XeonComputers.Services.Contracts;

namespace XeonComputers.Services
{
    public class EmailService : IEmailService
    {
        private const string REGISTERED_ORDER = "Регистрирана поръчка #{0}";

        private readonly IEmailSender emailSender;

        public EmailService(IEmailSender emailSender)
        {
            this.emailSender = emailSender;
        }

        public async Task SentConfirmationOrderEmail(Order order)
        {
            var emailMessageTempate = GlobalConstants.CONFIRM_ORDER_EMAIL_TEMPLATE;
            var message = string.Format(emailMessageTempate, order.Recipient, order.RecipientPhoneNumber, order.DeliveryAddress.City.Name, order.DeliveryAddress.City.Postcode,
                                                             order.DeliveryAddress.Street, order.DeliveryAddress.Description, order.TotalPrice);

            var subject = string.Format(REGISTERED_ORDER, order.Id);

            await this.emailSender.SendEmailAsync(order.XeonUser.Email, subject, message);
        }
    }
}
