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

namespace XeonComputers.Controllers
{
    [Authorize]
    public class PaymentsController : BaseController
    {
        private const string SUBMIT_URL_DEMO = "https://devep2.datamax.bg/ep2/epay2_demo/";
        private const string SUBMIT_URL = "https://www.epay.bg/";

        private readonly IOrdersService ordersService;
        private readonly IPaymentsService paymentService;

        public PaymentsController(IPaymentsService paymentService, IOrdersService ordersService)
        {
            this.paymentService = paymentService;
            this.ordersService = ordersService;
        }

        public IActionResult Pay(int orderId)
        {
            var order = this.ordersService.GetOrderById(orderId);

            if (order == null)
            {
                this.RedirectToAction("Index", "Home");
            }

            //TODO: 
            var encoded = this.paymentService.EPay(22.33M, "this is description");

            var payViewModel = new PayViewModel
            {
                Encoded = this.paymentService.Encoded,
                SubmitUrl = SUBMIT_URL_DEMO,
                ChechSum = encoded
            };

            return View(payViewModel);
        }
    }
}