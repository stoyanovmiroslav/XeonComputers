using System;
using System.Collections.Generic;
using XeonComputers.Enums;

namespace XeonComputers.Models
{
    public class Order
    {
        public int Id { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? EstimatedDeliveryDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public decimal TotalPrice { get; set; }

        public int XeonUserId { get; set; }
        public XeonUser XeonUser { get; set; }

        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}