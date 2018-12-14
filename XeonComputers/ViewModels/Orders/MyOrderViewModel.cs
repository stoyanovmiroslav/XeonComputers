using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Enums;
using XeonComputers.Models.Enums;

namespace XeonComputers.ViewModels.Orders
{
    public class MyOrderViewModel
    {
        public int Id { get; set; }

        public decimal TotalPrice { get; set; }

        public string PaymentStatus { get; set; }

        public string PaymentType { get; set; }

        public string Status { get; set; }
    }
}
