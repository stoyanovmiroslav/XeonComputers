using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Enums;
using XeonComputers.Models.Enums;

namespace XeonComputers.ViewModels.Orders
{
    public class OrderDetailsViewModel
    {
        public int Id { get; set; }

        public string Status { get; set; }

        public string PaymentStatus { get; set; }

        public string PaymentType { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? DispatchDate { get; set; }

        public decimal TotalPrice { get; set; }

        public string Recipient { get; set; }

        public string RecipientPhoneNumber { get; set; }

        public string XeonUserCompanyName { get; set; }

        public string DeliveryAddressDescription { get; set; }

        public string DeliveryAddressCityName { get; set; }

        public string DeliveryAddressCityPostcode { get; set; }

        public string DeliveryAddressStreet { get; set; }

        public string EasyPayNumber { get; set; }

        public string InvoiceNumber { get; set; }

        public IList<OrderProductsViewModel> OrderProductsViewModel { get; set; }

        public decimal DeliveryPrice { get; set; }
    }
}
