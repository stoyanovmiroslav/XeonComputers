using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Models.Enums;

namespace XeonComputers.ViewModels.Payments
{
    public class PayViewModel
    {
        public string Encoded { get; set; }

        public string SubmitUrl { get; set; }

        public string ChechSum { get; set; }

        public PaymentType PaymentType { get; set; }

        public string UrlOk { get; set; }

        public string UrlCancel { get; set; }

        public string EasyPayNumber { get; set; }
    }
}