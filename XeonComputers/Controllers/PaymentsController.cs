using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using XeonComputers.Services.Contracts;
using XeonComputers.Models;
using XeonComputers.ViewModels;
using XeonComputers.ViewModels.Home;
using XeonComputers.ViewModels.Payments;
using Microsoft.AspNetCore.Authorization;
using XeonComputers.Models.Enums;
using Microsoft.AspNetCore.Identity.UI.Services;
using XeonComputers.ViewModels.Orders;
using AutoMapper;

namespace XeonComputers.Controllers
{
    [Authorize]
    public class PaymentsController : BaseController
    {
        private const string PATH_CONFIRMATION_ORDER_EMAIL = "Views/EmailTemplates/ConfirmOrder.cshtml";
        private const string PAYMENT_STATUS_ERROR = "INVOICE={0}:STATUS=ERR\n";
        //if error for this invoice
        private const string PAYMENT_STATUS_OK = "INVOICE={invoice}:STATUS=OK\n";
        //if not recognise this invoice
        private const string PAYMENT_STATUS_NO = "INVOICE=$invoice:STATUS=NO\n";
        private const string REGISTERED_ORDER = "Регистрирана поръчка #{0}";
        private const string YOUR_ORDER_WAS_SUCCESSFULLY_RECEIVED = "Вашата поръчка беше получена успешно!";
        private const string YOUR_ORDER_WAS_UNSUCCESSFULLY_PAID = "Вашата поръчка не беше платена успешно!";
        private const string INVALID_ORDER = "Невалидна поръчка!";
        private const string SUBMIT_URL_DEMO = "https://devep2.datamax.bg/ep2/epay2_demo/";
        private const string SUBMIT_URL = "https://www.epay.bg/";

        private readonly IOrdersService ordersService;
        private readonly IPaymentsService paymentService;
        private readonly IUsersService usersService;
        private readonly IEmailSender emailSender;
        private readonly IMapper mapper;

        public PaymentsController(IPaymentsService paymentService, IOrdersService ordersService,
                                  IUsersService usersService, IEmailSender emailSender, IMapper mapper)
        {
            this.paymentService = paymentService;
            this.ordersService = ordersService;
            this.usersService = usersService;
            this.emailSender = emailSender;
            this.mapper = mapper;
        }

        public async Task<IActionResult> SuccessfulPayment(int id)
        {
            var order = this.ordersService.GetOrderById(id);
            if (order == null)
            {
                this.TempData["info"] = INVALID_ORDER;

                return this.RedirectToAction("Index", "Home");
            }

            var email = this.usersService.GetUserByUsername(this.User.Identity.Name).Email;

            var orderModel = mapper.Map<CompleteOrderViewModel>(order);
            var emailMessageTempate = System.IO.File.ReadAllText(PATH_CONFIRMATION_ORDER_EMAIL);
            var message = string.Format(emailMessageTempate, orderModel.Recipient, orderModel.RecipientPhoneNumber, orderModel.DeliveryAddressCityName, orderModel.DeliveryAddressCityPostCode,
                                                             orderModel.DeliveryAddressStreet, orderModel.DeliveryAddressDescription, orderModel.TotalPrice);

            var subject = string.Format(REGISTERED_ORDER, order.Id);

            await this.emailSender.SendEmailAsync(email, subject, message);

            this.TempData["info"] = YOUR_ORDER_WAS_SUCCESSFULLY_RECEIVED;

            return this.RedirectToAction("Index", "Home");
        }

        public IActionResult UnsuccessfulPayment(int id)
        {
            this.TempData["info"] = YOUR_ORDER_WAS_UNSUCCESSFULLY_PAID;

            return this.RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult PaymentStatus(string encoded, string checksum)
        {
            string encodedData = this.paymentService.GetDencodedData(encoded, checksum);

            if (encodedData == null)
            {
                return Content(string.Format(PAYMENT_STATUS_ERROR, null));
            }

            var data = encoded.Split("\n");
            var invoiceKeyValue = data.FirstOrDefault(x => x.Contains("INVOICE"));
            var invoice = invoiceKeyValue.Split("=").Last();

            var paymentStatusKeyValue = data.FirstOrDefault(x => x.Contains("STATUS"));
            var status = invoiceKeyValue.Split("=").Last();

            if (invoice == null || status == null)
            {
                return Content(string.Format(PAYMENT_STATUS_ERROR, null));
            }

            bool isStatusSet = ordersService.SetOrderStatusByInvoice(invoice, status);
            if (!isStatusSet)
            {
                return Content(string.Format(PAYMENT_STATUS_ERROR, invoice));
            }

            return Content(string.Format(PAYMENT_STATUS_OK, invoice));
        }
    }
}