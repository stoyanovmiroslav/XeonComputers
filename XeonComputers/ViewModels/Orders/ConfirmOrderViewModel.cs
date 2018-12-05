using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Models.Enums;

namespace XeonComputers.ViewModels.Orders
{
    public class ConfirmOrderViewModel
    {
        public decimal TotalPrice { get; set; }

        public decimal DeliveryPrice { get; set; }

        public string Recipient { get; set; }

        public string RecipientPhoneNumber { get; set; }

        public PaymentType PaymentType { get; set; }

        public string Description { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string PostCode { get; set; }
    }
}