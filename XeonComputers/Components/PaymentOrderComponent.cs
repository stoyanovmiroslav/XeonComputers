using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Common;
using XeonComputers.Models.Enums;
using XeonComputers.Services.Contracts;
using XeonComputers.ViewModels.Payments;
using XeonComputers.ViewModels.ShoppingCart;
using System.Net.Http;

namespace XeonComputers.Components
{
    public class PaymentOrderComponent : ViewComponent
    {
        private const string DESCRIPTION = "Xeon Computers поръчка номер {0}";
        private const string URL_PAYMENT_OK = "https://localhost:44374/Payments/SuccessfulPayment/{0}";
        private const string URL_PAYMENT_Cancel = "https://localhost:44374/Payments/UnsuccessfulPayment/{0}";

        private const string SUBMIT_PAYMENT_URL_DEMO = "https://demo.epay.bg/";
        private const string SUBMIT_PAYMENT_URL = "https://www.epay.bg/";

        private readonly IPaymentsService paymentsService;
        private readonly IOrdersService ordersService;

        public PaymentOrderComponent(IPaymentsService paymentsService, IOrdersService ordersService)
        {
            this.paymentsService = paymentsService;
            this.ordersService = ordersService;
        }

        public IViewComponentResult Invoke(int orderId)
        {
            var order = this.ordersService.GetOrderById(orderId);
            if (order == null)
            {
                TempData["error"] = "Невалидна поръчка, моля опитайте отново!";
                return this.View(new PayViewModel());
            }

            var description = string.Format(DESCRIPTION, orderId);
            var invoce = order.InvoiceNumber;
            var expDate = DateTime.UtcNow.AddDays(2).ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);

            var checkSum = this.paymentsService.GetEncodedData(order.TotalPrice, description, expDate, invoce);

            string easyPayNumber = null;
            if (order.PaymentType == PaymentType.EasyPay)
            {
                using (var client = new HttpClient())
                {
                    var uri = new Uri($"https://demo.epay.bg/ezp/reg_bill.cgi?ENCODED={this.paymentsService.Encoded}&CHECKSUM={checkSum}");

                    var response = client.GetAsync(uri).GetAwaiter().GetResult();

                    easyPayNumber = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    easyPayNumber = easyPayNumber.Replace("IDN=", "");
                    ordersService.SetEasyPayNumber(order, easyPayNumber);
                }
            }

            var payViewModel = new PayViewModel
            {
                Encoded = this.paymentsService.Encoded,
                SubmitUrl = SUBMIT_PAYMENT_URL_DEMO,
                ChechSum = checkSum,
                PaymentType = order.PaymentType,
                UrlOk = string.Format(URL_PAYMENT_OK, orderId),
                UrlCancel = string.Format(URL_PAYMENT_Cancel, orderId),
                EasyPayNumber = easyPayNumber
            };

            return this.View(payViewModel);
        }
    }
}