using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using XeonComputers.Areas.Administrator.Services.Contracts;
using XeonComputers.Models;
using XeonComputers.Services.Contracts;
using XeonComputers.ViewModels;
using XeonComputers.ViewModels.Home;
using XeonComputers.ViewModels.Payments;

namespace XeonComputers.Controllers
{
    public class PaymentsController : BaseController
    {
        public const string SUBMIT_URL_DEMO = "https://devep2.datamax.bg/ep2/epay2_demo/";
        public const string SUBMIT_URL = "https://www.epay.bg/";

        
        private IPaymentService paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }

        public IActionResult Pay()
        {
            var encoded = this.paymentService.EPay(22.33M, "this is description");

            var payViewModel = new PayViewModel
            {
                Encoded = this.paymentService.Encoded,
                SubmitUrl = SUBMIT_URL_DEMO,
                Chechsum = encoded
            };

            return View(payViewModel);
        }
    }
}