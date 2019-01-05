using System;
using System.Collections.Generic;
using XeonComputers.Enums;
using XeonComputers.Models.Enums;

namespace XeonComputers.Models
{
    public class Order
    {
        public int Id { get; set; }

        public OrderStatus Status { get; set; }

        public PaymentStatus PaymentStatus { get; set; }

        public DateTime? OrderDate { get; set; }

        public DateTime? EstimatedDeliveryDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public DateTime? DispatchDate { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal DeliveryPrice { get; set; }

        public string Recipient { get; set; }

        public string RecipientPhoneNumber { get; set; }

        public string InvoiceNumber { get; set; }

        public string EasyPayNumber { get; set; }

        public PaymentType PaymentType { get; set; }

        public string XeonUserId { get; set; }
        public XeonUser XeonUser { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }

        public int? DeliveryAddressId { get; set; }
        public virtual Address DeliveryAddress { get; set; }
    }
}