using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Models.Enums;

namespace XeonComputers.ViewModels.Orders
{
    public class ConfirmOrderViewModel
    {
        public int Id { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal DeliveryPrice { get; set; }

        public string Recipient { get; set; }

        public string RecipientPhoneNumber { get; set; }

        public PaymentType PaymentType { get; set; }

        public string PaymentTypeDisplayName { get; set; }

        public string DeliveryAddressDescription { get; set; }

        public string DeliveryAddressStreet { get; set; }

        public string DeliveryAddressCityName { get; set; }

        public string DeliveryAddressCityPostCode { get; set; }
    }
}