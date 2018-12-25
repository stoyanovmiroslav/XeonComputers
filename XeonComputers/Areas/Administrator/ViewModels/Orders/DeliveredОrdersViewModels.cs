using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XeonComputers.Enums;
using XeonComputers.Models.Enums;

namespace XeonComputers.Areas.Administrator.ViewModels.Orders
{
    public class DeliveredОrdersViewModels
    {
        public int Id { get; set; }

        public string PaymentStatus { get; set; }

        public DateTime? DeliveryDate { get; set; }
        
        public decimal TotalPrice { get; set; }

        public string PaymentType { get; set; }
    }
}
